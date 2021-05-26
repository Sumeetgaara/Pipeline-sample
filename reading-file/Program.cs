using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace reading_file
{
    [MemoryDiagnoser]
    public class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }

        [Benchmark(Baseline =true)]
        public async Task UsingSteamAsync()
        {
            int numberOfMatches = 0;
            using (var stream = File.OpenRead("article.txt"))
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (line.Contains("1NF"))
                    {
                        numberOfMatches++;
                    }
                    
                }
            }
            Console.WriteLine(numberOfMatches);
        }

        [Benchmark]
        public async Task UsingPipelineAsync()
        {
            int numberOfMatches = 0;
            int valueFromEachIteration;
            using (var stream = File.OpenRead("article.txt"))
            {
                PipeReader reader = PipeReader.Create(stream);
                while (true)
                {
                    ReadResult read = await reader.ReadAsync();
                    ReadOnlySequence<byte> buffer = read.Buffer;
                    while ((valueFromEachIteration = GenerateCharacterArray(ref buffer)) != -256)
                    {
                        if (valueFromEachIteration != -1)
                        {
                            numberOfMatches++;
                        }        
                    }
                    reader.AdvanceTo(buffer.Start, buffer.End);
                    if (read.IsCompleted) break;
                }
                await reader.CompleteAsync();
            }
            Console.WriteLine(numberOfMatches);
        }

        static int GenerateCharacterArray(ref ReadOnlySequence<byte> buffer)
        {
            SequenceReader<byte> reader = new SequenceReader<byte>(buffer);

            if (reader.TryReadTo(out ReadOnlySpan<byte> line, NewLine))
            {
                buffer = buffer.Slice(reader.Position);
                return line.IndexOf(searchText);
            }

            return -256; // this can be improved, better way to represent nothing is found.
        }


        private static ReadOnlySpan<byte> NewLine => new byte[] { (byte)'\r', (byte)'\n' };

        private static ReadOnlySpan<byte> searchText => Encoding.UTF8.GetBytes("1NF").AsSpan();


    }
}
