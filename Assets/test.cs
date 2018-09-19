using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using UnityEngine.U2D;
using System;
using System.Timers;
using System.Threading;
using UnityEngine.Jobs;

public class test : MonoBehaviour
{

    public AnimationClip run;
    public AnimationClip walk;

    Animator animator;
    AnimatorOverrideController overrideController;

    private void OnEnable()
    {
        this.animator = this.GetComponent<Animator>();
        this.animator.runtimeAnimatorController = this.overrideController = new AnimatorOverrideController(this.animator.runtimeAnimatorController);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            this.overrideController["idle"] = this.run;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            this.overrideController["idle"] = this.walk;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            this.overrideController["idle"] = null;
        }
    }


}

