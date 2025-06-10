using Assets.CodeBase.UI.Core;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.CodeBase.Gameplay.UI.Popups.BreedDog
{
    public class UIBreedPopupView : UIScreenView
    {
        public TextMeshProUGUI TitleText { get => _titleText; set => _titleText = value; }
        public TextMeshProUGUI DescriptionText { get => _descriptionText; set => _descriptionText = value; }
        public TextMeshProUGUI DetailsText { get => _detailsText; set => _detailsText = value; }
        public IObservable<Unit> OnExitButtonClicked => _exitButton.OnClickAsObservable();
        
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _detailsText;
        [SerializeField] private Button _exitButton;

    }
}
