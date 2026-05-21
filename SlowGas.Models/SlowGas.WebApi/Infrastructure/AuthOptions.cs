using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SlowGas.WebApi.Infrastructure
{
    public class AuthOptions
    {
        public const string ISSUER = "SlowGas_AuthServer";
        public const string AUDIENCE = "SlowGas_AuthClient";
        private const string KEY = "slowgas_secret_key_for_jwt_authentication_123!";

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
            => new(Encoding.UTF8.GetBytes(KEY));
    }
}