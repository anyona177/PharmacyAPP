using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CorpEntities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; } //Уникальный идентификатор продукта

        [MaxLength(100)]
        public required string Name { get; set; } //Название продукта

        public int Quantity { get; set; } //Количество на складе

        public decimal Price { get; set; } //Цена продукта

        public required string Supplier { get; set; } //Поставщик продукта

        // Связь "Многие к одному" с категорией
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; } //Внешний ключ на категорию

        public required virtual Category Category { get; set; } //Категория, к которой относится продукт

        [JsonIgnore] // Игнорируем для предотвращения циклов
        // Связь "Один ко многим" с проданными товарами
        public virtual ICollection<SoldItem> SoldItems { get; set; } = new List<SoldItem>(); // Проданные товары
    }
}
