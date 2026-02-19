using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core.Models
{
    public partial class User{
        public static bool conetcted { get; set; } = false;
        public static string userName { get; set; }
    }
}
