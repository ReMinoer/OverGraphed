using System;
using System.Collections.Generic;
using System.Linq;

namespace OverGraphed.Base
{
    public abstract class SimpleDirectedVertexBase<TVertexBase, TEdgeBase> : LinkableVertexBase<TVertexBase, TEdgeBase>
        where TVertexBase : class, ILinkableVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, ILinkableEdge<TVertexBase, TEdgeBase>
    {
        protected abstract ICollection<TEdgeBase> EdgesCollection { get; }
        protected abstract ICollection<TEdgeBase> PredecessorsCollection { get; }
        protected abstract ICollection<TEdgeBase> SuccessorsCollection { get; }

        public override event Event<TEdgeBase> EdgeAdded;
        public override event Event<TEdgeBase> EdgeRemoved;
        public override event Event<TEdgeBase> PredecessorAdded;
        public override event Event<TEdgeBase> PredecessorRemoved;
        public override event Event<TEdgeBase> SuccessorAdded;
        public override event Event<TEdgeBase> SuccessorRemoved;

        protected override event Event<IEdge> EdgeAddedExplicit;
        protected override event Event<IEdge> EdgeRemovedExplicit;
        protected override event Event<IEdge> PredecessorAddedExplicit;
        protected override event Event<IEdge> PredecessorRemovedExplicit;
        protected override event Event<IEdge> SuccessorAddedExplicit;
        protected override event Event<IEdge> SuccessorRemovedExplicit;

        protected SimpleDirectedVertexBase()
        {
        }

        protected SimpleDirectedVertexBase(TVertexBase owner)
            : base(owner)
        {
        }

        protected virtual bool CanRegister(TEdgeBase edge, TVertexBase start, TVertexBase end) => true;
        protected virtual bool CanUnregister(TEdgeBase edge) => true;

        protected override sealed bool CanRegisterEdge(TEdgeBase edge, TVertexBase start, TVertexBase end)
        {
            if (edge == null)
                throw new ArgumentNullException();
            if (start == null)
                throw new ArgumentNullException();
            if (end == null)
                throw new ArgumentNullException();

            // If edge with same connection exist, return false
            if (start == Owner)
            {
                if (SuccessorsCollection.Any(x => x.End == end))
                    return false;
            }
            else if (end == Owner)
            {
                if (PredecessorsCollection.Any(x => x.Start == start))
                    return false;
            }
            else
                return false;

            return CanRegister(edge, start, end);
        }

        protected override sealed bool CanUnregisterEdge(TEdgeBase edge)
        {
            if (edge == null)
                throw new ArgumentNullException();

            // Check if edge is an existing successor or predecessor
            if (edge.Start == Owner && edge.End != null)
            {
                if (!SuccessorsCollection.Contains(edge))
                    return false;
            }
            else if (edge.End == Owner && edge.Start != null)
            {
                if (!PredecessorsCollection.Contains(edge))
                    return false;
            }
            else
                return false;

            return CanUnregister(edge);
        }

        protected override sealed bool RegisterEdge(TEdgeBase edge)
        {
            if (edge == null)
                throw new ArgumentNullException();

            if (!CanRegisterEdge(edge, edge.Start, edge.End))
                throw new ArgumentException(nameof(edge));

            ICollection<TEdgeBase> collection;
            
            if (edge.Start == Owner)
                collection = SuccessorsCollection;
            else
                collection = PredecessorsCollection;

            EdgesCollection.Add(edge);
            collection.Add(edge);

            EdgeAdded?.Invoke(this, edge);
            EdgeAddedExplicit?.Invoke(this, edge);
            if (ReferenceEquals(collection, SuccessorsCollection))
            {
                SuccessorAdded?.Invoke(this, edge);
                SuccessorAddedExplicit?.Invoke(this, edge);
            }
            else
            {
                PredecessorAdded?.Invoke(this, edge);
                PredecessorAddedExplicit?.Invoke(this, edge);
            }

            return true;
        }

        protected override sealed bool UnregisterEdge(TEdgeBase edge)
        {
            if (edge == null)
                throw new ArgumentNullException();

            if (!CanUnregisterEdge(edge))
                throw new ArgumentException(nameof(edge));

            ICollection<TEdgeBase> collection;
            
            if (edge.Start == Owner)
                collection = SuccessorsCollection;
            else
                collection = PredecessorsCollection;
            
            EdgesCollection.Remove(edge);
            collection.Remove(edge);

            EdgeRemoved?.Invoke(this, edge);
            EdgeRemovedExplicit?.Invoke(this, edge);
            if (ReferenceEquals(collection, SuccessorsCollection))
            {
                SuccessorRemoved?.Invoke(this, edge);
                SuccessorRemovedExplicit?.Invoke(this, edge);
            }
            else
            {
                PredecessorRemoved?.Invoke(this, edge);
                PredecessorRemovedExplicit?.Invoke(this, edge);
            }

            return true;
        }
    }
}