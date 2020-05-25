using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var cert = new X509Certificate2("some.pfx", "somesecret");
            var signinCred = new SigningCredentials(new X509SecurityKey(cert), SecurityAlgorithms.RsaSha256);

            var header = GetHeader(signinCred);
            
            var payload = new JwtPayload(
                "aim",
                null,
                null,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1));
            
            var handler = new JwtSecurityTokenHandler();
            var result = handler.WriteToken(new JwtSecurityToken(header, payload));

            Console.WriteLine(result);
            
        }

        public static JwtHeader GetHeader(SigningCredentials signingCredentials)
        {
            var header = new JwtHeader(signingCredentials);

            if (signingCredentials.Key is X509SecurityKey x509key)
            {
                var cert = x509key.Certificate;
                
                header["x5t"] = Base64Url.Encode(cert.GetCertHash());
            }

            header["typ"] = "at+jwt";

            return header;
        }
    }
}