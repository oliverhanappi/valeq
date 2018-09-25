using System;

namespace Valeq.Configuration
{
    public class IgnoreCaseInComparisonAttribute : StringComparisonAttribute
    {
        public IgnoreCaseInComparisonAttribute() : base(StringComparison.InvariantCultureIgnoreCase) //TODO rethink defaults
        {
        }
    }
}
