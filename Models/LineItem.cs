using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeXiechengAPI.Models
{
    // LineItem  是处理购物车、订单和价格的联动的一个中间概念。
    public class LineItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   // Id 自增长
        public int Id { get; set; }
        [ForeignKey("TouristRouteId")]
        public Guid TouristRouteId { get; set; }
        public TouristRoute TouristRoute { get; set; }
        public Guid? ShoppingCartId { get; set; }
        [Column(TypeName = "decimal(18, 2")]
        public decimal OriginalPrice { get; set; }
        [Range(0.0, 1.0)]
        public double? DiscountPresent { get; set; }
    }
}
