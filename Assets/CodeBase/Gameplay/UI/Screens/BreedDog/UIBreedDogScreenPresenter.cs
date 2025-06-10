using Assets.CodeBase.Gameplay.Services.BreedDog.Data;
using Assets.CodeBase.Gameplay.UI.BreedDog.Item;
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
        [Inject] private readonly IFactory<BreedId, int, BreedItemView> _itemFactory;

        [Inject] private readonly BreedDogServerDataProvider _dataProvider;

        protected override async UniTask BeforeShow(CompositeDisposable disposables)
        {
            ClearPreviousCards();

            var breeds = await _dataProvider.GetBreedIdsAsync();

            for (int i = 0; i < breeds.Count; i++)
            {
                var breed = breeds[i];
                var card = _itemFactory.Create(breed, i + 1);

                card.transform.SetParent(_view.RequiresContainer, false);
                _view.ItemList.Add(card.GetComponent<RectTransform>());
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
