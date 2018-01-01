using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    public class DbPatchModel
    {
        public DbPatchModel()
        {
            Values = new Dictionary<string, object> { };
            IgnoredColumns = new List<string>();
        }

        public string Id { get; set; }
        public string Table { get; set; }
        public List<String> IgnoredColumns { get; set; }
        public Dictionary<String, Object> Values { get; set; }
    }
}
