namespace Valeq.Reflection
{
    public enum TypeCategory
    {
        /// <summary>
        /// A type whose value equality is determined without further investigating its structure.
        /// </summary>
        Simple,
        
        /// <summary>
        /// A type which consists of multiple elements and whose value equality is determined by the value equality of its elements.
        /// </summary>
        Collection,
        
        /// <summary>
        /// A type which has members and whose value equality is determined by the value equality of its members.
        /// </summary>
        Complex
    }
}
