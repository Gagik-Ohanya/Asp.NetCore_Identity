using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        [Required, Column(TypeName = "varchar(50)")]
        public string Username { get; set; }
        
        [Required, Column(TypeName = "varchar(50)")]
        public string Email { get; set; }
        
        public bool EmailConfirmed { get; set; }
        
        [Required, Column(TypeName = "nvarchar(MAX)")]
        public string PasswordHash { get; set; }
        
        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }
        
        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }
        
        public int Gender { get; set; }
    }
}