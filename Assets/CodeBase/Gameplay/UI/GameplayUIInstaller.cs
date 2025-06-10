using Assets.CodeBase.Gameplay.UI.BreedDog;
using Assets.CodeBase.Gameplay.UI.HUD;
using Assets.CodeBase.Gameplay.UI.Popups.BreedDog;
using Assets.CodeBase.Gameplay.UI.Weather;
using Assets.CodeBase.UI;
using UnityEngine;

namespace Assets.CodeBase.Gameplay.UI
{
    public class GameplayUIInstaller : UIInstaller<UIGameplayHUDView, UIGameplayHUDPresenter>
    {
        protected override string HudPrefabPath => "UI/HUD/Gameplay Hud";

        protected override void InstallBindingsInternal()
        {
            UIWeatherScreenInstaller.Install(Container);
            UIBreedDogScreenInstaller.Install(Container);
            BreedPopupsInstaller.Install(Container);
        }
    }
}