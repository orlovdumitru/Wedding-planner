
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WeddingPlanner.Models{
     public abstract class BaseEntity{}
    public class Users : BaseEntity{
        
        [Key]
        public int id{ get; set; }

        [Required(ErrorMessage="You must enter your first name")]
        [MinLength(4)]
        public string first_name { get; set; }

        [Required(ErrorMessage="You must enter your last name")]
        [MinLength(4)]
        public string last_name { get; set; }

        [Required(ErrorMessage="You must enter your email")]
        [EmailAddress]
        public string email { get; set; }

        [Required(ErrorMessage="You must enter passowrd")]
        [MinLength(8)]
        public string password { get; set; }

        public DateTime created_at { get; set; } 

        public List<WedPlan> wedPlan { get; set; }
        public List<Wedd> wedd { get; set; }

        public Users (){
            wedPlan = new List<WedPlan>();
            wedd = new List<Wedd>();
        }
    }
}