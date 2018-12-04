using System.Collections;
using System.Collections.Generic;

namespace OverGraphed.Utils
{
    public class ReadOnlyHashSet<T> : IReadOnlyHashSet<T>
    {
        private readonly HashSet<T> _hashSet;
        public int Count => _hashSet.Count;
        public IEqualityComparer<T> Comparer => _hashSet.Comparer;

        public ReadOnlyHashSet(HashSet<T> hashSet)
        {
            _hashSet = hashSet;
        }

        public bool Contains(T item) => _hashSet.Contains(item);
        public bool SetEquals(IEnumerable<T> other) => _hashSet.SetEquals(other);
        public bool Overlaps(IEnumerable<T> other) => _hashSet.Overlaps(other);
        public bool IsSubsetOf(IEnumerable<T> other) => _hashSet.IsSubsetOf(other);
        public bool IsSupersetOf(IEnumerable<T> other) => _hashSet.IsSupersetOf(other);
        public bool IsProperSubsetOf(IEnumerable<T> other) => _hashSet.IsProperSubsetOf(other);
        public bool IsProperSupersetOf(IEnumerable<T> other) => _hashSet.IsProperSupersetOf(other);

        public IEnumerator<T> GetEnumerator() => _hashSet.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}