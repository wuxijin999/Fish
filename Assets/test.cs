using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using UnityEngine.U2D;
using System;
using System.Timers;
using System.Threading;

public class test : MonoBehaviour
{
    [SerializeField] string m_input;

    private void OnEnable()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            var testConfig1 = TestConfig.Get(1);
            var testConfig2 = TestConfig.Get(2);
            var testConfig3 = TestConfig.Get(3);

            DebugEx.Log("aaaa");
        }
    }


}

