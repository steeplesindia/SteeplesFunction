using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SteeplesFunction.Service;
using SteeplesFunction.Models;

namespace SteeplesFunction
{
    public static class PGPFunction
    {
        [FunctionName("Encrypt")]
        public static async Task<IActionResult> Encrypt(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("PGP encryption...");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string partnerPublicKey = req.Query["PartnerPublicKey"];
            string myPrivateKey = req.Query["MyPrivateKey"];
            string password = req.Query["Password"];
            string plainText = req.Query["PlainText"];
            partnerPublicKey = partnerPublicKey ?? data?.PartnerPublicKey;
            myPrivateKey = myPrivateKey ?? data?.MyPrivateKey;
            password = password ?? data?.Password;
            plainText = plainText ?? data?.PlainText;

            if (partnerPublicKey == null)
                return new BadRequestObjectResult("Please pass 'PartnerPublicKey' in the request body.");
            if (myPrivateKey == null)
                return new BadRequestObjectResult("Please pass 'MyPrivateKey' in the request body.");
            if (password == null)
                return new BadRequestObjectResult("Please pass 'Password' in the request body.");
            if (plainText == null)
                return new BadRequestObjectResult("Please pass 'PlainText' in the request body.");
            PGPHelper helper = new PGPHelper(partnerPublicKey, myPrivateKey, "", password);
            return new OkObjectResult(await helper.encryptAsync(plainText));
        }

        [FunctionName("Decrypt")]
        public static async Task<IActionResult> Decrypt(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("PGP decryption...");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string partnerPublicKey = req.Query["PartnerPublicKey"];
            string myPrivateKey = req.Query["MyPrivateKey"];
            string password = req.Query["Password"];
            string encryptedText = req.Query["EncryptedText"];
            partnerPublicKey = partnerPublicKey ?? data?.PartnerPublicKey;
            myPrivateKey = myPrivateKey ?? data?.MyPrivateKey;
            password = password ?? data?.Password;
            encryptedText = encryptedText ?? data?.EncryptedText;

            if (partnerPublicKey == null)
                return new BadRequestObjectResult("Please pass 'PartnerPublicKey' in the request body.");
            if (myPrivateKey == null)
                return new BadRequestObjectResult("Please pass 'MyPrivateKey' in the request body.");
            if (password == null)
                return new BadRequestObjectResult("Please pass 'Password' in the request body.");
            if (encryptedText == null)
                return new BadRequestObjectResult("Please pass 'EncryptedText' in the request body.");
            PGPHelper helper = new PGPHelper(partnerPublicKey, myPrivateKey, "", password);
            return new OkObjectResult(await helper.decryptAsync(encryptedText));
        }
    }
}
