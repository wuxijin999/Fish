using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class UIUtil
{
    public static GameObject CreateElement(string sourceName, string name)
    {
        var prefab = UIAssets.LoadPrefab(sourceName);
        if (prefab == null)
        {
            return null;
        }

        var instance = GameObject.Instantiate(prefab);
        instance.name = string.IsNullOrEmpty(name) ? sourceName : name;
        return instance;
    }

    public static T GetComponent<T>(this Transform transform, string path) where T : Component
    {
        if (transform == null || string.IsNullOrEmpty(path))
        {
            return null;
        }

        var child = transform.Find(path);
        if (child == null)
        {
            return null;
        }
        else
        {
            return child.GetComponent<T>();
        }
    }

    public static T GetComponent<T>(this Component component, string path) where T : Component
    {
        if (component == null || string.IsNullOrEmpty(path))
        {
            return null;
        }

        var child = component.transform.Find(path);
        if (child == null)
        {
            return null;
        }
        else
        {
            return child.GetComponent<T>();
        }
    }

    public static T GetComponent<T>(this GameObject gameObject, string path) where T : Component
    {
        if (gameObject == null || string.IsNullOrEmpty(path))
        {
            return null;
        }

        var child = gameObject.transform.Find(path);
        if (child == null)
        {
            return null;
        }
        else
        {
            return child.GetComponent<T>();
        }
    }

    public static Vector2 GetMaxWorldPosition(this RectTransform rectTransform)
    {
        Vector2 max;
        var offsetY = (1 - rectTransform.pivot.y) * rectTransform.rect.height;
        var offsetX = (1 - rectTransform.pivot.x) * rectTransform.rect.width;
        max = rectTransform.TransformPoint(offsetX, offsetY, 0);

        return max;
    }

    public static Vector2 GetMinWorldPosition(this RectTransform rectTransform)
    {
        Vector2 min;
        var offsetY = -rectTransform.pivot.y * rectTransform.rect.height;
        var offsetX = -rectTransform.pivot.x * rectTransform.rect.width;
        min = rectTransform.TransformPoint(offsetX, offsetY, 0);

        return min;
    }

    public static Vector2 GetMaxReferencePosition(this RectTransform rectTransform, Transform reference)
    {
        Vector2 max;
        var offsetY = (1 - rectTransform.pivot.y) * rectTransform.rect.height;
        var offsetX = (1 - rectTransform.pivot.x) * rectTransform.rect.width;
        max = rectTransform.TransformPoint(offsetX, offsetY, 0);
        max = reference.InverseTransformVector(max);
        return max;
    }

    public static Vector2 GetMinReferencePosition(this RectTransform rectTransform, Transform reference)
    {
        Vector2 min;
        var offsetY = -rectTransform.pivot.y * rectTransform.rect.height;
        var offsetX = -rectTransform.pivot.x * rectTransform.rect.width;
        min = rectTransform.TransformPoint(offsetX, offsetY, 0);
        min = reference.InverseTransformVector(min);

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

    public static UIVertex PackageUIVertex(Vector3 position, Vector2 uv0, Color color)
    {
        var vertex = new UIVertex();
        vertex.position = position;
        vertex.uv0 = uv0;
        vertex.color = color;

        return vertex;
    }

    public static UIVertex PackageUIVertexUV1(Vector3 position, Vector2 uv1, Color color)
    {
        var vertex = new UIVertex();
        vertex.position = position;
        vertex.uv1 = uv1;
        vertex.color = color;

        return vertex;
    }

    public static Vector3 ClampWorldPosition(RectTransform area, PointerEventData data)
    {
        if (area == null || data == null)
        {
            return Vector3.zero;
        }
        else
        {
            var worldMousePos = Vector3.zero;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(area, data.position, data.pressEventCamera, out worldMousePos))
            {
                return worldMousePos;
            }
            else
            {
                return data.pointerCurrentRaycast.worldPosition;
            }
        }
    }

    public static bool RectTransformContain(RectTransform area, RectTransform test)
    {
        if (area == null || test == null)
        {
            return false;
        }

        var worldcornersA = new Vector3[4];
        area.GetWorldCorners(worldcornersA);
        var worldcornersB = new Vector3[4];
        test.GetWorldCorners(worldcornersB);

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

        public Quadrangle(float minX, float minY, float maxX, float maxY)
        {
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
        }
    }


    public static Vector2 GetEdge(List<UIVertex> vertexs, int axis)
    {
        if (vertexs == null || vertexs.Count == 0)
        {
            return Vector2.zero;
        }

        if (axis != 0 && axis != 1)
        {
            return Vector2.zero;
        }

        var count = vertexs.Count;
        var min = vertexs[0].position[axis];
        var max = vertexs[0].position[axis];

        for (int i = 1; i < count; i++)
        {
            if (vertexs[i].position[axis] < min)
            {
                min = vertexs[i].position[axis];
            }
            else if (vertexs[i].position[axis] > max)
            {
                max = vertexs[i].position[axis];
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

    public static string GetUIElementRelativePath(UIRoot root, Transform transform)
    {
        List<Transform> parents = new List<Transform>() { transform };
        GetParents(transform, ref parents);

        if (parents.Contains(root.transform))
        {
            parents.Remove(root.transform);
        }

        var names = new string[parents.Count];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = parents[i].gameObject.name;
        }

        return string.Join("/", names);
    }

    public static void GetParents(Transform transform, ref List<Transform> parents)
    {
        if (transform == null || parents == null)
        {
            return;
        }

        if (transform.parent != null)
        {
            parents.Insert(0, transform.parent);

            GetParents(transform.parent, ref parents);
        }
    }

}
