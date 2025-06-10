using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.CodeBase.ServerCore
{
    public interface IServerClient
    {
        IObservable<bool> IsBusy { get; }
        void LockBusy();
        void UnlockBusy();

        /*UniTask<(EResult Result, ServerCodeResponse Response, T ResultData)> Post<T>(string api);
        UniTask<(EResult Result, ServerCodeResponse Response, T ResultData)> Post<T>(string api, object data);
        UniTask<(EResult Result, ServerCodeResponse Response, T ResultData)> Get<T>(string api);

        UniTask<(EResult Result, ServerCodeResponse Response)> Connect();
        UniTask<(EResult Result, ServerCodeResponse Response)> Reconnect();*/
    }
}
