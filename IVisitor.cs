namespace Diese.Graph
{
    public interface IVisitor<in TGraph, in TVertex, TEdge, TVisitor>
        where TGraph : IGraph<TGraph, TVertex, TEdge, TVisitor>
        where TVertex : IVertex<TGraph, TVertex, TEdge, TVisitor>
        where TEdge : IEdge<TGraph, TVertex, TEdge, TVisitor>
        where TVisitor : IVisitor<TGraph, TVertex, TEdge, TVisitor>
    {
        void Process(TGraph graph);
        void Visit(TVertex vertex);
    }
}