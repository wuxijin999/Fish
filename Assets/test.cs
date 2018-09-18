﻿using System.Collections;
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
        animator = this.GetComponent<Animator>();
        animator.runtimeAnimatorController = overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            overrideController["idle"] = run;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            overrideController["idle"] = walk;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            overrideController["idle"] = null;
        }
    }


}

