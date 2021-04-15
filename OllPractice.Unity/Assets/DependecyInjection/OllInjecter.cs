using System;
using Injecter.Unity;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;

namespace OllPractie.DepenecyInjection
{
    [DefaultExecutionOrder(-999)]
    public sealed class OllInjecter : InjectStarter
    {
        // Override CreateServiceProvider to add service registrations
        protected override IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            // Mandatory to call AddSceneInjector, optionally configure options
            services.AddSceneInjector(
                injecterOptions => injecterOptions.UseCaching = true,
                sceneInjectorOptions =>
                {
                    sceneInjectorOptions.DontDestroyOnLoad = true;
                    sceneInjectorOptions.InjectionBehavior = SceneInjectorOptions.Behavior.Factory;
                });


            return services.BuildServiceProvider();
        }
    }
}
