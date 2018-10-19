using NUnit.Framework;
using Valeq.Configuration;
using Valeq.Reflection;
using Valeq.TestInfrastructure;

namespace Valeq.Metadata
{
    [TestFixture]
    public class AttributeMetadataProviderTests
    {
        private readonly AttributeMetadataProvider _metadataProvider = new AttributeMetadataProvider();
        
        [Test]
        public void GetTypeMetadata_Null_ThrowsException()
        {
            Assert.That(() => _metadataProvider.GetTypeMetadata(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetTypeMetadata_Class_GetsDirectAttributes()
        {
            var metadata = _metadataProvider.GetTypeMetadata(typeof(BaseClass));
            Assert.That(metadata.HasMetadata<IEqualityComparisonTypeMetadata>());
        }

        [Test]
        public void GetTypeMetadata_Class_DoesNotGetAttributesOfDerivedClasses()
        {
            var metadata = _metadataProvider.GetTypeMetadata(typeof(BaseClass));
            Assert.That(metadata.HasMetadata<IPropertySearchScopeMetadata>(), Is.False);
        }

        [Test]
        public void GetTypeMetadata_Class_GetsInheritedAttributesAsWell()
        {
            var metadata = _metadataProvider.GetTypeMetadata(typeof(SubClass));
            Assert.That(metadata.HasMetadata<IEqualityComparisonTypeMetadata>());
            Assert.That(metadata.HasMetadata<IPropertySearchScopeMetadata>());
        }

        [Test]
        public void GetTypeMetadata_Class_DoesNotGetInterfaceAttributes()
        {
            var metadata = _metadataProvider.GetTypeMetadata(typeof(SubClass));
            Assert.That(metadata.HasMetadata<IEqualityComparerMetadata>(), Is.False);
        }

        [Test]
        public void GetTypeMetadata_Interface_GetsDirectAttributes()
        {
            var metadata = _metadataProvider.GetTypeMetadata(typeof(IBaseInterface));
            Assert.That(metadata.HasMetadata<IEqualityComparerMetadata>());
        }

        [Test]
        public void GetTypeMetadata_Interface_DoesNotGetInheritedAttributes()
        {
            var metadata = _metadataProvider.GetTypeMetadata(typeof(ISubInterface));
            Assert.That(metadata.IsEmpty);
        }

        [Test]
        public void GetTypeMetadata_Struct_GetsAttributes()
        {
            var metadata = _metadataProvider.GetTypeMetadata(typeof(TestStruct));
            Assert.That(metadata.HasMetadata<IEqualityComparisonTypeMetadata>());
        }

        [Test]
        public void GetMemberMetadata_Null_ThrowsException()
        {
            Assert.That(() => _metadataProvider.GetMemberMetadata(null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void GetMemberMetadata_ClassField_GetsAttributes()
        {
            var member = Member.FromFieldInfo(typeof(BaseClass).GetField(nameof(BaseClass.BaseField)));
            var metadata = _metadataProvider.GetMemberMetadata(member);

            Assert.That(metadata.HasMetadata<IEqualityComparisonTypeMetadata>());
        }
        
        [Test]
        public void GetMemberMetadata_ClassProperty_GetsAttributes()
        {
            var member = Member.FromPropertyInfo(typeof(BaseClass).GetProperty(nameof(BaseClass.BaseProperty)));
            var metadata = _metadataProvider.GetMemberMetadata(member);

            Assert.That(metadata.HasMetadata<IEqualityComparisonTypeMetadata>());
        }
        
        [Test]
        public void GetMemberMetadata_ClassProperty_DoesNotGetAttributeOfDerivedClasses()
        {
            var member = Member.FromPropertyInfo(typeof(BaseClass).GetProperty(nameof(BaseClass.BaseProperty)));
            var metadata = _metadataProvider.GetMemberMetadata(member);

            Assert.That(metadata.HasMetadata<IIgnoredMemberMetadata>(), Is.False);
        }
        
        [Test]
        public void GetMemberMetadata_OverriddenClassProperty_GetsAttributesOfBaseClassAndSubClass()
        {
            var member = Member.FromPropertyInfo(typeof(SubClass).GetProperty(nameof(SubClass.BaseProperty)));
            var metadata = _metadataProvider.GetMemberMetadata(member);

            Assert.That(metadata.HasMetadata<IEqualityComparisonTypeMetadata>());
            Assert.That(metadata.HasMetadata<IIgnoredMemberMetadata>());
        }
        
        [Test]
        public void GetMemberMetadata_StructField_GetsAttributes()
        {
            var member = Member.FromFieldInfo(typeof(TestStruct).GetField(nameof(TestStruct.BaseField)));
            var metadata = _metadataProvider.GetMemberMetadata(member);

            Assert.That(metadata.HasMetadata<IEqualityComparisonTypeMetadata>());
        }
        
        [Test]
        public void GetMemberMetadata_StructProperty_GetsAttributes()
        {
            var member = Member.FromPropertyInfo(typeof(TestStruct).GetProperty(nameof(TestStruct.BaseProperty)));
            var metadata = _metadataProvider.GetMemberMetadata(member);

            Assert.That(metadata.HasMetadata<IEqualityComparisonTypeMetadata>());
        }

#pragma warning disable 649        
        
        [DefaultEquality]
        private class BaseClass
        {
            [DefaultEquality]
            public virtual string BaseProperty { get; set; }
            
            [DefaultEquality]
            public string BaseField;
        }

        [PropertySearchScope(PropertySearchScope.OnlyPublic)]
        private class SubClass : BaseClass, ISubInterface
        {
            [IgnoreInComparison]
            public override string BaseProperty
            {
                get => base.BaseProperty;
                set => base.BaseProperty = value;
            }
        }

        [EqualityComparer(typeof(AbsoluteIntegerEqualityComparer))]
        private interface IBaseInterface
        {
            [IgnoreCaseInComparison]
            string BaseProperty { get; }
        }

        private interface ISubInterface : IBaseInterface
        {
        }

        [DefaultEquality]
        private struct TestStruct
        {
            [DefaultEquality]
            public string BaseProperty { get; set; }
            
            [DefaultEquality]
            public string BaseField;
        }
    }
}
