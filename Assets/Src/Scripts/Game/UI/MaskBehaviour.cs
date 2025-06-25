using UnityEngine;
using YsoCorp;

public class MaskBehaviour : YCBehaviour {
    [SerializeField] Transform m_maskTarget;
    [SerializeField] Vector3 m_startAnimScale = Vector3.one * 3f;
    [SerializeField] Vector3 m_endAnimScale = new Vector3(.5f, .23f, 0f);
    [SerializeField] float m_animSpeed = 1f;

    //Lock Player Comtroller
    private void Start() {
        this.transform.localScale = this.ScalerelativeToOrientation(this.m_startAnimScale);
        this.player.m_controller.CanUse = false;
    }

    private void Update() {
        //Unlock Player controller
        if (Maths.RoughlyEqual(this.transform.localScale.x, this.ScalerelativeToOrientation(this.m_endAnimScale).x, 0.01f)) { this.player.m_controller.CanUse = true; }
        //Set endScale relative to Screen Orientation
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, this.ScalerelativeToOrientation(this.m_endAnimScale), Time.unscaledDeltaTime * this.m_animSpeed);
        //Set the mask position "around" the target pos relative to screen view
        this.transform.position = this.cam.m_ycCamera.WorldToScreenPoint(this.m_maskTarget.position);
    }

    Vector3 ScalerelativeToOrientation(Vector3 _targetScale) {
        return Screen.width > Screen.height ? new Vector3(_targetScale.y, _targetScale.x, _targetScale.z) * Screen.height / Screen.width : _targetScale;
    }
}
