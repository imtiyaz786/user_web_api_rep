using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USERWebApi.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]  
        public string RegistrationModelId { get; set; }
        [ForeignKey("RegistrationModelId")]
        public RegistrationModel RegistrationModel { get; set; }
    }
}
