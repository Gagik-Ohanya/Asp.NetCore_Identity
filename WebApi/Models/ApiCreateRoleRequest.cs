using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class ApiCreateRoleRequest
    {
        [Required]
        public string RoleName { get; set; }
    }
}