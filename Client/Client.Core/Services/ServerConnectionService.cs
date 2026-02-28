using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core.Services;

using Client.Core.Models;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;

public class ServerConnectionService : IServerConnectionService{
    public event Action? OnConnected;
    public event Action? OnDisconnected;
    public event Action<string>? OnMessLogsRecived;
    public event Action<string>? OnMessRecived;

    private ClientWebSocket ?webSocket;
    private Uri ?serverUri;
    private CancellationTokenSource cancellationTokenSource;
    public bool isConnected => webSocket?.State == WebSocketState.Open;

        
    public async Task<int> ConnectAsync(string ipAddress, string port)
    {
        try
        {
            webSocket = new ClientWebSocket();
            cancellationTokenSource = new CancellationTokenSource();
            serverUri = new Uri($"ws://{ipAddress}:{port}");

            await webSocket.ConnectAsync(serverUri, cancellationTokenSource.Token);

            if (webSocket.State == WebSocketState.Open)
            {
                _ = Task.Run(ListenServerAsync);
                OnConnected?.Invoke();
                return 0;
            }
            return 1;

        } catch(Exception ex){
            System.Diagnostics.Debug.WriteLine($"Connect Error: {ex.Message}");
            return -1;
        }
        
    }

    public void cancelAsyncTask()
    {
        cancellationTokenSource.Cancel();
    }

    public async Task DisconnectAsync()
    {
        if (isConnected)
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                                        "Client disconecting",
                                        CancellationToken.None);
            webSocket.Dispose();
            webSocket = null;
        }
        OnDisconnected?.Invoke();
    }

    public async Task ListenServerAsync()
    {
        try
        {
            var buffer = new byte[4096];
            while (isConnected)
            {
                var result = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    cancellationTokenSource.Token);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await DisconnectAsync();
                    break;
                }
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    if (message.StartsWith("mess:"))
                    {
                        OnMessRecived?.Invoke(message.Substring(5));
                    }
                    if (message.StartsWith("logs:"))
                    {
                        OnMessLogsRecived?.Invoke(message.Substring(5));
                    }
                }
            }
        } catch (Exception ex){
            System.Diagnostics.Debug.WriteLine("Receive loop error: " + ex.Message);
        }
    }

    private async Task<int> SendDataToServerAsync(string command)
    {
        if (!isConnected) return 1;

        var massage = Encoding.UTF8.GetBytes(command);
        await webSocket.SendAsync(
            new ArraySegment<byte>(massage),
            WebSocketMessageType.Text,
            true,
            cancellationTokenSource.Token);
        return 0;
    }

    public async Task RequestMessLogsAsync()
    {
        await SendDataToServerAsync("get_logs");
    }

    public async Task<int> SendMessageAsync(Message mess)
    {
        string json = JsonSerializer.Serialize(mess);
        string prefJson = $"mess:{json}";
        int res = await SendDataToServerAsync(prefJson);
        if (res == 0) { return 0; }
        return 1;
    }
}

