using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpEntities
{
    public class SoldItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SoldItemId { get; set; } // Уникальный идентификатор проданного товара

        // Связь "Многие к одному" с продажей
        [ForeignKey(nameof(Sale))]
        public int SaleId { get; set; } // Внешний ключ на продажу

        // Связь "Многие к одному" с продуктом
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; } // Внешний ключ на продукт
 
        public required virtual Sale Sale { get; set; } // Продажа
        public required virtual Product Product { get; set; } // Товар

        public required int Quantity { get; set; } // Количество проданного товара

        public required decimal Price { get; set; } // Цена товара на момент продажи
    }
}
