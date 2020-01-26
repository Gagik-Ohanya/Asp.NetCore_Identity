namespace WebApi.Models
{
    public class ApiLoginResponse : ApiResponseBase
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}