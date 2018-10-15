using System.Linq;
using NUnit.Framework;

namespace Valeq.Reflection
{
    [TestFixture]
    public class TypeExtensionsGetBaseTypesTests
    {
        [Test]
        public void GetBaseTypes_NoType_ThrowsException()
        {
            Assert.That(() => TypeExtensions.GetBaseTypes(null).ToList(), Throws.ArgumentNullException);
        }
        
        [Test]
        public void GetBaseTypes_Object_ReturnsSelf()
        {
            var types = typeof(object).GetBaseTypes().ToList();
            Assert.That(types, Is.EquivalentTo(new[] {typeof(object)}));
        }
        
        [Test]
        public void GetBaseTypes_WithoutSelf_WithInterfaces_ReturnsAllBaseClassesAndInterfaces()
        {
            var types = typeof(SubClass).GetBaseTypes(includeSelf: false, includeInterfaces: true).ToList();
            Assert.That(types, Is.EquivalentTo(new[]
            {
                typeof(object),
                typeof(BaseClass),
                typeof(IBaseInterface),
                typeof(IBaseInterface2),
                typeof(ISubInterface),
                typeof(ISubInterface2)
            }));
        }
        
        [Test]
        public void GetBaseTypes_WithSelf_WithInterfaces_ReturnsSelfAndAllBaseClassesAndInterfaces()
        {
            var types = typeof(SubClass).GetBaseTypes(includeInterfaces: true).ToList();
            Assert.That(types, Is.EquivalentTo(new[]
            {
                typeof(object),
                typeof(BaseClass),
                typeof(SubClass),
                typeof(IBaseInterface),
                typeof(IBaseInterface2),
                typeof(ISubInterface),
                typeof(ISubInterface2)
            }));
        }
        
        [Test]
        public void GetBaseTypes_WithoutSelf_WithoutInterfaces_ReturnsAllBaseClasses()
        {
            var types = typeof(SubClass).GetBaseTypes(includeSelf: false, includeInterfaces: false).ToList();
            Assert.That(types, Is.EquivalentTo(new[]
            {
                typeof(object),
                typeof(BaseClass),
            }));
        }
        
        [Test]
        public void GetBaseTypes_WithSelf_WithoutInterfaces_ReturnsSelfAndAllBaseClasses()
        {
            var types = typeof(SubClass).GetBaseTypes(includeSelf: true, includeInterfaces: false).ToList();
            Assert.That(types, Is.EquivalentTo(new[]
            {
                typeof(object),
                typeof(BaseClass),
                typeof(SubClass),
            }));
        }
        
        private class BaseClass : IBaseInterface
        {
        }

        private class SubClass : BaseClass, ISubInterface, ISubInterface2
        {
        }

        private interface IBaseInterface
        {
        }

        private interface ISubInterface
        {
        }
        
        private interface IBaseInterface2
        {
        }

        private interface ISubInterface2 : IBaseInterface2
        {
        }
    }
}
