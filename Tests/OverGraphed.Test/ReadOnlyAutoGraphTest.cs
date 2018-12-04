using NUnit.Framework;
using OverGraphed.Test.Base;
using OverGraphed.Test.Utils;

namespace OverGraphed.Test
{
    [TestFixture]
    public class ReadOnlyAutoGraphTest : AutoGraphTestBase
    {
        protected override IGraph<ITestVertex, ITestEdge> GetObservedGraph(AutoGraph<ITestVertex, ITestEdge> graph)
        {
            return new ReadOnlyGraph<ITestVertex, ITestEdge>(graph);
        }
    }
}