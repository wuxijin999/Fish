//--------------------------------------------------------
//    [Author]:           Fish
//    [  Date ]:           Thursday, October 18, 2018
//--------------------------------------------------------
using UnityEngine;
using System.Collections;

public class FollowIgnoreY : UIBase
{
    Transform m_Target;
    Camera m_Camera;
    float yRecord = 0f;

    public void Follow(Transform _target, Camera _camera)
    {
        m_Target = _target;
        m_Camera = _camera;

        if (m_Target != null)
        {
            yRecord = m_Target.position.y;
        }
    }

    public void OnLateUpdate()
    {
        if (m_Target != null && m_Camera != null)
        {
            var uiPosition = CameraUtil.ConvertPosition(m_Camera, UIRoot.uiCamera, m_Target.position.SetY(yRecord));
            this.transform.position = uiPosition.SetZ(UIRoot.hudRoot.transform.position.z);
        }
    }

}



