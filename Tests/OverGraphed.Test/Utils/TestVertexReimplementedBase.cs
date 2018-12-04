using System.Collections.Generic;

namespace OverGraphed.Test.Utils
{
    public class TestVertexReimplementedBase : ITestVertex
    {
        protected ILinkableVertex<ITestVertex, ITestEdge> Implementation;
        private IVertex ImplementationBase => Implementation;

        public IReadOnlyCollection<ITestEdge> Edges => Implementation.Edges;
        public IReadOnlyCollection<ITestEdge> Predecessors => Implementation.Predecessors;
        public IReadOnlyCollection<ITestEdge> Successors => Implementation.Successors;

        IReadOnlyCollection<IEdge> IVertex.Edges => Implementation.Edges;
        IReadOnlyCollection<IEdge> IVertex.Predecessors => Implementation.Predecessors;
        IReadOnlyCollection<IEdge> IVertex.Successors => Implementation.Successors;

        public event Event<ITestEdge> EdgeAdded
        {
            add => Implementation.EdgeAdded += value;
            remove => Implementation.EdgeAdded -= value;
        }

        public event Event<ITestEdge> EdgeRemoved
        {
            add => Implementation.EdgeRemoved += value;
            remove => Implementation.EdgeRemoved -= value;
        }

        public event Event<ITestEdge> PredecessorAdded
        {
            add => Implementation.PredecessorAdded += value;
            remove => Implementation.PredecessorAdded -= value;
        }

        public event Event<ITestEdge> PredecessorRemoved
        {
            add => Implementation.PredecessorRemoved += value;
            remove => Implementation.PredecessorRemoved -= value;
        }

        public event Event<ITestEdge> SuccessorAdded
        {
            add => Implementation.SuccessorAdded += value;
            remove => Implementation.SuccessorAdded -= value;
        }

        public event Event<ITestEdge> SuccessorRemoved
        {
            add => Implementation.SuccessorRemoved += value;
            remove => Implementation.SuccessorRemoved -= value;
        }

        event Event<IEdge> IVertex.EdgeAdded
        {
            add => ImplementationBase.EdgeAdded += value;
            remove => ImplementationBase.EdgeAdded -= value;
        }

        event Event<IEdge> IVertex.EdgeRemoved
        {
            add => ImplementationBase.EdgeRemoved += value;
            remove => ImplementationBase.EdgeRemoved -= value;
        }

        event Event<IEdge> IVertex.PredecessorAdded
        {
            add => ImplementationBase.PredecessorAdded += value;
            remove => ImplementationBase.PredecessorAdded -= value;
        }

        event Event<IEdge> IVertex.PredecessorRemoved
        {
            add => ImplementationBase.PredecessorRemoved += value;
            remove => ImplementationBase.PredecessorRemoved -= value;
        }

        event Event<IEdge> IVertex.SuccessorAdded
        {
            add => ImplementationBase.SuccessorAdded += value;
            remove => ImplementationBase.SuccessorAdded -= value;
        }

        event Event<IEdge> IVertex.SuccessorRemoved
        {
            add => ImplementationBase.SuccessorRemoved += value;
            remove => ImplementationBase.SuccessorRemoved -= value;
        }

        public event Event EdgesCleared
        {
            add => ImplementationBase.EdgesCleared += value;
            remove => ImplementationBase.EdgesCleared -= value;
        }

        bool ILinkableVertex<ITestVertex, ITestEdge>.CanRegisterEdge(ITestEdge edge, ITestVertex start, ITestVertex end) => Implementation.CanRegisterEdge(edge, start, end);
        bool ILinkableVertex<ITestVertex, ITestEdge>.CanUnregisterEdge(ITestEdge edge) => Implementation.CanUnregisterEdge(edge);
        bool ILinkableVertex<ITestVertex, ITestEdge>.RegisterEdge(ITestEdge edge) => Implementation.RegisterEdge(edge);
        bool ILinkableVertex<ITestVertex, ITestEdge>.UnregisterEdge(ITestEdge edge) => Implementation.UnregisterEdge(edge);
        public void UnlinkEdges() => Implementation.UnlinkEdges();
    }
}