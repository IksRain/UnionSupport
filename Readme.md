# UnionSupport

[![vibe coding](https://img.shields.io/badge/vibe_coding-interim-yellow)](#%EF%B8%8F-disclaimer)

A C# Source Generator providing **discriminated union** implementations, aligned with the [C# Language Proposal for Unions](https://github.com/dotnet/csharplang/blob/main/proposals/unions.md).
<br>
<sub>基于 C# Source Generator 的和类型实现，遵循 C# 语言官方提案标准。</sub>

> **Stop-Gap Solution**: Once .NET ships native `struct union` / `union struct`, a **Code Fix** will be provided to migrate seamlessly.

---

## Spec Compliance

This project implements the **union pattern** as defined in the official proposal:

- `[Union]` attribute marks a type as a union type
- `IUnion { object? Value { get; } }` interface (`System.Runtime.CompilerServices`)
- **Union conversion**: implicit conversions from each case type to the union
- **Union matching**: pattern matching unwraps `IUnion.Value` automatically
- **Non-boxing access pattern**: `HasValue` + per-case `TryGetValue(out T)` overloads

### Nullable Value Type Unwrapping

Per the proposal: *"If the case type is a nullable value type, the type of the parameter should be identity-convertible to the underlying type."*

`TryGetValue` and implicit operators strip `?` from nullable value type members — `int?` becomes `int`:

```csharp
[UnionImpl] partial struct MyUnion(int? a, float b);

MyUnion x = 42;                  // int → int (not int?)
x.TryGetValue(out int iv);       // out int, not out int?
```
<sub>按提案标准：nullable 值类型穿透为底层类型，TryGetValue 和隐式转换使用展开后的类型。</sub>

---

## Project Structure

```
UnionSupport.slnx
├── src/
│   ├── UnionSupport.Core/              IUnion, [UnionImpl], [Union], Strategy enum
│   ├── UnionSupport.Generator.Shared/  Shared code-gen + field name encoding
│   ├── UnionSupport.Generator.Product/  Strategy 1: Tagged product type
│   ├── UnionSupport.Generator.Unmanaged/Strategy 2: FieldOffset native sum type
│   ├── UnionSupport.Generator.Erasure/  Strategy 3: Object type erasure
│   ├── UnionSupport.Analyzer/          Compile-time diagnostics (UNION001-003)
│   └── UnionSupport.Type/             Pre-generated unions for 1-17 type params
├── tests/                             xUnit + Verify snapshot tests
└── demo/ConsoleApp1/                  Usage examples
```

## NuGet Packages

| Package | Description |
|---------|-------------|
| `UnionSupport.Core` | Core types — required |
| `UnionSupport.Generator.Product` | Tagged product type generator |
| `UnionSupport.Generator.Unmanaged` | FieldOffset native sum type generator |
| `UnionSupport.Generator.Erasure` | Object type erasure generator |
| `UnionSupport.Analyzer` | Compile-time analyzer |
| `UnionSupport.Type` | Pre-generated union types (no generator needed) |

## Quick Start

### Custom Unions

`[UnionImpl]` defaults to Product strategy.

```csharp
using UnionSupport;

// Product (default, general purpose)
[UnionImpl]
partial struct MyUnion(int a, float b, string c);

// Unmanaged FieldOffset (high perf, unmanaged types only)
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct IntOrFloat(int a, float b);

// Object Erasure (single object field, C# native)
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct AnyValue(int a, string b);

// ref struct (Product only)
[UnionImpl]
ref partial struct SpanUnion(int a, float b);
```

### Usage

```csharp
// Implicit conversions
MyUnion x = 42;
IntOrFloat y = 3.14f;
AnyValue z = "hello";

// Pattern matching (compiler auto-unwraps IUnion.Value)
switch (x)
{
    case int i:  Console.WriteLine($"int: {i}"); break;
    case float f: Console.WriteLine($"float: {f}"); break;
    case string s: Console.WriteLine($"string: {s}"); break;
}

// Non-boxing access
if (x.TryGetValue(out int iv))
    Console.WriteLine($"got int: {iv}");

// Direct access
Console.WriteLine(x.Value);
Console.WriteLine(x.HasValue);
```

### Pre-generated Generic Types

Add `UnionSupport.Type` package:

```csharp
Union<int, float, string> u = 42;      // Product
CUnion<int, float> cu = 3.14f;         // Unmanaged (T : unmanaged)
BoxedUnion<int, string> bu = "hi";     // Object Erasure

switch (u)
{
    case int i: ... break;
    case float f: ... break;
    case string s: ... break;
}
```

> Duplicate type parameters (e.g. `Union<int, int>`) are caught at instantiation by the C# compiler itself — duplicate `TryGetValue(out int)` signatures cause CS0111.

## Three Strategies

| | Product | Unmanaged | ObjectErasure |
|---|---|---|---|
| Storage | Separate fields + `byte` tag | FieldOffset overlapping | Single `object?` field |
| Layout | Compiler-managed | `[StructLayout(Explicit)]` | Regular |
| Memory | Sum of field sizes | Largest field + 1 byte | Object reference |
| Boxing | None | None | Value types boxed |
| Generic constraint | None | `T : unmanaged` | None |
| Use case | General purpose | High-perf, interop | Simple ref/value mix |
| ref struct | ✅ | ❌ | ❌ |
| Field init | `= paramName` | Constructor body | Constructor body |

### Generated Code (Product)

```csharp
partial struct MyUnion : IUnion
{
    private byte __private_flag;
    private int __System_Int32 = a;       // field initializer from primary ctor param
    private float __System_Single = b;    // → CS9113 eliminated

    public object? Value { get { return __private_flag switch { 1 => __System_Int32, 2 => __System_Single, _ => null }; } }
    public bool HasValue => __private_flag != 0;
    public bool TryGetValue(out int value) { if (__private_flag == 1) { value = __System_Int32; return true; } ... }

    public MyUnion(int value) : this(value, default!) { __private_flag = 1; }
    public MyUnion(float value) : this(default!, value) { __private_flag = 2; }

    public static implicit operator MyUnion(int value) => new(value);
    public static implicit operator MyUnion(float value) => new(value);
}
```

## Analyzer Rules

| ID | Rule |
|----|------|
| **UNION001** | Duplicate type: `(int, int)` / `(T, T)` |
| **UNION002** | Ref struct constraint: must use Product / ref member on non-ref struct |
| **UNION003** | Unmanaged strategy requires `!IsUnmanagedType` — no managed types |

---

## Disclaimer

> **This is a Vibe Coding project — a stop-gap for the absence of native `struct union` in C#.**
> <br><sub>本项目是 Vibe Coding 产物，是 .NET 官方 `struct union` 的过渡替代方案。</sub>

The C# language team has drafted the full union specification in [csharplang/proposals/unions.md](https://github.com/dotnet/csharplang/blob/main/proposals/unions.md). Once the compiler ships native `union struct` / `struct union` support:

1. A **Code Fix** will migrate `[UnionImpl(Product)] partial struct Foo(...)` → `union struct Foo(...)` seamlessly.
2. A proof-of-concept Code Fix demo for `Product → struct union` / `Product → union struct` already exists.
3. We will eliminate compile-time code generation overhead and switch to compiler-native support.
4. Migration tooling will be provided for existing projects.

**Contributions welcome.** If you're interested in the migration work or have ideas, join the discussion at [Issues](https://github.com/anomalyco/UnionSupport/issues).

## License

MIT
