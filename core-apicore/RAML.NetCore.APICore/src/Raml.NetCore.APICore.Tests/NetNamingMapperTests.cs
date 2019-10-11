using Xunit;
using Raml.Common;

namespace Raml.Api.Core.Tests
{
    
    public class NetNamingMapperTests
    {
        [Fact]
        public void Should_Convert_Object_Names()
        {
            Assert.Equal("GetSalesId", NetNamingMapper.GetObjectName("get-/sales/{id}"));
        }

        [Fact]
        public void Should_Convert_Method_Names()
        {
            Assert.Equal("GetContactsById", NetNamingMapper.GetMethodName("get-/contacts/{id}"));
        }

        [Fact]
        public void Should_Convert_Property_Names()
        {
            Assert.Equal("XRateMediaAbcDef", NetNamingMapper.GetPropertyName("X-Rate-Media:Abc/Def"));
        }

        [Fact]
        public void Should_Remove_QuestionMark_From_Property_Names()
        {
            Assert.Equal("Optional", NetNamingMapper.GetPropertyName("optional?"));
        }

        [Fact]
        public void Should_Remove_MediaTypeExtension_From_Object_Name()
        {
            Assert.Equal("Users", NetNamingMapper.GetObjectName("users{mediaTypeExtension}"));
        }

        [Fact]
        public void Should_Remove_QuestionMark_From_Object_Name()
        {
            Assert.Equal("Optional", NetNamingMapper.GetObjectName("optional?"));
        }

        [Fact]
        public void Should_Remove_MediaTypeExtension_From_Method_Name()
        {
            Assert.Equal("Users", NetNamingMapper.GetObjectName("users{mediaTypeExtension}"));
        }

        [Fact]
        public void Should_Avoid_Parentheses_In_Object_Name()
        {
            Assert.Equal("GetSalesId", NetNamingMapper.GetObjectName("get-/sales({id})"));
            Assert.Equal("GetSalesId", NetNamingMapper.GetObjectName("get-/sales(id)"));
        }

        [Fact]
        public void Should_Avoid_Parentheses_In_Method_Name()
        {
            Assert.Equal("GetSalesById", NetNamingMapper.GetMethodName("get-/sales({id})"));
            Assert.Equal("GetSalesId", NetNamingMapper.GetMethodName("get-/sales(id)"));
        }

        [Fact]
        public void Should_Avoid_Single_Quote_In_Object_Name()
        {
            Assert.Equal("GetSalesId", NetNamingMapper.GetObjectName("get-/sales('{id}')"));
            Assert.Equal("GetSalesId", NetNamingMapper.GetObjectName("get-/sales'id'"));
        }

        [Fact]
        public void Should_Avoid_Single_Quote_In_Method_Name()
        {
            Assert.Equal("GetSalesById", NetNamingMapper.GetMethodName("get-/sales('{id}')"));
            Assert.Equal("GetSalesId", NetNamingMapper.GetMethodName("get-/sales'id'"));
        }

        [Fact]
        public void Should_Avoid_Brackets_In_Property_Name()
        {
            Assert.Equal("Sales", NetNamingMapper.GetPropertyName("sales[]"));
            Assert.Equal("Salesperson", NetNamingMapper.GetPropertyName("(sales|person)[]"));
        }

        [Fact]
        public void Should_Avoid_Brackets_In_Object_Name()
        {
            Assert.Equal("Sales", NetNamingMapper.GetObjectName("sales[]"));
            Assert.Equal("Salesperson", NetNamingMapper.GetObjectName("(sales|person)[]"));
        }

        [Fact]
        public void Should_Remove_Dash_From_Namespace()
        {
            Assert.Equal("GetSales", NetNamingMapper.GetNamespace("get-sales"));
        }
    }
}