using System;

namespace OverGraphed.Test.Utils
{
    public class TestEdgeMock : ITestEdge
    {
        public ITestVertex Start { get; }
        public ITestVertex End { get; }

        IVertex IEdge.Start => Start;
        IVertex IEdge.End => End;

        public TestEdgeMock(ITestVertex start, ITestVertex end)
        {
            Start = start;
            End = end;
        }

        public bool Link(ITestVertex start, ITestVertex end) => throw new NotImplementedException();
        public bool ChangeStart(ITestVertex start) => throw new NotImplementedException();
        public bool ChangeEnd(ITestVertex end) => throw new NotImplementedException();
        public void Unlink() => throw new NotImplementedException();
    }
}