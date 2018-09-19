using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public class FightJudge : Singleton<FightJudge>
    {

        public bool CanAttack(FightActor attacker, FightActor victim, int skillId)
        {

            return true;
        }


    }

}

