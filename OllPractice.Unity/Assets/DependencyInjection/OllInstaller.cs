using Ble;
using Parser;
using Zenject;

namespace DependencyInjection
{
    public class OllInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<INotificationParser>().To<NotificationParser>().AsCached();
#if UNITY_ANDROID
            Container.Bind<IBle>().To<AndroidBle>().AsSingle();
#else
            Container.Bind<IBle>().To<DesktopBle>().AsSingle();
#endif
        }
    }
}