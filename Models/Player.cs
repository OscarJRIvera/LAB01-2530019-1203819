using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LAB01_2530019_1203819.Models
{
    public class Player
    {
        
        public int Id { get; set; }
        [Required]
        public string Club { get; set; }
        [Required]
        public string Last_name { get; set; }
        [Required]
        public string First_name { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public double Base_salary { get; set; }
        [Required]
        public double Guaranteed_compensation { get; set; }

    
    }
}
