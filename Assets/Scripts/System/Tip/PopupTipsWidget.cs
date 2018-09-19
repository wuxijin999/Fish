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
    public float popupSpeed { get { return Mathf.Clamp(this.m_PopupSpeed, 5f, 1000f); } }

    [SerializeField] RectTransform m_StartPoint;
    [SerializeField] RectTransform m_PriorPoint;

    GameObjectPool m_Pool;
    GameObjectPool pool { get { return this.m_Pool ?? (this.m_Pool = GameObjectPoolUtil.Create(UIAssets.LoadPrefab("PopupTipBehaviour"))); } }

    Queue<string> tips = new Queue<string>();
    List<PopupTipBehaviour> activedBehaviours = new List<PopupTipBehaviour>();

    float interval = 0.5f;
    float nextTipTime = 0f;

    public void Popup(string tip)
    {
        while (this.tips.Count > maxCount)
        {
            this.tips.Dequeue();
        }

        this.tips.Enqueue(tip);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (Time.time > this.nextTipTime && this.tips.Count > 0)
        {
            this.nextTipTime = Time.time + this.interval;

            while (this.activedBehaviours.Count >= maxCount)
            {
                this.activedBehaviours[0].FadeOut(1f, 0f, 0.5f, this.OnFadeOut);
                this.activedBehaviours.RemoveAt(0);
            }

            var tip = this.tips.Dequeue();
            var instance = this.pool.Get();
            var behaviour = instance.GetComponent<PopupTipBehaviour>();
            this.activedBehaviours.Add(behaviour);
            behaviour.transform.SetParentEx(this.transform)
                                             .SetLocalPosition(this.m_StartPoint.localPosition)
                                             .SetLocalEulerAngles(Vector3.zero)
                                             .SetScale(Vector3.one);

            var from = this.m_StartPoint.anchoredPosition.y;
            var to = this.m_PriorPoint.anchoredPosition.y;
            var duration = Mathf.Abs(to - from) / this.popupSpeed;

            behaviour.fadeOutTime = Time.time + eisxtTime;
            behaviour.Popup(tip, from, to, duration);

            if (this.activedBehaviours.Count > 1)
            {
                var delay = (Mathf.Abs(to - from) - behaviour.rectTransform.rect.height) / this.popupSpeed;
                StartCoroutine("Co_DelayReArrange", delay);
            }
        }
    }

    private void LateUpdate()
    {
        for (int i = this.activedBehaviours.Count - 1; i >= 0; i--)
        {
            var activedBehaviour = this.activedBehaviours[i];
            if (Time.time > activedBehaviour.fadeOutTime)
            {
                activedBehaviour.FadeOut(1f, 0f, 0.5f, this.OnFadeOut);
                this.activedBehaviours.Remove(activedBehaviour);
            }
        }
    }

    IEnumerator Co_DelayReArrange(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < this.activedBehaviours.Count - 1; i++)
        {
            var activedBehaviour = this.activedBehaviours[i];
            var formY = activedBehaviour.rectTransform.anchoredPosition.y;
            var toY = this.m_PriorPoint.anchoredPosition.y + activedBehaviour.rectTransform.rect.height * (this.activedBehaviours.Count - i - 1);
            activedBehaviour.Move(formY, toY, Mathf.Abs(formY - toY) / this.popupSpeed);
        }
    }

    private void OnFadeOut(PopupTipBehaviour behaviour)
    {
        this.pool.Release(behaviour.gameObject);
    }


}



