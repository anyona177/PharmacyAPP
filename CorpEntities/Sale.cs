using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CorpEntities
{
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleId { get; set; } //Уникальный идентификатор продажи

        // Связь "Многие к одному" с сотрудником
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; } //Внешний ключ на сотрудника
        public required virtual Employee Employee { get; set; } //Сотрудник, который произвел продажу
        
        [DataType(DataType.DateTime)]
        public required DateTime SaleDate { get; set; } //Дата продажи

        public decimal TotalAmount { get; set; } //Общая стоимость покупки

        [JsonIgnore] // Игнорируем для предотвращения циклов
        public virtual ICollection<SoldItem> SoldItems { get; set; } = new List<SoldItem>(); //Проданные товары
    }
}
