using System.Collections.Generic;
using System.Collections.ObjectModel;
using OverGraphed.Base;

namespace OverGraphed
{
    public class SimpleDirectedVertex<TVertexBase, TEdgeBase> : SimpleDirectedVertexBase<TVertexBase, TEdgeBase>
        where TVertexBase : class, ILinkableVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, ILinkableEdge<TVertexBase, TEdgeBase>
    {
        private readonly List<TEdgeBase> _edges = new List<TEdgeBase>();
        private readonly List<TEdgeBase> _predecessors = new List<TEdgeBase>();
        private readonly List<TEdgeBase> _successors = new List<TEdgeBase>();

        public ReadOnlyCollection<TEdgeBase> Edges { get; }
        public ReadOnlyCollection<TEdgeBase> Predecessors { get; }
        public ReadOnlyCollection<TEdgeBase> Successors { get; }

        protected override ICollection<TEdgeBase> EdgesCollection => _edges;
        protected override ICollection<TEdgeBase> PredecessorsCollection => _predecessors;
        protected override ICollection<TEdgeBase> SuccessorsCollection => _successors;

        protected override IReadOnlyCollection<TEdgeBase> EdgesBasesExplicit => Edges;
        protected override IReadOnlyCollection<TEdgeBase> PredecessorsBasesExplicit => Predecessors;
        protected override IReadOnlyCollection<TEdgeBase> SuccessorsBasesExplicit => Successors;

        public SimpleDirectedVertex()
        {
            Edges = new ReadOnlyCollection<TEdgeBase>(_edges);
            Predecessors = new ReadOnlyCollection<TEdgeBase>(_predecessors);
            Successors = new ReadOnlyCollection<TEdgeBase>(_successors);
        }

        public SimpleDirectedVertex(TVertexBase owner)
            : base(owner)
        {
            Edges = new ReadOnlyCollection<TEdgeBase>(_edges);
            Predecessors = new ReadOnlyCollection<TEdgeBase>(_predecessors);
            Successors = new ReadOnlyCollection<TEdgeBase>(_successors);
        }
    }
}