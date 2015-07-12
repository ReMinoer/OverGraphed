using System.Collections.Generic;

namespace Diese.Graph
{
    public interface IVertex<out TVertex, out TEdge>
        where TVertex : IVertex<TVertex, TEdge>
        where TEdge : IEdge<TEdge, TVertex>
    {
        IReadOnlyCollection<TEdge> Edges { get; }
        IReadOnlyCollection<TVertex> Predecessors { get; }
        IReadOnlyCollection<TVertex> Successors { get; }
    }
}