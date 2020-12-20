namespace eBot.Commands
{
    public interface IPublicAvailableCommand : IBotCommand
    {
        string HumanReadableDescription { get; }
    }
}