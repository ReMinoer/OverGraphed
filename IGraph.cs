using System.Collections.Generic;

namespace Diese.Graph
{
    public interface IGraph<TVertex, TEdge> : IEnumerable<TVertex>
        where TVertex : IVertex<TVertex, TEdge>
        where TEdge : IEdge<TEdge, TVertex>
    {
        IReadOnlyCollection<TVertex> Vertices { get; }
        IReadOnlyCollection<TEdge> Edges { get; }
        TEdge this[TVertex start, TVertex end] { get; }
        void AddVertex(TVertex vertex);
        void RemoveVertex(TVertex vertex);
        bool ContainsVertex(TVertex vertex);
        void ClearVertices();
        void AddEdge(ref TEdge edge);
        void RemoveEdge(ref TEdge edge);
        bool ContainsEdge(ref TEdge edge);
        void ClearEdges(TVertex vertex);
        void ClearEdges();
    }
}