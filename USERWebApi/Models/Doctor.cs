using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USERWebApi.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; } 
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string? Category { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        public string RegistrationModelId { get; set; } 
        [ForeignKey("RegistrationModelId")]
        public RegistrationModel RegistrationModel { get; set; }    
    }
}
