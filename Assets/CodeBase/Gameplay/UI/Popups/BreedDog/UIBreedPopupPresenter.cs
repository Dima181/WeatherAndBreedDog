using Assets.CodeBase.Gameplay.Services.BreedDog.Data;
using Assets.CodeBase.UI.Core;
using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace Assets.CodeBase.Gameplay.UI.Popups.BreedDog
{
    public class UIBreedPopupPresenter : UIScreenPresenter<UIBreedPopupView, BreedId>
    {
        [Inject] private BreedDogServerDataProvider _dataProvider;

        public Subject<(BreedId, bool)> IsLoading = new();

        protected override async UniTask BeforeShow(BreedId model, CompositeDisposable disposables)
        {
            IsLoading.OnNext((model, true));

            try
            {
                var data = await _dataProvider.GetBreedDetailsAsync(model.Id);

                if (data == null)
                {
                    _view.TitleText.text = "Error";
                    _view.DescriptionText.text = "Failed to load breed information.";
                    _view.DetailsText.text = "";
                    return;
                }

                _view.TitleText.text = data.attributes.name;
                _view.DescriptionText.text = data.attributes.description;

                _view.DetailsText.text = $"Hypoallergenic: {(data.attributes.hypoallergenic ? "Yes" : "No")} | " +
                                         $"Life Span: {data.attributes.life.min} - {data.attributes.life.max} years\n\n" +
                                         $"Male Weight: {data.attributes.male_weight.min} - {data.attributes.male_weight.max} kg | " +
                                         $"Female Weight: {data.attributes.female_weight.min} - {data.attributes.female_weight.max} kg";
            }
            finally
            {
                IsLoading.OnNext((model, false));
            }

            _view.OnExitButtonClicked
                .Subscribe(_ =>
                {
                    Hide().Forget();
                })
                .AddTo(disposables);
        }
    }
}
