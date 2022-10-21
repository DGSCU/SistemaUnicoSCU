using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futuro
{

    public class GuidSessionIDManager
    {
        public string CreateSessionID(HttpContext context)
        {
            return Guid.NewGuid().ToString();
        }

        public bool Validate(string id)
        {
            try
            {
                Guid testGuid = new Guid(id);
                if (id == testGuid.ToString())
                    return true;
            }
            catch
            {
            }

            return false;
        }
    }

}
