using System;
using System.Linq.Expressions;
using Valeq.Metadata;

namespace Valeq.Configuration
{
    public static class CustomMemberMetadataBuilderExtensions
    {
        public static ICustomMemberMetadataBuilder<TType, TProperty2> Property<TType, TProperty1, TProperty2>(
            this ICustomMemberMetadataBuilder<TType, TProperty1> memberMetadataBuilder,
            Expression<Func<TType, TProperty2>> property)
        {
            if (memberMetadataBuilder == null) throw new ArgumentNullException(nameof(memberMetadataBuilder));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return memberMetadataBuilder.Type<TType>().Property(property);
        }
        
        public static ICustomMemberMetadataBuilder<TType, TProperty> Ignore<TType, TProperty>(
            this ICustomMemberMetadataBuilder<TType, TProperty> memberMetadataBuilder)
        {
            if (memberMetadataBuilder == null) throw new ArgumentNullException(nameof(memberMetadataBuilder));

            memberMetadataBuilder.AddMemberMetadata(new IgnoreInComparisonAttribute());
            return memberMetadataBuilder;
        }

        public static ICustomMemberMetadataBuilder<TType, TProperty> IgnoreCase<TType, TProperty>(
            this ICustomMemberMetadataBuilder<TType, TProperty> memberMetadataBuilder)
        {
            if (memberMetadataBuilder == null) throw new ArgumentNullException(nameof(memberMetadataBuilder));

            memberMetadataBuilder.AddMemberMetadata(new IgnoreCaseInComparisonAttribute());
            return memberMetadataBuilder;
        }
    }
}
