using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CorpEntities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; } //Уникальный идентификатор сотрудника

        [MaxLength(50)]
        public required string Surname { get; set; } //Фамилия сотрудника

        [MaxLength(50)]
        public required string Name { get; set; } //Имя сотрудника

        [MaxLength(50)]
        public string? Patronymic { get; set; } //Отчество сотрудника (может быть null)

        [MaxLength(50)]
        public required string Post { get; set; } //Должность

        [MaxLength(15)]
        public required string Phone { get; set; } //Телефон

        [JsonIgnore] // Игнорируем для предотвращения циклов
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>(); //Продажи, которые выполнил сотрудник
    }
}
