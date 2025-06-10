using Assets.CodeBase.Gameplay.UI.BreedDog;
using Assets.CodeBase.Gameplay.UI.Weather;
using Assets.CodeBase.UI;
using Assets.CodeBase.UI.Core;
using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace Assets.CodeBase.Gameplay.UI.HUD
{
    public class UIGameplayHUDPresenter : UIScreenPresenter<UIGameplayHUDView>
    {
        [Inject] private readonly UINavigator _uiNavigator;

        protected override UniTask BeforeShow(CompositeDisposable disposables)
        {
            _view.OnWeatherClick
                .Subscribe(_ =>
                {
                    _uiNavigator.Perform<UIWeatherScreenPresenter>(p => p.ShowAndForget());
                })
                .AddTo(disposables);


            _view.OnBreedDogClick
                .Subscribe(_ =>
                {
                    _uiNavigator.Perform<UIBreedDogScreenPresenter>(p => p.ShowAndForget());
                })
                .AddTo(disposables);

            return Complited;
        }
    }
}
