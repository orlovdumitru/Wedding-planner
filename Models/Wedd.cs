
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WeddingPlanner.Models{
    // public abstract class BaseEntity{}
    public class Wedd{

        [Key]
        public int id{ get; set; }

        [Required(ErrorMessage="You must enter first wedder")]
        [MinLength(4)]
        public string wedderOne { get; set; }

        [Required(ErrorMessage="You must enter second wedder")]
        [MinLength(4)]
        public string wedderTwo { get; set; }
        
        [Required(ErrorMessage="You must enter address for the wedding")]
        [MinLength(8)]
        public string address { get; set; }
        
        [Required(ErrorMessage="Select a date in the future")]
        [DataType(DataType.DateTime)]
        public DateTime created_at { get; set; } 

        // Foreing key for wdding from user
        public int userId { get; set; }
        public Users user { get; set; }

        // wedding list
        public List<WedPlan> wedPlan { get; set; }

        public Wedd (){
            wedPlan = new List<WedPlan>();
        }        
    }
}
