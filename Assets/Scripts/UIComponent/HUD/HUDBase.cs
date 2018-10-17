using UnityEngine;
using System.Collections;

public class HUDBase : MonoBehaviour
{
    [SerializeField] FacingCamera m_FacingCamera;
    public FacingCamera facingCamera {
        get { return this.m_FacingCamera; }
    }

    [SerializeField] Transform m_Target;
    public Transform target {
        get { return this.m_Target; }
        set { this.m_Target = value; }
    }

    [SerializeField] Vector3 m_Offset;
    public Vector3 offset {
        get { return m_Offset; }
        set { m_Offset = value; }
    }

    [SerializeField] bool m_IsLocal = false;
    public bool isLocal {
        get { return m_IsLocal; }
    }

    Camera m_Camera;
    new public Camera camera {
        get { return this.m_Camera; }
        set {
            this.m_Camera = value;
            if (this.m_Camera != null && this.facingCamera != null)
            {
                this.facingCamera.camera = this.m_Camera;
            }
        }
    }

    Vector2 prePosition = new Vector2(-10000, 0);

    protected virtual void OnEnable()
    {
        SyncPosition(false);
    }

    protected virtual void LateUpdate()
    {
        SyncPosition(false);
    }

    public void SyncPosition(bool force)
    {
        if (target == null || camera == null)
        {
            return;
        }

        if (isLocal)
        {
            var uiposition = CameraUtil.ConvertPosition(camera, UIRoot.uiCamera, target.position + offset);
            if (force || Vector3.Distance(this.prePosition, uiposition) > 0.0001f)
            {
                prePosition = this.transform.position = uiposition;
                this.transform.localPosition = this.transform.localPosition.SetZ(0);
            }
        }
        else
        {
            if (force || Vector3.Distance(this.prePosition, target.position + offset) > 0.001f)
            {
                prePosition = this.transform.position = target.position + offset;
            }
        }
    }

}

