using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class AssociationRuleModel
    {
        public List<string> Antecedent { get; set; }
        public List<string> Consequent { get; set; }
        public double Support { get; set; }
        public double Confidence { get; set; }
    }
}
