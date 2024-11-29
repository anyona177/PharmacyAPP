using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpEntities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; } // Уникальный ID пользователя

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; } // Ссылка на сотрудника

        [Required, MaxLength(50)]
        public required string Username { get; set; } // Имя пользователя для входа

        [Required]
        public required string PasswordHash { get; set; } // Хеш пароля

        public required virtual Employee Employee { get; set; } // Навигационное свойство
    }
}
