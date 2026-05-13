// Polyfills for netstandard2.0 to support C# 12+ features

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}

namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Constructor)]
    internal sealed class SetsRequiredMembersAttribute : Attribute { }
}
