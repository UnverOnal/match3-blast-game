using System.Collections.Generic;

namespace Services.CommandService
{
public class CommandInvoker
{
    public Stack<ICommand> Commands { get; }
    
    private readonly Stack<ICommand> _undoStack = new();

    private readonly Stack<ICommand> _redoStack = new();

    public CommandInvoker()
    {
        Commands = new Stack<ICommand>();
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _undoStack.Push(command);

        _redoStack.Clear();
    }

    public void UndoCommand()
    {
        if (_undoStack.Count > 0)
        {
            var activeCommand = _undoStack.Pop();
            _redoStack.Push(activeCommand);
            Commands.Push(activeCommand);
            activeCommand.Undo();
        }
    }
    
    public void UndoCommandAll()
    {
        while (_undoStack.Count > 0)
            UndoCommand();
    }

    public void RedoCommand()
    {
        if (_redoStack.Count > 0)
        {
            var activeCommand = _redoStack.Pop();
            _undoStack.Push(activeCommand);
            Commands.Push(activeCommand);
            activeCommand.Execute();
        }
    }

    public void Reset()
    {
        _undoStack.Clear();
        _redoStack.Clear();
    }
}
}