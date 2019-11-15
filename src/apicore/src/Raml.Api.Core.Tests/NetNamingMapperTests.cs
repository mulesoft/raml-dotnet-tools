using NUnit.Framework;

namespace RAML.Api.Core.Tests
{
    [TestFixture]
    public class NetNamingMapperTests
    {
        [Test]
        public void Should_Convert_Object_Names()
        {
            Assert.AreEqual("GetSalesId", NetNamingMapper.GetObjectName("get-/sales/{id}"));
        }

        [Test]
        public void Should_Not_Capitalize_Enum_Values()
        {
            Assert.AreEqual("i", NetNamingMapper.GetEnumValueName("i"));
        }


        [Test]
        public void Should_Generate_Random_Values()
        {
            Assert.AreNotEqual(NetNamingMapper.RemoveInvalidChars(">"), NetNamingMapper.RemoveInvalidChars("<"));
        }


        [Test]
        public void Should_Convert_Method_Names()
        {
            Assert.AreEqual("GetContactsById", NetNamingMapper.GetMethodName("get-/contacts/{id}"));
        }

        [Test]
        public void Should_Convert_Property_Names()
        {
            Assert.AreEqual("XRateMediaAbcDef", NetNamingMapper.GetPropertyName("X-Rate-Media:Abc/Def"));
        }

        [Test]
        public void Should_Remove_QuestionMark_From_Property_Names()
        {
            Assert.AreEqual("Optional", NetNamingMapper.GetPropertyName("optional?"));
        }

        [Test]
        public void Should_Remove_MediaTypeExtension_From_Object_Name()
        {
            Assert.AreEqual("Users", NetNamingMapper.GetObjectName("users{mediaTypeExtension}"));
        }

        [Test]
        public void Should_Remove_QuestionMark_From_Object_Name()
        {
            Assert.AreEqual("Optional", NetNamingMapper.GetObjectName("optional?"));
        }

        [Test]
        public void Should_Remove_MediaTypeExtension_From_Method_Name()
        {
            Assert.AreEqual("Users", NetNamingMapper.GetObjectName("users{mediaTypeExtension}"));
        }

        [Test]
        public void Should_Avoid_Parentheses_In_Object_Name()
        {
            Assert.AreEqual("GetSalesId", NetNamingMapper.GetObjectName("get-/sales({id})"));
            Assert.AreEqual("GetSalesId", NetNamingMapper.GetObjectName("get-/sales(id)"));
        }

        [Test]
        public void Should_Avoid_Parentheses_In_Method_Name()
        {
            Assert.AreEqual("GetSalesById", NetNamingMapper.GetMethodName("get-/sales({id})"));
            Assert.AreEqual("GetSalesId", NetNamingMapper.GetMethodName("get-/sales(id)"));
        }

        [Test]
        public void Should_Avoid_Single_Quote_In_Object_Name()
        {
            Assert.AreEqual("GetSalesId", NetNamingMapper.GetObjectName("get-/sales('{id}')"));
            Assert.AreEqual("GetSalesId", NetNamingMapper.GetObjectName("get-/sales'id'"));
        }

        [Test]
        public void Should_Avoid_Single_Quote_In_Method_Name()
        {
            Assert.AreEqual("GetSalesById", NetNamingMapper.GetMethodName("get-/sales('{id}')"));
            Assert.AreEqual("GetSalesId", NetNamingMapper.GetMethodName("get-/sales'id'"));
        }

        [Test]
        public void Should_Avoid_Brackets_In_Property_Name()
        {
            Assert.AreEqual("Sales", NetNamingMapper.GetPropertyName("sales[]"));
            Assert.AreEqual("Salesperson", NetNamingMapper.GetPropertyName("(sales|person)[]"));
        }

        [Test]
        public void Should_Avoid_Brackets_In_Object_Name()
        {
            Assert.AreEqual("Sales", NetNamingMapper.GetObjectName("sales[]"));
            Assert.AreEqual("Salesperson", NetNamingMapper.GetObjectName("(sales|person)[]"));
        }

        [Test]
        public void Should_Remove_Dash_From_Namespace()
        {
            Assert.AreEqual("GetSales", NetNamingMapper.GetNamespace("get-sales"));
        }
    }
}