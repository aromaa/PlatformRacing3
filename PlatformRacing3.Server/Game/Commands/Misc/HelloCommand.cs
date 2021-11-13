using PlatformRacing3.Server.API.Game.Commands;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Commands.Misc
{
	internal class HelloCommand : ICommand
    {
        public string Permission => null;

        public void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args)
        {
            if (executor is ClientSession client)
            {
                executor.SendMessage($"Hello, {client.UserData.Username} !");
            }
            else
            {
                executor.SendMessage("Hello, someone!");
            }
        }
    }
}
