using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public abstract class DbRecord
    {
        [Key]
        [StringLength(36)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public String Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;
    }
}
