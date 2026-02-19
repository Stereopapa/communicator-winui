using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core.Services
{
    public interface IMainThreadDispatcher
    {
        void RunOnMainThread(Action action);
    }
}
