using UnityEngine;

namespace YsoCorp {

    public class SLookAtCam : YCBehaviour {

        public bool m_useCumtomCam;
        public Transform m_customCam;

        private Transform m_targetCam;

        private void Start() {
            if (this.m_useCumtomCam == true && this.m_customCam != null) {
                this.m_targetCam = this.m_customCam;
            } else {
                this.m_targetCam = this.cam.m_ycCamera.transform;
            }
        }
        private void LateUpdate() {
            this.LookAtCam();
        }

        public void LookAtCam() {
            this.transform.LookAt(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z) + this.m_targetCam.forward);
        }
    }
}
