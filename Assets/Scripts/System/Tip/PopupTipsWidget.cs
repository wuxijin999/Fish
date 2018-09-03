//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Monday, July 09, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PopupTipsWidget : Widget
{

    const int maxCount = 3;
    const float eisxtTime = 5f;

    [SerializeField] float m_PopupSpeed = 10f;
    public float popupSpeed { get { return Mathf.Clamp(m_PopupSpeed, 5f, 1000f); } }

    [SerializeField] RectTransform m_StartPoint;
    [SerializeField] RectTransform m_PriorPoint;

    GameObjectPool m_Pool;
    GameObjectPool pool { get { return m_Pool ?? (m_Pool = GameObjectPoolUtil.Create(UIAssets.LoadPrefab("PopupTipBehaviour"))); } }

    Queue<string> tips = new Queue<string>();
    List<PopupTipBehaviour> activedBehaviours = new List<PopupTipBehaviour>();

    float interval = 0.5f;
    float nextTipTime = 0f;

    public override void AddListeners()
    {
    }

    public override void BindControllers()
    {
    }

    public void Popup(string tip)
    {
        while (tips.Count > maxCount)
        {
            tips.Dequeue();
        }

        tips.Enqueue(tip);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (Time.time > nextTipTime && tips.Count > 0)
        {
            nextTipTime = Time.time + interval;

            while (activedBehaviours.Count >= maxCount)
            {
                activedBehaviours[0].FadeOut(1f, 0f, 0.5f, OnFadeOut);
                activedBehaviours.RemoveAt(0);
            }

            var tip = tips.Dequeue();
            var instance = pool.Get();
            var behaviour = instance.GetComponent<PopupTipBehaviour>();
            activedBehaviours.Add(behaviour);
            behaviour.transform.SetParentEx(this.transform, m_StartPoint.localPosition, Quaternion.identity, Vector3.one);

            var from = m_StartPoint.anchoredPosition.y;
            var to = m_PriorPoint.anchoredPosition.y;
            var duration = Mathf.Abs(to - from) / popupSpeed;

            behaviour.fadeOutTime = Time.time + eisxtTime;
            behaviour.Popup(tip, from, to, duration);

            if (activedBehaviours.Count > 1)
            {
                var delay = (Mathf.Abs(to - from) - behaviour.rectTransform.rect.height) / popupSpeed;
                StartCoroutine("Co_DelayReArrange", delay);
            }
        }
    }

    private void LateUpdate()
    {
        for (int i = activedBehaviours.Count - 1; i >= 0; i--)
        {
            var activedBehaviour = activedBehaviours[i];
            if (Time.time > activedBehaviour.fadeOutTime)
            {
                activedBehaviour.FadeOut(1f, 0f, 0.5f, OnFadeOut);
                activedBehaviours.Remove(activedBehaviour);
            }
        }
    }

    IEnumerator Co_DelayReArrange(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < activedBehaviours.Count - 1; i++)
        {
            var activedBehaviour = activedBehaviours[i];
            var formY = activedBehaviour.rectTransform.anchoredPosition.y;
            var toY = m_PriorPoint.anchoredPosition.y + activedBehaviour.rectTransform.rect.height * (activedBehaviours.Count - i - 1);
            activedBehaviour.Move(formY, toY, Mathf.Abs(formY - toY) / popupSpeed);
        }
    }

    private void OnFadeOut(PopupTipBehaviour behaviour)
    {
        pool.Release(behaviour.gameObject);
    }


}



