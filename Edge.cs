namespace Diese.Graph
{
    public class Edge<TVertexBase, TEdgeBase> : Edge<TVertexBase, TVertexBase, TVertexBase, TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
    }

    public class Edge<TStart, TEnd, TVertexBase, TEdgeBase> : IEdge<TStart, TEnd, TVertexBase, TEdgeBase>
        where TStart : TVertexBase
        where TEnd : TVertexBase
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        TVertexBase IEdge<TVertexBase, TEdgeBase>.Start
        {
            get { return Start; }
        }

        TVertexBase IEdge<TVertexBase, TEdgeBase>.End
        {
            get { return End; }
        }

        public TStart Start { get; internal set; }
        public TEnd End { get; internal set; }
    }
}