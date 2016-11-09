using System.Collections.Generic;

namespace Diese.Graph
{
    public interface IGraph<TVertexBase, out TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        IEnumerable<TVertexBase> Vertices { get; }
        IEnumerable<TEdgeBase> Edges { get; }
        TEdgeBase this[TVertexBase start, TVertexBase end] { get; }
        bool ContainsEdge(TVertexBase from, TVertexBase to);
    }

    // TODO : Remove constraint on Edge implemntation
    // TODO : Pass edge value and not edge implementation
    public interface IWritableGraph<TVertexBase, TEdgeBase> : IGraph<TVertexBase, TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        void AddVertex(TVertexBase vertex);
        void RemoveVertex(TVertexBase vertex);
        void ClearVertices();
        void AddEdge<TStart, TEnd, TEdge>(TStart from, TEnd to, TEdge edge)
            where TStart : TVertexBase
            where TEnd : TVertexBase
            where TEdge : Edge<TStart, TEnd, TVertexBase, TEdgeBase>, TEdgeBase;
        void RemoveEdge(TVertexBase from, TVertexBase to);
        void RemoveEdge(TEdgeBase edge);
        void ClearEdges(TVertexBase vertex);
        void ClearEdges();
    }
}