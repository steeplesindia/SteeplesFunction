using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeplesFunction.Models
{
    public class PGPRequest
    {
        public string MyPublicKey { get; set; }
        public string MyPrivateKey { get; set; }
        public string PartnerPublicKey { get; set; }
        public string PartnerPrivateKey { get; set; }
    }
}
