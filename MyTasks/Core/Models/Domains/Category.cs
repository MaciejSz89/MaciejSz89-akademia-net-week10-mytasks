﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;


namespace MyTasks.Core.Models.Domains
{
    public class Category
    {

        public Category()
        {
            Tasks = new Collection<Task>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole Nazwa jest wymagane")]
        public string Name { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
