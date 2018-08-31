using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class UIUtil
{
    public static GameObject CreateWidget(string _sourceName, string _name)
    {
        var prefab = UIAssets.LoadPrefab(_sourceName);
        if (prefab == null)
        {
            return null;
        }

        var instance = GameObject.Instantiate(prefab);
        instance.name = string.IsNullOrEmpty(_name) ? _sourceName : _name;
        return instance;
    }

    public static T GetComponent<T>(this Transform transform, string path) where T : Component
    {
        if (transform == null || string.IsNullOrEmpty(path))
        {
            return null;
        }

        var _child = transform.Find(path);
        if (_child == null)
        {
            return null;
        }
        else
        {
            return _child.GetComponent<T>();
        }
    }

    public static T GetComponent<T>(this Component component, string path) where T : Component
    {
        if (component == null || string.IsNullOrEmpty(path))
        {
            return null;
        }

        var _child = component.transform.Find(path);
        if (_child == null)
        {
            return null;
        }
        else
        {
            return _child.GetComponent<T>();
        }
    }

    public static T GetComponent<T>(this GameObject gameObject, string path) where T : Component
    {
        if (gameObject == null || string.IsNullOrEmpty(path))
        {
            return null;
        }

        var _child = gameObject.transform.Find(path);
        if (_child == null)
        {
            return null;
        }
        else
        {
            return _child.GetComponent<T>();
        }
    }

    public static Vector2 GetMaxWorldPosition(this RectTransform _rectTransform)
    {
        Vector2 max;
        var offsetY = (1 - _rectTransform.pivot.y) * _rectTransform.rect.height;
        var offsetX = (1 - _rectTransform.pivot.x) * _rectTransform.rect.width;
        max = _rectTransform.TransformPoint(offsetX, offsetY, 0);

        return max;
    }

    public static Vector2 GetMinWorldPosition(this RectTransform _rectTransform)
    {
        Vector2 min;
        var offsetY = -_rectTransform.pivot.y * _rectTransform.rect.height;
        var offsetX = -_rectTransform.pivot.x * _rectTransform.rect.width;
        min = _rectTransform.TransformPoint(offsetX, offsetY, 0);

        return min;
    }

    public static Vector2 GetMaxReferencePosition(this RectTransform _rectTransform, Transform _reference)
    {
        Vector2 max;
        var offsetY = (1 - _rectTransform.pivot.y) * _rectTransform.rect.height;
        var offsetX = (1 - _rectTransform.pivot.x) * _rectTransform.rect.width;
        max = _rectTransform.TransformPoint(offsetX, offsetY, 0);
        max = _reference.InverseTransformVector(max);
        return max;
    }

    public static Vector2 GetMinReferencePosition(this RectTransform _rectTransform, Transform _reference)
    {
        Vector2 min;
        var offsetY = -_rectTransform.pivot.y * _rectTransform.rect.height;
        var offsetX = -_rectTransform.pivot.x * _rectTransform.rect.width;
        min = _rectTransform.TransformPoint(offsetX, offsetY, 0);
        min = _reference.InverseTransformVector(min);

        return min;
    }

    public static int RayCrossingCount(Vector2 p, List<Vector3> vertices)
    {
        var crossNum = 0;
        for (int i = 0, count = vertices.Count; i < count; i++)
        {
            var v1 = vertices[i];
            var v2 = vertices[(i + 1) % count];

            if (((v1.y <= p.y) && (v2.y > p.y))
                || ((v1.y > p.y) && (v2.y <= p.y)))
            {
                if (p.x < v1.x + (p.y - v1.y) / (v2.y - v1.y) * (v2.x - v1.x))
                {
                    crossNum += 1;
                }
            }
        }

        return crossNum;
    }

    public static UIVertex PackageUIVertex(Vector3 _position, Vector2 _uv0, Color _color)
    {
        var vertex = new UIVertex();
        vertex.position = _position;
        vertex.uv0 = _uv0;
        vertex.color = _color;

        return vertex;
    }

    public static UIVertex PackageUIVertexUV1(Vector3 _position, Vector2 _uv1, Color _color)
    {
        var vertex = new UIVertex();
        vertex.position = _position;
        vertex.uv1 = _uv1;
        vertex.color = _color;

        return vertex;
    }

    public static Vector3 ClampWorldPosition(RectTransform _area, PointerEventData _data)
    {
        if (_area == null || _data == null)
        {
            return Vector3.zero;
        }
        else
        {
            var worldMousePos = Vector3.zero;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_area, _data.position, _data.pressEventCamera, out worldMousePos))
            {
                return worldMousePos;
            }
            else
            {
                return _data.pointerCurrentRaycast.worldPosition;
            }
        }
    }

    public static bool RectTransformContain(RectTransform _area, RectTransform _test)
    {
        if (_area == null || _test == null)
        {
            return false;
        }

        var worldcornersA = new Vector3[4];
        _area.GetWorldCorners(worldcornersA);
        var worldcornersB = new Vector3[4];
        _test.GetWorldCorners(worldcornersB);

        var a = new Quadrangle(worldcornersA[0].x, worldcornersA[0].y, worldcornersA[2].x, worldcornersA[2].y);
        var b = new Quadrangle(worldcornersB[0].x, worldcornersB[0].y, worldcornersB[2].x, worldcornersB[2].y);

        if (a.minX > b.maxX || a.maxX < b.minX || a.minY > b.maxY || a.maxY < b.minY)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    struct Quadrangle
    {
        public float minX;
        public float minY;
        public float maxX;
        public float maxY;

        public Quadrangle(float _minX, float _minY, float _maxX, float _maxY)
        {
            this.minX = _minX;
            this.minY = _minY;
            this.maxX = _maxX;
            this.maxY = _maxY;
        }
    }


    public static Vector2 GetEdge(List<UIVertex> _vertexs, int _axis)
    {
        if (_vertexs == null || _vertexs.Count == 0)
        {
            return Vector2.zero;
        }

        if (_axis != 0 && _axis != 1)
        {
            return Vector2.zero;
        }

        var count = _vertexs.Count;
        var min = _vertexs[0].position[_axis];
        var max = _vertexs[0].position[_axis];

        for (int i = 1; i < count; i++)
        {
            if (_vertexs[i].position[_axis] < min)
            {
                min = _vertexs[i].position[_axis];
            }
            else if (_vertexs[i].position[_axis] > max)
            {
                max = _vertexs[i].position[_axis];
            }
        }

        return new Vector2(min, max);
    }

    public static void AddQuad(VertexHelper vertexHelper, Vector3[] quadPositions, Color32 color, Vector2[] quadUVs)
    {
        if (quadPositions == null || quadPositions.Length < 4)
        {
            return;
        }

        if (quadUVs == null || quadUVs.Length < 4)
        {
            return;
        }

        var currentVertCount = vertexHelper.currentVertCount;
        for (var i = 0; i < 4; i++)
        {
            vertexHelper.AddVert(quadPositions[i], color, quadUVs[i]);
        }
        vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
        vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
    }

    public static void AddTriangle(VertexHelper vertexHelper, Vector3[] positions, Color32 color, Vector2[] uvs)
    {
        if (positions == null || positions.Length < 3)
        {
            return;
        }

        if (uvs == null || uvs.Length < 3)
        {
            return;
        }

        var currentVertCount = vertexHelper.currentVertCount;
        for (var i = 0; i < 3; i++)
        {
            vertexHelper.AddVert(positions[i], color, uvs[i]);
        }
        vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
    }

    public static string GetUIElementRelativePath(UIRoot _root, Transform _transform)
    {
        List<Transform> parents = new List<Transform>() { _transform };
        GetParents(_transform, ref parents);

        if (parents.Contains(_root.transform))
        {
            parents.Remove(_root.transform);
        }

        var names = new string[parents.Count];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = parents[i].gameObject.name;
        }

        return string.Join("/", names);
    }

    public static void GetParents(Transform _transform, ref List<Transform> _parents)
    {
        if (_transform == null || _parents == null)
        {
            return;
        }

        if (_transform.parent != null)
        {
            _parents.Insert(0, _transform.parent);

            GetParents(_transform.parent, ref _parents);
        }
    }

}
