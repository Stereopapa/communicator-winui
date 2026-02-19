using Client.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Core.Services;
public interface IServerConnectionService{

    public event Action? OnConnected;
    public event Action? OnDisconnected;
    public event Action<string>? OnMessLogsRecived;
    public event Action<string>? OnMessRecived;

    bool isConnected { get; }

    Task<int> ConnectAsync(string ipAddress, string port);
    void cancelAsyncTask();
    Task DisconnectAsync();
    Task ListenServerAsync();
    Task RequestMessLogsAsync();
    Task<int> SendMessageAsync(Message message);

}