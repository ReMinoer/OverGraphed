using System.Collections.Generic;

namespace Diese.Graph
{
    public interface IGraph<TGraph, TVertex, TEdge, in TVisitor> : IEnumerable<TVertex>
        where TGraph : IGraph<TGraph, TVertex, TEdge, TVisitor>
        where TVertex : IVertex<TGraph, TVertex, TEdge, TVisitor>
        where TEdge : IEdge<TGraph, TVertex, TEdge, TVisitor>
        where TVisitor : IVisitor<TGraph, TVertex, TEdge, TVisitor>
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