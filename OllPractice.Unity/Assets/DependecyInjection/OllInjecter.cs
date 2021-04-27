using Injecter;
using Injecter.Unity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Ble;
using Parser;
using UnityEngine;

namespace OllPractice.DepenecyInjection
{
    [DefaultExecutionOrder(-999)]
    public sealed class OllInjecter : InjectStarter
    {
        private IHost _host;

        protected override IServiceProvider CreateServiceProvider()
        {
            _host = new HostBuilder()
                .ConfigureServices(OllInjector.AddOll)
                .Build();

            CompositionRoot.ServiceProvider = _host.Services;

            return CompositionRoot.ServiceProvider;
        }

        protected override void Awake()
        {
            base.Awake();

            _host.Start();
        }

        private void OnApplicationQuit() => _host?.Dispose();
    }

    public static class OllInjector
    {
        public static void AddOll(HostBuilderContext hostBuilderContext, IServiceCollection services)
        {
            services.AddSceneInjector(
                injecterOptions => injecterOptions.UseCaching = true,
                sceneInjectorOptions =>
                {
                    sceneInjectorOptions.DontDestroyOnLoad = true;
                    sceneInjectorOptions.InjectionBehavior = SceneInjectorOptions.Behavior.CompositionRoot;
                });


            services.AddSingleton<INotificationParser, NotificationParser>();
            services.AddSingleton<IBle, DesktopBle>();
        }
    }
}
