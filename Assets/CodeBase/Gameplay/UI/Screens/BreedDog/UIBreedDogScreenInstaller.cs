using Assets.CodeBase.Gameplay.Services.BreedDog.Data;
using Assets.CodeBase.Gameplay.UI.BreedDog.Item;
using Assets.CodeBase.Gameplay.UI.HUD;
using Assets.CodeBase.UI.Core;
using Zenject;

namespace Assets.CodeBase.Gameplay.UI.BreedDog
{
    public class UIBreedDogScreenInstaller : Installer<UIBreedDogScreenInstaller>
    {
        private string BREEDDOG_SCREEN_PATH => "UI/Screens/BreedDog Screen";

        public override void InstallBindings()
        {
            Container.BindPresenter<UIBreedDogScreenPresenter>()
                .WithViewFromPrefab(BREEDDOG_SCREEN_PATH)
                .AsScreen();

            Container.Bind<BreedDogServerDataProvider>()
                .AsSingle();
        }
    }
}
