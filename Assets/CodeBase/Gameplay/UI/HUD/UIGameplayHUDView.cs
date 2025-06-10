using Assets.CodeBase.UI.Core;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.CodeBase.Gameplay.UI.HUD
{
    public class UIGameplayHUDView : UIScreenView
    {
        public IObservable<Unit> OnWeatherClick => _weatherButton.OnClickAsObservable();
        public IObservable<Unit> OnBreedDogClick => _breedDogButton.OnClickAsObservable();

        [Header("Botton panel")]
        [SerializeField] private Button _weatherButton;
        [SerializeField] private Button _breedDogButton;
    }
}
