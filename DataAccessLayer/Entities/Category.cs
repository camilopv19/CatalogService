using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        public int? ParentCategoryId { get; set; }

        public Category(int id)
        {
            Id = id;
            Name = "No Category was Found";
        }
    }
}
