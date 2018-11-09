using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Actor
{
    public class ActorBase
    {
        int m_InstanceId = 0;
        public int instanceId {
            get { return this.m_InstanceId; }
            set { this.m_InstanceId = value; }
        }

        bool m_Enable = false;
        public bool enable {
            get { return this.m_Enable; }
            set {
                this.m_Enable = value;
            }
        }

        ActorBrainState m_BrainState = ActorBrainState.Sane;
        public ActorBrainState brainState {
            get { return this.m_BrainState; }
            set { this.m_BrainState = value; }
        }

        public ActorType actorType { get; set; }
        public float speed { get { return propertyController.GetProperty(FightProperty.MoveSpeed) * 0.001f; } }
        public bool alive { get { return propertyController.GetProperty(FightProperty.Hp) > 0; } }

        public readonly PropertyController propertyController = new PropertyController();
        public readonly LogicController logicController = new LogicController();
        public ActorBrain actorBrain { get; private set; }
        public PathFinder pathFinder { get; private set; }
        public Transform transform { get; private set; }
        public Vector3 position {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public ActorBase(Transform transform)
        {
            if (transform == null)
            {
                throw new ArgumentNullException("transform is null");
            }

            this.transform = transform;
            this.pathFinder = new PathFinder(this);
            this.actorBrain = new ActorBrain(this);
            ActorEngine.Instance.Register(this);
        }

        public void Dispose()
        {
            enable = false;
            actorBrain = null;
            pathFinder = null;
            transform = null;
            ActorEngine.Instance.UnRegister(this);
        }

        public virtual void PushCommand(CommandType type, object value)
        {
            this.actorBrain.PushCommand(type, value);
        }

        public virtual void ProcessMoveEvent(int rangeLeft, int rangeRight)
        {

        }

        internal virtual void MoveTo(Vector3 position)
        {
            logicController.DoAction(LogicStateType.Move);
            this.pathFinder.MoveTo(position);
        }

        internal virtual void Stop()
        {
            this.pathFinder.Stop();
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnUpdate1()
        {
            this.pathFinder.Update();
        }

        public virtual void OnUpdate2()
        {
            this.actorBrain.Update();
        }

        public virtual void OnLateUpdate1()
        {
        }

        public virtual void OnLateUpdate2()
        {
        }

    }


    public enum ActorBrainState
    {
        Sane = 1,                   //理智状态，可以正常思考
        Obstinate = 2,            //执着于做某件事情，直到做完为止
        Lost = 3,                    //失去控制，完全不能控制自己的行为
    }

    public enum ActorType
    {
        Emeny,
        NPC,
    }
}



