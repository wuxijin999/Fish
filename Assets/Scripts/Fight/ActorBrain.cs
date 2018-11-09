using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBrain
{
    const int commandMaxCount = 2;
    List<Command> commands = new List<Command>();

    State m_State = State.Leisure;
    public State state { get { return this.m_State; } }

    ActorBase actorBase;

    public ActorBrain(ActorBase actorBase)
    {
        this.actorBase = actorBase;
    }

    public void PushCommand(CommandType type, object value)
    {
        switch (type)
        {
            case CommandType.Stop:
                this.commands.Clear();
                this.commands.Add(new Command(type, value));
                break;
            default:
                var priority = GetPriority(type);
                for (int i = this.commands.Count - 1; i >= 0; i--)
                {
                    var command = this.commands[i];
                    if (priority > GetPriority(command.type))
                    {
                        this.commands.Remove(command);
                    }
                }

                this.commands.Add(new Command(type, value));
                while (this.commands.Count > 2)
                {
                    this.commands.RemoveAt(0);
                }
                break;
        }
    }

    public bool GetCommand(out Command command)
    {
        if (this.commands.Count > 0)
        {
            command = this.commands[0];
            this.commands.RemoveAt(0);
            return true;
        }
        else
        {
            command = default(Command);
            return false;
        }
    }

    public void Update()
    {
        if (this.state == State.Leisure)
        {
            Command cmd;
            if (GetCommand(out cmd))
            {
                ExecuteCommand(cmd);
                if (cmd.type != CommandType.Stop)
                {
                    this.m_State = State.Busy;
                }
            }
        }
    }

    private void ExecuteCommand(Command command)
    {
        switch (command.type)
        {
            case CommandType.Stop:
                this.actorBase.Stop();
                break;
            case CommandType.Move:
                this.actorBase.MoveTo((Vector3)command.value);
                break;
            case CommandType.Attack:
                (this.actorBase as FightActor).Attack((int)command.value);
                break;
            case CommandType.Skill:
                (this.actorBase as FightActor).CastSkill((int)command.value);
                break;
            default:
                break;
        }
    }

    public static int GetPriority(CommandType type)
    {
        return ((int)type) / 10;
    }

    public struct Command
    {
        public readonly CommandType type;
        public readonly object value;

        public Command(CommandType type)
        {
            this.type = type;
            this.value = null;
        }

        public Command(CommandType type, object value)
        {
            this.type = type;
            this.value = value;
        }
    }

    public enum State
    {
        Leisure = 0,
        Busy = 1,
    }

}

public enum CommandType//命令优先级为命令值除以10
{
    Move = 1,

    Attack = 10,
    Skill = 20,

    Stop = 100000,
}


