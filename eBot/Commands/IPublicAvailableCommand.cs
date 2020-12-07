namespace eBot.Commands
{
    public interface IPublicAvailableCommand : ICommand
    {
        string HumanReadableDescription { get; }
    }
}