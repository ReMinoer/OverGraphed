using System.Collections.Generic;

namespace Diese.Graph
{
    public class VertexBase<TVertex, TEdge> : IVertex<TVertex, TEdge>
        where TVertex : VertexBase<TVertex, TEdge>
        where TEdge : EdgeBase<TEdge, TVertex>
    {
        private readonly List<TEdge> _edges;
        private readonly IReadOnlyCollection<TEdge> _readOnlyEdges;
        private readonly List<TVertex> _predecessors;
        private readonly IReadOnlyCollection<TVertex> _readOnlyPredecessors;
        private readonly List<TVertex> _successors;
        private readonly IReadOnlyCollection<TVertex> _readOnlySuccessors;

        public IReadOnlyCollection<TEdge> Edges
        {
            get { return _readOnlyEdges; }
        }

        public IReadOnlyCollection<TVertex> Predecessors
        {
            get { return _readOnlyPredecessors; }
        }

        public IReadOnlyCollection<TVertex> Successors
        {
            get { return _readOnlySuccessors; }
        }

        public VertexBase()
        {
            _edges = new List<TEdge>();
            _predecessors = new List<TVertex>();
            _successors = new List<TVertex>();

            _readOnlyEdges = _edges.AsReadOnly();
            _readOnlyPredecessors = _predecessors.AsReadOnly();
            _readOnlySuccessors = _successors.AsReadOnly();
        }

        internal void AddEdge(TEdge edge)
        {
            if (this == edge.Start)
                _successors.Add(edge.End);
            else
                _predecessors.Add(edge.Start);

            _edges.Add(edge);
        }

        internal void RemoveEdge(TEdge edge)
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