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
    public RectTransform content { get { return this.m_Content; } }

    [SerializeField] protected BoundOffset m_BoundOffset;
    [SerializeField] protected Vector2 m_CellSize = new Vector2(100, 100);
    public Vector2 cellSize { get { return this.m_CellSize; } }

    [SerializeField] protected Vector2 m_Spacing;
    public Vector2 spacing { get { return this.m_Spacing; } }

    [SerializeField] Align m_Align = Align.Top;
    public Align align { get { return this.m_Align; } }

    [SerializeField] bool m_ElasticityOn = true;
    public bool elasticityOn { get { return this.m_ElasticityOn; } }

    [SerializeField] private float m_Elasticity = 0.1f;
    public float elasticity { get { return this.m_Elasticity; } }

    [SerializeField] private float m_DecelerationRate = 0.135f;
    public float decelerationRate { get { return this.m_DecelerationRate; } }

    public float normalizedPosition
    {
        get
        {
            if (this.content == null)
            {
                return 0f;
            }

            var offset = (this.dataMaxOffset - this.dataMinOffset);
            var axisIndex = this.align == Align.Left || this.align == Align.Right ? 0 : 1;
            return Mathf.Clamp01(Mathf.Abs((this.content.anchoredPosition[axisIndex] - this.dataMinOffset[axisIndex]) / offset[axisIndex]));
        }
        set
        {
            if (this.content == null)
            {
                return;
            }

            value = Mathf.Clamp01(value);
            var normalize = 0f;
            var offset = (this.dataMaxOffset - this.dataMinOffset);
            switch (this.align)
            {
                case Align.Top:
                case Align.Bottom:
                    normalize = (this.content.anchoredPosition.y - this.dataMinOffset.y) / offset.y;
                    if (Mathf.Abs(normalize - value) > 0.0001f)
                    {
                        this.targetOffset = this.dataMinOffset + new Vector2(0, this.dataMaxOffset.y * value);
                        this.velocity = 0f;
                        this.autoLerp = true;
                        this.refAutoLerpPosition = Vector2.zero;
                    }
                    break;
                case Align.Left:
                case Align.Right:
                    normalize = (this.content.anchoredPosition.x - this.dataMinOffset.x) / offset.x;
                    if (Mathf.Abs(normalize - value) > 0.0001f)
                    {
                        this.targetOffset = this.dataMinOffset + new Vector2(this.dataMaxOffset.x * value, 0);
                        this.velocity = 0f;
                        this.autoLerp = true;
                        this.refAutoLerpPosition = Vector2.zero;
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
    protected List<ScrollBehaviour> infiniteItems = new List<ScrollBehaviour>();
    List<ScrollBehaviour> tempList = new List<ScrollBehaviour>();

    Vector2 startMousePosition = Vector2.zero;
    Vector2 startContentPosition = Vector2.zero;
    Vector2 prevPosition = Vector2.zero;

    protected float velocity = 0f;
    protected bool dragging = false;

    public int dataCount { get { return this.datas == null ? 0 : this.datas.Count; } }
    bool moveNextable { get { return this.hostIndex < this.dataCount - 1; } }
    bool moveLastable { get { return this.preIndex > 0; } }
    protected int preIndex { get; set; }
    protected int hostIndex { get; set; }

    Vector2 maxOffset { get { return this.rectTransform.GetMaxReferencePosition(this.rectTransform); } }
    Vector2 minOffset { get { return this.rectTransform.GetMinReferencePosition(this.rectTransform); } }

    public virtual void Init<T>(List<T> _datas, bool _stepByStep = false)
    {
        if (_datas == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        this.datas = _datas;

        ReArrange();
        FillBatchData(0);

        if (_stepByStep)
        {
            for (int i = 0; i < this.infiniteItems.Count; i++)
            {
                var infiniteItem = this.infiniteItems[i];
                if (infiniteItem != null)
                {
                    infiniteItem.gameObject.SetActive(false);
                }
            }
        }

        this.gameObject.SetActive(true);

        this.dataMinOffset = this.content.anchoredPosition;
        var totalOffset = this.dataCount == 0 ? Vector2.zero : this.cellSize * this.dataCount + this.spacing * (this.dataCount - 1)
            + new Vector2(this.m_BoundOffset.left + this.m_BoundOffset.right, this.m_BoundOffset.top + this.m_BoundOffset.bottom);

        var longer = Mathf.Abs(totalOffset.x) > Mathf.Abs(this.rectTransform.rect.width);
        var higher = Mathf.Abs(totalOffset.y) > Mathf.Abs(this.rectTransform.rect.height);
        switch (this.align)
        {
            case Align.Left:
                this.dataMaxOffset = longer ? this.dataMinOffset - new Vector2(totalOffset.x - this.rectTransform.rect.width, 0) : this.dataMinOffset;
                break;
            case Align.Right:
                this.dataMaxOffset = longer ? this.dataMinOffset + new Vector2(totalOffset.x - this.rectTransform.rect.width, 0) : this.dataMinOffset;
                break;
            case Align.Top:
                this.dataMaxOffset = higher ? this.dataMinOffset + new Vector2(0, totalOffset.y - this.rectTransform.rect.height) : this.dataMinOffset;
                break;
            case Align.Bottom:
                this.dataMaxOffset = higher ? this.dataMinOffset - new Vector2(0, totalOffset.y - this.rectTransform.rect.height) : this.dataMinOffset;
                break;
        }

        if (_stepByStep)
        {
            StartCoroutine("Co_StepByStepAppear");
        }
    }

    public void Dispose()
    {
        this.velocity = 0f;
        StopAllCoroutines();
        for (int i = 0; i < this.infiniteItems.Count; i++)
        {
            var infiniteItem = this.infiniteItems[i];
            if (infiniteItem != null)
            {
                infiniteItem.Dispose();
            }
        }
    }

    public void HideAll()
    {
        StopAllCoroutines();
        for (int i = 0; i < this.content.childCount; i++)
        {
            this.content.GetChild(i).gameObject.SetActive(false);
        }
    }

    IEnumerator Co_StepByStepAppear()
    {
        for (int i = 0; i < this.infiniteItems.Count; i++)
        {
            var infiniteItem = this.infiniteItems[i];
            if (infiniteItem != null && i < this.datas.Count)
            {
                infiniteItem.gameObject.SetActive(true);
                yield return WaitingForConst.millisecond100;
            }
        }
    }

    public void MoveToCenter(int _dataIndex)
    {
        if (_dataIndex < 0 || _dataIndex >= this.dataCount)
        {
            return;
        }

        var fillSpace = (_dataIndex + 0.5f) * this.cellSize + _dataIndex * this.spacing;
        switch (this.align)
        {
            case Align.Left:
                var leftOffsetX = Mathf.Clamp(this.m_BoundOffset.left + fillSpace[0] - this.rectTransform.rect.width * 0.5f, 0, Mathf.Abs(this.dataMaxOffset.x));
                this.targetOffset = new Vector2(this.dataMinOffset.x - leftOffsetX, 0);
                break;
            case Align.Right:
                var rightOffsetX = Mathf.Clamp(this.m_BoundOffset.right + fillSpace[0] - this.rectTransform.rect.width * 0.5f, 0, Mathf.Abs(this.dataMaxOffset.x));
                this.targetOffset = new Vector2(this.dataMinOffset.x + rightOffsetX, 0);
                break;
            case Align.Top:
                var topOffsetY = Mathf.Clamp(this.m_BoundOffset.top + fillSpace[1] - this.rectTransform.rect.height * 0.5f, 0, Mathf.Abs(this.dataMaxOffset.y));
                this.targetOffset = new Vector2(0, this.dataMinOffset.y + topOffsetY);
                break;
            case Align.Bottom:
                var bottomOffsetY = Mathf.Clamp(this.m_BoundOffset.bottom + fillSpace[1] - this.rectTransform.rect.height * 0.5f, 0, Mathf.Abs(this.dataMaxOffset.y));
                this.targetOffset = new Vector2(0, this.dataMinOffset.y - bottomOffsetY);
                break;
        }

        this.autoLerp = true;
    }

    #region Drag Process

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.autoLerp || eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        this.startMousePosition = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, eventData.position, eventData.pressEventCamera, out this.startMousePosition);
        this.prevPosition = this.startContentPosition = this.content.anchoredPosition;
        this.velocity = 0f;
        this.dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.autoLerp || eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        var localMouse = new Vector2();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, eventData.position, eventData.pressEventCamera, out localMouse))
        {
            var pointerDelta = localMouse - this.startMousePosition;
            var position = this.startContentPosition + pointerDelta;
            SetContentAnchoredPosition(position, this.align);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.autoLerp || eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        this.dragging = false;
    }
    #endregion

    public override void OnUpdate()
    {
        if (this.autoLerp && Vector2.Distance(this.content.anchoredPosition, this.targetOffset) > 0.1f)
        {
            var deltaTime = Time.unscaledDeltaTime;
            var newPosition = Vector2.SmoothDamp(this.content.anchoredPosition, this.targetOffset, ref this.refAutoLerpPosition, this.m_Elasticity, Mathf.Infinity, deltaTime);
            SetContentAnchoredPosition(newPosition, this.align);
        }
        else
        {
            this.autoLerp = false;
        }
    }

    public override void OnLateUpdate()
    {
        if (this.content == null)
        {
            return;
        }

        var deltaTime = Time.unscaledDeltaTime;
        var offset = CalculateOffset(this.align);
        var axisIndex = this.align == Align.Left || this.align == Align.Right ? 0 : 1;

        if (!this.dragging && (this.velocity != 0f || Vector2.SqrMagnitude(offset - Vector2.zero) > 0.1f))
        {
            var position = this.content.anchoredPosition;
            if (Vector2.SqrMagnitude(offset - Vector2.zero) > 0.1f)
            {
                var speed = this.velocity;
                var current = position[axisIndex];
                var target = position[axisIndex] - offset[axisIndex];
                position[axisIndex] = Mathf.SmoothDamp(current, target, ref speed, this.m_Elasticity, Mathf.Infinity, deltaTime);

                if (Mathf.Abs(speed) < 1)
                {
                    speed = 0f;
                }
                this.velocity = speed;
            }
            else
            {
                this.velocity *= Mathf.Pow(this.m_DecelerationRate, deltaTime);
                if (Mathf.Abs(this.velocity) < 50)
                {
                    this.velocity = 0;
                }
                position[axisIndex] += this.velocity * deltaTime;
            }

            SetContentAnchoredPosition(position, this.align);
        }

        if (this.dragging)
        {
            var newVelocity = (this.content.anchoredPosition[axisIndex] - this.prevPosition[axisIndex]) / deltaTime;
            this.velocity = Mathf.Lerp(this.velocity, newVelocity, deltaTime * 10);
            this.prevPosition = this.content.anchoredPosition;
        }
    }

    protected virtual void ProcessMoveNext()
    {
        ScrollBehaviour lastRect = null;
        ScrollBehaviour item = null;

        this.tempList.Clear();
        for (int i = 0; i < this.infiniteItems.Count; i++)
        {
            this.tempList.Add(this.infiniteItems[i]);
        }
        if (this.tempList.Count <= 0)
        {
            return;
        }
        lastRect = this.tempList[this.tempList.Count - 1];
        for (int i = 0; i < this.tempList.Count; i++)
        {
            item = this.tempList[i];
            var able = this.moveNextable && CanMoveNext(this.align, item);
            if (able)
            {
                this.infiniteItems.Remove(item);
                this.infiniteItems.Add(item);
                var offset = CalculateElementOffset(this.align);
                item.rectTransform.anchoredPosition = lastRect.rectTransform.anchoredPosition + offset;
                lastRect = item;

                this.hostIndex++;
                this.preIndex++;
                item.Dispose();
                item.Display(this.datas[this.hostIndex]);
            }
            else
            {
                break;
            }
        }
    }

    protected virtual void ProcessMoveLast()
    {
        ScrollBehaviour firstRect = null;
        ScrollBehaviour item = null;

        this.tempList.Clear();
        for (int i = 0; i < this.infiniteItems.Count; i++)
        {
            this.tempList.Add(this.infiniteItems[i]);
        }

        firstRect = this.tempList[0];
        for (int i = this.tempList.Count - 1; i >= 0; i--)
        {
            item = this.tempList[i];
            var able = this.moveLastable && CanMoveLast(this.align, item);
            if (able)
            {
                this.infiniteItems.Remove(item);
                this.infiniteItems.Insert(0, item);
                var offset = CalculateElementOffset(this.align);
                item.rectTransform.anchoredPosition = firstRect.rectTransform.anchoredPosition - offset;
                firstRect = item;
                this.hostIndex--;
                this.preIndex--;
                item.Dispose();
                item.Display(this.datas[this.preIndex]);
            }
            else
            {
                break;
            }
        }
    }

    private void SetContentAnchoredPosition(Vector2 _position, Align _align)
    {
        if (_position == this.content.anchoredPosition)
        {
            return;
        }

        if (!this.elasticityOn)
        {
            _position.y = Mathf.Clamp(_position.y, this.dataMinOffset.y, this.dataMaxOffset.y);
            _position.x = Mathf.Clamp(_position.x, this.dataMinOffset.x, this.dataMaxOffset.x);
        }

        var offset = _position - this.content.anchoredPosition;

        switch (_align)
        {
            case Align.Left:
            case Align.Right:
                this.content.anchoredPosition.SetX(_position.x);
                break;
            case Align.Top:
            case Align.Bottom:
                this.content.anchoredPosition.SetY(_position.y);
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
                if (this.content.anchoredPosition.x > this.dataMinOffset.x)
                {
                    offset = this.content.anchoredPosition - this.dataMinOffset;
                }
                else if (this.content.anchoredPosition.x < this.dataMaxOffset.x)
                {
                    offset = this.content.anchoredPosition - this.dataMaxOffset;
                }
                break;
            case Align.Right:
                if (this.content.anchoredPosition.x < this.dataMinOffset.x)
                {
                    offset = this.content.anchoredPosition - this.dataMinOffset;
                }
                else if (this.content.anchoredPosition.x > this.dataMaxOffset.x)
                {
                    offset = this.content.anchoredPosition - this.dataMaxOffset;
                }
                break;
            case Align.Bottom:
                if (this.content.anchoredPosition.y > this.dataMinOffset.y)
                {
                    offset = this.content.anchoredPosition - this.dataMinOffset;
                }
                else if (this.content.anchoredPosition.y < this.dataMaxOffset.y)
                {
                    offset = this.content.anchoredPosition - this.dataMaxOffset;
                }
                break;
            case Align.Top:
                if (this.content.anchoredPosition.y < this.dataMinOffset.y)
                {
                    offset = this.content.anchoredPosition - this.dataMinOffset;
                }
                else if (this.content.anchoredPosition.y > this.dataMaxOffset.y)
                {
                    offset = this.content.anchoredPosition - this.dataMaxOffset;
                }
                break;
        }

        return offset;
    }

    [ContextMenu("Arrange")]
    public virtual void ReArrange()
    {
        this.velocity = 0f;
        this.autoLerp = false;
        ElementsMatch();
        Arrange(this.align);
    }

    private void FillBatchData(int _startIndex)
    {
        int min = Mathf.Min(this.infiniteItems.Count, this.dataCount);
        this.preIndex = Mathf.Clamp(_startIndex, 0, this.dataCount - min);
        this.hostIndex = this.preIndex + min - 1;

        for (int i = 0; i < this.infiniteItems.Count; i++)
        {
            var item = this.infiniteItems[i];
            if (i <= this.hostIndex - this.preIndex)
            {
                item.gameObject.SetActive(true);
                item.Display(this.datas[this.preIndex + i]);
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
        var head = this.infiniteItems[0];

        var offset1 = Vector2.zero;
        switch (_align)
        {
            case Align.Left:
                offset1 = new Vector2(-this.content.rect.width * 0.5f + head.rectTransform.rect.width * 0.5f + this.m_BoundOffset.left, 0);
                break;
            case Align.Right:
                offset1 = new Vector2(this.content.rect.width * 0.5f - head.rectTransform.rect.width * 0.5f - this.m_BoundOffset.right, 0);
                break;
            case Align.Top:
                offset1 = new Vector2(0f, this.content.rect.height * 0.5f - head.rectTransform.rect.height * 0.5f - this.m_BoundOffset.top);
                break;
            case Align.Bottom:
                offset1 = new Vector2(0f, -this.content.rect.height * 0.5f + head.rectTransform.rect.height * 0.5f + this.m_BoundOffset.bottom);
                break;
        }

        head.rectTransform.anchoredPosition = this.content.anchoredPosition + offset1;

        var offset2 = CalculateElementOffset(_align);
        for (int i = 1; i < this.infiniteItems.Count; i++)
        {
            var item = this.infiniteItems[i];
            item.rectTransform.anchoredPosition = head.rectTransform.anchoredPosition + offset2 * i;
        }
    }

    private void ElementsMatch()
    {
        if (this.content == null)
        {
            DebugEx.Log("Content 不能为空！");
            return;
        }

        this.infiniteItems.Clear();
        for (int i = 0; i < this.content.childCount; i++)
        {
            var infiniteItem = this.content.GetChild(i).GetComponent<ScrollBehaviour>();
            if (infiniteItem != null)
            {
                this.infiniteItems.Add(infiniteItem);
                infiniteItem.rectTransform.sizeDelta = this.cellSize;
                infiniteItem.rectTransform.anchorMax = Vector2.one * 0.5f;
                infiniteItem.rectTransform.anchorMin = Vector2.one * 0.5f;
                infiniteItem.rectTransform.pivot = Vector2.one * 0.5f;
            }
        }

        this.content.anchorMax = this.content.anchorMin = this.content.pivot = Vector2.one * 0.5f;
        this.content.sizeDelta = this.rectTransform.sizeDelta;
        this.content.anchoredPosition = Vector2.zero;
    }

    private Vector2 CalculateElementOffset(Align _align)
    {
        switch (_align)
        {
            case Align.Left:
                return new Vector2(this.cellSize.x + this.spacing.x, 0);
            case Align.Right:
                return new Vector2(-this.cellSize.x - this.spacing.x, 0);
            case Align.Top:
                return new Vector2(0, -this.cellSize.y - this.spacing.y);
            case Align.Bottom:
                return new Vector2(0, this.cellSize.y + this.spacing.y);
            default:
                return Vector2.zero;
        }
    }

    private bool CanMoveNext(Align _align, ScrollBehaviour _item)
    {
        var itemMinPosition = _item.rectTransform.GetMinReferencePosition(this.rectTransform);
        var itemMaxPosition = _item.rectTransform.GetMaxReferencePosition(this.rectTransform);

        switch (_align)
        {
            case Align.Left:
                return itemMaxPosition.x < this.minOffset.x;
            case Align.Bottom:
                return itemMaxPosition.y < this.minOffset.y;
            case Align.Right:
                return itemMinPosition.x > this.maxOffset.x;
            case Align.Top:
                return itemMinPosition.y > this.maxOffset.y;
            default:
                return false;
        }
    }

    private bool CanMoveLast(Align _align, ScrollBehaviour _item)
    {
        var itemMinPosition = _item.rectTransform.GetMinReferencePosition(this.rectTransform);
        var itemMaxPosition = _item.rectTransform.GetMaxReferencePosition(this.rectTransform);

        switch (_align)
        {
            case Align.Left:
                return itemMinPosition.x > this.maxOffset.x;
            case Align.Bottom:
                return itemMinPosition.y > this.maxOffset.y;
            case Align.Right:
                return itemMaxPosition.x < this.minOffset.x;
            case Align.Top:
                return itemMaxPosition.y < this.minOffset.y;
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




