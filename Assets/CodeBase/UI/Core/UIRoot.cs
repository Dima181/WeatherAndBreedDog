using Assets.CodeBase.Cam;
using UnityEngine;
using Zenject;

namespace Assets.CodeBase.UI.Core
{
    public class UIRoot : MonoBehaviour
    {
        public Canvas Popups => _popups;
        public Canvas Screens => _screens;
        public Canvas LayerUnderScreen => _layerUnderScreen;

        [SerializeField] private Canvas _popups;
        [SerializeField] private Canvas _screens;
        [SerializeField] private Canvas _layerUnderScreen;

        [Inject]
        private void Construct(Cameras cameras)
        {
            _popups.worldCamera = cameras.ScreenCamera;
            _screens.worldCamera = cameras.ScreenCamera;
            _layerUnderScreen.worldCamera = cameras.ScreenCamera;
        }
    }
}
