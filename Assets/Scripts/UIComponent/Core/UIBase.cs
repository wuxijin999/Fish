using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public abstract class UIBase : MonoBehaviour
{
    RectTransform m_RecTransform;
    public RectTransform rectTransform { get { return this.m_RecTransform ?? (this.m_RecTransform = this.transform as RectTransform); } }

    protected virtual void Awake()
    {
        UIEngine.Instance.Register(this);
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnLateUpdate()
    {

    }

    protected virtual void OnDestory()
    {
        UIEngine.Instance.UnRegister(this);
    }

}
