using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityFrameworkCore.BootKit.UnitTest.Tables
{
    public class PizzaOrder : DbRecord, IDbRecord
    {
        [MaxLength(32)]
        public String OrderNumber { get; set; } 

        [MaxLength(64)]
        public String CustomerName { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }
    }
}
