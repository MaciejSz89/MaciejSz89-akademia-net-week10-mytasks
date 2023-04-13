using System.ComponentModel.DataAnnotations;

namespace MyTasks.Core.Models
{
    public class DefaultCategory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole Nazwa jest wymagane")]
        public string Name { get; set; }
    }
}
