using Assets.CodeBase.Gameplay.Services.Weather.Data;
using Assets.CodeBase.UI.Core;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.CodeBase.Gameplay.UI.Weather
{
    public class UIWeatherScreenPresenter : UIScreenPresenter<UIWeatherScreenView>
    {
        [Inject] private readonly WeatherServerDataProvider _dataProvider;

        private CancellationToken _cancellationToken;

        private CancellationTokenSource _cts;

        protected override UniTask BeforeShow(CompositeDisposable disposables)
        {
            _cts = new CancellationTokenSource();
            disposables.Add(Disposable.Create(() => _cts.Cancel()));

            _view.OnExitButtonClick
                .Subscribe(_ =>
                {
                    _cts.Cancel();
                    Hide().Forget();
                })
                .AddTo(disposables);

            UpdateWeatherLoop(_cts.Token).Forget();

            return UniTask.CompletedTask;
        }

        private async UniTaskVoid UpdateWeatherLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await _dataProvider.GetWeatherAsync(_view.Description, _view.WeatherIcon, token);
                    await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {
                Debug.LogError($"Error in weather update cycle: {ex}");
            }
        }
    }
}
