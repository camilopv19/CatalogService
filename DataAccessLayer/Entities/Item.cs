using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class Item
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }

    public class ItemResponse : Item
    {
        public ItemResponse(Item item)
        {
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            Image = item.Image;
            CategoryId = item.CategoryId;
            Price = item.Price;
            Amount = item.Amount;
            Links = new List<LinkDto>();
        }
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }

}
