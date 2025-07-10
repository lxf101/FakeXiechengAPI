using FakeXiechengAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace FakeXiechengAPI.Dtos
{
    public class ShoppingCartDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ICollection<LineItem> ShoppingCartItems { get; set; }
    }
}
