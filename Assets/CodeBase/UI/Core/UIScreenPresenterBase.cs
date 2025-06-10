using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using Zenject;

namespace Assets.CodeBase.UI.Core
{
    public interface IUIScreenPresenter : IDisposable
    {
        public static ReactiveCommand<IUIScreenPresenter> OnShow { get; } = new();
        public static ReactiveCommand<IUIScreenPresenter> OnHide { get; } = new();

        EScreenType ScreenType { get; }
        UniTask Hide();
    }

    public interface ISimpleUIScreenPresenter : IUIScreenPresenter
    {
        UniTask Show();
    }

    public interface IModelUIScreenPresenter<TModel> : IUIScreenPresenter
    {
        UniTask Show(TModel model);
    }

    public abstract class UIScreenPresenterBase<TView> : IUIScreenPresenter
        where TView : UIScreenView
    {
        public EScreenType ScreenType => _screenType;

        [Inject] private EScreenType _screenType;
        [Inject] protected TView _view;

        protected UniTask Complited => UniTask.CompletedTask;

        public async UniTask Hide()
        {
            if (_view)
            {
                await _view.Hide();
                await AfterHide();
            }
            Dispose();
            IUIScreenPresenter.OnHide.Execute(this);
        }

        public void HideAndForget() => Hide().Forget();

        public abstract void Dispose();

        protected virtual UniTask AfterHide() => UniTask.CompletedTask;
    }
}
