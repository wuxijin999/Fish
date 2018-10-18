using UnityEngine;
using System.Collections;

public class HUDBase : MonoBehaviour
{
    [SerializeField] FacingCamera m_FacingCamera;
    [SerializeField] Transform m_Target;
    [SerializeField] Vector3 m_Offset;
    [SerializeField] bool m_IsLocal = false;

    Camera m_Camera;
    new public Camera camera {
        get { return this.m_Camera; }
        set {
            this.m_Camera = value;
            if (this.m_Camera != null && this.m_FacingCamera != null)
            {
                this.m_FacingCamera.camera = this.m_Camera;
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

    public virtual void Follow(Transform target, Vector3 offset, Camera camera)
    {
        SyncPosition(true);
        this.m_Target = target;
        this.m_Offset = offset;
        this.camera = camera;
    }

    public virtual void Dispose()
    {
        this.m_Target = null;
        this.camera = null;
        if (this.m_FacingCamera != null)
        {
            this.m_FacingCamera.camera = null;
        }
    }

    void SyncPosition(bool force)
    {
        if (this.m_Target == null || this.camera == null)
        {
            return;
        }

        if (this.m_IsLocal)
        {
            var uiposition = CameraUtil.ConvertPosition(this.camera, UIRoot.uiCamera, this.m_Target.position + this.m_Offset);
            if (force || Vector3.Distance(this.prePosition, uiposition) > 0.0001f)
            {
                this.prePosition = this.transform.position = uiposition;
                this.transform.localPosition = this.transform.localPosition.SetZ(0);
            }
        }
        else
        {
            if (force || Vector3.Distance(this.prePosition, this.m_Target.position + this.m_Offset) > 0.001f)
            {
                this.prePosition = this.transform.position = this.m_Target.position + this.m_Offset;
            }
        }
    }

}

