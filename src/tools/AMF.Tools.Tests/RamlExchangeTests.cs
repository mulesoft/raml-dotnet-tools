using NUnit.Framework;
using RAML.Parser;
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
            Assert.AreEqual(25, model.Objects.Count());
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
            Assert.IsTrue(model.Objects.FirstOrDefault(o => o.Name == "IdentifierMap") != null);
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
            Assert.AreEqual(4, model.Controllers.Count());
        }

        [Test]
        public async Task anypoint_clinic_experience_api_raml()
        {
            var model = await BuildModel("exchange/anypoint-clinic-experience-api-1.0.0-raml/web-portal-experience-api.raml");
            Assert.AreEqual(5, model.Controllers.Count());
        }

        [Test]
        public async Task api_designer_experience_api_raml()
        {
            var model = await BuildModel("exchange/api-designer-experience-api-1.0.0-raml/api.raml");
            Assert.AreEqual(4, model.Controllers.Count());
        }

        [Test]
        public async Task appointments_process_api_raml()
        {
            var model = await BuildModel("exchange/appointments-process-api-1.0.0-raml/appointments-process-api.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task atm_and_branch_location_api_raml()
        {
            var model = await BuildModel("exchange/atm-and-branch-location-api-1.0.0-raml/financialservices-locations.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task box_api_raml()
        {
            var model = await BuildModel("exchange/box-api-1.0.0-raml/api.raml");
            Assert.AreEqual(12, model.Controllers.Count());
        }

        [Test]
        public async Task cardconnect_rest_api_raml()
        {
            var model = await BuildModel("exchange/cardconnect-rest-api-1.0.5-raml/cardconnect-rest-api.raml");
            Assert.AreEqual(12, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_healthcare_appointment_api_raml()
        {
            var model = await BuildModel("exchange/catalyst-healthcare-appointment-api-2.0.1-raml/healthcare_appointment_process_api.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_healthcare_ehr_to_crm_papi_raml()
        {
            var model = await BuildModel("exchange/catalyst-healthcare-ehr-to-crm-papi-2.0.1-raml/healthcare_ehr2crm_sync_process_api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_healthcare_fitbit_sapi_raml()
        {
            var model = await BuildModel("exchange/catalyst-healthcare-fitbit-sapi-2.0.1-raml/healthcare_fitbit2fhir_system_api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_healthcare_fitness_papi_raml()
        {
            var model = await BuildModel("exchange/catalyst-healthcare-fitness-papi-2.0.1-raml/healthcare_fitness2crm_process_api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_healthcare_onboarding_api_raml()
        {
            var model = await BuildModel("exchange/catalyst-healthcare-onboarding-api-2.0.1-raml/healthcare_onboarding_process_api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_healthcare_portal_api_raml()
        {
            var model = await BuildModel("exchange/catalyst-healthcare-portal-api-2.0.0-raml/healthcare_portal_experience_api.raml");
            Assert.AreEqual(5, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_retail_inventory_api_raml()
        {
            var model = await BuildModel("exchange/catalyst-retail-inventory-api-2.0.1-raml/retail_inventory_system_api.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_retail_location_api_raml()
        {
            var model = await BuildModel("exchange/catalyst-retail-location-api-2.0.1-raml/retail_locations_system_api.raml");
            Assert.AreEqual(3, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_retail_notification_api_raml()
        {
            var model = await BuildModel("exchange/catalyst-retail-notification-api-2.0.1-raml/retail_notifications_system_api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_retail_product_api_raml()
        {
            var model = await BuildModel("exchange/catalyst-retail-product-api-2.0.1-raml/retail_product_system_api.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task catalyst_retail_product_availabilit_raml()
        {
            var model = await BuildModel("exchange/catalyst-retail-product-availabilit-2.0.1-raml/retail_product_availability_process_api.raml");
            Assert.AreEqual(3, model.Controllers.Count());
        }

        [Test]
        public async Task crm_fhir_system_api_raml()
        {
            var model = await BuildModel("exchange/crm-fhir-system-api-2.0.1-raml/healthcare_sfhc2fhir_system_api.raml");
            Assert.AreEqual(8, model.Controllers.Count());
        }

        [Test]
        public async Task customer_api_for_visual_editing_raml()
        {
            var model = await BuildModel("exchange/customer-api-for-visual-editing-1.0.0-raml/customer-api-for-visual-editing.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task ehr_fhir_system_api_raml()
        {
            var model = await BuildModel("exchange/ehr-fhir-system-api-2.0.1-raml/healthcare_ehr2fhir_system_api.raml");
            Assert.AreEqual(7, model.Controllers.Count());
        }

        [Test]
        public async Task fhir_apis_raml()
        {
            var model = await BuildModel("exchange/fhir-apis-2.0.0-raml/fhir_api.raml");
            Assert.AreEqual(12, model.Controllers.Count());
        }

        [Test]
        public async Task fitness_fhir_system_api_raml()
        {
            var model = await BuildModel("exchange/fitness-fhir-system-api-1.0.0-raml/healthcare-fitbit2fhir-system-api.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task github_api_raml()
        {
            var model = await BuildModel("exchange/github-api-1.0.0-raml/api.raml");
            Assert.AreEqual(19, model.Controllers.Count());
        }

        [Test]
        public async Task google_contacts_api_raml()
        {
            var model = await BuildModel("exchange/google-contacts-api-1.0.0-raml/api.raml");
            Assert.AreEqual(3, model.Controllers.Count());
        }

        [Test]
        public async Task google_drive_api_raml()
        {
            var model = await BuildModel("exchange/google-drive-api-1.0.0-raml/api.raml");
            Assert.AreEqual(6, model.Controllers.Count());
        }

        [Test]
        public async Task linkedin_api_raml()
        {
            var model = await BuildModel("exchange/linkedin-api-1.0.0-raml/api.raml");
            Assert.AreEqual(10, model.Controllers.Count());
        }

        [Test]
        public async Task mule_twilio_connector_raml()
        {
            var model = await BuildModel("exchange/mule-twilio-connector-3.0.0-raml/twilio-connector.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task new_relic_api_raml()
        {
            var model = await BuildModel("exchange/new-relic-api-2.0.3-raml/new-relic-api.raml");
            Assert.AreEqual(12, model.Controllers.Count());
        }

        [Test]
        public async Task nexmo_messages_api_raml()
        {
            var model = await BuildModel("exchange/nexmo-messages-api-1.0.0-raml/nexmo_messages.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task nexmo_sms_api_raml()
        {
            var model = await BuildModel("exchange/nexmo-sms-api-2.0.4-raml/nexmo-sms-api.raml");
            Assert.AreEqual(6, model.Controllers.Count());
        }

        [Test]
        public async Task open_bank_system_api_raml()
        {
            var model = await BuildModel("exchange/open-bank-system-api-1.0.1-raml/api.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task optymyze_api_raml()
        {
            var model = await BuildModel("exchange/optymyze-api-1.0.5-raml/optymyze-api.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task pega_api_raml()
        {
            var model = await BuildModel("exchange/pega-api-2.0.8-raml/pega-api-v1.raml");
            Assert.AreEqual(9, model.Controllers.Count());
        }

        [Test]
        public async Task pokitdok_pharmacy_coverage_api_raml()
        {
            var model = await BuildModel("exchange/pokitdok-pharmacy-coverage-api-4.0.0-raml/pokitdok-pharmacy-coverage-api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task salesforce_raml_raml()
        {
            var model = await BuildModel("exchange/salesforce-raml-1.0.0-raml/api.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task stibo_api_raml()
        {
            var model = await BuildModel("exchange/stibo-api-1.0.0-raml/STEP REST API V2.raml");
            Assert.AreEqual(11, model.Controllers.Count());
        }

        [Test]
        public async Task stripe_api_raml()
        {
            var model = await BuildModel("exchange/stripe-api-1.0.0-raml/api.raml");
            Assert.AreEqual(13, model.Controllers.Count());
        }

        [Test]
        public async Task tutorial_cookbook_raml_raml()
        {
            var model = await BuildModel("exchange/tutorial-cookbook-raml-1.0.0-raml/api.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task twitter_api_raml()
        {
            var model = await BuildModel("exchange/twitter-api-1.0.0-raml/api.raml");
            Assert.AreEqual(17, model.Controllers.Count());
        }

        [Test]
        public async Task workiva_wdesk_spreadsheets_api_raml()
        {
            var model = await BuildModel("exchange/workiva-wdesk-spreadsheets-api-1.0.0-raml/workiva-wdesk-spreadsheets-api.raml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task yammer_raml_raml()
        {
            var model = await BuildModel("exchange/yammer-raml-1.0.0-raml/api.raml");
            Assert.AreEqual(16, model.Controllers.Count());
        }

        [Test]
        public async Task zendesk_api_raml()
        {
            var model = await BuildModel("exchange/zendesk-api-1.0.0-raml/api.raml");
            Assert.AreEqual(81, model.Controllers.Count());
        }

        [Test]
        public async Task zuora_raml_raml()
        {
            var model = await BuildModel("exchange/zuora-raml-1.0.0-raml/api.raml");
            Assert.AreEqual(9, model.Controllers.Count());
        }

        private async Task<WebApiGeneratorModel> BuildModel(string ramlFile)
        {
            IncrementTestCount();
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", string.Empty));
            var fullPath = Path.Combine(path + "\\", ramlFile.Replace("/","//"));
            var fi = new FileInfo(fullPath);
            var raml = await new RamlParser().Load(fi.FullName);
            var model = new WebApiGeneratorService(raml, "TargetNamespace", "TargetNamespace.Models").BuildModel();
            return model;
        }
    }
}