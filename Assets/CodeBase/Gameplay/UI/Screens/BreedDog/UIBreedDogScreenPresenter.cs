using Assets.CodeBase.Gameplay.Services.BreedDog.Data;
using Assets.CodeBase.Gameplay.UI.Weather;
using Assets.CodeBase.UI.Core;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEditor.VersionControl;
using UnityEngine;
using Zenject;

namespace Assets.CodeBase.Gameplay.UI.BreedDog
{
    public class UIBreedDogScreenPresenter : UIScreenPresenter<UIBreedDogScreenView>
    {
        [Inject] private readonly BreedDogServerDataProvider _dataProvider;
        [Inject] private DiContainer _container;

        protected override async UniTask BeforeShow(CompositeDisposable disposables)
        {
            ClearPreviousCards();

            var breeds = await _dataProvider.GetBreedIdsAsync();

            for (int i = 0; i < breeds.Count; i++)
            {
                string breedName = breeds[i].Id;
                var cardInstance = UnityEngine.Object.Instantiate(_view.ItemPrefab, _view.RequiresContainer);

                _container.Inject(cardInstance);

                cardInstance.BreedId = breeds[i];
                cardInstance.TextNumber.text = (i + 1).ToString();
                cardInstance.TextName.text = breeds[i].Name;

                _view.ItemList.Add(cardInstance.GetComponent<RectTransform>());
            }

            _view.OnExitButtonClick
                .Subscribe(_ =>
                {
                    Hide().Forget();
                })
                .AddTo(disposables);
        }

        private void ClearPreviousCards()
        {
            foreach (var item in _view.ItemList)
            {
                UnityEngine.Object.Destroy(item.gameObject);
            }
            _view.ItemList.Clear();
        }
    }
}
