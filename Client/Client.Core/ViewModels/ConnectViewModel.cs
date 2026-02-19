using Client.Core.Models;
using Client.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Core.ViewModels;
public partial class ConnectViewModel: ObservableObject
{
    private readonly IServerConnectionService serverConnection;
    private readonly IUserService _userService;
    private readonly IMainThreadDispatcher mainThreadDispatcher;
    public ConnectViewModel(IServerConnectionService serverConnection, IUserService userService, IMainThreadDispatcher mainThreadDispatcher)
    {
        this.serverConnection = serverConnection;
        serverConnection.OnConnected += connectEvent;
        serverConnection.OnDisconnected += disconnectEvent;
        _userService = userService;

        ConnectAsyncCommand = new AsyncRelayCommand(ConnectAsync);
        DisconnectAsyncCommand = new AsyncRelayCommand(DisconnectAsync);

        this.mainThreadDispatcher = mainThreadDispatcher;

    }

    [ObservableProperty]
    public partial string username { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ipAddress { get; set; } = "127.0.0.1";

    [ObservableProperty]
    public partial string port { get; set; } = "8765";

    [ObservableProperty]
    public partial bool isConnected { get; set; } = false;



    [ObservableProperty]
    public partial bool isError { get; set; } = false;

    [ObservableProperty]
    public partial string ErrorMes { get; set; } = string.Empty;

    private void setError(string message)
    {
        if (message == String.Empty) isError = false;
        else isError = true;
        ErrorMes = message;
    }

    public IAsyncRelayCommand ConnectAsyncCommand { get; }
    private async Task ConnectAsync()
    {
        _userService.logIn(username);
        int res = await serverConnection.ConnectAsync(ipAddress, port);
        setError("");
        if (res != 0) { setError("ERROR: conecting to the server");  }
    }


    public IAsyncRelayCommand DisconnectAsyncCommand { get; }
    private async Task DisconnectAsync()
    {
        _userService.logOut();
        await serverConnection.DisconnectAsync();
    }

    public void connectEvent() => isConnected = true;
    public void disconnectEvent(){
        mainThreadDispatcher.RunOnMainThread(
            () => isConnected = false
            );
    }

}
