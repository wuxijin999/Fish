using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class WindowSetting : MonoBehaviour
{
    [SerializeField] int m_Id;
    public int id { get { return this.m_Id; } }

    [SerializeField] Tween m_Tween;
    public Tween tween { get { return m_Tween; } }

    [SerializeField] ButtonEx m_Close;
    public ButtonEx close { get { return m_Close; } }

    [SerializeField] RectTransform m_BackGround;
    public RectTransform backGround { get { return m_BackGround; } }

    [SerializeField] RectTransform m_Content;
    public RectTransform content { get { return m_Content; } }

    [ExecuteInEditMode]
    private void Awake()
    {
        if (m_BackGround == null)
        {
            var temp = this.transform.Find("BackGround");
            if (temp != null)
            {
                m_BackGround = temp as RectTransform;
            }
        }

        if (m_Content == null)
        {
            var temp = this.transform.Find("Content");
            if (temp != null)
            {
                m_Content = temp as RectTransform;
            }
        }

    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            var uiroot = GameObject.FindObjectOfType<UIRoot>();
            if (uiroot == null)
            {
                var prefab = BuiltInAssets.LoadPrefab("UIRoot");
                var instance = GameObject.Instantiate(prefab);
                instance.name = "UIRoot";

                uiroot = instance.GetComponent<UIRoot>();
                var windowRoot = uiroot.transform.GetChildTransformDeeply("WindowRoot");
                if (windowRoot != null)
                {
                    (this.transform as RectTransform).MatchWhith(windowRoot as RectTransform);
                }

                var uicamera = uiroot.GetComponentInChildren<Camera>(true);
                uicamera.clearFlags = CameraClearFlags.SolidColor;
            }
        }
    }

#endif

}
