using BenchmarkDotNet.Running;
using E314.DI.Benchmark;

BenchmarkRunner.Run<Benchmark>();

/*
| Method            | Mean      | Error     | StdDev     | Median    | Rank | Gen0   | Allocated |
|------------------ |----------:|----------:|-----------:|----------:|-----:|-------:|----------:|
| CreateContainer   |  18.76 ns |  0.730 ns |   2.024 ns |  18.20 ns |    1 | 0.0110 |     184 B |
| BindTransient     |  85.30 ns |  0.876 ns |   0.819 ns |  85.18 ns |    2 | 0.0416 |     696 B |
| BindSingleton     | 145.31 ns |  2.938 ns |   3.820 ns | 146.00 ns |    3 | 0.0501 |     840 B |
| BindScope         | 141.83 ns |  3.788 ns |  11.111 ns | 136.72 ns |    3 | 0.0496 |     832 B |
| BindComplex       | 339.26 ns |  5.282 ns |   4.411 ns | 339.85 ns |    5 | 0.0925 |    1552 B |
| ResolveTransient  | 210.85 ns |  4.131 ns |   5.073 ns | 211.23 ns |    4 | 0.0544 |     912 B |
| ResolveISingleton | 307.44 ns |  5.988 ns |   7.354 ns | 308.49 ns |    5 | 0.0629 |    1056 B |
| ResolveIScope3    | 325.31 ns |  6.512 ns |  12.070 ns | 324.48 ns |    5 | 0.0701 |    1176 B |
| ResolveComplex    | 883.65 ns | 42.509 ns | 119.897 ns | 838.65 ns |    6 | 0.1383 |    2328 B |
*/