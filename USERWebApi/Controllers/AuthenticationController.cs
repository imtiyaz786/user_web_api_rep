using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using USERWebApi.Models;
using USERWebApi.Repository;

namespace USERWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<RegistrationModel> userManager;
        private readonly RoleManager <IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly IRepository<Doctor> DoctorsRepository;
        private readonly IRepository<Patient> PatientsRepository;

        public AuthenticationController(UserManager<RegistrationModel> userManager,RoleManager<IdentityRole>roleManager,IConfiguration configuration, IRepository<Doctor> DoctorsRepository, IRepository<Patient> PatientsRepository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager; 
            this.configuration = configuration; 
            this.DoctorsRepository = DoctorsRepository;
            this.PatientsRepository = PatientsRepository;
        }

        [HttpPost]
        [Route("registerPatient")]
        public async Task<IActionResult> Register(RegistrationPatientModel model)
        {
            RegistrationModel appUser = new RegistrationModel
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Category = "Patient",

            };

            IdentityResult result = await userManager.CreateAsync(appUser,model.Password);

            if (!result.Succeeded) return BadRequest("User not Registered");

            if (!await roleManager.RoleExistsAsync("Patient"))
            await roleManager.CreateAsync(new IdentityRole { Name = "Patient" });

            result = await userManager.AddToRoleAsync(appUser, "Patient");

            Patient patient = new Patient
            {
                RegistrationModel = appUser,
                UserName = model.UserName
            };


            PatientsRepository.Add(patient);
            await PatientsRepository.SaveAsync();

            if (!result.Succeeded) return BadRequest("Role Not Added for current user");
            return Ok();
        }

        [HttpPost]
        [Route("registerDoctor")]
        public async Task<IActionResult> Register(RegistrationDoctorModel model)
        {

            RegistrationModel appUser = new RegistrationModel
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Category = model.Category,  
            };
            IdentityResult result = await userManager.CreateAsync(appUser,model.Password);
            
            if (!result.Succeeded) return BadRequest("User not Registered");
            
            if (!await roleManager.RoleExistsAsync("Doctor"))
                await roleManager.CreateAsync(new IdentityRole { Name = "Doctor" });
            
            result = await userManager.AddToRoleAsync(appUser, "Doctor");

            Doctor doctor = new Doctor
            {
                RegistrationModel = appUser,
                Category = model.Category,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName
            };

            DoctorsRepository.Add(doctor);
            await DoctorsRepository.SaveAsync();

            if (!result.Succeeded) return BadRequest("Role Not Added for current user");
            
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult>Login(LoginModel model)
        {
            RegistrationModel appUser = await userManager.FindByNameAsync(model.UserName);

            if (appUser == null) return BadRequest("User Not Found");
            
            bool isValid = await userManager.CheckPasswordAsync(appUser,model.Password);
            
            if (!isValid) return BadRequest("Password Not Matched");
            
            string key = configuration["JWT:Key"];
            string issuer = configuration["JWT:Issuer"];
            string audience = configuration["JWT:Audience"];

            IList<Claim> userClaims = await userManager.GetClaimsAsync(appUser);
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, appUser.UserName));

            var roles=await userManager.GetRolesAsync(appUser);  
            foreach(var role in roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            byte[] KeyBytes = System.Text.Encoding.ASCII.GetBytes(key);
            SecurityKey securityKey = new SymmetricSecurityKey(KeyBytes);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (
                issuer: issuer,
                audience: audience,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signingCredentials,
                claims: userClaims
                );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(jwtSecurityToken);

            var response = new LoginResponse { Jwt = token, Name = appUser.UserName, Role = roles.First(), UserId = 0 };

            if (response.Role == "Patient")
            {
                response.UserId = await PatientsRepository.GetByUserId(appUser.Id);
            }
            if (response.Role == "Doctor")
            {
                response.UserId = await DoctorsRepository.GetByUserId(appUser.Id);
            }
            return Ok(response);
        }
    }
}