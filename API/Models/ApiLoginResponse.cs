namespace API.Models
{
    public class ApiLoginResponse : ApiResponseBase
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}