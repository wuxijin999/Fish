using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{



}

public enum ActionState
{
    CombatIdle = 1,
    Walk = 2,
    Run = 3,
    Jump = 4,

    Attack1 = 11,
    Attack2 = 12,
    Attack3 = 13,
    Attack4 = 14,
    Skill1 = 21,
    Skill2 = 22,
    Skill3 = 23,
    Skill4 = 24,
    Skill5 = 25,

    Hurt = 31,
    FaceUp = 32,
    FaceDown = 33,
    Stun = 34,
}
