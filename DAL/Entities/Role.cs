using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required, Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
    }
}