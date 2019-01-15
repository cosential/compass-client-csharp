using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosential.Integrations.Compass.Client.Models
{
    public class Sf330ProfileCode
    {
        public int      ProfileCodeId   { get; set; }
        public string   ProfileCodeName { get; set; }
        public string   ProfileCodeNum  { get; set; }
        public bool     IsDefault       { get; set; }
    }
}
