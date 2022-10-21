using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace RegistrazioneSistemaUnico.Helpers
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
           /*if (principal?.Identity?.Name!=null)
            {
				try
				{
					Utente utente = new DataAccess<Utente>().GetByName(principal.Identity.Name);
					if (utente != null)
					{
						ClaimsIdentity identity = (ClaimsIdentity)principal.Identity;

						identity.AddClaim(new Claim("Id", utente.Id.ToString()));
						foreach (var role in utente.Ruoli)
						{
							var roleClaim = new Claim(identity.RoleClaimType, role.Name);
							identity.AddClaim(roleClaim);
						}
					}
				}
				catch (Exception exception)
				{
					Logger.Logger log = new Logger.Logger("Claims", null);
					log.Error("Errore nell'autenticazione", exception, null);
				}
            }*/
            return new CustomClaimsPrincipal(principal);
        }
    }
}
