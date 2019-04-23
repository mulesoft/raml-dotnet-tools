using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Common;

namespace Raml.Api.Core.Tests
{
    [TestClass]
    public class NetNamingMapperTests
    {
        [TestMethod]
        public void Should_Convert_Object_Names()
        {
            Assert.AreEqual("GetSalesId", NetNamingMapper.GetObjectName("get-/sales/{id}"));
        }

        [TestMethod]
        public void Should_Convert_Method_Names()
        {
            Assert.AreEqual("GetContactsById", NetNamingMapper.GetMethodName("get-/contacts/{id}"));
        }

        [TestMethod]
        public void Should_Convert_Property_Names()
        {
            Assert.AreEqual("XRateMediaAbcDef", NetNamingMapper.GetPropertyName("X-Rate-Media:Abc/Def"));
        }

        [TestMethod]
        public void Should_Remove_QuestionMark_From_Property_Names()
        {
            Assert.AreEqual("Optional", NetNamingMapper.GetPropertyName("optional?"));
        }

        [TestMethod]
        public void Should_Remove_MediaTypeExtension_From_Object_Name()
        {
            Assert.AreEqual("Users", NetNamingMapper.GetObjectName("users{mediaTypeExtension}"));
        }

        [TestMethod]
        public void Should_Remove_QuestionMark_From_Object_Name()
        {
            Assert.AreEqual("Optional", NetNamingMapper.GetObjectName("optional?"));
        }

        [TestMethod]
        public void Should_Remove_MediaTypeExtension_From_Method_Name()
        {
            Assert.AreEqual("Users", NetNamingMapper.GetObjectName("users{mediaTypeExtension}"));
        }

        [TestMethod]
        public void Should_Avoid_Parentheses_In_Object_Name()
        {
            Assert.AreEqual("GetSalesId", NetNamingMapper.GetObjectName("get-/sales({id})"));
            Assert.AreEqual("GetSalesId", NetNamingMapper.GetObjectName("get-/sales(id)"));
        }

        [TestMethod]
        public void Should_Avoid_Parentheses_In_Method_Name()
        {
            Assert.AreEqual("GetSalesById", NetNamingMapper.GetMethodName("get-/sales({id})"));
            Assert.AreEqual("GetSalesId", NetNamingMapper.GetMethodName("get-/sales(id)"));
        }

        [TestMethod]
        public void Should_Avoid_Single_Quote_In_Object_Name()
        {
            Assert.AreEqual("GetSalesId", NetNamingMapper.GetObjectName("get-/sales('{id}')"));
            Assert.AreEqual("GetSalesId", NetNamingMapper.GetObjectName("get-/sales'id'"));
        }

        [TestMethod]
        public void Should_Avoid_Single_Quote_In_Method_Name()
        {
            Assert.AreEqual("GetSalesById", NetNamingMapper.GetMethodName("get-/sales('{id}')"));
            Assert.AreEqual("GetSalesId", NetNamingMapper.GetMethodName("get-/sales'id'"));
        }

        [TestMethod]
        public void Should_Avoid_Brackets_In_Property_Name()
        {
            Assert.AreEqual("Sales", NetNamingMapper.GetPropertyName("sales[]"));
            Assert.AreEqual("Salesperson", NetNamingMapper.GetPropertyName("(sales|person)[]"));
        }

        [TestMethod]
        public void Should_Avoid_Brackets_In_Object_Name()
        {
            Assert.AreEqual("Sales", NetNamingMapper.GetObjectName("sales[]"));
            Assert.AreEqual("Salesperson", NetNamingMapper.GetObjectName("(sales|person)[]"));
        }

        [TestMethod]
        public void Should_Remove_Dash_From_Namespace()
        {
            Assert.AreEqual("GetSales", NetNamingMapper.GetNamespace("get-sales"));
        }
    }
}