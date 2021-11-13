namespace PlatformRacing3.Server.API.Game.Commands
{
	public interface ICommand
    {
        string Permission { get; }
        void OnCommand(ICommandExecutor executor, string label, ReadOnlySpan<string> args);
    }
}
