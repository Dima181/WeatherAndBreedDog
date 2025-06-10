using Assets.CodeBase.Gameplay.Services.Weather.Data;
using Assets.CodeBase.UI.Core;
using Zenject;

namespace Assets.CodeBase.Gameplay.UI.Weather
{
    public class UIWeatherScreenInstaller : Installer<UIWeatherScreenInstaller>
    {
        private string WEATHER_SCREEN_PATH => "UI/Screens/Weather Screen";

        public override void InstallBindings()
        {
            Container.BindPresenter<UIWeatherScreenPresenter>()
                .WithViewFromPrefab(WEATHER_SCREEN_PATH)
                .AsScreen();

            Container.Bind<WeatherServerDataProvider>()
                .AsSingle();
        }
    }
}
