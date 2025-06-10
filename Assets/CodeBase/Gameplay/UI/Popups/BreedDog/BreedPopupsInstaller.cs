using Assets.CodeBase.UI.Core;
using Zenject;

namespace Assets.CodeBase.Gameplay.UI.Popups.BreedDog
{
    public class BreedPopupsInstaller : Installer<BreedPopupsInstaller>
    {
        private const string BREED_POPUP_PATH = "UI/Popups/BreedDog/Breed Popup";

        public override void InstallBindings()
        {
            Container.BindPresenter<UIBreedPopupPresenter>()
                .WithViewFromPrefab(BREED_POPUP_PATH)
                .AsPopup();
        }
    }
}
