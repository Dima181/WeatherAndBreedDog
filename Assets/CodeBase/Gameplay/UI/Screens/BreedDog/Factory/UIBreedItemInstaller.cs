using Assets.CodeBase.Gameplay.Services.BreedDog.Data;
using UnityEngine;
using Zenject;

namespace Assets.CodeBase.Gameplay.UI.BreedDog.Item
{
    public class UIBreedItemInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _breedItemPrefab;
        [SerializeField] private Transform _breedItemParent;

        public override void InstallBindings()
        {
            Container
                .BindMemoryPool<BreedItemView, BreedItemView.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_breedItemPrefab)
                .UnderTransform(_breedItemParent);

            Container
                .BindIFactory<BreedId, int, BreedItemView>()
                .FromFactory<BreedItemViewFactory>();
        }
    }
}
