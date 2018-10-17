using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Actor
{
    public class FightActor : ActorBase
    {
        public FightActor(Transform transform) : base(transform)
        {
            if (transform == null)
            {
                throw new ArgumentNullException("transform is null");
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        public override void OnLateUpdate1()
        {
            base.OnLateUpdate1();
        }

        public override void OnLateUpdate2()
        {
            base.OnLateUpdate2();
        }

        public override void OnUpdate1()
        {
            base.OnUpdate1();
        }

        public override void OnUpdate2()
        {
            base.OnUpdate2();
        }

        internal virtual void Attack(int index)
        {

        }

        internal virtual void CastSkill(int index)
        {

        }

        public virtual void ProcessAttackEvent(int attackIndex, int rangeLeft, int rangeRight)
        {

        }

        public virtual void ProcessSkillEvent(int skillIndex, int rangeLeft, int rangeRight)
        {

        }



    }
}


