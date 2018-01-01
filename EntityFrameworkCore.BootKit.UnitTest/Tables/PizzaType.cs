using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityFrameworkCore.BootKit.UnitTest.Tables
{
    public class PizzaType : DbRecord, IDbRecord
    {
        [Required]
        public String OrderId { get; set; }

        [Required]
        [MaxLength(64)]
        public String Name { get; set; }

        public Decimal Amount { get; set; }
    }
}
