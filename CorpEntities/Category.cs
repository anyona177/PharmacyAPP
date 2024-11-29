using CorpEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Category
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryId { get; set; } //Уникальный идентификатор категории

    [MaxLength(50)]
    public required string CategoryName { get; set; } //Название категории


    //Связь "Один ко многим" с продуктами
    [JsonIgnore] // Игнорируем для предотвращения циклов
    public virtual ICollection<Product> Products { get; set; } = new List<Product>(); //Продукты в категории
}
