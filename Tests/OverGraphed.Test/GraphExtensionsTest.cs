using FluentAssertions;
using NUnit.Framework;
using OverGraphed.Test.Utils;

namespace OverGraphed.Test
{
    public class GraphExtensionsTest
    {
        [Test]
        public void ContainsLinkTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();

            var firstVertex = new TestVertex();
            var secondVertex = new TestVertex();
            var edge = new TestEdge();
            edge.Link(firstVertex, secondVertex);

            graph.RegisterVertex(firstVertex);
            graph.RegisterVertex(secondVertex);

            ((IGraph)graph).ContainsLink(firstVertex, secondVertex).Should().BeFalse();
            graph.ContainsLink(firstVertex, secondVertex).Should().BeFalse();

            graph.RegisterEdge(edge);
            
            ((IGraph)graph).ContainsLink(firstVertex, secondVertex).Should().BeTrue();
            graph.ContainsLink(firstVertex, secondVertex).Should().BeTrue();
            
            graph.UnregisterEdge(edge);

            ((IGraph)graph).ContainsLink(firstVertex, secondVertex).Should().BeFalse();
            graph.ContainsLink(firstVertex, secondVertex).Should().BeFalse();
        }

        [Test]
        public void ContainsLinkWithOutTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();

            var firstVertex = new TestVertex();
            var secondVertex = new TestVertex();
            var edge = new TestEdge();
            edge.Link(firstVertex, secondVertex);

            graph.RegisterVertex(firstVertex);
            graph.RegisterVertex(secondVertex);

            graph.ContainsLink(firstVertex, secondVertex, out IEdge linkedBaseEdge).Should().BeFalse();
            linkedBaseEdge.Should().BeNull();
            graph.ContainsLink(firstVertex, secondVertex, out ITestEdge linkedEdge).Should().BeFalse();
            linkedEdge.Should().BeNull();

            graph.RegisterEdge(edge);
            
            graph.ContainsLink(firstVertex, secondVertex, out linkedBaseEdge).Should().BeTrue();
            linkedBaseEdge.Should().BeSameAs(edge);
            graph.ContainsLink(firstVertex, secondVertex, out linkedEdge).Should().BeTrue();
            linkedEdge.Should().BeSameAs(edge);
            
            graph.UnregisterEdge(edge);
            
            graph.ContainsLink(firstVertex, secondVertex, out linkedBaseEdge).Should().BeFalse();
            linkedBaseEdge.Should().BeNull();
            graph.ContainsLink(firstVertex, secondVertex, out linkedEdge).Should().BeFalse();
            linkedEdge.Should().BeNull();
        }

        private class TestVertex : SimpleDirectedVertex<ITestVertex, ITestEdge>, ITestVertex
        {
        }

        private class TestEdge : Edge<ITestVertex, ITestEdge>, ITestEdge
        {
        }
    }
}