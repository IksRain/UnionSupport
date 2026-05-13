# UnionSupport

[![vibe coding](https://img.shields.io/badge/vibe_coding-interim-yellow)](#-disclaimer)

**UnionSupport** 是一套基于 C# Source Generator 的**运行时和类型**（Discriminated Union）实现。在当前 C# 语言尚未原生支持 `struct union` / `union struct` 的过渡阶段，通过编译期代码生成提供三种联合体实现策略。

> **Stop-Gap Solution**: 本项目是面向未来的**过渡方案**。一旦 .NET 官方完成 `struct union` 原生支持，将编写 **Code Fix** 协助项目无缝迁移至官方语法。

---

## 项目结构

```
UnionSupport.slnx
├── src/
│   ├── UnionSupport.Core/              核心类型: IUnion, [UnionImpl], UnionAttribute
│   ├── UnionSupport.Generator.Shared/   共享代码生成逻辑
│   ├── UnionSupport.Generator.Product/  策略1: 积类型模拟
│   ├── UnionSupport.Generator.Unmanaged/策略2: FieldOffset
│   ├── UnionSupport.Generator.Erasure/  策略3: Object 擦除
│   ├── UnionSupport.Analyzer/           编译期分析器 (UNION001-003)
│   └── UnionSupport.Type/              预生成泛型联合体 1..17 参数
├── tests/                              xUnit + Verify 快照测试
└── demo/ConsoleApp1/                   使用示例
```

## NuGet 包

| 包名 | 说明                           |
|------|------------------------------|
| `UnionSupport.Core` | 核心类型定义 (必需)                  |
| `UnionSupport.Generator.Product` | 积类型模拟实现生成器                   |
| `UnionSupport.Generator.Unmanaged` | FieldOffset 原生和类型C样式Union实现生成器 |
| `UnionSupport.Generator.Erasure` | Object 类型擦除实现生成器             |
| `UnionSupport.Analyzer` | 编译期分析器                       |
| `UnionSupport.Type` | 预生成 1-17 泛型参数的联合体类型，无需导入源生成器 |

## 快速开始

### 自定义联合体

```csharp
using UnionSupport;

// 策略1: 积类型模拟 (默认，适用最广)
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct MyUnion(int a, float b, string c);

// 策略2: FieldOffset (高性能，仅 unmanaged 类型)
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct IntOrFloat(int a, float b);

// 策略3: Object 擦除 (C# 原生方案，单字段)
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct AnyValue(int a, string b);

// ref struct (仅支持 Product 策略)
[UnionImpl(UnionImplementationStrategy.Product)]
ref partial struct SpanUnion(int a, float b);
```

### 使用生成的类型

```csharp
// 隐式转换
MyUnion x = 42;
IntOrFloat y = 3.14f;
AnyValue z = "hello";

// pattern matching (C# 编译器自动解包 IUnion.Value)
switch (x)
{
    case int i:  Console.WriteLine($"int: {i}"); break;
    case float f: Console.WriteLine($"float: {f}"); break;
    case string s: Console.WriteLine($"string: {s}"); break;
}

// TryGetValue 访问
if (x.TryGetValue(out int iv))
    Console.WriteLine($"got int: {iv}");

// 直接访问 Value 属性
Console.WriteLine(x.Value);
Console.WriteLine(x.HasValue);
```

### 使用预生成泛型类型

```csharp
// 引用 UnionSupport.Type 包后即可使用
Union<int, float, string> u = 42;
CUnion<int, float> cu = 3.14f;      // where T : unmanaged
BoxedUnion<int, string> bu = "hi";   // Object 擦除

switch (u)
{
    case int i: ... break;
    case float f: ... break;
    case string s: ... break;
}
```

## 三种策略对比

| | Product | Unmanaged | ObjectErasure |
|---|---|---|---|
| 存储 | 独立字段 + byte 标志 | FieldOffset 重叠 | 单 object? 字段 |
| 内存 | struct 大小 = 字段和 | 最大字段 + 1 byte | object 引用 |
| 装箱 | 无 | 无 | 值类型装箱 |
| 泛型约束 | 无 | `T : unmanaged` | 无 |
| 适用场景 | 通用 | 高性能/互操作 | 简单引用类型混合 |
| ref struct | ✅ | ❌ | ❌ |

## 分析器规则

| ID | 规则 |
|----|------|
| **UNION001** | 类型重复: `(int, int)` / `(T, T)` |
| **UNION002** | ref struct 约束: 必须用 Product / 成员不能放普通 struct |
| **UNION003** | Unmanaged 策略要求 managed 类型 (无引用类型字段) |

---

## ⚠️ Disclaimer

> **本项目是 Vibe Coding 产物，是 .NET 官方 `struct union` 的过渡方案。**

C# 语言团队已在 [csharplang/proposals/unions.md](https://github.com/dotnet/csharplang/blob/main/proposals/unions.md) 中制定了完整的联合体语言规范。一旦 .NET 官方完成编译器的 `struct union` / `union struct` 原生实现，我们将：

1. 编写 **Code Fix** 将 `[UnionImpl(Product)] partial struct Foo(...)` 无缝迁移为 `union struct Foo(...)` 我们已实现一个小demo用作迁移union struct和struct union，我们尚不清楚官方的语法会如何，但是我们已经做好了准备
3. 消除编译期代码生成开销，直接使用编译器原生支持
4. 提供迁移工具链帮助现有项目平滑升级

**欢迎参与迁移贡献。** 如果你对此项目有建议或希望在 .NET 官方实现后协助维护 Code Fix，请参与 [Issues](https://github.com/anomalyco/UnionSupport/issues) 讨论。

## License

MIT
