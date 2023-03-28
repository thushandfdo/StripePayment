using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using Stripe;

namespace StripePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public class Item
        {
            [JsonProperty("id")]
            public string Id { get; set; } = string.Empty;
        }

        public class PaymentIntentCreateRequest
        {
            [JsonProperty("items")]
            public Item[] Items { get; set; }
        }

        [HttpPost("Stripe")]
        public ActionResult Create(PaymentIntentCreateRequest request)
        {
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = 1000, // This value should be calculated -> $10.00
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            });

            //return Json(new { clientSecret = paymentIntent.ClientSecret });

            // Serialize the client secret into a JSON object with a "clientSecret" property.
            var responseObject = new { clientSecret = paymentIntent.ClientSecret };
            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(responseObject);

            // Return the JSON response.
            return Content(jsonResponse, "application/json");
        }
    }
}
