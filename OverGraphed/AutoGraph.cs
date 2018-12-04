using System;
using System.Collections.Generic;
using System.Linq;
using OverGraphed.Base;

namespace OverGraphed
{
    public class AutoGraph<TVertexBase, TEdgeBase> : GraphBase<TVertexBase, TEdgeBase>
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

        public bool RegisterVertex(TVertexBase vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (!VerticesSet.Add(vertex))
                return false;

            vertex.EdgeAdded += OnEdgeAdded;
            vertex.EdgeRemoved += OnEdgeRemoved;

            var registeredEdges = new List<TEdgeBase>();
            foreach (TEdgeBase edge in vertex.Edges)
            {
                if (!VerticesSet.Contains(edge.Start) || !VerticesSet.Contains(edge.End))
                    continue;

                if (EdgesSet.Add(edge))
                    registeredEdges.Add(edge);
            }

            VertexAdded?.Invoke(this, vertex);
            VertexAddedExplicit?.Invoke(this, vertex);
            foreach (TEdgeBase edge in registeredEdges)
            {
                EdgeAdded?.Invoke(this, edge);
                EdgeAddedExplicit?.Invoke(this, edge);
            }

            return true;
        }

        public bool UnregisterVertex(TVertexBase vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (!VerticesSet.Remove(vertex))
                return false;

            vertex.EdgeAdded -= OnEdgeAdded;
            vertex.EdgeRemoved -= OnEdgeRemoved;

            TEdgeBase[] unregisteredEdges = vertex.Edges.Where(edge => EdgesSet.Remove(edge)).ToArray();

            foreach (TEdgeBase edge in unregisteredEdges)
            {
                EdgeRemoved?.Invoke(this, edge);
                EdgeRemovedExplicit?.Invoke(this, edge);
            }
            VertexRemoved?.Invoke(this, vertex);
            VertexRemovedExplicit?.Invoke(this, vertex);

            return true;
        }

        public override void Clear()
        {
            foreach (TVertexBase vertex in VerticesSet)
            {
                vertex.EdgeAdded -= OnEdgeAdded;
                vertex.EdgeRemoved -= OnEdgeRemoved;
            }

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

        private void OnEdgeAdded(object sender, TEdgeBase edge)
        {
            if (!VerticesSet.Contains(edge.Start) || !VerticesSet.Contains(edge.End))
                return;

            if (EdgesSet.Add(edge))
            {
                EdgeAdded?.Invoke(this, edge);
                EdgeAddedExplicit?.Invoke(this, edge);
            }
        }

        private void OnEdgeRemoved(object sender, TEdgeBase edge)
        {
            if (EdgesSet.Remove(edge))
            {
                EdgeRemoved?.Invoke(this, edge);
                EdgeRemovedExplicit?.Invoke(this, edge);
            }
        }
    }
}