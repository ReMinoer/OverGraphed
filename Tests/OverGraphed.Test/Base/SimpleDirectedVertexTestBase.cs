using FluentAssertions;
using NUnit.Framework;
using OverGraphed.Test.Utils;

namespace OverGraphed.Test.Base
{
    public abstract class SimpleDirectedVertexTestBase : VertexTestBase
    {
        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void AlreadyLinkedTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex end = GetVertex(type);

            ITestEdge edge = GetEdge(type);
            ITestEdge reverseEdge = GetEdge(type);
            edge.Link(start, end);
            reverseEdge.Link(end, start);

            ITestEdge otherEdge = GetEdge(type);
            ITestEdge otherReverseEdge = GetEdge(type);
            otherEdge.Link(start, end).Should().BeFalse();
            otherReverseEdge.Link(end, start).Should().BeFalse();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void AlreadyLinkedOnChangeStartTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex end = GetVertex(type);
            ITestVertex other = GetVertex(type);

            ITestEdge edge = GetEdge(type);
            ITestEdge reverseEdge = GetEdge(type);
            edge.Link(start, end);
            reverseEdge.Link(end, start);

            ITestEdge secondEdge = GetEdge(type);
            ITestEdge secondReverseEdge = GetEdge(type);
            secondEdge.Link(other, end);
            secondReverseEdge.Link(other, start);

            secondEdge.ChangeStart(start).Should().BeFalse();
            secondReverseEdge.ChangeStart(end).Should().BeFalse();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void AlreadyLinkedOnChangeEndTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex end = GetVertex(type);
            ITestVertex other = GetVertex(type);

            ITestEdge edge = GetEdge(type);
            ITestEdge reverseEdge = GetEdge(type);
            edge.Link(start, end);
            reverseEdge.Link(end, start);

            ITestEdge secondEdge = GetEdge(type);
            ITestEdge secondReverseEdge = GetEdge(type);
            secondEdge.Link(start, other);
            secondReverseEdge.Link(end, other);

            secondEdge.ChangeEnd(end).Should().BeFalse();
            secondReverseEdge.ChangeEnd(start).Should().BeFalse();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void CanRegisterAlreadyLinkedTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex end = GetVertex(type);

            ITestEdge edge = GetEdge(type);
            ITestEdge reverseEdge = GetEdge(type);
            edge.Link(start, end);
            reverseEdge.Link(end, start);

            ITestEdge otherEdge = GetEdge(type);
            ITestEdge otherReverseEdge = GetEdge(type);

            start.CanRegisterEdge(otherEdge, start, end).Should().BeFalse();
            end.CanRegisterEdge(otherEdge, start, end).Should().BeFalse();
            start.CanRegisterEdge(otherReverseEdge, end, start).Should().BeFalse();
            end.CanRegisterEdge(otherReverseEdge, end, start).Should().BeFalse();
        }
    }
}