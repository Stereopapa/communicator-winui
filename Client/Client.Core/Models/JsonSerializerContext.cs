using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client.Core.Models
{
    [JsonSerializable(typeof(ObservableCollection<Message>))]
    [JsonSerializable(typeof(List<Message>))]
    [JsonSerializable(typeof(Message))]
    internal partial class AppJsonContext : JsonSerializerContext
    {
    }
}
