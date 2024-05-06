using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeplesFunction.Models
{
    public class PGPResponse
    {
        public string EncryptedText { get; set; }
        public string DecryptedText { get; set; }
        public bool Error { get; set; }
        public string ErrorMsg { get; set; }
    }
}
