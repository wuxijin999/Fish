using UnityEngine;
using System.Collections;

public class FacingCamera : MonoBehaviour
{
    [SerializeField]
    Camera m_Camera;
    public new Camera camera {
        get { return m_Camera; }
        set { m_Camera = value; }
    }

    private void LateUpdate()
    {
        if (camera == null)
        {
            return;
        }

        this.transform.rotation = camera.transform.rotation;
    }
}
