using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core.Models
{
    public class Message
    {
        public string User { get; set; }
        public string Content { get; set; }


        [NonSerialized()] public bool IsMine;

    }
}
