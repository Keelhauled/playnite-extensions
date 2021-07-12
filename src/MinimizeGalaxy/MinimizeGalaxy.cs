using System;
using System.Diagnostics;
using Playnite.SDK;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;

[assembly: System.Reflection.AssemblyFileVersion("1.0.0")]

namespace MinimizeGalaxy
{
    public class MinimizeGalaxy : Plugin
    {
        private readonly ILogger logger;
        public override Guid Id { get; } = new Guid("E314923B-A716-4500-8DB3-A11F7E253A7F");
        
        public MinimizeGalaxy(IPlayniteAPI api) : base(api)
        {
            logger = api.CreateLogger();
        }

        public override void OnGameStarted(Game game)
        {
            if(game.Source.Name == "GOG")
            {
                foreach(var proc in Process.GetProcessesByName("GalaxyClient"))
                {
                    logger.Info($"Minimizing GOG Galaxy ({proc.Id})");
                    proc.CloseMainWindow();
                }
            }
        }
    }
}