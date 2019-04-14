using System;

using Gwent.Core.Environment;
using Gwent.Core.Game;
using Gwent.Environment;
using Gwent.Game;

using Microsoft.Extensions.DependencyInjection;

namespace Gwent
{
    internal static class Program
    {
        private static void Main()
        {
            InitContainer().InitServices().StartGwentGame();
        }

        private static IServiceCollection InitContainer()
        {
            return new ServiceCollection();
        }

        private static IServiceProvider InitServices(this IServiceCollection services)
        {
            services.AddSingleton<GwentGame>();
            services.AddSingleton<IGwentGameOperations, GwentGameOperations>();
            services.AddSingleton<IGwentGameIO, GwentGameIO>();

            services.AddTransient<IPlayer, Player>();
            services.AddTransient<IDeck, Deck>();

            return services.BuildServiceProvider();
        }

        private static void StartGwentGame(this IServiceProvider services)
        {
            services.GetRequiredService<GwentGame>().Start();
        }
    }
}
