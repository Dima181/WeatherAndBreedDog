using Assets.CodeBase.UI.Core;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;
using Assets.CodeBase.Extensions;

namespace Assets.CodeBase.UI
{
    public class UINavigator : IInitializable, IDisposable
    {
        [Inject(Id = "HUD")] private ISimpleUIScreenPresenter _hud;

        [Inject] private DiContainer _di;

        public ReactiveCommand<IUIScreenPresenter> ScreenOpened { get; private set; }
        public ReactiveCommand<IUIScreenPresenter> ScreenClosed { get; private set; }

        private readonly List<IUIScreenPresenter> _screens = new();
        private readonly List<IUIScreenPresenter> _popups = new();

        private CompositeDisposable _disposables = new();

        public void Perform<T>(
            Action<T> action)
            where T : class, IUIScreenPresenter
        {
            var presenter = _di.TryResolve<T>();
            if (presenter != null)
                action(presenter);
            else
                Debug.LogError($"[UI] Presenter {typeof(T).Name}: Unable to resolve");
        }

        public void Perform<T>(
            Type type,
            Action<T> action)
            where T : class, IUIScreenPresenter
        {
            var presenter = (T)_di.TryResolve(type);
            if (presenter != null)
                action(presenter);
            else
                Debug.LogError($"[UI] Presenter {typeof(T).Name}: Unable to resolve");
        }

        public void Show<T>(Action<T> afterShoeCallback = null)
            where T : class, ISimpleUIScreenPresenter
        {
            var presenter = _di.TryResolve<T>();
            if (presenter == null)
            {
                Debug.LogError($"[UI] Presenter {typeof(T).Name}: Unable to resolve");
                return;
            }

            if (afterShoeCallback == null)
                presenter.Show().Forget();
            else
                presenter.Show().ContinueWith(() => afterShoeCallback(presenter)).Forget();
        }

        public void Show<T, TModel>(TModel model, Action<T> afterShowCallback = null)
            where T : class, IModelUIScreenPresenter<TModel>
        {
            var presenter = _di.TryResolve<T>();
            if (presenter == null)
            {
                Debug.LogError($"[UI] Presenter {typeof(T).Name}: Unable to resolve");
                return;
            }

            if (afterShowCallback == null)
                presenter.Show(model).Forget();
            else
                presenter.Show(model).ContinueWith(() => afterShowCallback(presenter)).Forget();

        }

        public void HideAll()
        {
            for (int i = _screens.Count - 1; i >= 0; i--)
                _screens[i].Hide();
        }

        public void HideAllExceptHUD()
        {
            HideAll();
            _hud.Show();
        }
        public void Show(Type type)
        {
            var presenter = (ISimpleUIScreenPresenter)_di.TryResolve(type);
            if (presenter == null)
            {
                Debug.LogError($"[UI] Presenter {type.Name}: Unable to resolve");
                return;
            }

            presenter.Show().Forget();
        }

        public void Initialize()
        {
            _disposables = new();

            ScreenOpened = new();
            ScreenClosed = new();

            ScreenClosed.AddTo(_disposables);
            ScreenClosed.AddTo(_disposables);

            IUIScreenPresenter.OnShow.Subscribe(OnShow).AddTo(_disposables);
            IUIScreenPresenter.OnHide.Subscribe(OnHide).AddTo(_disposables);
        }
        public void Dispose() =>
            _disposables.Dispose();

        private void OnShow(IUIScreenPresenter screen)
        {
            if (_screens.Count == 1 && _screens[0] == _hud && screen.ScreenType is EScreenType.Screen)
                _hud.Hide();

            if (screen.ScreenType is EScreenType.Screen or EScreenType.ScreenWithHud)
                _screens.Add(screen);
            else if (screen.ScreenType is EScreenType.Popup)
                _popups.Add(screen);

            ScreenOpened.Execute(screen);
        }

        private void OnHide(IUIScreenPresenter screen)
        {
            if (screen.ScreenType is EScreenType.Screen or EScreenType.ScreenWithHud)
                OnHide(screen, _screens);
            else if (screen.ScreenType == EScreenType.Popup)
                OnHide(screen, _popups);
            if (_screens.Count == 0 && screen != _hud)
                _hud.Show().Forget();
        }

        private void OnHide(IUIScreenPresenter screen, List<IUIScreenPresenter> stack)
        {
            for (int i = stack.Count - 1; i >= 0; i--)
            {

                if (!i.InBounds(stack))
                    break;
                var lastScreen = stack[i];

                ScreenClosed.Execute(lastScreen);

                stack.RemoveAt(i);
                if (lastScreen == screen)
                    break;
                lastScreen.Hide().Forget();

            }
        }
    }
}
