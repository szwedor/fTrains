using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public class Attribute:Entity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
