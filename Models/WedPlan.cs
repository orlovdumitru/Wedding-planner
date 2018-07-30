
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WeddingPlanner.Models{
    // public abstract class BaseEntity{}
    public class WedPlan{

        [Key]
        public int id{ get; set; }

       public int weddingId { get; set; }
       public Wedd wedding { get; set; }
       public int userId { get; set; }
       public Users user { get; set; }

      
    }
}
