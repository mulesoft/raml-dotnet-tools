// Template: Client Proxy T4 Template (RAMLClient.t4) version 5.0

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RAML.Api.Core;
using Raml.Common;
using OrdersClientSample.OrdersXml.Models;

namespace OrdersClientSample.OrdersXml
{
    public partial class Shipped
    {
        private readonly OrdersXmlClient proxy;

        internal Shipped(OrdersXmlClient proxy)
        {
            this.proxy = proxy;
        }


        /// <summary>
		/// gets already shipped orders - /shipped
		/// </summary>
        public virtual async Task<Models.ShippedGetResponse> Get()
        {

            var url = "orders/shipped";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.ShippedGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// gets already shipped orders - /shipped
		/// </summary>
		/// <param name="request">ApiRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.ShippedGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "orders/shipped";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);

            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
	        var response = await proxy.Client.SendAsync(req);
            return new Models.ShippedGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }

    }

    public partial class Notshipped
    {
        private readonly OrdersXmlClient proxy;

        internal Notshipped(OrdersXmlClient proxy)
        {
            this.proxy = proxy;
        }


        /// <summary>
		/// gets not shipped orders - /notshipped
		/// </summary>
        public virtual async Task<Models.NotshippedGetResponse> Get()
        {

            var url = "orders/notshipped";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.NotshippedGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// gets not shipped orders - /notshipped
		/// </summary>
		/// <param name="request">ApiRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.NotshippedGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "orders/notshipped";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);

            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
	        var response = await proxy.Client.SendAsync(req);
            return new Models.NotshippedGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }

    }

    public partial class Ship
    {
        private readonly OrdersXmlClient proxy;

        internal Ship(OrdersXmlClient proxy)
        {
            this.proxy = proxy;
        }


        /// <summary>
		/// marks order as shipped - /ship
		/// </summary>
		/// <param name="content"></param>
		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Post(string content, string id)
        {

            var url = "orders/{id}/ship";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Content = new StringContent(content);
	        var response = await proxy.Client.SendAsync(req);

            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// marks order as shipped - /ship
		/// </summary>
		/// <param name="request">Models.ShipPostRequest</param>
        public virtual async Task<ApiResponse> Post(Models.ShipPostRequest request)
        {

            var url = "orders/{id}/ship";
			if(request.UriParameters == null)
				throw new InvalidOperationException("Uri Parameters cannot be null");               

			if(request.UriParameters.Id == null)
				throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);

            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            req.Content = request.Content;
	        var response = await proxy.Client.SendAsync(req);
            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }

    }

    public partial class Orders
    {
        private readonly OrdersXmlClient proxy;

        internal Orders(OrdersXmlClient proxy)
        {
            this.proxy = proxy;
        }

        public virtual Shipped Shipped
        {
            get { return new Shipped(proxy); }
        }

        public virtual Notshipped Notshipped
        {
            get { return new Notshipped(proxy); }
        }

        public virtual Ship Ship
        {
            get { return new Ship(proxy); }
        }


        /// <summary>
		/// Create a new purchase order - /orders
		/// </summary>
		/// <param name="purchaseordertype"></param>
        public virtual async Task<ApiResponse> Post(Models.PurchaseOrderType purchaseordertype)
        {

            var url = "orders";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);

            var stringWriter = new StringWriter();
        	new XmlSerializer(typeof (Models.PurchaseOrderType)).Serialize(stringWriter, purchaseordertype);
            req.Content = new  StringContent(stringWriter.GetStringBuilder().ToString(), Encoding.UTF8, "application/xml");     
	        var response = await proxy.Client.SendAsync(req);

            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// Create a new purchase order - /orders
		/// </summary>
		/// <param name="request">Models.OrdersPostRequest</param>
        public virtual async Task<ApiResponse> Post(Models.OrdersPostRequest request)
        {

            var url = "orders";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);

            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            var stringWriter = new StringWriter();
        	new XmlSerializer(typeof (Models.PurchaseOrderType)).Serialize(stringWriter, request.Content);
            req.Content = new  StringContent(stringWriter.GetStringBuilder().ToString(), Encoding.UTF8, "application/xml");     
	        var response = await proxy.Client.SendAsync(req);
            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        /// <summary>
		/// gets an order by id - /{id}
		/// </summary>
		/// <param name="id"></param>
        public virtual async Task<Models.OrdersGetResponse> Get(string id)
        {

            var url = "orders/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.OrdersGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// gets an order by id - /{id}
		/// </summary>
		/// <param name="request">Models.OrdersGetRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.OrdersGetResponse> Get(Models.OrdersGetRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "orders/{id}";
			if(request.UriParameters == null)
				throw new InvalidOperationException("Uri Parameters cannot be null");               

			if(request.UriParameters.Id == null)
				throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);

            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
	        var response = await proxy.Client.SendAsync(req);
            return new Models.OrdersGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        /// <summary>
		/// updates an order - /{id}
		/// </summary>
		/// <param name="purchaseordertype"></param>
		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Put(Models.PurchaseOrderType purchaseordertype, string id)
        {

            var url = "orders/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Put, url);

            var stringWriter = new StringWriter();
        	new XmlSerializer(typeof (Models.PurchaseOrderType)).Serialize(stringWriter, purchaseordertype);
            req.Content = new  StringContent(stringWriter.GetStringBuilder().ToString(), Encoding.UTF8, "application/xml");     
	        var response = await proxy.Client.SendAsync(req);

            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// updates an order - /{id}
		/// </summary>
		/// <param name="request">Models.OrdersPutRequest</param>
        public virtual async Task<ApiResponse> Put(Models.OrdersPutRequest request)
        {

            var url = "orders/{id}";
			if(request.UriParameters == null)
				throw new InvalidOperationException("Uri Parameters cannot be null");               

			if(request.UriParameters.Id == null)
				throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Put, url);

            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            var stringWriter = new StringWriter();
        	new XmlSerializer(typeof (Models.PurchaseOrderType)).Serialize(stringWriter, request.Content);
            req.Content = new  StringContent(stringWriter.GetStringBuilder().ToString(), Encoding.UTF8, "application/xml");     
	        var response = await proxy.Client.SendAsync(req);
            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }

    }

    /// <summary>
    /// Main class for grouping root resources. Nested resources are defined as properties. The constructor can optionally receive an URL and HttpClient instance to override the default ones.
    /// </summary>
    public partial class OrdersXmlClient
    {

		public SchemaValidationSettings SchemaValidation { get; private set; } 

        protected readonly HttpClient client;
        public const string BaseUri = "/";

        internal HttpClient Client { get { return client; } }




        public OrdersXmlClient(string endpointUrl)
        {
            SchemaValidation = new SchemaValidationSettings
			{
				Enabled = true,
				RaiseExceptions = true
			};

			if(string.IsNullOrWhiteSpace(endpointUrl))
                throw new ArgumentException("You must specify the endpoint URL", "endpointUrl");

			if (endpointUrl.Contains("{"))
			{
				var regex = new Regex(@"\{([^\}]+)\}");
				var matches = regex.Matches(endpointUrl);
				var parameters = new List<string>();
				foreach (Match match in matches)
				{
					parameters.Add(match.Groups[1].Value);
				}
				throw new InvalidOperationException("Please replace parameter/s " + string.Join(", ", parameters) + " in the URL before passing it to the constructor ");
			}

            client = new HttpClient {BaseAddress = new Uri(endpointUrl)};
        }

        public OrdersXmlClient(HttpClient httpClient)
        {
            if(httpClient.BaseAddress == null)
                throw new InvalidOperationException("You must set the BaseAddress property of the HttpClient instance");

            client = httpClient;

			SchemaValidation = new SchemaValidationSettings
			{
				Enabled = true,
				RaiseExceptions = true
			};
        }

        

        public virtual Orders Orders
        {
            get { return new Orders(this); }
        }
                


		public void AddDefaultRequestHeader(string name, string value)
		{
			client.DefaultRequestHeaders.Add(name, value);
		}

		public void AddDefaultRequestHeader(string name, IEnumerable<string> values)
		{
			client.DefaultRequestHeaders.Add(name, values);
		}


    }

} // end namespace




namespace OrdersClientSample.OrdersXml.Models {
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.com/IPO")]
    [System.Xml.Serialization.XmlRootAttribute("purchaseOrder", Namespace="http://www.example.com/IPO", IsNullable=false)]
    public partial class PurchaseOrderType {
        
        private AddressType[] itemsField;
        
        private ItemsChoiceType[] itemsElementNameField;
        
        private string itemField;
        
        private ItemChoiceType itemElementNameField;
        
        private ItemsType itemsField1;
        
        private System.DateTime orderDateField;
        
        private bool orderDateFieldSpecified;
        
        private string idField;
        
        private bool shippedField;
        
        private bool shippedFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("billTo", typeof(AddressType), Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("shipTo", typeof(AddressType), Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("singleAddress", typeof(AddressType), Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public AddressType[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName {
            get {
                return this.itemsElementNameField;
            }
            set {
                this.itemsElementNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("comment", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("customerComment", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("shipComment", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName {
            get {
                return this.itemElementNameField;
            }
            set {
                this.itemElementNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ItemsType items {
            get {
                return this.itemsField1;
            }
            set {
                this.itemsField1 = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
        public System.DateTime orderDate {
            get {
                return this.orderDateField;
            }
            set {
                this.orderDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderDateSpecified {
            get {
                return this.orderDateFieldSpecified;
            }
            set {
                this.orderDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool shipped {
            get {
                return this.shippedField;
            }
            set {
                this.shippedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool shippedSpecified {
            get {
                return this.shippedFieldSpecified;
            }
            set {
                this.shippedFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UKAddress))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(USAddress))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.com/IPO")]
    public partial class AddressType {
        
        private string nameField;
        
        private string streetField;
        
        private string cityField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string street {
            get {
                return this.streetField;
            }
            set {
                this.streetField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string city {
            get {
                return this.cityField;
            }
            set {
                this.cityField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.com/IPO")]
    public partial class ItemsType {
        
        private ItemsTypeItem[] itemField;
        
        private string[] textField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ItemsTypeItem[] item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text {
            get {
                return this.textField;
            }
            set {
                this.textField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.example.com/IPO")]
    public partial class ItemsTypeItem {
        
        private string productNameField;
        
        private string quantityField;
        
        private decimal uSPriceField;
        
        private string[] itemsField;
        
        private ItemsChoiceType1[] itemsElementNameField;
        
        private System.DateTime shipDateField;
        
        private bool shipDateFieldSpecified;
        
        private string partNumField;
        
        private decimal weightKgField;
        
        private bool weightKgFieldSpecified;
        
        private ItemsTypeItemShipBy shipByField;
        
        private bool shipByFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string productName {
            get {
                return this.productNameField;
            }
            set {
                this.productNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="positiveInteger")]
        public string quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal USPrice {
            get {
                return this.uSPriceField;
            }
            set {
                this.uSPriceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("comment", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("customerComment", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("shipComment", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public string[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType1[] ItemsElementName {
            get {
                return this.itemsElementNameField;
            }
            set {
                this.itemsElementNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="date")]
        public System.DateTime shipDate {
            get {
                return this.shipDateField;
            }
            set {
                this.shipDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool shipDateSpecified {
            get {
                return this.shipDateFieldSpecified;
            }
            set {
                this.shipDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string partNum {
            get {
                return this.partNumField;
            }
            set {
                this.partNumField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal weightKg {
            get {
                return this.weightKgField;
            }
            set {
                this.weightKgField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool weightKgSpecified {
            get {
                return this.weightKgFieldSpecified;
            }
            set {
                this.weightKgFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ItemsTypeItemShipBy shipBy {
            get {
                return this.shipByField;
            }
            set {
                this.shipByField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool shipBySpecified {
            get {
                return this.shipByFieldSpecified;
            }
            set {
                this.shipByFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.com/IPO", IncludeInSchema=false)]
    public enum ItemsChoiceType1 {
        
        /// <remarks/>
        comment,
        
        /// <remarks/>
        customerComment,
        
        /// <remarks/>
        shipComment,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.example.com/IPO")]
    public enum ItemsTypeItemShipBy {
        
        /// <remarks/>
        air,
        
        /// <remarks/>
        land,
        
        /// <remarks/>
        any,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.com/IPO")]
    public partial class UKAddress : AddressType {
        
        private string postcodeField;
        
        private string exportCodeField;
        
        public UKAddress() {
            this.exportCodeField = "1";
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string postcode {
            get {
                return this.postcodeField;
            }
            set {
                this.postcodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="positiveInteger")]
        public string exportCode {
            get {
                return this.exportCodeField;
            }
            set {
                this.exportCodeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.com/IPO")]
    public partial class USAddress : AddressType {
        
        private USState stateField;
        
        private string zipField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public USState state {
            get {
                return this.stateField;
            }
            set {
                this.stateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="positiveInteger")]
        public string zip {
            get {
                return this.zipField;
            }
            set {
                this.zipField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.com/IPO")]
    public enum USState {
        
        /// <remarks/>
        AK,
        
        /// <remarks/>
        AL,
        
        /// <remarks/>
        AR,
        
        /// <remarks/>
        CA,
        
        /// <remarks/>
        PA,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.com/IPO", IncludeInSchema=false)]
    public enum ItemsChoiceType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":billTo")]
        billTo,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":shipTo")]
        shipTo,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(":singleAddress")]
        singleAddress,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.com/IPO", IncludeInSchema=false)]
    public enum ItemChoiceType {
        
        /// <remarks/>
        comment,
        
        /// <remarks/>
        customerComment,
        
        /// <remarks/>
        shipComment,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.com/IPO")]
    [System.Xml.Serialization.XmlRootAttribute("orders", Namespace="http://www.example.com/IPO", IsNullable=false)]
    public partial class PurchaseOrdersType {
        
        private PurchaseOrderType[] ordersField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("orders", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PurchaseOrderType[] orders {
            get {
                return this.ordersField;
            }
            set {
                this.ordersField = value;
            }
        }
    }
}








namespace OrdersClientSample.OrdersXml.Models
{
    /// <summary>
    /// Uri Parameters for resource /{id}
    /// </summary>
    public partial class  OrdersIdUriParameters 
    {

		[JsonProperty("id")]
        public string Id { get; set; }


    } // end class

    /// <summary>
    /// Uri Parameters for resource /ship
    /// </summary>
    public partial class  OrdersIdShipUriParameters 
    {

		[JsonProperty("id")]
        public string Id { get; set; }


    } // end class

    /// <summary>
    /// Request object for method Post of class Ship
    /// </summary>
    public partial class ShipPostRequest : ApiRequest
    {
        public ShipPostRequest(OrdersIdShipUriParameters UriParameters, HttpContent Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public HttpContent Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public OrdersIdShipUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Post of class Orders
    /// </summary>
    public partial class OrdersPostRequest : ApiRequest
    {
        public OrdersPostRequest(PurchaseOrderType Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public PurchaseOrderType Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Get of class Orders
    /// </summary>
    public partial class OrdersGetRequest : ApiRequest
    {
        public OrdersGetRequest(OrdersIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public OrdersIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Put of class Orders
    /// </summary>
    public partial class OrdersPutRequest : ApiRequest
    {
        public OrdersPutRequest(OrdersIdUriParameters UriParameters, PurchaseOrderType Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public PurchaseOrderType Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public OrdersIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Response object for method Get of class Shipped
    /// </summary>

    public partial class ShippedGetResponse : ApiResponse
    {


	    private PurchaseOrdersType typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public PurchaseOrdersType Content 
    	{
	        get
	        {
		        if (typedContent != null)
			        return typedContent;

                IEnumerable<string> values = new List<string>();
                if (RawContent != null && RawContent.Headers != null)
                    RawContent.Headers.TryGetValues("Content-Type", out values);

                if (values.Any(hv => hv.ToLowerInvariant().Contains("xml")) &&
                    !values.Any(hv => hv.ToLowerInvariant().Contains("json")))
                {
                    var task = RawContent.ReadAsStreamAsync();

                    var xmlStream = task.GetAwaiter().GetResult();
                    typedContent = (PurchaseOrdersType)new XmlSerializer(typeof(PurchaseOrdersType)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<PurchaseOrdersType>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<PurchaseOrdersType>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method Get of class Notshipped
    /// </summary>

    public partial class NotshippedGetResponse : ApiResponse
    {


	    private PurchaseOrdersType typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public PurchaseOrdersType Content 
    	{
	        get
	        {
		        if (typedContent != null)
			        return typedContent;

                IEnumerable<string> values = new List<string>();
                if (RawContent != null && RawContent.Headers != null)
                    RawContent.Headers.TryGetValues("Content-Type", out values);

                if (values.Any(hv => hv.ToLowerInvariant().Contains("xml")) &&
                    !values.Any(hv => hv.ToLowerInvariant().Contains("json")))
                {
                    var task = RawContent.ReadAsStreamAsync();

                    var xmlStream = task.GetAwaiter().GetResult();
                    typedContent = (PurchaseOrdersType)new XmlSerializer(typeof(PurchaseOrdersType)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<PurchaseOrdersType>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<PurchaseOrdersType>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method Get of class Orders
    /// </summary>

    public partial class OrdersGetResponse : ApiResponse
    {


	    private PurchaseOrderType typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public PurchaseOrderType Content 
    	{
	        get
	        {
		        if (typedContent != null)
			        return typedContent;

                IEnumerable<string> values = new List<string>();
                if (RawContent != null && RawContent.Headers != null)
                    RawContent.Headers.TryGetValues("Content-Type", out values);

                if (values.Any(hv => hv.ToLowerInvariant().Contains("xml")) &&
                    !values.Any(hv => hv.ToLowerInvariant().Contains("json")))
                {
                    var task = RawContent.ReadAsStreamAsync();

                    var xmlStream = task.GetAwaiter().GetResult();
                    typedContent = (PurchaseOrderType)new XmlSerializer(typeof(PurchaseOrderType)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<PurchaseOrderType>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<PurchaseOrderType>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class


} // end Models namespace
