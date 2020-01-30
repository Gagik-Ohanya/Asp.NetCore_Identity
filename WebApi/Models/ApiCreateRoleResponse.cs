namespace WebApi.Models
{
    public class ApiCreateRoleResponse : ApiResponseBase
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}