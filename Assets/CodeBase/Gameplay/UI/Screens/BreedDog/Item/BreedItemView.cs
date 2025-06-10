using Assets.CodeBase.Gameplay.Services.BreedDog.Data;
using Assets.CodeBase.Gameplay.UI.Popups.BreedDog;
using Assets.CodeBase.UI;
using Assets.CodeBase.UI.Core;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.CodeBase.Gameplay.UI.BreedDog.Item
{
    public class BreedItemView : MonoBehaviour
    {
        [Inject] private readonly UINavigator _uiNavigator;
        [Inject] private readonly UIBreedPopupPresenter _breedPopupPresenter;

        [field: SerializeField] public TextMeshProUGUI TextNumber { get; set; }
        [field: SerializeField] public TextMeshProUGUI TextName { get; set; }
        public IObservable<Unit> OnOpenPopoupClicked => _openPopupButton.OnClickAsObservable();
        public BreedId BreedId { get; set; }

        [SerializeField] private Button _openPopupButton;
        [SerializeField] public GameObject _loadingIcon;

        public void Construct(BreedId breedId, int index)
        {
            BreedId = breedId;
            TextNumber.text = index.ToString();
            TextName.text = breedId.Name;
        }
        
        private void OnValidate()
        {
            if (_openPopupButton == null)
                _openPopupButton = GetComponent<Button>();
        }

        private void Awake()
        {
            OnOpenPopoupClicked
                .Subscribe(_ =>
                {
                    _breedPopupPresenter.IsLoading
                        .Subscribe(tuple =>
                        {
                            var (id, isLoading) = tuple;
                            if (id.Id == BreedId.Id)
                                _loadingIcon.SetActive(isLoading);
                        })
                        .AddTo(this);

                    _uiNavigator.Show<UIBreedPopupPresenter, BreedId>(BreedId);
                })
                .AddTo(this);
        }

        public class Pool : MonoMemoryPool<BreedItemView>
        {
            protected override void OnSpawned(BreedItemView item)
            {
                base.OnSpawned(item);
                item.gameObject.SetActive(true);
            }

            protected override void OnDespawned(BreedItemView item)
            {
                base.OnDespawned(item);
                item.gameObject.SetActive(false);
            }
        }
    }
}
