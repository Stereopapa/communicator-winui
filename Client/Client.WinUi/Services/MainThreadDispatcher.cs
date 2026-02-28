using Client.Core;
using Client.Core.Services;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using DispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue;

namespace Client.WUI.Services;

public class MainThreadDispatcher : IMainThreadDispatcher
{
    private readonly DispatcherQueue dispatcherQueue;

    public MainThreadDispatcher()
    {
        dispatcherQueue = DispatcherQueue.GetForCurrentThread();
    }

    public void RunOnMainThread(Action action)
    {
        if (dispatcherQueue.HasThreadAccess)
            action();
        else
            dispatcherQueue.TryEnqueue(() => action());
    }
}