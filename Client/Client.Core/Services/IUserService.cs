using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;

namespace Client.Core.Services;
public interface IUserService{
        string? userName { get;}
        event Action? userChanged;
        void logIn(string userName);
        void logOut();
    }
