using System;
using UnityEngine;

namespace YsoCorp {
    public class Cursor : YCBehaviour {
        public LayerMask m_layerRayTargetRing;
        public SpriteRenderer m_spriteRenderer;

        private void Start() {
            if (this.player.m_controller.m_showCursorInGame) {
                this.player.m_controller.FingerEvent += this.CursorFingerBehaviour;
            }
        }

        private void OnDestroy() {
            this.player.m_controller.FingerEvent -= this.CursorFingerBehaviour;
        }

        private void CursorFingerBehaviour(object sender, FingerArgs fingerArgs) {
            switch (fingerArgs.m_fingerState) {
                case FingerState.None:
                    break;
                case FingerState.Began:
                    this.CursorBegan();
                    break;
                case FingerState.Executing:
                    this.CursorExecuting(fingerArgs.m_currentPosition);
                    break;
                case FingerState.Ended:
                    this.CursorEnded();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CursorBegan() {
            this.gameObject.SetActive(true);
        }
        private void CursorExecuting(Vector3 currentPosition) {
            Ray ray = this.cam.m_ycCamera.ScreenPointToRay(currentPosition);
            Physics.Raycast(ray, out RaycastHit hit, 100f, m_layerRayTargetRing);
            this.transform.position = hit.point;
        }

        private void CursorEnded() {
            this.gameObject.SetActive(false);
        }
    }
}
