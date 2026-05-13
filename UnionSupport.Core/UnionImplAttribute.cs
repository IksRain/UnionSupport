namespace UnionSupport;

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false)]
public sealed class UnionImplAttribute : Attribute
{
    public UnionImplementationStrategy Strategy { get; }

    public UnionImplAttribute(UnionImplementationStrategy strategy)
    {
        Strategy = strategy;
    }
}
