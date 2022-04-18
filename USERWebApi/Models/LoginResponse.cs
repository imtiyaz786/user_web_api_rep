namespace USERWebApi.Models
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Jwt { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
