namespace Valeq.Reflection
{
    public enum CollectionCategory
    {
        /// <summary>
        /// A collection which is considered value equal to another if the elements and their order matches.
        /// </summary>
        Sequence = 1,

        /// <summary>
        /// A collection which is considered value equal to another if the number of occurrences
        /// of each value equal element matches. The order of elements does not matter. 
        /// </summary>
        Bag = 2,

        /// <summary>
        /// A collection which is considered value equal to another if the same elements occur in both.
        /// Order and number of occurrences do not matter.
        /// </summary>
        Set = 3,
    }
}
