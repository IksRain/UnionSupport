using UnionSupport;

Console.WriteLine("--- Product Strategy ---");
MyUnion p1 = 42;
Console.WriteLine($"Value: {p1.Value}, HasValue: {p1.HasValue}");
if (p1.TryGetValue(out int iv))
    Console.WriteLine($"Got int: {iv}");

MyUnion p2 = 3.14f;
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
switch (e2)
{
    case int i: Console.WriteLine($"int({i})"); break;
    case string s: Console.WriteLine($"string({s})"); break;
}

Console.WriteLine("\n--- Empty ---");
Empty empty = default;
Console.WriteLine($"Empty HasValue: {empty.HasValue}");
Console.WriteLine("Done.");

[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
public partial struct BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17);

