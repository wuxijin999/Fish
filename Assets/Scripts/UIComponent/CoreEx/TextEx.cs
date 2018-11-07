//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Friday, August 31, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class TextEx : TextMeshProUGUI
{

    [SerializeField] int m_LanguageKey;

    string currentLanguge = string.Empty;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (m_LanguageKey != 0)
        {
            if (currentLanguge != Language.currentLanguage)
            {
                SetText(Language.Get(m_LanguageKey));
                currentLanguge = Language.currentLanguage;
            }
        }
    }


}



