//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Saturday, September 15, 2018
//--------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[DisallowMultipleComponent]
public class CyclicScroll : UIBase, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform m_Content;
    public RectTransform content { get { return m_Content; } }

    [SerializeField] protected BoundOffset m_BoundOffset;
    [SerializeField] protected Vector2 m_CellSize = new Vector2(100, 100);
    public Vector2 cellSize { get { return m_CellSize; } }

    [SerializeField] protected Vector2 m_Spacing;
    public Vector2 spacing { get { return m_Spacing; } }

    [SerializeField] Align m_Align = Align.Top;
    public Align align { get { return m_Align; } }

    [SerializeField] bool m_ElasticityOn = true;
    public bool elasticityOn { get { return m_ElasticityOn; } }

    [SerializeField] private float m_Elasticity = 0.1f;
    public float elasticity { get { return m_Elasticity; } }

    [SerializeField] private float m_DecelerationRate = 0.135f;
    public float decelerationRate { get { return m_DecelerationRate; } }

    public float normalizedPosition
    {
        get
        {
            if (content == null)
            {
                return 0f;
            }

            var offset = (dataMaxOffset - dataMinOffset);
            var axisIndex = align == Align.Left || align == Align.Right ? 0 : 1;
            return Mathf.Clamp01(Mathf.Abs((content.anchoredPosition[axisIndex] - dataMinOffset[axisIndex]) / offset[axisIndex]));
        }
        set
        {
            if (content == null)
            {
                return;
            }

            value = Mathf.Clamp01(value);
            var normalize = 0f;
            var offset = (dataMaxOffset - dataMinOffset);
            switch (align)
            {
                case Align.Top:
                case Align.Bottom:
                    normalize = (content.anchoredPosition.y - dataMinOffset.y) / offset.y;
                    if (Mathf.Abs(normalize - value) > 0.0001f)
                    {
                        targetOffset = dataMinOffset + new Vector2(0, dataMaxOffset.y * value);
                        velocity = 0f;
                        autoLerp = true;
                        refAutoLerpPosition = Vector2.zero;
                    }
                    break;
                case Align.Left:
                case Align.Right:
                    normalize = (content.anchoredPosition.x - dataMinOffset.x) / offset.x;
                    if (Mathf.Abs(normalize - value) > 0.0001f)
                    {
                        targetOffset = dataMinOffset + new Vector2(dataMaxOffset.x * value, 0);
                        velocity = 0f;
                        autoLerp = true;
                        refAutoLerpPosition = Vector2.zero;
                    }
                    break;
            }
        }
    }

    protected Vector2 dataMaxOffset = Vector2.zero;
    protected Vector2 dataMinOffset = Vector2.zero;

    public bool autoLerp { get; private set; }

    Vector2 targetOffset = Vector2.zero;
    Vector2 refAutoLerpPosition = Vector2.zero;

    protected IList datas;
    protected List<ScrollItem> infiniteItems = new List<ScrollItem>();
    List<ScrollItem> tempList = new List<ScrollItem>();

    Vector2 startMousePosition = Vector2.zero;
    Vector2 startContentPosition = Vector2.zero;
    Vector2 prevPosition = Vector2.zero;

    protected float velocity = 0f;
    protected bool dragging = false;

    public int dataCount { get { return datas == null ? 0 : datas.Count; } }
    bool moveNextable { get { return hostIndex < dataCount - 1; } }
    bool moveLastable { get { return preIndex > 0; } }
    protected int preIndex { get; set; }
    protected int hostIndex { get; set; }

    Vector2 maxOffset { get { return rectTransform.GetMaxReferencePosition(rectTransform); } }
    Vector2 minOffset { get { return rectTransform.GetMinReferencePosition(rectTransform); } }

    public virtual void Init<T>(List<T> _datas, bool _stepByStep = false)
    {
        if (_datas == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        datas = _datas;

        ReArrange();
        FillBatchData(0);

        if (_stepByStep)
        {
            for (int i = 0; i < infiniteItems.Count; i++)
            {
                var infiniteItem = infiniteItems[i];
                if (infiniteItem != null)
                {
                    infiniteItem.gameObject.SetActive(false);
                }
            }
        }

        this.gameObject.SetActive(true);

        dataMinOffset = content.anchoredPosition;
        var totalOffset = dataCount == 0 ? Vector2.zero : cellSize * dataCount + spacing * (dataCount - 1)
            + new Vector2(m_BoundOffset.left + m_BoundOffset.right, m_BoundOffset.top + m_BoundOffset.bottom);

        var longer = Mathf.Abs(totalOffset.x) > Mathf.Abs(rectTransform.rect.width);
        var higher = Mathf.Abs(totalOffset.y) > Mathf.Abs(rectTransform.rect.height);
        switch (align)
        {
            case Align.Left:
                dataMaxOffset = longer ? dataMinOffset - new Vector2(totalOffset.x - rectTransform.rect.width, 0) : dataMinOffset;
                break;
            case Align.Right:
                dataMaxOffset = longer ? dataMinOffset + new Vector2(totalOffset.x - rectTransform.rect.width, 0) : dataMinOffset;
                break;
            case Align.Top:
                dataMaxOffset = higher ? dataMinOffset + new Vector2(0, totalOffset.y - rectTransform.rect.height) : dataMinOffset;
                break;
            case Align.Bottom:
                dataMaxOffset = higher ? dataMinOffset - new Vector2(0, totalOffset.y - rectTransform.rect.height) : dataMinOffset;
                break;
        }

        if (_stepByStep)
        {
            StartCoroutine("Co_StepByStepAppear");
        }
    }

    public void Dispose()
    {
        velocity = 0f;
        StopAllCoroutines();
        for (int i = 0; i < infiniteItems.Count; i++)
        {
            var infiniteItem = infiniteItems[i];
            if (infiniteItem != null)
            {
                infiniteItem.Dispose();
            }
        }
    }

    public void HideAll()
    {
        StopAllCoroutines();
        for (int i = 0; i < content.childCount; i++)
        {
            content.GetChild(i).gameObject.SetActive(false);
        }
    }

    IEnumerator Co_StepByStepAppear()
    {
        for (int i = 0; i < infiniteItems.Count; i++)
        {
            var infiniteItem = infiniteItems[i];
            if (infiniteItem != null && i < datas.Count)
            {
                infiniteItem.gameObject.SetActive(true);
                yield return WaitingForConst.millisecond100;
            }
        }
    }

    public void MoveToCenter(int _dataIndex)
    {
        if (_dataIndex < 0 || _dataIndex >= dataCount)
        {
            return;
        }

        var fillSpace = (_dataIndex + 0.5f) * cellSize + _dataIndex * spacing;
        switch (align)
        {
            case Align.Left:
                var leftOffsetX = Mathf.Clamp(m_BoundOffset.left + fillSpace[0] - rectTransform.rect.width * 0.5f, 0, Mathf.Abs(dataMaxOffset.x));
                targetOffset = new Vector2(dataMinOffset.x - leftOffsetX, 0);
                break;
            case Align.Right:
                var rightOffsetX = Mathf.Clamp(m_BoundOffset.right + fillSpace[0] - rectTransform.rect.width * 0.5f, 0, Mathf.Abs(dataMaxOffset.x));
                targetOffset = new Vector2(dataMinOffset.x + rightOffsetX, 0);
                break;
            case Align.Top:
                var topOffsetY = Mathf.Clamp(m_BoundOffset.top + fillSpace[1] - rectTransform.rect.height * 0.5f, 0, Mathf.Abs(dataMaxOffset.y));
                targetOffset = new Vector2(0, dataMinOffset.y + topOffsetY);
                break;
            case Align.Bottom:
                var bottomOffsetY = Mathf.Clamp(m_BoundOffset.bottom + fillSpace[1] - rectTransform.rect.height * 0.5f, 0, Mathf.Abs(dataMaxOffset.y));
                targetOffset = new Vector2(0, dataMinOffset.y - bottomOffsetY);
                break;
        }

        autoLerp = true;
    }

    #region Drag Process

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (autoLerp || eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        startMousePosition = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out startMousePosition);
        prevPosition = startContentPosition = content.anchoredPosition;
        velocity = 0f;
        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (autoLerp || eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        var localMouse = new Vector2();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localMouse))
        {
            var pointerDelta = localMouse - startMousePosition;
            var position = startContentPosition + pointerDelta;
            SetContentAnchoredPosition(position, align);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (autoLerp || eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        dragging = false;
    }
    #endregion

    public override void OnUpdate()
    {
        if (autoLerp && Vector2.Distance(content.anchoredPosition, targetOffset) > 0.1f)
        {
            var deltaTime = Time.unscaledDeltaTime;
            var newPosition = Vector2.SmoothDamp(content.anchoredPosition, targetOffset, ref refAutoLerpPosition, m_Elasticity, Mathf.Infinity, deltaTime);
            SetContentAnchoredPosition(newPosition, align);
        }
        else
        {
            autoLerp = false;
        }
    }

    public override void OnLateUpdate()
    {
        if (content == null)
        {
            return;
        }

        var deltaTime = Time.unscaledDeltaTime;
        var offset = CalculateOffset(align);
        var axisIndex = align == Align.Left || align == Align.Right ? 0 : 1;

        if (!dragging && (velocity != 0f || Vector2.SqrMagnitude(offset - Vector2.zero) > 0.1f))
        {
            var position = content.anchoredPosition;
            if (Vector2.SqrMagnitude(offset - Vector2.zero) > 0.1f)
            {
                var speed = velocity;
                var current = position[axisIndex];
                var target = position[axisIndex] - offset[axisIndex];
                position[axisIndex] = Mathf.SmoothDamp(current, target, ref speed, m_Elasticity, Mathf.Infinity, deltaTime);

                if (Mathf.Abs(speed) < 1)
                {
                    speed = 0f;
                }
                velocity = speed;
            }
            else
            {
                velocity *= Mathf.Pow(m_DecelerationRate, deltaTime);
                if (Mathf.Abs(velocity) < 50)
                {
                    velocity = 0;
                }
                position[axisIndex] += velocity * deltaTime;
            }

            SetContentAnchoredPosition(position, align);
        }

        if (dragging)
        {
            var newVelocity = (content.anchoredPosition[axisIndex] - prevPosition[axisIndex]) / deltaTime;
            velocity = Mathf.Lerp(velocity, newVelocity, deltaTime * 10);
            prevPosition = content.anchoredPosition;
        }
    }

    protected virtual void ProcessMoveNext()
    {
        ScrollItem lastRect = null;
        ScrollItem item = null;

        tempList.Clear();
        for (int i = 0; i < infiniteItems.Count; i++)
        {
            tempList.Add(infiniteItems[i]);
        }
        if (tempList.Count <= 0)
        {
            return;
        }
        lastRect = tempList[tempList.Count - 1];
        for (int i = 0; i < tempList.Count; i++)
        {
            item = tempList[i];
            var able = moveNextable && CanMoveNext(align, item);
            if (able)
            {
                infiniteItems.Remove(item);
                infiniteItems.Add(item);
                var offset = CalculateElementOffset(align);
                item.rectTransform.anchoredPosition = lastRect.rectTransform.anchoredPosition + offset;
                lastRect = item;

                hostIndex++;
                preIndex++;
                item.Dispose();
                item.Display(datas[hostIndex]);
            }
            else
            {
                break;
            }
        }
    }

    protected virtual void ProcessMoveLast()
    {
        ScrollItem firstRect = null;
        ScrollItem item = null;

        tempList.Clear();
        for (int i = 0; i < infiniteItems.Count; i++)
        {
            tempList.Add(infiniteItems[i]);
        }

        firstRect = tempList[0];
        for (int i = tempList.Count - 1; i >= 0; i--)
        {
            item = tempList[i];
            var able = moveLastable && CanMoveLast(align, item);
            if (able)
            {
                infiniteItems.Remove(item);
                infiniteItems.Insert(0, item);
                var offset = CalculateElementOffset(align);
                item.rectTransform.anchoredPosition = firstRect.rectTransform.anchoredPosition - offset;
                firstRect = item;
                hostIndex--;
                preIndex--;
                item.Dispose();
                item.Display(datas[preIndex]);
            }
            else
            {
                break;
            }
        }
    }

    private void SetContentAnchoredPosition(Vector2 _position, Align _align)
    {
        if (_position == content.anchoredPosition)
        {
            return;
        }

        if (!elasticityOn)
        {
            _position.y = Mathf.Clamp(_position.y, dataMinOffset.y, dataMaxOffset.y);
            _position.x = Mathf.Clamp(_position.x, dataMinOffset.x, dataMaxOffset.x);
        }

        var offset = _position - content.anchoredPosition;

        switch (_align)
        {
            case Align.Left:
            case Align.Right:
                content.anchoredPosition.SetX(_position.x);
                break;
            case Align.Top:
            case Align.Bottom:
                content.anchoredPosition.SetY(_position.y);
                break;
        }

        var moveNext = (_align == Align.Left && offset.x < 0f) || (_align == Align.Right && offset.x > 0f) ||
            (_align == Align.Top && offset.y > 0f) || (_align == Align.Bottom && offset.y < 0f);

        if (moveNext)
        {
            ProcessMoveNext();
        }
        else
        {
            ProcessMoveLast();
        }
    }

    private Vector2 CalculateOffset(Align _align)
    {
        var offset = Vector2.zero;
        switch (_align)
        {
            case Align.Left:
                if (content.anchoredPosition.x > dataMinOffset.x)
                {
                    offset = content.anchoredPosition - dataMinOffset;
                }
                else if (content.anchoredPosition.x < dataMaxOffset.x)
                {
                    offset = content.anchoredPosition - dataMaxOffset;
                }
                break;
            case Align.Right:
                if (content.anchoredPosition.x < dataMinOffset.x)
                {
                    offset = content.anchoredPosition - dataMinOffset;
                }
                else if (content.anchoredPosition.x > dataMaxOffset.x)
                {
                    offset = content.anchoredPosition - dataMaxOffset;
                }
                break;
            case Align.Bottom:
                if (content.anchoredPosition.y > dataMinOffset.y)
                {
                    offset = content.anchoredPosition - dataMinOffset;
                }
                else if (content.anchoredPosition.y < dataMaxOffset.y)
                {
                    offset = content.anchoredPosition - dataMaxOffset;
                }
                break;
            case Align.Top:
                if (content.anchoredPosition.y < dataMinOffset.y)
                {
                    offset = content.anchoredPosition - dataMinOffset;
                }
                else if (content.anchoredPosition.y > dataMaxOffset.y)
                {
                    offset = content.anchoredPosition - dataMaxOffset;
                }
                break;
        }

        return offset;
    }

    [ContextMenu("Arrange")]
    public virtual void ReArrange()
    {
        velocity = 0f;
        autoLerp = false;
        ElementsMatch();
        Arrange(align);
    }

    private void FillBatchData(int _startIndex)
    {
        int min = Mathf.Min(infiniteItems.Count, dataCount);
        preIndex = Mathf.Clamp(_startIndex, 0, dataCount - min);
        hostIndex = preIndex + min - 1;

        for (int i = 0; i < infiniteItems.Count; i++)
        {
            var item = infiniteItems[i];
            if (i <= hostIndex - preIndex)
            {
                item.gameObject.SetActive(true);
                item.Display(datas[preIndex + i]);
            }
            else
            {
                item.gameObject.SetActive(false);
                item.Dispose();
            }
        }
    }

    private void Arrange(Align _align)
    {
        var head = infiniteItems[0];

        var offset1 = Vector2.zero;
        switch (_align)
        {
            case Align.Left:
                offset1 = new Vector2(-content.rect.width * 0.5f + head.rectTransform.rect.width * 0.5f + m_BoundOffset.left, 0);
                break;
            case Align.Right:
                offset1 = new Vector2(content.rect.width * 0.5f - head.rectTransform.rect.width * 0.5f - m_BoundOffset.right, 0);
                break;
            case Align.Top:
                offset1 = new Vector2(0f, content.rect.height * 0.5f - head.rectTransform.rect.height * 0.5f - m_BoundOffset.top);
                break;
            case Align.Bottom:
                offset1 = new Vector2(0f, -content.rect.height * 0.5f + head.rectTransform.rect.height * 0.5f + m_BoundOffset.bottom);
                break;
        }

        head.rectTransform.anchoredPosition = content.anchoredPosition + offset1;

        var offset2 = CalculateElementOffset(_align);
        for (int i = 1; i < infiniteItems.Count; i++)
        {
            var item = infiniteItems[i];
            item.rectTransform.anchoredPosition = head.rectTransform.anchoredPosition + offset2 * i;
        }
    }

    private void ElementsMatch()
    {
        if (content == null)
        {
            DebugEx.Log("Content 不能为空！");
            return;
        }

        infiniteItems.Clear();
        for (int i = 0; i < content.childCount; i++)
        {
            var infiniteItem = content.GetChild(i).GetComponent<ScrollItem>();
            if (infiniteItem != null)
            {
                infiniteItems.Add(infiniteItem);
                infiniteItem.rectTransform.sizeDelta = cellSize;
                infiniteItem.rectTransform.anchorMax = Vector2.one * 0.5f;
                infiniteItem.rectTransform.anchorMin = Vector2.one * 0.5f;
                infiniteItem.rectTransform.pivot = Vector2.one * 0.5f;
            }
        }

        content.anchorMax = content.anchorMin = content.pivot = Vector2.one * 0.5f;
        content.sizeDelta = rectTransform.sizeDelta;
        content.anchoredPosition = Vector2.zero;
    }

    private Vector2 CalculateElementOffset(Align _align)
    {
        switch (_align)
        {
            case Align.Left:
                return new Vector2(cellSize.x + spacing.x, 0);
            case Align.Right:
                return new Vector2(-cellSize.x - spacing.x, 0);
            case Align.Top:
                return new Vector2(0, -cellSize.y - spacing.y);
            case Align.Bottom:
                return new Vector2(0, cellSize.y + spacing.y);
            default:
                return Vector2.zero;
        }
    }

    private bool CanMoveNext(Align _align, ScrollItem _item)
    {
        var itemMinPosition = _item.rectTransform.GetMinReferencePosition(rectTransform);
        var itemMaxPosition = _item.rectTransform.GetMaxReferencePosition(rectTransform);

        switch (_align)
        {
            case Align.Left:
                return itemMaxPosition.x < minOffset.x;
            case Align.Bottom:
                return itemMaxPosition.y < minOffset.y;
            case Align.Right:
                return itemMinPosition.x > maxOffset.x;
            case Align.Top:
                return itemMinPosition.y > maxOffset.y;
            default:
                return false;
        }
    }

    private bool CanMoveLast(Align _align, ScrollItem _item)
    {
        var itemMinPosition = _item.rectTransform.GetMinReferencePosition(rectTransform);
        var itemMaxPosition = _item.rectTransform.GetMaxReferencePosition(rectTransform);

        switch (_align)
        {
            case Align.Left:
                return itemMinPosition.x > maxOffset.x;
            case Align.Bottom:
                return itemMinPosition.y > maxOffset.y;
            case Align.Right:
                return itemMaxPosition.x < minOffset.x;
            case Align.Top:
                return itemMaxPosition.y < minOffset.y;
            default:
                return false;
        }
    }

    public enum Align
    {
        Left,
        Right,
        Top,
        Bottom,
    }

    [Serializable]
    public struct BoundOffset
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }

}




