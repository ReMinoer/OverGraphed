using System.Linq;

namespace OverGraphed
{
    static public class GraphExtensions
    {
        static public bool ContainsLink(this IGraph graph, IVertex start, IVertex end)
        {
            return graph.Edges.Any(x => x.Start == start && x.End == end);
        }

        static public bool ContainsLink(this IGraph graph, IVertex start, IVertex end, out IEdge edge)
        {
            edge = graph.Edges.FirstOrDefault(x => x.Start == start && x.End == end);
            return edge != null;
        }

        static public bool ContainsLink<TVertexBase, TEdgeBase>(this IGraph<TVertexBase, TEdgeBase> graph, TVertexBase start, TVertexBase end)
            where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
            where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
        {
            return graph.Edges.Any(x => x.Start == start && x.End == end);
        }

        static public bool ContainsLink<TVertexBase, TEdgeBase>(this IGraph<TVertexBase, TEdgeBase> graph, TVertexBase start, TVertexBase end, out TEdgeBase edge)
            where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
            where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
        {
            edge = graph.Edges.FirstOrDefault(x => x.Start == start && x.End == end);
            return edge != null;
        }
    }
}