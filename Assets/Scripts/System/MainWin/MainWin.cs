using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class MainWin : Window
{

    protected override void OnPreClose()
    {
        SetWidgetActive<SkillWidget>(false);
        SetWidgetActive<JoyStickWidget>(false);
    }

    protected override void OnPreOpen()
    {
    }

    protected override void OnActived()
    {
        base.OnActived();
        SetWidgetActive<SkillWidget>(true);
        SetWidgetActive<JoyStickWidget>(true);
    }

}
