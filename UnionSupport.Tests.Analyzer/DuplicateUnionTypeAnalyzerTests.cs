namespace UnionSupport.Tests.Analyzer;

public class DuplicateUnionTypeAnalyzerTests
{
    [Fact]
    public Task ConcreteDuplicate_IntInt()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Product)]
            partial struct MyUnion(int a, int b);
            """;
        return AnalyzerTestHelper.Verify(source);
    }

    [Fact]
    public Task TypeParamDuplicate_TT()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Product)]
            partial struct MyUnion<T>(T a, T b);
            """;
        return AnalyzerTestHelper.Verify(source);
    }

    [Fact]
    public Task NoDuplicate_IntFloat()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Product)]
            partial struct MyUnion(int a, float b);
            """;
        return AnalyzerTestHelper.Verify(source);
    }

    [Fact]
    public Task NoDuplicate_WithTypeParam()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Product)]
            partial struct MyUnion<T>(int a, float b, T c);
            """;
        return AnalyzerTestHelper.Verify(source);
    }

    [Fact]
    public Task ConcreteDuplicate_IntFloatInt()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Product)]
            partial struct MyUnion(int a, float b, int c);
            """;
        return AnalyzerTestHelper.Verify(source);
    }
}
