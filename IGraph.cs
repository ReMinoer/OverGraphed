using System.Collections.Generic;

namespace Diese.Graph
{
    public interface IGraph<TVertexBase, TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        IEnumerable<TVertexBase> Vertices { get; }
        IEnumerable<TEdgeBase> Edges { get; }
        TEdgeBase this[TVertexBase start, TVertexBase end] { get; }
        void AddVertex(TVertexBase vertex);
        void RemoveVertex(TVertexBase vertex);
        bool ContainsVertex(TVertexBase vertex);
        void ClearVertices();
        void AddEdge<TStart, TEnd, TEdge>(TStart from, TEnd to, TEdge edge)
            where TStart : TVertexBase
            where TEnd : TVertexBase
            where TEdge : Edge<TStart, TEnd, TVertexBase, TEdgeBase>, TEdgeBase;
        void RemoveEdge(TVertexBase from, TVertexBase to);
        void RemoveEdge(TEdgeBase edge);
        bool ContainsEdge(TVertexBase from, TVertexBase to);
        bool ContainsEdge(TEdgeBase edge);
        void ClearEdges(TVertexBase vertex);
        void ClearEdges();
    }
}