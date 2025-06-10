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
    public abstract class UIScreenPresenter<TView> : UIScreenPresenterBase<TView>, ISimpleUIScreenPresenter
        where TView : UIScreenView
    {
        [Inject] private List<IUIScreenSubPresenter> _subPresenters;

        private CompositeDisposable _disposables;
        protected CompositeDisposable Disposables => _disposables;

        public async UniTask Show()
        {
            _disposables?.Dispose();
            _disposables = new();

            await BeforeShow(_disposables);
            foreach (var subPresenter in _subPresenters)
                await subPresenter.BeforeShow(_disposables);

            await _view.Show();

            await AfterShow(_disposables);
            foreach (var subPresenter in _subPresenters)
                await subPresenter.AfterShow(_disposables);

            IUIScreenPresenter.OnShow.Execute(this);
        }

        public void ShowAndForget() => Show().Forget();

        protected virtual void OnDispose() { }

        public sealed override void Dispose()
        {
            OnDispose();
            _disposables?.Dispose();
            _disposables = null;
        }

        protected abstract UniTask BeforeShow(CompositeDisposable disposables);
        protected virtual UniTask AfterShow(CompositeDisposable disposables) => UniTask.CompletedTask;
        protected override async UniTask AfterHide()
        {
            foreach (var presenter in _subPresenters)
                await presenter.AfterHide();
        }
    }

    public abstract class UIScreenPresenter<TView, TModel> : UIScreenPresenterBase<TView>, IModelUIScreenPresenter<TModel>
        where TView : UIScreenView
    {
        [Inject] private List<IUIScreenSubPresenter> _subPresenters;
        [Inject] private List<IUIScreenSubPresenter<TModel>> _subPresentersArgs;
        private CompositeDisposable _disposables;

        protected CompositeDisposable Disposables => _disposables;

        public async UniTask Show(TModel model)
        {
            _disposables?.Dispose();
            _disposables = new();

            await BeforeShow(model, _disposables);
            foreach (var subPresenter in _subPresenters)
                await subPresenter.BeforeShow(_disposables);
            foreach (var subPresenter in _subPresentersArgs)
                await subPresenter.BeforeShow(model, _disposables);

            await _view.Show();

            await AfterShow(model, _disposables);
            foreach (var subPresenter in _subPresenters)
                await subPresenter.AfterShow(_disposables);
            foreach (var subPresenter in _subPresentersArgs)
                await subPresenter.AfterShow(model, _disposables);

            IUIScreenPresenter.OnShow.Execute(this);
        }

        public void ShowAndForget(TModel model) => Show(model).Forget();

        public sealed override void Dispose()
        {
            if (_disposables == null)
                return;
            _disposables.Dispose();
            _disposables = null;
        }

        protected virtual UniTask AfterShow(TModel model, CompositeDisposable disposables) => UniTask.CompletedTask;
        protected abstract UniTask BeforeShow(TModel model, CompositeDisposable disposables);
        protected override async UniTask AfterHide()
        {
            foreach (var presenter in _subPresenters)
                await presenter.AfterHide();
            foreach (var presenter in _subPresentersArgs)
                await presenter.AfterHide();
        }
    }
}
