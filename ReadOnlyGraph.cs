using System.Collections.Generic;

namespace Diese.Graph
{
    public class ReadOnlyGraph<TVertex, TEdge> : IGraph<TVertex, TEdge>
        where TEdge : class, IEdge<TVertex, TEdge>
        where TVertex : class, IVertex<TVertex, TEdge>
    {
        private readonly IGraph<TVertex, TEdge> _graph;
        public IEnumerable<TVertex> Vertices => _graph.Vertices;
        public IEnumerable<TEdge> Edges => _graph.Edges;
        public TEdge this[TVertex start, TVertex end] => _graph[start, end];

        public ReadOnlyGraph(IGraph<TVertex, TEdge> graph)
        {
            _graph = graph;
        }

        public bool ContainsEdge(TVertex from, TVertex to)
        {
            return _graph.ContainsEdge(from, to);
        }
    }
}