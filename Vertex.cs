﻿using System.Collections.Generic;

namespace Diese.Graph
{
    public class Vertex<TVertexBase, TEdgeBase> : IVertex<TVertexBase, TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        private readonly List<TEdgeBase> _edges;
        private readonly IReadOnlyCollection<TEdgeBase> _readOnlyEdges;
        private readonly List<TEdgeBase> _predecessors;
        private readonly IReadOnlyCollection<TEdgeBase> _readOnlyPredecessors;
        private readonly List<TEdgeBase> _successors;
        private readonly IReadOnlyCollection<TEdgeBase> _readOnlySuccessors;
        public IEnumerable<TEdgeBase> Edges => _readOnlyEdges;
        public IEnumerable<TEdgeBase> Predecessors => _readOnlyPredecessors;
        public IEnumerable<TEdgeBase> Successors => _readOnlySuccessors;

        public Vertex()
        {
            _edges = new List<TEdgeBase>();
            _predecessors = new List<TEdgeBase>();
            _successors = new List<TEdgeBase>();

            _readOnlyEdges = _edges.AsReadOnly();
            _readOnlyPredecessors = _predecessors.AsReadOnly();
            _readOnlySuccessors = _successors.AsReadOnly();
        }

        internal void AddEdge(TEdgeBase edge)
        {
            if (this == edge.Start)
                _successors.Add(edge);
            else
                _predecessors.Add(edge);

            _edges.Add(edge);
        }

        internal void RemoveEdge(TEdgeBase edge)
        {
            if (this == edge.Start)
                _successors.Remove(edge);
            else
                _predecessors.Remove(edge);

            _edges.Remove(edge);
        }

        internal void ClearEdges()
        {
            _edges.Clear();
            _predecessors.Clear();
            _successors.Clear();
        }
    }
}