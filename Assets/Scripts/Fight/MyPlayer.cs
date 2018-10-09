using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Actor
{
    public interface IMyPlayer
    {

    }

    public sealed class MyPlayer : FightActor, IMyPlayer
    {
        public MyPlayer(Transform model) : base(model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("transform is null");
            }


        }

    }
}

