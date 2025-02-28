using System.ComponentModel.DataAnnotations;

namespace MyFirstAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public String? Name { get; set; }
        [Required]        
        public  String? Email { get; set; }
    }
}
