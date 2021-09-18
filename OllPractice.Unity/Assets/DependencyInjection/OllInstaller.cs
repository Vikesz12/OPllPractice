using Ble;
using EventBus;
using Parser;
using Zenject;

namespace DependencyInjection
{
    public class OllInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IEventBus>().To<EventBus.EventBus>().AsSingle();
            Container.Bind<INotificationParser>().To<NotificationParser>().AsCached();
#if UNITY_ANDROID
            Container.Bind<IBle>().To<AndroidBle>().AsSingle();
#else
            Container.Bind<IBle>().To<DesktopBle>().AsSingle();
#endif
        }
    }
}