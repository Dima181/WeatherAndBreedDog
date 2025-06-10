using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.CodeBase.UI.Core
{
    public interface IUIScreenSubPresenter
    {
        UniTask BeforeShow(CompositeDisposable disposables);
        UniTask AfterShow(CompositeDisposable disposables);
        UniTask AfterHide();
    }
    public interface IUIScreenSubPresenter<TModel>
    {
        UniTask BeforeShow(TModel model, CompositeDisposable disposables);
        UniTask AfterShow(TModel model, CompositeDisposable disposables);
        UniTask AfterHide();
    }

    public abstract class UIScreenSubPresenterBase<TView>
        where TView : Component
    {
        [Inject] protected TView _view;

        public TView View => _view;

        public virtual UniTask AfterHide() => UniTask.CompletedTask;
    }

    public abstract class UIScreenSubPresenter<TView> : UIScreenSubPresenterBase<TView>, IUIScreenSubPresenter
        where TView : Component
    {
        public abstract UniTask BeforeShow(CompositeDisposable disposables);
        public virtual UniTask AfterShow(CompositeDisposable disposables) => UniTask.CompletedTask;
    }

    public abstract class UIScreenSubPresenter<TView, TModel> :
        UIScreenSubPresenterBase<TView>, IUIScreenSubPresenter<TModel>
        where TView : Component
    {
        public abstract UniTask BeforeShow(TModel model, CompositeDisposable disposables);
        public virtual UniTask AfterShow(TModel model, CompositeDisposable disposables) => UniTask.CompletedTask;
    }
}
