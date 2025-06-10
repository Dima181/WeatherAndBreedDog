using Assets.CodeBase.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEditor.Progress;
using TMPro;
using UnityEngine;
using Assets.CodeBase.Gameplay.UI.BreedDog.Item;
using UnityEngine.UI;
using UniRx;

namespace Assets.CodeBase.Gameplay.UI.BreedDog
{
    public class UIBreedDogScreenView : UIScreenView
    {
        public RectTransform RequiresContainer => _requiresContainer;
        public BreedItemView ItemPrefab => _item;
        public List<RectTransform> ItemList { get => _itemList; set => _itemList = value; }
        public IObservable<Unit> OnExitButtonClick => _exitButton.OnClickAsObservable();

        [SerializeField] private RectTransform _requiresContainer;
        [SerializeField] private BreedItemView _item;
        [SerializeField] private Button _exitButton;
        [SerializeField] private List<RectTransform> _itemList;
    }
}
