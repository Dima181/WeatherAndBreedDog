using Zenject;

namespace Assets.CodeBase.UI.Core
{
    public static class UIBindingExtensions
    {
        public static PresenterBinderGeneric<TPresenter> BindPresenter<TPresenter>(this DiContainer di)
            where TPresenter : IUIScreenPresenter
        {
            return new PresenterBinderGeneric<TPresenter>(di.StartBinding());
        }
    }
}
