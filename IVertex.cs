using System.Collections.Generic;

namespace Diese.Graph
{
    public interface IVertex<TGraph, out TVertex, out TEdge, in TVisitor>
        where TGraph : IGraph<TGraph, TVertex, TEdge, TVisitor>
        where TVertex : IVertex<TGraph, TVertex, TEdge, TVisitor>
        where TEdge : IEdge<TGraph, TVertex, TEdge, TVisitor>
        where TVisitor : IVisitor<TGraph, TVertex, TEdge, TVisitor>
    {
        IReadOnlyCollection<TEdge> Edges { get; }
        IReadOnlyCollection<TVertex> Predecessors { get; }
        IReadOnlyCollection<TVertex> Successors { get; }
        void Accept(TVisitor visitor);
    }
}