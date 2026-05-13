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

    [Fact]
    public Task RefStruct_Unmanaged_Error()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            ref partial struct BadRef(int a, float b);
            """;
        return AnalyzerTestHelper.Verify(source);
    }

    [Fact]
    public Task RefStruct_Erasure_Error()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            ref partial struct BadRef(int a, string b);
            """;
        return AnalyzerTestHelper.Verify(source);
    }

    [Fact]
    public Task RefStruct_Product_Ok()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Product)]
            ref partial struct GoodRef(int a, float b);
            """;
        return AnalyzerTestHelper.Verify(source);
    }

    [Fact]
    public Task NonRefStruct_WithRefStructMember_Error()
    {
        var source = """
            using UnionSupport;
            using System;
            [UnionImpl(UnionImplementationStrategy.Product)]
            partial struct BadUnion(int a, Span<byte> b);
            """;
        return AnalyzerTestHelper.Verify(source);
    }

    [Fact]
    public Task Unmanaged_RefType_Error()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct BadUnmanaged(int a, string b);
            """;
        return AnalyzerTestHelper.Verify(source);
    }

    [Fact]
    public Task Unmanaged_TypeParam_NoConstraint_Error()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct BadUnmanaged<T>(int a, T b);
            """;
        return AnalyzerTestHelper.Verify(source);
    }

    [Fact]
    public Task Unmanaged_TypeParam_WithConstraint_Ok()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct GoodUnmanaged<T>(int a, T b) where T : unmanaged;
            """;
        return AnalyzerTestHelper.Verify(source);
    }

    [Fact]
    public Task Unmanaged_AllUnmanagedTypes_Ok()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct GoodUnmanaged(int a, float b, long c);
            """;
        return AnalyzerTestHelper.Verify(source);
    }
}
