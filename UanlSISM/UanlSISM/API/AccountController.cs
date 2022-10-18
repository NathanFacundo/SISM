using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.IdentityModel.Tokens;
using UanlSISM.Models;
using System.Linq;

namespace UanlSISM.API
{

    public class AccountController : ApiController
    {

        [Route("api/Account/Login")]
        [HttpPost]
        public async Task<IHttpActionResult> Login(LoginViewModel LoginParams)
        {
            var Logged = await UserLogin(LoginParams);

            if (Logged != null)
            {
                return Ok(Logged);
            }

            else
            {
                return BadRequest();
            }

        }


        public async Task<Object> UserLogin([FromBody] LoginViewModel model)
        {

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var _configuration = System.Configuration.ConfigurationManager.AppSettings;

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user.Id);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Issuer"],
                    audience: _configuration["Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );


                return new
                {
                    Email = user.Email,
                    Password = user.PasswordHash,
                    Role = userRoles
                };
            }
            return null;
        }


        [Route("api/Account/GetDhabiente")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDhabiente(string id)
        {
            var dhabiente =  GetDhabienteInfo(id);

            if (dhabiente != null)
            {
                return Ok(dhabiente);
            }

            else
            {
                return BadRequest();
            }

        }


        public DHABIENTES GetDhabienteInfo(string id)
        {
            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();

            var dhabientes = (from a in db.DHABIENTES
                                where a.NUMEMP == id
                                select a).FirstOrDefault();

            return dhabientes;
        }

    }
}
