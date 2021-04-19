using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Book.Models
{
    public class CoverType
    {
        [Key] public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        [DisplayName("Cover Name")]
        public string Name { get; set; }
    }
}