using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RegistrazioneSistemaUnico.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using SpidService;
using RegistrazioneSistemaUnico.Data;

namespace RegistrazioneSistemaUnico.Services
{
	/// <summary>
	/// Classe che governa tutte le funzionalità relative alla gestione dei token JWT
	/// </summary>
	public class TokenService
	{
		/// <summary>
		/// Parametro di configurazione che imposta la validità in minuti del token
		/// </summary>
		private static int lifetime;
		/// <summary>
		/// Chiave di cifratura per la firma del token (viene impostata in fase di configurazione)
		/// </summary>
		public static SymmetricSecurityKey Key { get; private set; }
		/// <summary>
		/// Indica se Il gestore dei tokens è stato inizializzato
		/// </summary>
		public static bool IsInitialized { get; private set; } = false;
		/// <summary>
		/// Inizializzatore del geswtore dei token. Vengono impostati durata del token e chiave di cifratura
		/// </summary>
		/// <param name="tokenLifetimeMinutes">durata di validità in munuti del token</param>
		/// <param name="signingKey">chiave di cifratura (deve essere di almento 16 caratteri)</param>
		public static void Config(int tokenLifetimeMinutes, string signingKey){
			if (tokenLifetimeMinutes<=0)
			{
				throw new Exception("tokenLifetimeMinutes must be positive");
			}
			if (signingKey.Length<16)
			{
				throw new Exception("signingKey length must be 16 characters minimum");
			}
			lifetime = tokenLifetimeMinutes;
			Key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKey));
			IsInitialized = true;
		}
		/// <summary>
		/// Genera un token semplice con 
		/// </summary>
		/// <param name="codiceFiscale">Identificativo dell'utente</param>
		/// <param name="user">utente loggato all'applicazione</param>
		/// <returns></returns>
		public static string GenerateSimpleToken(userInfo user)
		{
			if (!IsInitialized)
			{
				throw new Exception("TokenService not initialized - use TokenServie.Config method");
			}
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.FiscalNumber)
			};
			if (user!=null)
			{
				claims.Add(new Claim("User", JsonConvert.SerializeObject(user)));
			}
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Issuer = Assembly.GetEntryAssembly().GetName().Name,
				Expires = DateTime.UtcNow.AddMinutes(lifetime),
				SigningCredentials = new SigningCredentials(Key,SecurityAlgorithms.HmacSha256Signature)
			};
			JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
			return token.RawData;
		}

		/// <summary>
		/// Genera un token JWT dalle informazioni di accesso
		/// </summary>
		/// <param name="tokenAccesso">Informazioni di accesso</param>
		/// <returns>token JWT in formato stringa</returns>
		public static string GenerateAccessToken(TokenAccesso tokenAccesso)
		{
			if (!IsInitialized)
			{
				throw new Exception("TokenService not initialized - use TokenServie.Config method");
			}
			List<Claim> claims = new List<Claim>
			{
				new Claim("Id", tokenAccesso.Id.ToString()),
				new Claim("CodiceFiscaleRappresentanteLegale", tokenAccesso.CodiceFiscale),
				new Claim("CodiceFiscaleEnte", tokenAccesso.CodiceFiscaleEnte),
				new Claim("Albo", tokenAccesso.Albo)
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Issuer = Assembly.GetEntryAssembly().GetName().Name,
				Expires = tokenAccesso.DataScadenza,
				SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature)
			};
			JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
			return token.RawData;
		}

		public class TokenAccessoInfo
		{
			private readonly bool isValid;
			private readonly bool isExpired;
			private readonly TokenAccesso token;

			public TokenAccessoInfo(bool isValid, bool isExpired, TokenAccesso token)
			{
				this.isValid = isValid;
				this.isExpired = isExpired;
				this.token = token;
			}
			public bool IsValid { get { return isValid; } }
			public bool IsExpired { get { return isExpired; } }
			public TokenAccesso Token { get { return token; } }
		}


			/// <summary>
			/// Validatore Della scadenza del token
			/// </summary>
			/// <param name="notBefore"></param>
			/// <param name="expires"></param>
			/// <param name="token"></param>
			/// <param name="params"></param>
			/// <returns></returns>
			public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
			{
				if (notBefore == null)
				{
					return false;
				}

				if (expires == null)
				{
					return false;
				}
				return notBefore < DateTime.UtcNow && expires > DateTime.UtcNow;
			}


		public static TokenAccessoInfo VerifyAccessoToken(string token)
		{
			try
			{
				JwtSecurityToken jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
				TokenAccesso tokenAccesso = new TokenAccesso()
				{
					Id = Guid.Parse(jwtToken.Claims
						.FirstOrDefault(c => c.Type == "Id")?.Value),
					CodiceFiscale = jwtToken.Claims
						.FirstOrDefault(c => c.Type == "CodiceFiscaleRappresentanteLegale")?.Value,
					CodiceFiscaleEnte = jwtToken.Claims
						.FirstOrDefault(c => c.Type == "CodiceFiscaleEnte")?.Value,
					Albo = jwtToken.Claims
						.FirstOrDefault(c => c.Type == "Albo")?.Value
				};
				return new TokenAccessoInfo(true, DateTime.UtcNow > jwtToken.ValidTo, tokenAccesso);
			}
			catch
			{
				return new TokenAccessoInfo(false, false, null);
			}
		}


		public class TokenInfo{
			private readonly bool isValid;
			private readonly bool isExpired;
			private readonly string username;
			private readonly userInfo userInfo;

			public TokenInfo(bool isValid,bool isExpired,string username,userInfo userInfo){
				this.isValid = isValid;
				this.isExpired = isExpired;
				this.username = username;
				this.userInfo = userInfo;
			}
			public bool IsValid { get { return isValid; } }
			public bool IsExpired { get { return isExpired; } }
			public string Username { get { return username; } }
			public userInfo UserInfo { get { return userInfo; } }


		}
		public static TokenInfo VerifyToken(string token){
			try
			{
				JwtSecurityToken jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
				string username = jwtToken.Claims
					.FirstOrDefault(c => c.Type == "unique_name")?.Value;

				userInfo userInfo = GetUser(token);
				return new TokenInfo(true, DateTime.UtcNow > jwtToken.ValidTo, username,userInfo);
			}
			catch{
				return new TokenInfo(false,false,null,null);
			}
		}

		public static userInfo GetUser(string token){
			JwtSecurityToken jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
			var payload = jwtToken.Payload["User"];
			if (payload==null)
			{
				return null;
			}
			if (payload is string userInfo)
			{
				return JsonConvert.DeserializeObject<userInfo>(userInfo);

			}
			return null;
		}
	}
}
