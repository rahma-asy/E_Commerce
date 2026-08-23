  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Comman
{
    public class PaymentGatewaySettings
    {
        public string DefultCurrency { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public string WebhookSecret {  get; set; } = default!;  
    }
}
