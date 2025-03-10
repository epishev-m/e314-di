using BenchmarkDotNet.Running;
using E314.DI.Benchmark;

BenchmarkRunner.Run<Benchmark>();

/*
| Method            | Mean      | Error    | StdDev   | Rank | Gen0   | Gen1   | Allocated |
|------------------ |----------:|---------:|---------:|-----:|-------:|-------:|----------:|
| CreateContainer   |  14.98 ns | 0.050 ns | 0.042 ns |    1 | 0.0110 |      - |     184 B |
| BindTransient     |  78.43 ns | 1.081 ns | 0.958 ns |    3 | 0.0416 |      - |     696 B |
| BindSingleton     | 168.93 ns | 2.216 ns | 1.965 ns |    6 | 0.0501 |      - |     840 B |
| BindScope         | 129.74 ns | 0.726 ns | 0.643 ns |    4 | 0.0496 |      - |     832 B |
| BindComplex       | 305.24 ns | 6.134 ns | 5.738 ns |    9 | 0.0925 |      - |    1552 B |
| BindFactory       |  74.39 ns | 0.983 ns | 0.919 ns |    2 | 0.0440 | 0.0001 |     736 B |
| ResolveTransient  | 192.31 ns | 3.029 ns | 2.833 ns |    7 | 0.0544 |      - |     912 B |
| ResolveISingleton | 283.32 ns | 2.988 ns | 2.649 ns |    8 | 0.0629 |      - |    1056 B |
| ResolveIScope3    | 333.42 ns | 4.461 ns | 4.173 ns |   10 | 0.0701 |      - |    1176 B |
| ResolveFactory    | 135.18 ns | 1.557 ns | 1.300 ns |    5 | 0.0529 |      - |     888 B |
| ResolveComplex    | 728.31 ns | 7.877 ns | 6.982 ns |   11 | 0.1383 |      - |    2328 B |
*/