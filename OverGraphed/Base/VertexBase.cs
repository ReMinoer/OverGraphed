using System;
using System.Collections.Generic;

namespace OverGraphed.Base
{
    public abstract class VertexBase : IVertex
    {
        protected abstract IReadOnlyCollection<IEdge> EdgesExplicit { get; }
        protected abstract IReadOnlyCollection<IEdge> PredecessorsExplicit { get; }
        protected abstract IReadOnlyCollection<IEdge> SuccessorsExplicit { get; }

        IReadOnlyCollection<IEdge> IVertex.Edges => EdgesExplicit;
        IReadOnlyCollection<IEdge> IVertex.Predecessors => PredecessorsExplicit;
        IReadOnlyCollection<IEdge> IVertex.Successors => SuccessorsExplicit;

        protected abstract event Event<IEdge> EdgeAddedExplicit;
        protected abstract event Event<IEdge> EdgeRemovedExplicit;
        protected abstract event Event<IEdge> PredecessorAddedExplicit;
        protected abstract event Event<IEdge> PredecessorRemovedExplicit;
        protected abstract event Event<IEdge> SuccessorAddedExplicit;
        protected abstract event Event<IEdge> SuccessorRemovedExplicit;
        public abstract event Event EdgesCleared;

        event Event<IEdge> IVertex.EdgeAdded
        {
            add => EdgeAddedExplicit += value;
            remove => EdgeAddedExplicit -= value;
        }

        event Event<IEdge> IVertex.EdgeRemoved
        {
            add => EdgeRemovedExplicit += value;
            remove => EdgeRemovedExplicit -= value;
        }

        event Event<IEdge> IVertex.PredecessorAdded
        {
            add => PredecessorAddedExplicit += value;
            remove => PredecessorAddedExplicit -= value;
        }

        event Event<IEdge> IVertex.PredecessorRemoved
        {
            add => PredecessorRemovedExplicit += value;
            remove => PredecessorRemovedExplicit -= value;
        }

        event Event<IEdge> IVertex.SuccessorAdded
        {
            add => SuccessorAddedExplicit += value;
            remove => SuccessorAddedExplicit -= value;
        }

        event Event<IEdge> IVertex.SuccessorRemoved
        {
            add => SuccessorRemovedExplicit += value;
            remove => SuccessorRemovedExplicit -= value;
        }
    }

    public abstract class VertexBase<TVertexBase, TEdgeBase> : VertexBase, IVertex<TVertexBase, TEdgeBase>
        where TVertexBase : class, ILinkableVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, ILinkableEdge<TVertexBase, TEdgeBase>
    {
        protected TVertexBase Owner { get; }

        protected abstract IReadOnlyCollection<TEdgeBase> EdgesBasesExplicit { get; }
        protected abstract IReadOnlyCollection<TEdgeBase> PredecessorsBasesExplicit { get; }
        protected abstract IReadOnlyCollection<TEdgeBase> SuccessorsBasesExplicit { get; }

        protected override IReadOnlyCollection<IEdge> EdgesExplicit => EdgesBasesExplicit;
        protected override IReadOnlyCollection<IEdge> PredecessorsExplicit => PredecessorsBasesExplicit;
        protected override IReadOnlyCollection<IEdge> SuccessorsExplicit => SuccessorsBasesExplicit;

        IReadOnlyCollection<TEdgeBase> IVertex<TVertexBase, TEdgeBase>.Edges => EdgesBasesExplicit;
        IReadOnlyCollection<TEdgeBase> IVertex<TVertexBase, TEdgeBase>.Predecessors => PredecessorsBasesExplicit;
        IReadOnlyCollection<TEdgeBase> IVertex<TVertexBase, TEdgeBase>.Successors => SuccessorsBasesExplicit;

        public abstract event Event<TEdgeBase> EdgeAdded;
        public abstract event Event<TEdgeBase> EdgeRemoved;
        public abstract event Event<TEdgeBase> PredecessorAdded;
        public abstract event Event<TEdgeBase> PredecessorRemoved;
        public abstract event Event<TEdgeBase> SuccessorAdded;
        public abstract event Event<TEdgeBase> SuccessorRemoved;

        protected VertexBase()
        {
            Owner = this as TVertexBase;
            if (Owner == null)
                throw new InvalidOperationException();
        }

        protected VertexBase(TVertexBase owner)
        {
            Owner = owner;
        }
    }
}