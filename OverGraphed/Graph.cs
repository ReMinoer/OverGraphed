using System;
using System.Linq;
using OverGraphed.Base;

namespace OverGraphed
{
    public class Graph<TVertexBase, TEdgeBase> : GraphBase<TVertexBase, TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        public override event Event<TVertexBase> VertexAdded;
        public override event Event<TVertexBase> VertexRemoved;
        public override event Event<TEdgeBase> EdgeAdded;
        public override event Event<TEdgeBase> EdgeRemoved;
        public override event Event Cleared;

        protected override event Event<IVertex> VertexAddedExplicit;
        protected override event Event<IVertex> VertexRemovedExplicit;
        protected override event Event<IEdge> EdgeAddedExplicit;
        protected override event Event<IEdge> EdgeRemovedExplicit;

        public virtual bool RegisterVertex(TVertexBase vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (!VerticesSet.Add(vertex))
                return false;

            VertexAdded?.Invoke(this, vertex);
            VertexAddedExplicit?.Invoke(this, vertex);
            return true;
        }

        public virtual bool UnregisterVertex(TVertexBase vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (!VerticesSet.Remove(vertex))
                return false;

            VertexRemoved?.Invoke(this, vertex);
            VertexRemovedExplicit?.Invoke(this, vertex);
            return true;
        }

        public bool RegisterEdge(TEdgeBase edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (!VerticesSet.Contains(edge.Start) || !VerticesSet.Contains(edge.End))
                return false;

            if (!EdgesSet.Add(edge))
                return false;

            EdgeAdded?.Invoke(this, edge);
            EdgeAddedExplicit?.Invoke(this, edge);
            return true;
        }

        public bool UnregisterEdge(TEdgeBase edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (!EdgesSet.Remove(edge))
                return false;

            EdgeRemoved?.Invoke(this, edge);
            EdgeRemovedExplicit?.Invoke(this, edge);
            return true;
        }

        public override void Clear()
        {
            TVertexBase[] vertices = VerticesSet.ToArray();
            TEdgeBase[] edges = EdgesSet.ToArray();

            VerticesSet.Clear();
            EdgesSet.Clear();

            Cleared?.Invoke(this);

            foreach (TEdgeBase edge in edges)
            {
                EdgeRemoved?.Invoke(this, edge);
                EdgeRemovedExplicit?.Invoke(this, edge);
            }

            foreach (TVertexBase vertex in vertices)
            {
                VertexRemoved?.Invoke(this, vertex);
                VertexRemovedExplicit?.Invoke(this, vertex);
            }
        }
    }
}