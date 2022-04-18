using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using USERWebApi.Models;
using USERWebApi.Repository;
namespace USERWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetrieveController : ControllerBase
    {
        private readonly IRepository<Doctor> DoctorsRepository;
        private readonly IRepository<Patient> PatientRepository;
        public RetrieveController(IRepository<Doctor> DoctorsRepository, IRepository<Patient> PatientRepository)
        {
            this.PatientRepository = PatientRepository; 
            this.DoctorsRepository = DoctorsRepository;
        }
        
        [HttpGet("GetDoctors")]
        public async Task<IActionResult> GetDoctors()
        {
            List<Doctor> doctor = await DoctorsRepository.GetAll("RegistrationModel");
            return Ok(doctor);
        }
        [HttpGet("GetPatients")]
        public async Task<IActionResult> GetPatients()
        {
            List<Patient> patient = await PatientRepository.GetAll("RegistrationModel");
            return Ok(patient);
        }
    }
}
