``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19041.928 (2004/May2020Update/20H1)
Intel Core i7-10510U CPU 1.80GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.100-preview.4.21255.9
  [Host]     : .NET 5.0.5 (5.0.521.16609), X64 RyuJIT
  DefaultJob : .NET 5.0.5 (5.0.521.16609), X64 RyuJIT


```
|             Method |       Mean |    Error |   StdDev | Ratio | RatioSD |    Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------- |-----------:|---------:|---------:|------:|--------:|---------:|-------:|------:|----------:|
|    UsingSteamAsync | 1,614.9 μs | 32.22 μs | 51.10 μs |  1.00 |    0.00 | 103.5156 | 1.9531 |     - |    401 KB |
| UsingPipelineAsync |   825.4 μs | 10.51 μs |  9.83 μs |  0.50 |    0.02 |   7.8125 |      - |     - |     32 KB |
