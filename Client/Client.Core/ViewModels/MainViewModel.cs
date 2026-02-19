using Client.Core.Models;
using CommunityToolkit.Mvvm;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using Client.Core.Services;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Core.ViewModels;
public partial class MainViewModel : ObservableObject{

    private readonly IServerConnectionService serverConnection;
    private readonly IUserService userService;
    private readonly IMainThreadDispatcher mainThreadDispatcher;
    public string? userName = null;
    public MainViewModel(
        IServerConnectionService serverConnection,
        IUserService userService,
        IMainThreadDispatcher mainThreadDispatcher)
    {
        this.serverConnection = serverConnection;
        serverConnection.OnConnected += Connected;
        serverConnection.OnDisconnected += Disconnected;
        serverConnection.OnMessLogsRecived += messLogsRecived;
        serverConnection.OnMessRecived += messRecived;

        this.userService = userService;
        userService.userChanged += userChanged;

        this.mainThreadDispatcher = mainThreadDispatcher;

    }

    [ObservableProperty]
    ObservableCollection<Message> messages = new();


    [ObservableProperty]
    public partial string CurrentMessage { get; set; } = string.Empty;


    [RelayCommand]
    public void SendMessage()
    {
        if (CurrentMessage == string.Empty || userName == null) return;
        serverConnection.SendMessageAsync(new Message { User = userName, Content = CurrentMessage  });
        CurrentMessage = "";
    }

    public void messLogsRecived(string logsJson)
    {
        Messages = JsonSerializer.Deserialize<ObservableCollection<Message>>(logsJson);
        setIsMineMessagesInfo();
    }

    public void setIsMineMessagesInfo()
    {
        foreach (Message mess in messages)
        {
            mess.IsMine = (mess.User == userName);
        }
    }

    public void messRecived(string messJson)
    {
        mainThreadDispatcher.RunOnMainThread(
            () => {
                Message mess = JsonSerializer.Deserialize<Message>(messJson);
                mess.IsMine = (mess.User == userName);
                messages.Add(mess);
                }
            ); 
    }

    public void Connected()
    {
        serverConnection.RequestMessLogsAsync();
    }
    public void Disconnected()
    {
        Messages.Clear();
    }

    public void userChanged()
    {
        userName = userService.userName;
    }
    

}

