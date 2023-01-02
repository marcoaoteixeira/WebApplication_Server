using System.Collections;

namespace Nameless.WebApplication {

    /// <summary>
    /// Extension methods for <see cref="IEnumerable"/> and <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtension {

        #region Public Static Methods

        /// <summary>
        /// Checks if an <see cref="IEnumerable"/> is empty.
        /// </summary>
        /// <param name="self">The <see cref="IEnumerable"/> instance.</param>
        /// <returns><c>true</c>, if is empty, otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="self"/> is <c>null</c>.</exception>
        public static bool IsNullOrEmpty(this IEnumerable self) {
            Prevent.Null(self, nameof(self));

            // Costs O(1)
            if (self is ICollection collection) { return collection.Count == 0; }

            // Costs O(N)
            var enumerator = self.GetEnumerator();
            var canMoveNext = enumerator.MoveNext();

            if (enumerator is IDisposable disposable) {
                disposable.Dispose();
            }

            return !canMoveNext;
        }

        #endregion
    }
}