using Assets.CodeBase.Gameplay.Services.BreedDog.Data;
using Zenject;

namespace Assets.CodeBase.Gameplay.UI.BreedDog.Item
{
    public class BreedItemViewFactory : IFactory<BreedId, int, BreedItemView>
    {
        private readonly BreedItemView.Pool _pool;

        public BreedItemViewFactory(BreedItemView.Pool pool)
        {
            _pool = pool;
        }

        public BreedItemView Create(BreedId breedId, int index)
        {
            var item = _pool.Spawn();
            item.Construct(breedId, index);
            return item;
        }
    }
}
