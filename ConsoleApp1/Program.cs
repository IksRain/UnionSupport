using UnionSupport;
using System.Runtime.CompilerServices;

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
Console.WriteLine($"Value: {u1.Value}");
switch (u1)
{
    case int i: Console.WriteLine($"int({i})"); break;
    case float f: Console.WriteLine($"float({f})"); break;
}

Console.WriteLine("\n--- ObjectErasure Strategy ---");
AnyValue e2 = "hello";
Console.WriteLine($"Value: {e2.Value}");
switch (e2)
{
    case int i: Console.WriteLine($"int({i})"); break;
    case string s: Console.WriteLine($"string({s})"); break;
}

Console.WriteLine("\n--- Ref Struct Product ---");
RefUnion r1 = 42;
Console.WriteLine($"HasValue: {r1.HasValue}");
if (r1.TryGetValue(out int ri))
    Console.WriteLine($"Got int: {ri}");
try {  _ = r1.Value; } catch (NotSupportedException ex) { Console.WriteLine($"Value throws: {ex.Message}"); }

Console.WriteLine("\n--- Empty ---");
Empty empty = default;
Console.WriteLine($"Empty HasValue: {empty.HasValue}");
Console.WriteLine("Done.");
