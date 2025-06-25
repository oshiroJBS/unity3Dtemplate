using UnityEngine;

namespace YsoCorp {
    public class TriggerCam : YCBehaviour {

        public int m_virtualCamIndex = 1;
        public bool m_isFollowingCharacter = false;

        protected bool m_isTriggerUp = true;

        virtual public void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player") {
                this.cam.CamFollowCharacter = m_isFollowingCharacter;
                if (this.m_isTriggerUp) { this.cam.SwitchCam(m_virtualCamIndex); } 
                else {
                    this.cam.SwitchCam(0);
                    this.cam.CamFollowCharacter = true;
                }
                m_isTriggerUp = !m_isTriggerUp;
            }
        }
    }
}