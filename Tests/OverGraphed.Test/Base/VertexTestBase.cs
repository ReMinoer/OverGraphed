using System;
using FluentAssertions;
using FluentAssertions.Events;
using NUnit.Framework;
using OverGraphed.Test.Utils;

namespace OverGraphed.Test.Base
{
    [TestFixture]
    public abstract class VertexTestBase
    {
        protected abstract ITestVertex GetVertex(ImplementationType type, bool refuseAllRegistration = false);
        protected abstract void InvalidVertexConstructor();

        protected ITestEdge GetEdge(ImplementationType type, bool refuseAllLinks = false)
        {
            switch (type)
            {
                case ImplementationType.Inherited:
                    return new TestEdgeInherited(refuseAllLinks);
                case ImplementationType.Reimplemented:
                    return new TestEdgeReimplemented(refuseAllLinks);
                default:
                    throw new NotSupportedException();
            }
        }

        protected ITestEdge GetEdgeMock(ITestVertex start, ITestVertex end)
        {
            return new TestEdgeMock(start, end);
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void ConstructorTest(ImplementationType type)
        {
            ITestVertex vertex = GetVertex(type);
            
            vertex.Edges.Should().BeEmpty();
            vertex.Predecessors.Should().BeEmpty();
            vertex.Successors.Should().BeEmpty();
        }
        
        [Test]
        public void InvalidClassConstructorTest()
        {
            Action constructor = InvalidVertexConstructor;
            constructor.Should().Throw<InvalidOperationException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void LinkTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex middle = GetVertex(type);
            ITestVertex end = GetVertex(type);

            ITestEdge startToMiddleEdge = GetEdge(type);
            ITestEdge middleToEndEdge = GetEdge(type);

            using (IMonitor<IVertex<ITestVertex, ITestEdge>> startMonitor = start.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex<ITestVertex, ITestEdge>> middleMonitor = middle.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex<ITestVertex, ITestEdge>> endMonitor = end.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex> baseStartMonitor = start.Monitor<IVertex>())
            using (IMonitor<IVertex> baseMiddleMonitor = middle.Monitor<IVertex>())
            using (IMonitor<IVertex> baseEndMonitor = end.Monitor<IVertex>())
            {
                startToMiddleEdge.Link(start, middle).Should().BeTrue();

                startMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded)).WithArgs(startToMiddleEdge);
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved));
                startMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded)).WithArgs(startToMiddleEdge);
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved));

                middleMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded)).WithArgs(startToMiddleEdge);
                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved));
                middleMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded)).WithArgs(startToMiddleEdge);
                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved));
                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded));
                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved));

                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved));

                baseStartMonitor.Should().Raise(nameof(IVertex.EdgeAdded)).WithArgs(startToMiddleEdge);
                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgeRemoved));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseStartMonitor.Should().Raise(nameof(IVertex.SuccessorAdded)).WithArgs(startToMiddleEdge);
                baseStartMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));

                baseMiddleMonitor.Should().Raise(nameof(IVertex.EdgeAdded)).WithArgs(startToMiddleEdge);
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.EdgeRemoved));
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseMiddleMonitor.Should().Raise(nameof(IVertex.PredecessorAdded)).WithArgs(startToMiddleEdge);
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));

                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgeRemoved));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));
            }

            start.Edges.Should().ContainOnlyAndInOrder(startToMiddleEdge);
            start.Predecessors.Should().BeEmpty();
            start.Successors.Should().ContainOnlyAndInOrder(startToMiddleEdge);
            ((IVertex)start).Edges.Should().ContainOnlyAndInOrder(startToMiddleEdge);
            ((IVertex)start).Predecessors.Should().BeEmpty();
            ((IVertex)start).Successors.Should().ContainOnlyAndInOrder(startToMiddleEdge);

            middle.Edges.Should().ContainOnlyAndInOrder(startToMiddleEdge);
            middle.Predecessors.Should().ContainOnlyAndInOrder(startToMiddleEdge);
            middle.Successors.Should().BeEmpty();
            ((IVertex)middle).Edges.Should().ContainOnlyAndInOrder(startToMiddleEdge);
            ((IVertex)middle).Predecessors.Should().ContainOnlyAndInOrder(startToMiddleEdge);
            ((IVertex)middle).Successors.Should().BeEmpty();

            end.Edges.Should().BeEmpty();
            end.Predecessors.Should().BeEmpty();
            end.Successors.Should().BeEmpty();
            ((IVertex)end).Edges.Should().BeEmpty();
            ((IVertex)end).Predecessors.Should().BeEmpty();
            ((IVertex)end).Successors.Should().BeEmpty();

            startToMiddleEdge.Start.Should().Be(start);
            startToMiddleEdge.End.Should().Be(middle);
            ((IEdge)startToMiddleEdge).Start.Should().Be(start);
            ((IEdge)startToMiddleEdge).End.Should().Be(middle);

            middleToEndEdge.Start.Should().BeNull();
            middleToEndEdge.End.Should().BeNull();
            ((IEdge)middleToEndEdge).Start.Should().BeNull();
            ((IEdge)middleToEndEdge).End.Should().BeNull();

            using (IMonitor<IVertex<ITestVertex, ITestEdge>> startMonitor = start.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex<ITestVertex, ITestEdge>> middleMonitor = middle.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex<ITestVertex, ITestEdge>> endMonitor = end.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex> baseStartMonitor = start.Monitor<IVertex>())
            using (IMonitor<IVertex> baseMiddleMonitor = middle.Monitor<IVertex>())
            using (IMonitor<IVertex> baseEndMonitor = end.Monitor<IVertex>())
            {
                middleToEndEdge.Link(middle, end).Should().BeTrue();

                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved));

                middleMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded)).WithArgs(middleToEndEdge);
                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved));
                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded));
                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved));
                middleMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded)).WithArgs(middleToEndEdge);
                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved));

                endMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded)).WithArgs(middleToEndEdge);
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved));
                endMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded)).WithArgs(middleToEndEdge);
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved));

                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgeRemoved));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));

                baseMiddleMonitor.Should().Raise(nameof(IVertex.EdgeAdded)).WithArgs(middleToEndEdge);
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.EdgeRemoved));
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseMiddleMonitor.Should().Raise(nameof(IVertex.SuccessorAdded)).WithArgs(middleToEndEdge);
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));

                baseEndMonitor.Should().Raise(nameof(IVertex.EdgeAdded)).WithArgs(middleToEndEdge);
                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgeRemoved));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseEndMonitor.Should().Raise(nameof(IVertex.PredecessorAdded)).WithArgs(middleToEndEdge);
                baseEndMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));
            }

            start.Edges.Should().ContainOnlyAndInOrder(startToMiddleEdge);
            start.Predecessors.Should().BeEmpty();
            start.Successors.Should().ContainOnlyAndInOrder(startToMiddleEdge);
            ((IVertex)start).Edges.Should().ContainOnlyAndInOrder(startToMiddleEdge);
            ((IVertex)start).Predecessors.Should().BeEmpty();
            ((IVertex)start).Successors.Should().ContainOnlyAndInOrder(startToMiddleEdge);

            middle.Edges.Should().ContainOnlyAndInOrder(startToMiddleEdge, middleToEndEdge);
            middle.Predecessors.Should().ContainOnlyAndInOrder(startToMiddleEdge);
            middle.Successors.Should().ContainOnlyAndInOrder(middleToEndEdge);
            ((IVertex)middle).Edges.Should().ContainOnlyAndInOrder(startToMiddleEdge, middleToEndEdge);
            ((IVertex)middle).Predecessors.Should().ContainOnlyAndInOrder(startToMiddleEdge);
            ((IVertex)middle).Successors.Should().ContainOnlyAndInOrder(middleToEndEdge);

            end.Edges.Should().ContainOnlyAndInOrder(middleToEndEdge);
            end.Predecessors.Should().ContainOnlyAndInOrder(middleToEndEdge);
            end.Successors.Should().BeEmpty();
            ((IVertex)end).Edges.Should().ContainOnlyAndInOrder(middleToEndEdge);
            ((IVertex)end).Predecessors.Should().ContainOnlyAndInOrder(middleToEndEdge);
            ((IVertex)end).Successors.Should().BeEmpty();

            startToMiddleEdge.Start.Should().Be(start);
            startToMiddleEdge.End.Should().Be(middle);
            ((IEdge)startToMiddleEdge).Start.Should().Be(start);
            ((IEdge)startToMiddleEdge).End.Should().Be(middle);

            middleToEndEdge.Start.Should().Be(middle);
            middleToEndEdge.End.Should().Be(end);
            ((IEdge)middleToEndEdge).Start.Should().Be(middle);
            ((IEdge)middleToEndEdge).End.Should().Be(end);
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void LinkNullVertexTest(ImplementationType type)
        {
            ITestVertex vertex = GetVertex(type);
            ITestEdge edge = GetEdge(type);

            edge.Invoking(x => x.Link(null, null)).Should().Throw<ArgumentNullException>();
            edge.Invoking(x => x.Link(vertex, null)).Should().Throw<ArgumentNullException>();
            edge.Invoking(x => x.Link(null, vertex)).Should().Throw<ArgumentNullException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void LinkNotLinkableVerticeTest(ImplementationType type)
        {
            ITestVertex vertex = GetVertex(type);
            ITestVertex notLinkableVertex = GetVertex(type, refuseAllRegistration: true);
            ITestEdge edge = GetEdge(type);

            edge.Link(vertex, notLinkableVertex).Should().BeFalse();
            edge.Link(notLinkableVertex, vertex).Should().BeFalse();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void LinkNotLinkableEdgeTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex end = GetVertex(type);
            ITestEdge edge = GetEdge(type, refuseAllLinks: true);

            edge.Link(start, end).Should().BeFalse();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void UnlinkTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex middle = GetVertex(type);
            ITestVertex end = GetVertex(type);

            ITestEdge startToMiddleEdge = GetEdge(type);
            ITestEdge middleToEndEdge = GetEdge(type);

            startToMiddleEdge.Link(start, middle);
            middleToEndEdge.Link(middle, end);

            using (IMonitor<IVertex<ITestVertex, ITestEdge>> startMonitor = start.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex<ITestVertex, ITestEdge>> middleMonitor = middle.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex<ITestVertex, ITestEdge>> endMonitor = end.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex> baseStartMonitor = start.Monitor<IVertex>())
            using (IMonitor<IVertex> baseMiddleMonitor = middle.Monitor<IVertex>())
            using (IMonitor<IVertex> baseEndMonitor = end.Monitor<IVertex>())
            {
                startToMiddleEdge.Unlink();

                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded));
                startMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved)).WithArgs(startToMiddleEdge);
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded));
                startMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved)).WithArgs(startToMiddleEdge);

                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded));
                middleMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved)).WithArgs(startToMiddleEdge);
                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded));
                middleMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved)).WithArgs(startToMiddleEdge);
                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded));
                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved));

                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved));

                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                baseStartMonitor.Should().Raise(nameof(IVertex.EdgeRemoved)).WithArgs(startToMiddleEdge);
                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseStartMonitor.Should().Raise(nameof(IVertex.SuccessorRemoved)).WithArgs(startToMiddleEdge);

                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                baseMiddleMonitor.Should().Raise(nameof(IVertex.EdgeRemoved)).WithArgs(startToMiddleEdge);
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseMiddleMonitor.Should().Raise(nameof(IVertex.PredecessorRemoved)).WithArgs(startToMiddleEdge);
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));

                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgeRemoved));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));
            }

            start.Edges.Should().BeEmpty();
            start.Predecessors.Should().BeEmpty();
            start.Successors.Should().BeEmpty();
            ((IVertex)start).Edges.Should().BeEmpty();
            ((IVertex)start).Predecessors.Should().BeEmpty();
            ((IVertex)start).Successors.Should().BeEmpty();

            middle.Edges.Should().ContainOnlyAndInOrder(middleToEndEdge);
            middle.Predecessors.Should().BeEmpty();
            middle.Successors.Should().ContainOnlyAndInOrder(middleToEndEdge);
            ((IVertex)middle).Edges.Should().ContainOnlyAndInOrder(middleToEndEdge);
            ((IVertex)middle).Predecessors.Should().BeEmpty();
            ((IVertex)middle).Successors.Should().ContainOnlyAndInOrder(middleToEndEdge);

            end.Edges.Should().ContainOnlyAndInOrder(middleToEndEdge);
            end.Predecessors.Should().ContainOnlyAndInOrder(middleToEndEdge);
            end.Successors.Should().BeEmpty();
            ((IVertex)end).Edges.Should().ContainOnlyAndInOrder(middleToEndEdge);
            ((IVertex)end).Predecessors.Should().ContainOnlyAndInOrder(middleToEndEdge);
            ((IVertex)end).Successors.Should().BeEmpty();

            startToMiddleEdge.Start.Should().BeNull();
            startToMiddleEdge.End.Should().BeNull();
            ((IEdge)startToMiddleEdge).Start.Should().BeNull();
            ((IEdge)startToMiddleEdge).End.Should().BeNull();

            middleToEndEdge.Start.Should().Be(middle);
            middleToEndEdge.End.Should().Be(end);
            ((IEdge)middleToEndEdge).Start.Should().Be(middle);
            ((IEdge)middleToEndEdge).End.Should().Be(end);

            using (IMonitor<IVertex<ITestVertex, ITestEdge>> startMonitor = start.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex<ITestVertex, ITestEdge>> middleMonitor = middle.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex<ITestVertex, ITestEdge>> endMonitor = end.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex> baseStartMonitor = start.Monitor<IVertex>())
            using (IMonitor<IVertex> baseMiddleMonitor = middle.Monitor<IVertex>())
            using (IMonitor<IVertex> baseEndMonitor = end.Monitor<IVertex>())
            {
                middleToEndEdge.Unlink();

                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved));

                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded));
                middleMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved)).WithArgs(middleToEndEdge);
                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded));
                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved));
                middleMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded));
                middleMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved)).WithArgs(middleToEndEdge);

                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded));
                endMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved)).WithArgs(middleToEndEdge);
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded));
                endMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved)).WithArgs(middleToEndEdge);
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved));

                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgeRemoved));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));

                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                baseMiddleMonitor.Should().Raise(nameof(IVertex.EdgeRemoved)).WithArgs(middleToEndEdge);
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseMiddleMonitor.Should().Raise(nameof(IVertex.SuccessorRemoved)).WithArgs(middleToEndEdge);

                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                baseEndMonitor.Should().Raise(nameof(IVertex.EdgeRemoved)).WithArgs(middleToEndEdge);
                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseEndMonitor.Should().Raise(nameof(IVertex.PredecessorRemoved)).WithArgs(middleToEndEdge);
                baseEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));
            }

            start.Edges.Should().BeEmpty();
            start.Predecessors.Should().BeEmpty();
            start.Successors.Should().BeEmpty();
            ((IVertex)start).Edges.Should().BeEmpty();
            ((IVertex)start).Predecessors.Should().BeEmpty();
            ((IVertex)start).Successors.Should().BeEmpty();

            middle.Edges.Should().BeEmpty();
            middle.Predecessors.Should().BeEmpty();
            middle.Successors.Should().BeEmpty();
            ((IVertex)middle).Edges.Should().BeEmpty();
            ((IVertex)middle).Predecessors.Should().BeEmpty();
            ((IVertex)middle).Successors.Should().BeEmpty();

            end.Edges.Should().BeEmpty();
            end.Predecessors.Should().BeEmpty();
            end.Successors.Should().BeEmpty();
            ((IVertex)end).Edges.Should().BeEmpty();
            ((IVertex)end).Predecessors.Should().BeEmpty();
            ((IVertex)end).Successors.Should().BeEmpty();

            startToMiddleEdge.Start.Should().BeNull();
            startToMiddleEdge.End.Should().BeNull();
            ((IEdge)startToMiddleEdge).Start.Should().BeNull();
            ((IEdge)startToMiddleEdge).End.Should().BeNull();

            middleToEndEdge.Start.Should().BeNull();
            middleToEndEdge.End.Should().BeNull();
            ((IEdge)middleToEndEdge).Start.Should().BeNull();
            ((IEdge)middleToEndEdge).End.Should().BeNull();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void UnlinkUnlinkedEdgeTest(ImplementationType type)
        {
            ITestEdge edge = GetEdge(type);
            edge.Invoking(x => x.Unlink()).Should().Throw<InvalidOperationException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void ChangeStartTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex end = GetVertex(type);
            ITestVertex newStart = GetVertex(type);

            ITestEdge edge = GetEdge(type);

            edge.Link(start, end);

            using (IMonitor<IVertex<ITestVertex, ITestEdge>> startMonitor = start.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex<ITestVertex, ITestEdge>> endMonitor = end.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex<ITestVertex, ITestEdge>> newStartMonitor = newStart.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex> baseStartMonitor = start.Monitor<IVertex>())
            using (IMonitor<IVertex> baseEndMonitor = end.Monitor<IVertex>())
            using (IMonitor<IVertex> baseNewStartMonitor = newStart.Monitor<IVertex>())
            {
                edge.ChangeStart(newStart).Should().BeTrue();

                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded));
                startMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved)).WithArgs(edge);
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded));
                startMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved)).WithArgs(edge);

                endMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded)).WithArgs(edge);
                endMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved)).WithArgs(edge);
                endMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded)).WithArgs(edge);
                endMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved)).WithArgs(edge);
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved));

                newStartMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded)).WithArgs(edge);
                newStartMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved));
                newStartMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded));
                newStartMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved));
                newStartMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded)).WithArgs(edge);
                newStartMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved));

                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                baseStartMonitor.Should().Raise(nameof(IVertex.EdgeRemoved)).WithArgs(edge);
                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseStartMonitor.Should().Raise(nameof(IVertex.SuccessorRemoved)).WithArgs(edge);

                baseEndMonitor.Should().Raise(nameof(IVertex.EdgeAdded)).WithArgs(edge);
                baseEndMonitor.Should().Raise(nameof(IVertex.EdgeRemoved)).WithArgs(edge);
                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseEndMonitor.Should().Raise(nameof(IVertex.PredecessorAdded)).WithArgs(edge);
                baseEndMonitor.Should().Raise(nameof(IVertex.PredecessorRemoved)).WithArgs(edge);
                baseEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));

                baseNewStartMonitor.Should().Raise(nameof(IVertex.EdgeAdded)).WithArgs(edge);
                baseNewStartMonitor.Should().NotRaise(nameof(IVertex.EdgeRemoved));
                baseNewStartMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseNewStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseNewStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseNewStartMonitor.Should().Raise(nameof(IVertex.SuccessorAdded)).WithArgs(edge);
                baseNewStartMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));
            }

            start.Edges.Should().BeEmpty();
            start.Predecessors.Should().BeEmpty();
            start.Successors.Should().BeEmpty();
            ((IVertex)start).Edges.Should().BeEmpty();
            ((IVertex)start).Predecessors.Should().BeEmpty();
            ((IVertex)start).Successors.Should().BeEmpty();

            end.Edges.Should().ContainOnlyAndInOrder(edge);
            end.Predecessors.Should().ContainOnlyAndInOrder(edge);
            end.Successors.Should().BeEmpty();
            ((IVertex)end).Edges.Should().ContainOnlyAndInOrder(edge);
            ((IVertex)end).Predecessors.Should().ContainOnlyAndInOrder(edge);
            ((IVertex)end).Successors.Should().BeEmpty();

            newStart.Edges.Should().ContainOnlyAndInOrder(edge);
            newStart.Predecessors.Should().BeEmpty();
            newStart.Successors.Should().ContainOnlyAndInOrder(edge);
            ((IVertex)newStart).Edges.Should().ContainOnlyAndInOrder(edge);
            ((IVertex)newStart).Predecessors.Should().BeEmpty();
            ((IVertex)newStart).Successors.Should().ContainOnlyAndInOrder(edge);

            edge.Start.Should().Be(newStart);
            edge.End.Should().Be(end);
            ((IEdge)edge).Start.Should().Be(newStart);
            ((IEdge)edge).End.Should().Be(end);
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void ChangeStartNullVertexTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex end = GetVertex(type);
            ITestEdge edge = GetEdge(type);

            edge.Link(start, end);

            edge.Invoking(x => x.ChangeStart(null)).Should().Throw<ArgumentNullException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void ChangeStartUnlinkedEdgeTest(ImplementationType type)
        {
            ITestVertex vertex = GetVertex(type);
            ITestEdge edge = GetEdge(type);

            edge.Invoking(x => x.ChangeStart(vertex)).Should().Throw<InvalidOperationException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void ChangeStartToNotLinkableVerticeTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex end = GetVertex(type);
            ITestVertex notLinkableVertex = GetVertex(type, refuseAllRegistration: true);
            ITestEdge edge = GetEdge(type);

            edge.Link(start, end);

            edge.ChangeStart(notLinkableVertex).Should().BeFalse();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void ChangeEndTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex end = GetVertex(type);
            ITestVertex newEnd = GetVertex(type);

            ITestEdge edge = GetEdge(type);

            edge.Link(start, end);

            using (IMonitor<IVertex<ITestVertex, ITestEdge>> startMonitor = start.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex<ITestVertex, ITestEdge>> endMonitor = end.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex<ITestVertex, ITestEdge>> newEndMonitor = newEnd.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex> baseStartMonitor = start.Monitor<IVertex>())
            using (IMonitor<IVertex> baseEndMonitor = end.Monitor<IVertex>())
            using (IMonitor<IVertex> baseNewEndMonitor = newEnd.Monitor<IVertex>())
            {
                edge.ChangeEnd(newEnd).Should().BeTrue();

                startMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded)).WithArgs(edge);
                startMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved)).WithArgs(edge);
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded));
                startMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved));
                startMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded)).WithArgs(edge);
                startMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved)).WithArgs(edge);

                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded));
                endMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved)).WithArgs(edge);
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded));
                endMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved)).WithArgs(edge);
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded));
                endMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved));

                newEndMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeAdded)).WithArgs(edge);
                newEndMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.EdgeRemoved));
                newEndMonitor.Should().Raise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorAdded)).WithArgs(edge);
                newEndMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.PredecessorRemoved));
                newEndMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorAdded));
                newEndMonitor.Should().NotRaise(nameof(IVertex<ITestVertex, ITestEdge>.SuccessorRemoved));

                baseStartMonitor.Should().Raise(nameof(IVertex.EdgeAdded)).WithArgs(edge);
                baseStartMonitor.Should().Raise(nameof(IVertex.EdgeRemoved)).WithArgs(edge);
                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseStartMonitor.Should().Raise(nameof(IVertex.SuccessorAdded)).WithArgs(edge);
                baseStartMonitor.Should().Raise(nameof(IVertex.SuccessorRemoved)).WithArgs(edge);

                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                baseEndMonitor.Should().Raise(nameof(IVertex.EdgeRemoved)).WithArgs(edge);
                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseEndMonitor.Should().Raise(nameof(IVertex.PredecessorRemoved)).WithArgs(edge);
                baseEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));

                baseNewEndMonitor.Should().Raise(nameof(IVertex.EdgeAdded)).WithArgs(edge);
                baseNewEndMonitor.Should().NotRaise(nameof(IVertex.EdgeRemoved));
                baseNewEndMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseNewEndMonitor.Should().Raise(nameof(IVertex.PredecessorAdded)).WithArgs(edge);
                baseNewEndMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseNewEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseNewEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));
            }

            start.Edges.Should().ContainOnlyAndInOrder(edge);
            start.Predecessors.Should().BeEmpty();
            start.Successors.Should().ContainOnlyAndInOrder(edge);
            ((IVertex)start).Edges.Should().ContainOnlyAndInOrder(edge);
            ((IVertex)start).Predecessors.Should().BeEmpty();
            ((IVertex)start).Successors.Should().ContainOnlyAndInOrder(edge);

            end.Edges.Should().BeEmpty();
            end.Predecessors.Should().BeEmpty();
            end.Successors.Should().BeEmpty();
            ((IVertex)end).Edges.Should().BeEmpty();
            ((IVertex)end).Predecessors.Should().BeEmpty();
            ((IVertex)end).Successors.Should().BeEmpty();

            newEnd.Edges.Should().ContainOnlyAndInOrder(edge);
            newEnd.Predecessors.Should().ContainOnlyAndInOrder(edge);
            newEnd.Successors.Should().BeEmpty();
            ((IVertex)newEnd).Edges.Should().ContainOnlyAndInOrder(edge);
            ((IVertex)newEnd).Predecessors.Should().ContainOnlyAndInOrder(edge);
            ((IVertex)newEnd).Successors.Should().BeEmpty();

            edge.Start.Should().Be(start);
            edge.End.Should().Be(newEnd);
            ((IEdge)edge).Start.Should().Be(start);
            ((IEdge)edge).End.Should().Be(newEnd);
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void ChangeEndNullVertexTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex end = GetVertex(type);
            ITestEdge edge = GetEdge(type);
            edge.Link(start, end);

            edge.Invoking(x => x.ChangeEnd(null)).Should().Throw<ArgumentNullException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void ChangeEndUnlinkedEdgeTest(ImplementationType type)
        {
            ITestVertex vertex = GetVertex(type);
            ITestEdge edge = GetEdge(type);

            edge.Invoking(x => x.ChangeEnd(vertex)).Should().Throw<InvalidOperationException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void ChangeEndToNotLinkableVerticeTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex end = GetVertex(type);
            ITestVertex notLinkableVertex = GetVertex(type, refuseAllRegistration: true);
            ITestEdge edge = GetEdge(type);

            edge.Link(start, end);

            edge.ChangeEnd(notLinkableVertex).Should().BeFalse();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void UnlinkEdgesTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex middle = GetVertex(type);
            ITestVertex end = GetVertex(type);
            ITestEdge startToMiddleEdge = GetEdge(type);
            ITestEdge middleToEndEdge = GetEdge(type);

            startToMiddleEdge.Link(start, middle);
            middleToEndEdge.Link(middle, end);

            using (IMonitor<IVertex<ITestVertex, ITestEdge>> startMonitor = start.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex<ITestVertex, ITestEdge>> middleMonitor = middle.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex<ITestVertex, ITestEdge>> endMonitor = end.Monitor<IVertex<ITestVertex, ITestEdge>>())
            using (IMonitor<IVertex> baseStartMonitor = start.Monitor<IVertex>())
            using (IMonitor<IVertex> baseMiddleMonitor = middle.Monitor<IVertex>())
            using (IMonitor<IVertex> baseEndMonitor = end.Monitor<IVertex>())
            {
                middle.UnlinkEdges();

                startMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                startMonitor.Should().Raise(nameof(IVertex.EdgeRemoved)).WithArgs(startToMiddleEdge);
                startMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                startMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                startMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                startMonitor.Should().Raise(nameof(IVertex.SuccessorRemoved)).WithArgs(startToMiddleEdge);

                middleMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                middleMonitor.Should().Raise(nameof(IVertex.EdgeRemoved)).WithArgs(startToMiddleEdge, middleToEndEdge);
                middleMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                middleMonitor.Should().Raise(nameof(IVertex.PredecessorRemoved)).WithArgs(startToMiddleEdge);
                middleMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                middleMonitor.Should().Raise(nameof(IVertex.SuccessorRemoved)).WithArgs(middleToEndEdge);

                endMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                endMonitor.Should().Raise(nameof(IVertex.EdgeRemoved)).WithArgs(middleToEndEdge);
                endMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                endMonitor.Should().Raise(nameof(IVertex.PredecessorRemoved)).WithArgs(middleToEndEdge);
                endMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                endMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));

                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                baseStartMonitor.Should().Raise(nameof(IVertex.EdgeRemoved)).WithArgs(startToMiddleEdge);
                baseStartMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.PredecessorRemoved));
                baseStartMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseStartMonitor.Should().Raise(nameof(IVertex.SuccessorRemoved)).WithArgs(startToMiddleEdge);

                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                baseMiddleMonitor.Should().Raise(nameof(IVertex.EdgeRemoved)).WithArgs(startToMiddleEdge, middleToEndEdge);
                baseMiddleMonitor.Should().Raise(nameof(IVertex.EdgesCleared));
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseMiddleMonitor.Should().Raise(nameof(IVertex.PredecessorRemoved)).WithArgs(startToMiddleEdge);
                baseMiddleMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseMiddleMonitor.Should().Raise(nameof(IVertex.SuccessorRemoved)).WithArgs(middleToEndEdge);

                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgeAdded));
                baseEndMonitor.Should().Raise(nameof(IVertex.EdgeRemoved)).WithArgs(middleToEndEdge);
                baseEndMonitor.Should().NotRaise(nameof(IVertex.EdgesCleared));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.PredecessorAdded));
                baseEndMonitor.Should().Raise(nameof(IVertex.PredecessorRemoved)).WithArgs(middleToEndEdge);
                baseEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorAdded));
                baseEndMonitor.Should().NotRaise(nameof(IVertex.SuccessorRemoved));
            }

            start.Edges.Should().BeEmpty();
            start.Predecessors.Should().BeEmpty();
            start.Successors.Should().BeEmpty();
            ((IVertex)start).Edges.Should().BeEmpty();
            ((IVertex)start).Predecessors.Should().BeEmpty();
            ((IVertex)start).Successors.Should().BeEmpty();

            middle.Edges.Should().BeEmpty();
            middle.Predecessors.Should().BeEmpty();
            middle.Successors.Should().BeEmpty();
            ((IVertex)middle).Edges.Should().BeEmpty();
            ((IVertex)middle).Predecessors.Should().BeEmpty();
            ((IVertex)middle).Successors.Should().BeEmpty();

            end.Edges.Should().BeEmpty();
            end.Predecessors.Should().BeEmpty();
            end.Successors.Should().BeEmpty();
            ((IVertex)end).Edges.Should().BeEmpty();
            ((IVertex)end).Predecessors.Should().BeEmpty();
            ((IVertex)end).Successors.Should().BeEmpty();

            startToMiddleEdge.Start.Should().BeNull();
            startToMiddleEdge.End.Should().BeNull();
            ((IEdge)startToMiddleEdge).Start.Should().BeNull();
            ((IEdge)startToMiddleEdge).End.Should().BeNull();

            middleToEndEdge.Start.Should().BeNull();
            middleToEndEdge.End.Should().BeNull();
            ((IEdge)middleToEndEdge).Start.Should().BeNull();
            ((IEdge)middleToEndEdge).End.Should().BeNull();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void RegisterEdgeOnInvalidVertexTest(ImplementationType type)
        {
            ITestVertex newStart = GetVertex(type);
            ITestVertex newEnd = GetVertex(type);
            ITestVertex otherVertex = GetVertex(type);
            ITestEdge edge = GetEdge(type);

            edge.Link(newStart, newEnd);

            otherVertex.Invoking(x => x.RegisterEdge(edge)).Should().Throw<ArgumentException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void RegisterUnlinkedEdgeTest(ImplementationType type)
        {
            ITestVertex vertex = GetVertex(type);
            ITestEdge edge = GetEdge(type);

            vertex.Invoking(x => x.RegisterEdge(edge)).Should().Throw<ArgumentException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void RegisterNullEdgeTest(ImplementationType type)
        {
            ITestVertex vertex = GetVertex(type);

            vertex.Invoking(x => x.RegisterEdge(null)).Should().Throw<ArgumentNullException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void UnregisterNotRegisteredEdgeTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex middle = GetVertex(type);
            ITestVertex end = GetVertex(type);

            ITestEdge startToMiddleEdge = GetEdge(type);
            ITestEdge middleToEndEdge = GetEdge(type);
            startToMiddleEdge.Link(start, middle);
            middleToEndEdge.Link(middle, end);

            end.Invoking(x => x.UnregisterEdge(startToMiddleEdge)).Should().Throw<ArgumentException>();
            start.Invoking(x => x.UnregisterEdge(middleToEndEdge)).Should().Throw<ArgumentException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void UnregisterEdgeOnInvalidVertexTest(ImplementationType type)
        {
            ITestVertex newStart = GetVertex(type);
            ITestVertex newEnd = GetVertex(type);
            ITestVertex otherVertex = GetVertex(type);
            ITestEdge edge = GetEdge(type);

            edge.Link(newStart, newEnd);

            otherVertex.Invoking(x => x.UnregisterEdge(edge)).Should().Throw<ArgumentException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void UnregisterUnlinkedEdgeTest(ImplementationType type)
        {
            ITestVertex vertex = GetVertex(type);
            ITestEdge edge = GetEdge(type);

            vertex.Invoking(x => x.UnregisterEdge(edge)).Should().Throw<ArgumentException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void UnregisterNullEdgeTest(ImplementationType type)
        {
            ITestVertex vertex = GetVertex(type);

            vertex.Invoking(x => x.UnregisterEdge(null)).Should().Throw<ArgumentNullException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void CanRegisterNullEdgeTest(ImplementationType type)
        {
            ITestVertex newStart = GetVertex(type);
            ITestVertex newEnd = GetVertex(type);
            ITestEdge edge = GetEdge(type);

            newStart.Invoking(x => x.CanRegisterEdge(null, newStart, newEnd)).Should().Throw<ArgumentNullException>();
            newStart.Invoking(x => x.CanRegisterEdge(edge, null, newEnd)).Should().Throw<ArgumentNullException>();
            newStart.Invoking(x => x.CanRegisterEdge(edge, newStart, null)).Should().Throw<ArgumentNullException>();
            newStart.Invoking(x => x.CanRegisterEdge(null, null, newEnd)).Should().Throw<ArgumentNullException>();
            newStart.Invoking(x => x.CanRegisterEdge(edge, null, null)).Should().Throw<ArgumentNullException>();
            newStart.Invoking(x => x.CanRegisterEdge(null, newStart, null)).Should().Throw<ArgumentNullException>();
            newStart.Invoking(x => x.CanRegisterEdge(null, null, null)).Should().Throw<ArgumentNullException>();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void CanUnregisterNotRegisteredEdgeTest(ImplementationType type)
        {
            ITestVertex start = GetVertex(type);
            ITestVertex middle = GetVertex(type);
            ITestVertex end = GetVertex(type);
            
            ITestEdge startToMiddleEdge = GetEdge(type);
            ITestEdge middleToEndEdge = GetEdge(type);
            startToMiddleEdge.Link(start, middle);
            middleToEndEdge.Link(middle, end);

            end.CanUnregisterEdge(startToMiddleEdge).Should().BeFalse();
            start.CanUnregisterEdge(middleToEndEdge).Should().BeFalse();

            ITestEdge startToEndEdgeMock = GetEdgeMock(start, end);
            start.CanUnregisterEdge(startToEndEdgeMock).Should().BeFalse();
            end.CanUnregisterEdge(startToEndEdgeMock).Should().BeFalse();
        }

        [TestCase(ImplementationType.Inherited)]
        [TestCase(ImplementationType.Reimplemented)]
        public void CanUnregisterNullEdgeTest(ImplementationType type)
        {
            ITestVertex vertex = GetVertex(type);

            vertex.Invoking(x => x.CanUnregisterEdge(null)).Should().Throw<ArgumentNullException>();
        }
    }
}