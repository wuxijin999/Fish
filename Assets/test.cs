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
    [SerializeField] int m_Quality;
    [SerializeField] Color m_Color;
    [SerializeField] Text m_Text;



    private void OnEnable()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            m_Color = ColorUtility.QualityColor(m_Quality);

            m_Text.text = ColorUtility.QualityColorString(m_Quality, "aaaa");

            DebugEx.Log("aaaa");
        }
    }


}

