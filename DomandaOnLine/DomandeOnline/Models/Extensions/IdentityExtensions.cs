using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Security.Principal;

namespace DomandeOnline.Models.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetNome(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Nome");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetCognome(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Cognome");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetIndirizzo(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Indirizzo");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetCodiceFiscale(this IIdentity identity)
        {
            Claim claims = ((ClaimsIdentity)identity).FindFirst("CodiceFiscale");

            return (claims != null) ? claims.Value : string.Empty;
        }

        public static DateTime? GetDataNascita(this IIdentity identity)
        {
            DateTime? result = null;

            Claim claims = ((ClaimsIdentity)identity).FindFirst("DataNascita");

            if (claims.Value != null)
            {
                try
                {
                    result = DateTime.Parse(claims.Value);
                }
                catch
                {
                    return null;
                }
            }

            return result;
        }
    }
}