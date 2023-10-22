using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
