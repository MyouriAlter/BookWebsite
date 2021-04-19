using System.ComponentModel.DataAnnotations;

namespace Book.Models
{
    public class Category
    {
        [Key] public int Id { get; set; }

        [Display(Name = "Category Name")]
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }
    }
}