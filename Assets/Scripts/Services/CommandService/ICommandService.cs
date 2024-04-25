namespace Services.CommandService
{
    public interface ICommandService
    {
        CommandInvoker GetCommandInvoker();
    }
}