using System.Runtime.CompilerServices;
using UnionSupport;

Console.WriteLine("--- Product Strategy ---");
MyUnion p1 = 42;
MyUnion p2 = 3.14f;
switch (p1)
{
    case int i: Console.WriteLine($"int({i})"); break;
    case float f: Console.WriteLine($"float({f})"); break;
}
switch (p2)
{
    case int i: Console.WriteLine($"int({i})"); break;
    case float f: Console.WriteLine($"float({f})"); break;
}

Console.WriteLine("\n--- Unmanaged Strategy ---");
IntOrFloat u1 = 100;
switch (u1)
{
    case int i: Console.WriteLine($"int({i})"); break;
    case float f: Console.WriteLine($"float({f})"); break;
}

Console.WriteLine("\n--- ObjectErasure Strategy ---");
AnyValue e1 = 42;
AnyValue e2 = "hello";
switch (e1)
{
    case int i: Console.WriteLine($"int({i})"); break;
    case string s: Console.WriteLine($"string({s})"); break;
}
switch (e2)
{
    case int i: Console.WriteLine($"int({i})"); break;
    case string s: Console.WriteLine($"string({s})"); break;
}

Console.WriteLine("\n--- Empty ---");
Empty empty = default;
Console.WriteLine($"Empty HasValue: {empty.HasValue}");
Console.WriteLine("Done.");

[UnionImpl]
partial struct UnboxingUnion<T1,T2,T3>(T1 t1, T2 t2, T3 t3);

[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1,T2,T3>(T1 t1, T2 t2, T3 t3)
    where T1 : unmanaged
    where T2 : unmanaged
    where T3 : unmanaged;
    
