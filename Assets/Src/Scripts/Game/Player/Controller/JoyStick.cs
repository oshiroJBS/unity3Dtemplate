using System;
using UnityEngine;

namespace YsoCorp {
    public class JoyStick : YCBehaviour {
        public Transform m_start;
        public Transform m_current;

        private void Start() {
            if (this.player.m_controller.m_showJoystickUI) {
                this.player.m_controller.FingerEvent += this.JoyStickFingerBehaviour;
            }
        }

        private void OnDestroy() {
            if (this.player.m_controller.m_showJoystickUI) {
                this.player.m_controller.FingerEvent -= this.JoyStickFingerBehaviour;
            }
        }

        private void JoyStickFingerBehaviour(object sender, FingerArgs fingerArgs) {
            switch (fingerArgs.m_fingerState) {
                case FingerState.None:
                    break;
                case FingerState.Began:
                    this.gameObject.SetActive(true);
                    break;
                case FingerState.Executing:
                    m_start.transform.position = fingerArgs.m_startPosition;
                    m_current.transform.position = fingerArgs.m_currentPosition;
                    break;
                case FingerState.Ended:
                    this.gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
