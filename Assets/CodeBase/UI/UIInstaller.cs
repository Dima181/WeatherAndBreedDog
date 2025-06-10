using Assets.CodeBase.UI.Core;
using Zenject;

namespace Assets.CodeBase.UI
{
    public abstract class UIInstaller<THudView, THudPresenter> : MonoInstaller
        where THudPresenter : UIScreenPresenter<THudView>, ISimpleUIScreenPresenter
        where THudView : UIScreenView
    {
        protected abstract string HudPrefabPath { get; }

        public override void InstallBindings()
        {
            Container.Bind<UIRoot>()
                .FromComponentInNewPrefabResource("Systems/UI Root")
                .AsSingle();

            Container.BindInterfacesAndSelfTo<UINavigator>()
                .AsSingle();

            Container.BindInitializableExecutionOrder<UINavigator>(-200);

            Container.Bind<THudView>()
                .FromComponentInNewPrefabResource(HudPrefabPath)
                .WithGameObjectName("HUD")
                .UnderTransform(ic => ic.Container.Resolve<UIRoot>().Screens.transform)
                .AsSingle()
                .OnInstantiated<THudView>((ic, o) => o.gameObject.SetActive(true));

            Container.Bind<THudPresenter>()
                .AsSingle()
                .WithArgumentsExplicit(new[] { new TypeValuePair(typeof(EScreenType), EScreenType.Screen) })
                .OnInstantiated<THudPresenter>((ic, presenter) =>
                {
                    presenter.ShowAndForget();
                });

            Container.Bind<ISimpleUIScreenPresenter>()
                .WithId("HUD")
                .FromResolveGetter<THudPresenter>(t => t)
                .AsSingle();

            InstallBindingsInternal();
        }

        protected abstract void InstallBindingsInternal();
    }
}
