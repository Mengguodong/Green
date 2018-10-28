using DapperExAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class GlobalConfig
    {

         [Id(true)]
       public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
    }
}
