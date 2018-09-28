namespace Valeq.Configuration
{
    public enum EqualityComparisonType
    {
        /// <summary>
        /// A type is compared using default equality semantics as implemented in its Equals/GetHashCode methods.
        /// </summary>
        DefaultEquality,
        
        /// <summary>
        /// A type is compared using value equality, thus ignoring the default equality semantics implemented
        /// in its Equals/GetHashCode methods.
        /// </summary>
        ValueEquality
    }
}
