namespace Diese.Graph
{
    public abstract class VisitorBase<TGraph, TVertex, TEdge, TVisitor> : IVisitor<TGraph, TVertex, TEdge, TVisitor>
        where TGraph : GraphBase<TGraph, TVertex, TEdge, TVisitor>
        where TVertex : VertexBase<TGraph, TVertex, TEdge, TVisitor>
        where TEdge : EdgeBase<TGraph, TVertex, TEdge, TVisitor>
        where TVisitor : VisitorBase<TGraph, TVertex, TEdge, TVisitor>
    {
        public abstract void Process(TGraph graph);
        public abstract void Visit(TVertex vertex);
    }
}