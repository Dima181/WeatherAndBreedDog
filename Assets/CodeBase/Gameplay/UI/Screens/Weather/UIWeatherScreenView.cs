using Assets.CodeBase.UI.Core;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.CodeBase.Gameplay.UI.Weather
{
    public class UIWeatherScreenView : UIScreenView
    {
        public TextMeshProUGUI Description => _description;
        public RawImage WeatherIcon => _weatherIcon;
        public IObservable<Unit> OnExitButtonClick => _exitButton.OnClickAsObservable();

        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private RawImage _weatherIcon;
        [SerializeField] private Button _exitButton;
    }
}
