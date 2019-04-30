using NUnit.Framework;
using AMF.Parser;
using System.Linq;
using System.Threading.Tasks;
using AMF.Tools.Core.WebApiGenerator;
using AMF.Tools.Core;
using System.IO;

namespace Raml.Tools.Tests
{
    [TestFixture]
    public class RamlExchangeTests
    {
        public int TestCount = 0;
        private void IncrementTestCount()
        {
            TestCount++;
        }

        [Test]
        public async Task banking_aisp_experience_api()
        {
            var model = await BuildModel("exchange/account-information-api-2.0.2-raml/banking_aisp_experience_api.raml");
            Assert.AreEqual(3, model.Controllers.Count());
            Assert.AreEqual(3, model.ResponseObjects.Count());
        }

        [Test]
        public async Task appian_api_raml()
        {
            var model = await BuildModel("exchange/appian-api-1.0.5-raml/api.raml");
            Assert.AreEqual(6, model.Controllers.Count());
            Assert.IsTrue(model.Controllers.All(c => c.Methods.All(m => m.SecuredBy.First() == "basic")));
            Assert.AreEqual(2, model.Controllers.First(c => c.Name == "ProcessModels").Methods.First().QueryParameters.Count);
        }

        [Test]
        public async Task catalyst_retail_partners_api_raml()
        {
            var model = await BuildModel("exchange/catalyst-retail-partners-api-1.0.1-raml/api.raml");
            Assert.AreEqual(3, model.Controllers.Count());
            Assert.AreEqual(3, model.Controllers.First(c => c.Name == "ProductSearch").Methods.First().QueryParameters.Count);
        }

        [Test]
        public async Task devrel_quick_start_product_api_raml()
        {
            var model = await BuildModel("exchange/devrel-quick_start-product_api-1.0.8-raml/devrel-quick_start_products_api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
            Assert.IsTrue(model.Controllers.All(c => c.Methods.All(m => m.SecuredBy.First() == "basic")));
        }

        [Test]
        public async Task here_geocoder_api_autocomplete_raml()
        {
            var model = await BuildModel("exchange/here-geocoder-api-autocomplete-1.0.1-raml/GeocoderAutocomplete.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task here_geocoder_api_batch_raml()
        {
            var model = await BuildModel("exchange/here-geocoder-api-batch-1.0.1-raml/BatchGeocoder.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task here_geocoder_api_forward_raml()
        {
            var model = await BuildModel("exchange/here-geocoder-api-forward-1.0.1-raml/Geocoder_API.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task here_geocoder_api_reverse_raml()
        {
            var model = await BuildModel("exchange/here-geocoder-api-reverse-1.0.1-raml/Geocoder_reverse.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task paypal_orders_api_raml()
        {
            var model = await BuildModel("exchange/paypal-orders-api-1.0.0-raml/orders_v2_api-spec.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task paypal_payments_api_oas()
        {
            var model = await BuildModel("exchange/paypal-payments-api-1.0.0-oas/api.json");
            Assert.AreEqual(3, model.Controllers.Count());
        }

        [Test]
        public async Task paypal_payments_api_raml()
        {
            var model = await BuildModel("exchange/paypal-payments-api-1.0.0-raml/payments_v2_api-spec.raml");
            Assert.AreEqual(3, model.Controllers.Count());
        }

        [Test]
        public async Task quick_start_nto_orders_api_raml()
        {
            var model = await BuildModel("exchange/quick-start-nto-orders-api-1.0.0-raml/nto-orders-api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task training_american_flights_api_oas()
        {
            var model = await BuildModel("exchange/training-american-flights-api-1.0.4-oas/api.json");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task training_american_flights_api_raml()
        {
            var model = await BuildModel("exchange/training-american-flights-api-1.0.4-raml/american-flights-api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task account_aggregation_process_api_raml()
        {
            var model = await BuildModel("exchange/account-aggregation-process-api-2.0.0-raml/banking_accounts_process_api.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task anypoint_bank_experience_api_raml()
        {
            var model = await BuildModel("exchange/anypoint-bank-experience-api-2.0.1-raml/Banking_Portal_Experience_API.raml");
            Assert.AreEqual(4, model.Controllers.Count());
        }

        [Test]
        public async Task authorization_server_raml()
        {
            var model = await BuildModel("exchange/authorization-server-2.0.0-raml/banking_authorization_server.raml");
            Assert.AreEqual(8, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_banking_as400_system_api_raml()
        {
            var model = await BuildModel("exchange/catalyst-banking-as400-system-api-2.0.0-raml/banking_as400_system_api.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_banking_payment_api_raml()
        {
            var model = await BuildModel("exchange/catalyst-banking-payment-api-2.0.0-raml/banking_payment_process_api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_retail_customer_onboarding_raml()
        {
            var model = await BuildModel("exchange/catalyst-retail-customer-onboarding-2.0.2-raml/retail-onboarding-process-api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_retail_customer_system_api_raml()
        {
            var model = await BuildModel("exchange/catalyst-retail-customer-system-api-2.0.2-raml/retail-customers2sfdc-system-api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_retail_fulfilment_process_raml()
        {
            var model = await BuildModel("exchange/catalyst-retail-fulfilment-process-2.0.2-raml/retail_order_fulfilment_process_api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_retail_omnichannel_xp_api_raml()
        {
            var model = await BuildModel("exchange/catalyst-retail-omnichannel-xp-api-2.0.3-raml/api.raml");
            Assert.AreEqual(8, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_retail_order_system_api_raml()
        {
            var model = await BuildModel("exchange/catalyst-retail-order-system-api-2.0.2-raml/retail-orders2sfdc-system-api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_retail_partner_system_api_raml()
        {
            var model = await BuildModel("exchange/catalyst-retail-partner-system-api-2.0.1-raml/retail_partners_system_api.raml");
            Assert.AreEqual(3, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_retail_payment_process_raml()
        {
            var model = await BuildModel("exchange/catalyst-retail-payment-process-2.0.2-raml/retail_payment_process_api.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_retail_shopping_cart_proc_raml()
        {
            var model = await BuildModel("exchange/catalyst-retail-shopping-cart-proc-2.0.2-raml/retail_shopping_cart_process_api.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task customer_system_api_raml()
        {
            var model = await BuildModel("exchange/customer-system-api-2.0.1-raml/banking_contact_system_api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task payment_initiation_api_raml()
        {
            var model = await BuildModel("exchange/payment-initiation-api-2.0.1-raml/banking_pisp_experience_api.raml");
            Assert.AreEqual(5, model.Controllers.Count());
        }

        [Test]
        public async Task qoppa_api_raml()
        {
            var model = await BuildModel("exchange/qoppa-api-2.0.0-raml/qoppapdf.raml");
            Assert.AreEqual(33, model.Controllers.Count());
        }


        private async Task<WebApiGeneratorModel> BuildModel(string ramlFile)
        {
            IncrementTestCount();
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", string.Empty));
            var fullPath = Path.Combine(path + "\\", ramlFile.Replace("/","//"));
            var fi = new FileInfo(fullPath);
            var raml = await new AmfParser().Load(fi.FullName);
            var model = new WebApiGeneratorService(raml, "TargetNamespace", "TargetNamespace.Models").BuildModel();
            return model;
        }
    }
}