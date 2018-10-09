using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Actor
{
    public interface IOtherPlayer
    {

    }

    public sealed class OtherPlayer : FightActor, IOtherPlayer
    {
        public OtherPlayer(Transform model) : base(model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model is null");
            }

        }

    }
}

