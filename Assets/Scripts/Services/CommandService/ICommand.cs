namespace Services.CommandService
{
    public interface ICommand
    {
        public void Execute();
        public void Undo();
    }
}