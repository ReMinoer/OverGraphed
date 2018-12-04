using System.Collections.Generic;

namespace OverGraphed.Utils
{
    public interface IReadOnlyHashSet<T> : IReadOnlySet<T>
    {
        IEqualityComparer<T> Comparer { get; }
    }
}