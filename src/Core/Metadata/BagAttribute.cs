using Valeq.Reflection;

namespace Valeq.Metadata
{
    public class BagAttribute : CollectionCategoryAttribute
    {
        public BagAttribute()
            : base(CollectionCategory.Bag)
        {
        }
    }
}
