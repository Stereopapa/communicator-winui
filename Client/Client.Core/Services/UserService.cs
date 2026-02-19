using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core.Services;
public partial class UserService : IUserService
{
    public string? userName { get; set; }


    public event Action? userChanged;

    public void logIn(string userName)
    {
        this.userName = userName;
        userChanged?.Invoke();
    }

    public void logOut()
    {
        userName = null;
        userChanged?.Invoke();
    }
}
