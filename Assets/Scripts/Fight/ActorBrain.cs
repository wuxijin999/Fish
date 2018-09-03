using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBrain
{
    const int commandMaxCount = 2;
    List<Command> commands = new List<Command>();

    public void PushCommand(CommandType type)
    {
        PushCommand(type, 0);
    }

    public void PushCommand(CommandType type, int value)
    {
        switch (type)
        {
            case CommandType.Stop:
                commands.Clear();
                commands.Add(new Command(type, value));
                break;
            default:
                var priority = GetPriority(type);
                for (int i = commands.Count - 1; i >= 0; i--)
                {
                    var command = commands[i];
                    if (priority > GetPriority(command.type))
                    {
                        commands.Remove(command);
                    }
                }
                break;
        }
    }

    public bool GetCommand(out Command command)
    {
        if (commands.Count > 0)
        {
            command = commands[0];
            commands.RemoveAt(0);
            return true;
        }
        else
        {
            command = default(Command);
            return false;
        }
    }

    public static int GetPriority(CommandType type)
    {
        return ((int)type) / 10;
    }

    public struct Command
    {
        public readonly CommandType type;
        public readonly int value;

        public Command(CommandType type)
        {
            this.type = type;
            this.value = 0;
        }

        public Command(CommandType type, int value)
        {
            this.type = type;
            this.value = value;
        }
    }

    public enum CommandType//命令优先级为命令值除以10
    {
        Walk = 1,
        Run = 2,

        Attack = 10,
        Skill = 20,

        Stop = 100000,
    }


}


