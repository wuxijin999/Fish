using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBehaviour : UIComponentBase
{
    [SerializeField] Text m_Text1;
    [SerializeField] Text m_Text2;

    TestBiz testBiz = new TestBiz();

    public override void OnUpdate()
    {
        if (testBiz.testString1.dirty)
        {
            m_Text1.text = testBiz.testString1.value;
            testBiz.testString1.dirty = false;
        }

        if (testBiz.testInt1.dirty)
        {
            m_Text2.text = testBiz.testInt1.value.ToString();
            testBiz.testInt1.dirty = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            testBiz.UpdateInt();
            testBiz.UpdateString();
        }
    }

}
