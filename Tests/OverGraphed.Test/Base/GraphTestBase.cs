using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Events;
using NUnit.Framework;
using OverGraphed.Test.Utils;

namespace OverGraphed.Test.Base
{
    public abstract class GraphTestBase
    {
        protected abstract IGraph<ITestVertex, ITestEdge> GetObservedGraph(Graph<ITestVertex, ITestEdge> graph);

        [Test]
        public void ConstructorTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            IGraph<ITestVertex, ITestEdge> observedGraph = GetObservedGraph(graph);

            graph.Vertices.Should().BeEmpty();
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().BeEmpty();
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().BeEmpty();
            observedGraph.Edges.Should().BeEmpty();

            (observedGraph as IDisposable)?.Dispose();
        }

        [Test]
        public void RegisterVertexTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            IGraph<ITestVertex, ITestEdge> observedGraph = GetObservedGraph(graph);

            var firstVertex = new TestVertex();
            var secondVertex = new TestVertex();

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.RegisterVertex(firstVertex).Should().BeTrue();

                monitor.Should().Raise(nameof(IGraph.VertexAdded)).WithArgs(firstVertex);
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));

                baseMonitor.Should().Raise(nameof(IGraph.VertexAdded)).WithArgs(firstVertex);
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().ContainOnly(firstVertex);
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().ContainOnly(firstVertex);
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().ContainOnly(firstVertex);
            observedGraph.Edges.Should().BeEmpty();

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.RegisterVertex(secondVertex).Should().BeTrue();
                
                monitor.Should().Raise(nameof(IGraph.VertexAdded)).WithArgs(secondVertex);
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                
                baseMonitor.Should().Raise(nameof(IGraph.VertexAdded)).WithArgs(secondVertex);
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().ContainOnly(firstVertex, secondVertex);
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            observedGraph.Edges.Should().BeEmpty();

            (observedGraph as IDisposable)?.Dispose();
        }

        [Test]
        public void RegisterVertexNullTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            graph.Invoking(x => x.RegisterVertex(null)).Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void AlreadyRegisteredVertexTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            IGraph<ITestVertex, ITestEdge> observedGraph = GetObservedGraph(graph);

            var vertex = new TestVertex();
            graph.RegisterVertex(vertex);

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.RegisterVertex(vertex).Should().BeFalse();

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().ContainOnlyAndInOrder(vertex);
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().ContainOnlyAndInOrder(vertex);
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().ContainOnlyAndInOrder(vertex);
            observedGraph.Edges.Should().BeEmpty();

            (observedGraph as IDisposable)?.Dispose();
        }

        [Test]
        public void UnregisterVertexTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            IGraph<ITestVertex, ITestEdge> observedGraph = GetObservedGraph(graph);

            var firstVertex = new TestVertex();
            var secondVertex = new TestVertex();

            graph.RegisterVertex(firstVertex);
            graph.RegisterVertex(secondVertex);

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.UnregisterVertex(firstVertex).Should().BeTrue();
                
                monitor.Should().Raise(nameof(IGraph.VertexRemoved)).WithArgs(firstVertex);

                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                
                baseMonitor.Should().Raise(nameof(IGraph.VertexRemoved)).WithArgs(firstVertex);

                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().ContainOnly(secondVertex);
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().ContainOnly(secondVertex);
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().ContainOnly(secondVertex);
            observedGraph.Edges.Should().BeEmpty();

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.UnregisterVertex(secondVertex).Should().BeTrue();

                monitor.Should().Raise(nameof(IGraph.VertexRemoved)).WithArgs(secondVertex);

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));

                baseMonitor.Should().Raise(nameof(IGraph.VertexRemoved)).WithArgs(secondVertex);

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().BeEmpty();
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().BeEmpty();
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().BeEmpty();
            observedGraph.Edges.Should().BeEmpty();

            (observedGraph as IDisposable)?.Dispose();
        }

        [Test]
        public void UnregisterVertexNullTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            graph.Invoking(x => x.UnregisterVertex(null)).Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void AlreadyUnregisteredVertexTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            IGraph<ITestVertex, ITestEdge> observedGraph = GetObservedGraph(graph);

            var vertex = new TestVertex();
            graph.RegisterVertex(vertex);
            graph.UnregisterVertex(vertex);

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.UnregisterVertex(vertex).Should().BeFalse();

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().BeEmpty();
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().BeEmpty();
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().BeEmpty();
            observedGraph.Edges.Should().BeEmpty();

            (observedGraph as IDisposable)?.Dispose();
        }

        [Test]
        public void UnregisteredNotRegisteredVertexTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            IGraph<ITestVertex, ITestEdge> observedGraph = GetObservedGraph(graph);

            var vertex = new TestVertex();

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.UnregisterVertex(vertex).Should().BeFalse();

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().BeEmpty();
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().BeEmpty();
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().BeEmpty();
            observedGraph.Edges.Should().BeEmpty();

            (observedGraph as IDisposable)?.Dispose();
        }

        [Test]
        public void RegisterEdgeTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            IGraph<ITestVertex, ITestEdge> observedGraph = GetObservedGraph(graph);

            var firstVertex = new TestVertex();
            var secondVertex = new TestVertex();

            var firstToSecondEdge = new TestEdge();
            var secondToFirstEdge = new TestEdge();

            firstToSecondEdge.Link(firstVertex, secondVertex);
            secondToFirstEdge.Link(secondVertex, firstVertex);

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.RegisterVertex(firstVertex).Should().BeTrue();
                graph.RegisterVertex(secondVertex).Should().BeTrue();

                monitor.Should().Raise(nameof(IGraph.VertexAdded)).WithArgs(firstVertex, secondVertex);

                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));

                baseMonitor.Should().Raise(nameof(IGraph.VertexAdded)).WithArgs(firstVertex, secondVertex);

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().ContainOnly(firstVertex, secondVertex);
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            observedGraph.Edges.Should().BeEmpty();

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.RegisterEdge(firstToSecondEdge).Should().BeTrue();

                monitor.Should().Raise(nameof(IGraph.EdgeAdded)).WithArgs(firstToSecondEdge);

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));

                baseMonitor.Should().Raise(nameof(IGraph.EdgeAdded)).WithArgs(firstToSecondEdge);

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            graph.Edges.Should().ContainOnly(firstToSecondEdge);

            ((IGraph)observedGraph).Vertices.Should().ContainOnly(firstVertex, secondVertex);
            ((IGraph)observedGraph).Edges.Should().ContainOnly(firstToSecondEdge);
            observedGraph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            observedGraph.Edges.Should().ContainOnly(firstToSecondEdge);

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.RegisterEdge(secondToFirstEdge).Should().BeTrue();

                monitor.Should().Raise(nameof(IGraph.EdgeAdded)).WithArgs(secondToFirstEdge);

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));

                baseMonitor.Should().Raise(nameof(IGraph.EdgeAdded)).WithArgs(secondToFirstEdge);

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            graph.Edges.Should().ContainOnly(firstToSecondEdge, secondToFirstEdge);

            ((IGraph)observedGraph).Vertices.Should().ContainOnly(firstVertex, secondVertex);
            ((IGraph)observedGraph).Edges.Should().ContainOnly(firstToSecondEdge, secondToFirstEdge);
            observedGraph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            observedGraph.Edges.Should().ContainOnly(firstToSecondEdge, secondToFirstEdge);

            (observedGraph as IDisposable)?.Dispose();
        }

        [Test]
        public void RegisterEdgeNullTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            graph.Invoking(x => x.RegisterEdge(null)).Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void AlreadyRegisteredEdgeTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            IGraph<ITestVertex, ITestEdge> observedGraph = GetObservedGraph(graph);

            var firstVertex = new TestVertex();
            var secondVertex = new TestVertex();
            var edge = new TestEdge();

            edge.Link(firstVertex, secondVertex);

            graph.RegisterVertex(firstVertex);
            graph.RegisterVertex(secondVertex);
            graph.RegisterEdge(edge);

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.RegisterEdge(edge).Should().BeFalse();

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            graph.Edges.Should().ContainOnly(edge);

            ((IGraph)observedGraph).Vertices.Should().ContainOnly(firstVertex, secondVertex);
            ((IGraph)observedGraph).Edges.Should().ContainOnly(edge);
            observedGraph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            observedGraph.Edges.Should().ContainOnly(edge);

            (observedGraph as IDisposable)?.Dispose();
        }

        [Test]
        public void UnregisterEdgeTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            IGraph<ITestVertex, ITestEdge> observedGraph = GetObservedGraph(graph);

            var firstVertex = new TestVertex();
            var secondVertex = new TestVertex();

            var firstToSecondEdge = new TestEdge();
            var secondToFirstEdge = new TestEdge();

            firstToSecondEdge.Link(firstVertex, secondVertex);
            secondToFirstEdge.Link(secondVertex, firstVertex);

            graph.RegisterVertex(firstVertex).Should().BeTrue();
            graph.RegisterVertex(secondVertex).Should().BeTrue();
            graph.RegisterEdge(firstToSecondEdge).Should().BeTrue();
            graph.RegisterEdge(secondToFirstEdge).Should().BeTrue();

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.UnregisterEdge(firstToSecondEdge).Should().BeTrue();

                monitor.Should().Raise(nameof(IGraph.EdgeRemoved)).WithArgs(firstToSecondEdge);

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));

                baseMonitor.Should().Raise(nameof(IGraph.EdgeRemoved)).WithArgs(firstToSecondEdge);

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            graph.Edges.Should().ContainOnly(secondToFirstEdge);

            ((IGraph)observedGraph).Vertices.Should().ContainOnly(firstVertex, secondVertex);
            ((IGraph)observedGraph).Edges.Should().ContainOnly(secondToFirstEdge);
            observedGraph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            observedGraph.Edges.Should().ContainOnly(secondToFirstEdge);

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.UnregisterEdge(secondToFirstEdge).Should().BeTrue();

                monitor.Should().Raise(nameof(IGraph.EdgeRemoved)).WithArgs(secondToFirstEdge);

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));

                baseMonitor.Should().Raise(nameof(IGraph.EdgeRemoved)).WithArgs(secondToFirstEdge);

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().ContainOnly(firstVertex, secondVertex);
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            observedGraph.Edges.Should().BeEmpty();

            (observedGraph as IDisposable)?.Dispose();
        }

        [Test]
        public void UnregisterEdgeNullTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            graph.Invoking(x => x.UnregisterEdge(null)).Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void AlreadyUnregisteredEdgeTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            IGraph<ITestVertex, ITestEdge> observedGraph = GetObservedGraph(graph);

            var firstVertex = new TestVertex();
            var secondVertex = new TestVertex();
            var edge = new TestEdge();

            graph.RegisterVertex(firstVertex);
            graph.RegisterVertex(secondVertex);
            graph.RegisterEdge(edge);
            graph.UnregisterEdge(edge);

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.UnregisterEdge(edge).Should().BeFalse();

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().ContainOnly(firstVertex, secondVertex);
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            observedGraph.Edges.Should().BeEmpty();

            (observedGraph as IDisposable)?.Dispose();
        }

        [Test]
        public void UnregisteredNotRegisteredEdgeTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            IGraph<ITestVertex, ITestEdge> observedGraph = GetObservedGraph(graph);

            var firstVertex = new TestVertex();
            var secondVertex = new TestVertex();
            var edge = new TestEdge();

            graph.RegisterVertex(firstVertex);
            graph.RegisterVertex(secondVertex);

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.UnregisterEdge(edge).Should().BeFalse();

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().ContainOnly(firstVertex, secondVertex);
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().ContainOnly(firstVertex, secondVertex);
            observedGraph.Edges.Should().BeEmpty();

            (observedGraph as IDisposable)?.Dispose();
        }

        [Test]
        public void RegisteredEdgeOnNotRegisteredVertexTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            IGraph<ITestVertex, ITestEdge> observedGraph = GetObservedGraph(graph);

            var firstVertex = new TestVertex();
            var secondVertex = new TestVertex();

            var edge = new TestEdge();
            edge.Link(firstVertex, secondVertex);

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.RegisterEdge(edge).Should().BeFalse();

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().BeEmpty();
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().BeEmpty();
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().BeEmpty();
            observedGraph.Edges.Should().BeEmpty();

            graph.RegisterVertex(firstVertex);

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.RegisterEdge(edge).Should().BeFalse();

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().ContainOnly(firstVertex);
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().ContainOnly(firstVertex);
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().ContainOnly(firstVertex);
            observedGraph.Edges.Should().BeEmpty();

            graph.UnregisterVertex(firstVertex);
            graph.RegisterVertex(secondVertex);

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.RegisterEdge(edge).Should().BeFalse();

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.Cleared));
            }

            graph.Vertices.Should().ContainOnly(secondVertex);
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().ContainOnly(secondVertex);
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().ContainOnly(secondVertex);
            observedGraph.Edges.Should().BeEmpty();

            (observedGraph as IDisposable)?.Dispose();
        }

        [Test]
        public void ClearTest()
        {
            var graph = new Graph<ITestVertex, ITestEdge>();
            IGraph<ITestVertex, ITestEdge> observedGraph = GetObservedGraph(graph);

            var firstVertex = new TestVertex();
            var secondVertex = new TestVertex();

            var edge = new TestEdge();
            edge.Link(firstVertex, secondVertex);

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.Clear();

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
                
                baseMonitor.Should().Raise(nameof(IGraph.Cleared));

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.VertexRemoved));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeRemoved));
            }

            graph.Vertices.Should().BeEmpty();
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().BeEmpty();
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().BeEmpty();
            observedGraph.Edges.Should().BeEmpty();

            graph.RegisterVertex(firstVertex);
            graph.RegisterVertex(secondVertex);
            graph.RegisterEdge(edge);

            using (IMonitor<IGraph<ITestVertex, ITestEdge>> monitor = observedGraph.Monitor())
            using (IMonitor<IGraph> baseMonitor = observedGraph.Monitor<IGraph>())
            {
                graph.Clear();

                monitor.Should().Raise(nameof(IGraph.VertexRemoved)).WithArgs(firstVertex, secondVertex);
                monitor.Should().Raise(nameof(IGraph.EdgeRemoved)).WithArgs(edge);

                monitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                monitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
                
                baseMonitor.Should().Raise(nameof(IGraph.VertexRemoved)).WithArgs(firstVertex, secondVertex);
                baseMonitor.Should().Raise(nameof(IGraph.EdgeRemoved)).WithArgs(edge);
                baseMonitor.Should().Raise(nameof(IGraph.Cleared));
                baseMonitor.OccurredEvents.First().EventName.Should().Be(nameof(IGraph.Cleared));

                baseMonitor.Should().NotRaise(nameof(IGraph.VertexAdded));
                baseMonitor.Should().NotRaise(nameof(IGraph.EdgeAdded));
            }

            graph.Vertices.Should().BeEmpty();
            graph.Edges.Should().BeEmpty();

            ((IGraph)observedGraph).Vertices.Should().BeEmpty();
            ((IGraph)observedGraph).Edges.Should().BeEmpty();
            observedGraph.Vertices.Should().BeEmpty();
            observedGraph.Edges.Should().BeEmpty();
        }

        private class TestVertex : SimpleDirectedVertex<ITestVertex, ITestEdge>, ITestVertex
        {
        }

        private class TestEdge : Edge<ITestVertex, ITestEdge>, ITestEdge
        {
        }
    }
}