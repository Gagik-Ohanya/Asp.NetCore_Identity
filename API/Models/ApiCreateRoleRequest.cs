using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class ApiCreateRoleRequest
    {
        [Required]
        public string RoleName { get; set; }
    }
}