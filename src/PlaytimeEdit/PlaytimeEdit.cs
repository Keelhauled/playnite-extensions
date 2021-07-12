using System;
using System.Collections.Generic;
using Playnite.SDK;
using Playnite.SDK.Plugins;

[assembly: System.Reflection.AssemblyFileVersion("1.0.0")]

namespace PlaytimeEdit
{
    public class PlaytimeEdit : Plugin
    {
        private readonly ILogger logger;
        public override Guid Id { get; } = new Guid("10D00F7B-D644-4864-A623-F158C5594254");
        
        public PlaytimeEdit(IPlayniteAPI api) : base(api)
        {
            logger = api.CreateLogger();
        }

        public override List<GameMenuItem> GetGameMenuItems(GetGameMenuItemsArgs args)
        {
            return new List<GameMenuItem>
            {
                new GameMenuItem
                {
                    Description = "Edit Playtime",
                    Action = InvokeGameMenuFunction
                }
            };
        }

        private void InvokeGameMenuFunction(GameMenuItemActionArgs args)
        {
            foreach(var game in args.Games)
            {
                var time = SecondsToTime(game.Playtime);
                var input = PlayniteApi.Dialogs.SelectString($"Current playtime is {time}.{Environment.NewLine + Environment.NewLine}Set new playtime:", game.Name, time);

                if(input.Result)
                {
                    var newTime = ParseTime(input.SelectedString);
                    game.Playtime = (long)newTime.TotalSeconds;
                    PlayniteApi.Database.Games.Update(game);
                }
            }
        }

        private string SecondsToTime(long totalSeconds)
        {
            var span = TimeSpan.FromSeconds(totalSeconds);
            var hours = (span.Days * 24 + span.Hours).ToString("00");
            var minutes = span.Minutes.ToString("00");
            var seconds = span.Seconds.ToString("00");
            return $"{hours}:{minutes}:{seconds}";
        }
        
        private TimeSpan ParseTime(string time)
        {
            var split = time.Split(':');
            var hours = int.Parse(split[0]);
            var minutes = int.Parse(split[1]);
            var seconds = int.Parse(split[2]);
            return new TimeSpan(hours, minutes, seconds);
        }
    }
}