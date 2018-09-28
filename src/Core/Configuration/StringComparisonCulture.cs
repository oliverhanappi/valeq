namespace Valeq.Configuration
{
    public enum StringComparisonCulture
    {
        /// <summary>
        /// Strings are compared using ordinals.
        /// </summary>
        None,

        /// <summary>
        /// Strings are compared using the current culture.
        /// </summary>
        Current,

        /// <summary>
        /// Strings are compared using the invariant culture.
        /// </summary>
        Invariant
    }
}