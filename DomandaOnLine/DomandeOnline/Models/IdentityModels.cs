using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DomandeOnline.Models
{
    // È possibile aggiungere dati del profilo per l'utente aggiungendo altre proprietà alla classe ApplicationUser. Per altre informazioni, vedere https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenere presente che il valore di authenticationType deve corrispondere a quello definito in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Aggiungere qui i reclami utente personalizzati
            userIdentity.AddClaim(new Claim("Nome", this.Nome ?? string.Empty));
            userIdentity.AddClaim(new Claim("Cognome", this.Cognome ?? string.Empty));
            userIdentity.AddClaim(new Claim("Indirizzo", this.Indirizzo??string.Empty));
            userIdentity.AddClaim(new Claim("CodiceFiscale", this.CodiceFiscale ?? string.Empty));
            userIdentity.AddClaim(new Claim("DataNascita", this.DataNascita.HasValue ? this.DataNascita.Value.ToString("yyyy-MM-dd") : string.Empty));

            return userIdentity;
        }

		public string CodiceFiscale { get; set; } //FiscalNumber
		public string Nome { get; set; }//Name
		public string Cognome { get; set; }//Surname
		public string Spidcode { get; set; }
		public string Genere { get; set; }//Gender
		public string LuogoNascita { get; set; }//PlaceOfBirth
		public string NazioneNascita { get; set; }//CountyOfBirth
		public string Telefono { get; set; }//MobilePhone
		public string Indirizzo { get; set; }//Address
		public string Documento { get; set; }//IdCard
		public string ExpirationDate { get; set; }//ExpirationDate
		public DateTime? DataNascita { get; set; }//DateOfBirth
		public string Cittadinanza { get; set; }
	}

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}