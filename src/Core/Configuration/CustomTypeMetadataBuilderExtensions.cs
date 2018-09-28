using System;
using System.Linq.Expressions;
using Valeq.Comparers;
using Valeq.Metadata;
using Valeq.Reflection;

namespace Valeq.Configuration
{
    public static class CustomTypeMetadataBuilderExtensions
    {
        public static ICustomTypeMetadataBuilder<TType> ByReference<TType>(
            this ICustomTypeMetadataBuilder<TType> typeMetadataBuilder)
        {
            if (typeMetadataBuilder == null) throw new ArgumentNullException(nameof(typeMetadataBuilder));

            typeMetadataBuilder.AddTypeMetadata(
                new EqualityComparerAttribute(typeof(ReferenceEqualityComparer<TType>)));
            return typeMetadataBuilder;
        }

        public static ICustomMemberMetadataBuilder<TType, TProperty> Property<TType, TProperty>(
            this ICustomTypeMetadataBuilder<TType> typeMetadataBuilder, Expression<Func<TType, TProperty>> property)
        {
            if (typeMetadataBuilder == null) throw new ArgumentNullException(nameof(typeMetadataBuilder));
            if (property == null) throw new ArgumentNullException(nameof(property));

            var propertyInfo = ExpressionUtility.GetPropertyInfo(property);
            var member = Member.FromPropertyInfo(propertyInfo);

            return typeMetadataBuilder.GetMemberMetadataBuilder<TProperty>(member);
        }
    }
}
