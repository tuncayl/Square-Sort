using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandStack
{
    private Stack<ICommand> commandhistory = new Stack<ICommand>();

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandhistory.Push(command);
    }
    public void UndoCommand()
    {
        if (commandhistory.Count <= 0) return;
        commandhistory.Pop().Undo();
    }

}
