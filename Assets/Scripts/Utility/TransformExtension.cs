using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TransformExtension
{

    public static void SetParentEx(this Transform transform, Transform parent, Vector3 localPosition, Quaternion rotation, Vector3 scale)
    {

        if (transform != null && parent != null)
        {
            transform.SetParent(parent);
            transform.localPosition = localPosition;
            transform.localRotation = rotation;
            transform.localScale = scale;
        }
    }

    public static void SetParentEx(this Transform transform, Transform parent, Vector3 localPosition, Vector3 eulerAngles, Vector3 scale)
    {
        if (transform != null && parent != null)
        {
            transform.SetParent(parent);
            transform.localPosition = localPosition;
            transform.localEulerAngles = eulerAngles;
            transform.localScale = scale;
        }
    }


    public static T[] GetComponentsInChildren<T>(this Transform transform, bool includeInactive, bool includeSelf) where T : Component
    {

        if (includeSelf)
        {
            return transform.GetComponentsInChildren<T>(includeInactive);
        }
        else
        {
            int childCount = transform.childCount;
            List<T> list = new List<T>();
            T t = null;
            for (int i = 0; i < childCount; i++)
            {
                t = transform.GetComponent<T>();
                if (t != null)
                {
                    list.Add(t);
                }
            }
            return list.ToArray();
        }

    }

    public static Transform GetChildTransformDeeply(this Transform transform, string childName, bool includeSelf = false)
    {

        if (includeSelf)
        {
            if (transform.name.Equals(childName))
            {
                return transform;
            }
        }

        int count = transform.childCount;

        Transform tempChild = null;

        for (int i = 0; i < count; ++i)
        {

            tempChild = transform.GetChild(i);

            if (tempChild.name.Equals(childName))
            {
                return tempChild;
            }

            tempChild = transform.GetChild(i).GetChildTransformDeeply(childName, false);
            if (tempChild)
            {
                return tempChild;
            }
        }

        return null;
    }

    /// <summary>
    /// 以锚四个角的方式进行匹配
    /// 并且将对象设置为父对象
    /// </summary>
    /// <param name="child"></param>
    /// <param name="parent"></param>
    public static void MatchWhith(this RectTransform child, RectTransform parent)
    {

        if (child.parent != parent)
        {
            child.SetParent(parent);
        }
        child.anchoredPosition3D = Vector3.zero;
        child.sizeDelta = Vector2.zero;
        child.anchorMin = Vector2.zero;
        child.anchorMax = Vector2.one;
        child.pivot = Vector2.one * 0.5f;
        child.localRotation = Quaternion.identity;
        child.localScale = Vector3.one;
    }


    public static bool ContainWorldPosition(this RectTransform _rectTransform, Vector3 _worldPosition)
    {
        var worldCorners = new Vector3[4];
        _rectTransform.GetWorldCorners(worldCorners);
        if (_worldPosition.x >= worldCorners[0].x && _worldPosition.x <= worldCorners[2].x
            && _worldPosition.y >= worldCorners[0].y && _worldPosition.y <= worldCorners[2].y)
        {
            return true;
        }

        return false;
    }

    public static bool RectTransformContain(this RectTransform _rectTransform, RectTransform _target)
    {
        var targetWorldCorners = new Vector3[4];
        _target.GetWorldCorners(targetWorldCorners);

        for (int i = 0; i < targetWorldCorners.Length; i++)
        {
            var position = targetWorldCorners[i];
            if (_rectTransform.ContainWorldPosition(position))
            {
                return true;
            }
        }

        return false;
    }

    public static Transform GetRoot(this Transform transform)
    {
        return transform && transform.parent ? GetRoot(transform.parent) : transform;
    }

}
