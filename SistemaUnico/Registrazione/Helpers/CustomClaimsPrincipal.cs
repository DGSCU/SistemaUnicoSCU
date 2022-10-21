using RegistrazioneSistemaUnico.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers
{
    public class CustomClaimsPrincipal : ClaimsPrincipal
    {
        IPrincipal principal;
        public CustomClaimsPrincipal(IPrincipal principal) : base(principal)
        {
            this.principal = principal;
        }

        public override bool IsInRole(string role)
        {
            if (principal is WindowsPrincipal windowsPrincipal)
            {
                ClaimsIdentity identity = (ClaimsIdentity)principal.Identity;

                return windowsPrincipal.Claims.Any(c => c.Type == identity.RoleClaimType && c.Value == role);
            } else {
                Logger.Logger log = new Logger.Logger("Claims Role", "IsInRole", null);
                log.Warning(LogEvent.IDENTITA_NON_VALIDA, parameters:new
                    {
                        IdentityType = principal.Identity.GetType().FullName,
                        Identity = principal.Identity
                    }); 
            }
            return false;
        }
    }
}
