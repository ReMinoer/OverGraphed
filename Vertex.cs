using System.Collections.Generic;

namespace Diese.Graph
{
    public class Vertex<TVertexBase, TEdgeBase> : IVertex<TVertexBase, TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        private readonly List<TEdgeBase> _edges;
        private readonly IReadOnlyCollection<TEdgeBase> _readOnlyEdges;
        private readonly List<TVertexBase> _predecessors;
        private readonly IReadOnlyCollection<TVertexBase> _readOnlyPredecessors;
        private readonly List<TVertexBase> _successors;
        private readonly IReadOnlyCollection<TVertexBase> _readOnlySuccessors;

        public IEnumerable<TEdgeBase> Edges
        {
            get { return _readOnlyEdges; }
        }

        public IEnumerable<TVertexBase> Predecessors
        {
            get { return _readOnlyPredecessors; }
        }

        public IEnumerable<TVertexBase> Successors
        {
            get { return _readOnlySuccessors; }
        }

        public Vertex()
        {
            _edges = new List<TEdgeBase>();
            _predecessors = new List<TVertexBase>();
            _successors = new List<TVertexBase>();

            _readOnlyEdges = _edges.AsReadOnly();
            _readOnlyPredecessors = _predecessors.AsReadOnly();
            _readOnlySuccessors = _successors.AsReadOnly();
        }

        internal void AddEdge(TEdgeBase edge)
        {
            if (this == edge.Start)
                _successors.Add(edge.End);
            else
                _predecessors.Add(edge.Start);

            _edges.Add(edge);
        }

        internal void RemoveEdge(TEdgeBase edge)
        {
            if (this == edge.Start)
                _successors.Remove(edge.End);
            else
                _predecessors.Remove(edge.Start);

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