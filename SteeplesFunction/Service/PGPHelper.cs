using PgpCore;
using SteeplesFunction.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeplesFunction.Service
{
    internal class PGPHelper
    {
        public string PartnerPublicKey { get; set; }
        public string MyPrivateKey { get; set; }
        public string PartnerPrivateKey { get; set; }
        public string Password { get; set; }         

        public PGPHelper(string partnerPublicKey, string myPrivateKey, string partnerPrivateKey, string password)
        {
            this.PartnerPublicKey= partnerPublicKey;
            this.MyPrivateKey= myPrivateKey;
            this.PartnerPrivateKey= partnerPrivateKey;
            this.Password= password;
        }
        public async Task<PGPResponse> encryptAsync(string plainText)
        {
            PGPResponse response = new PGPResponse();
            try
            {
                EncryptionKeys encryptionKeys = new EncryptionKeys(PartnerPublicKey, MyPrivateKey, Password);
                PGP pgp = new PGP(encryptionKeys);
                response.EncryptedText = await pgp.EncryptAndSignAsync(plainText);                
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorMsg = ex.Message;
            }
            return response;
        }

        public async Task<PGPResponse> decryptAsync(string encryptedText)
        {
            PGPResponse response = new PGPResponse();
            try
            {
                EncryptionKeys encryptionKeys = new EncryptionKeys(MyPrivateKey, Password);
                PGP pgp = new PGP(encryptionKeys);
                response.DecryptedText = await pgp.DecryptAsync(encryptedText);
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorMsg = ex.Message;
            }
            return response;
        }
    }
}
