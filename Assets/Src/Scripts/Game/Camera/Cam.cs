using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YsoCorp {
    [Serializable]
    public class TransformInfo {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }

    public class Cam : YCBehaviour {
        public Camera m_ycCamera;
        public List<TransformInfo> m_virtualCameras;

        public Transform m_target = null;
        [SerializeField] private bool m_camFollowCharacter = false;
        [SerializeField] private bool m_camLookAtCharacterForward = false;
        public float m_smoothSpeed = 1f;
        public float m_smoothTurnSpeed = 1f;
        public float m_smoothTransitionSpeed = 1f;

        private Vector3 m_currentCamPosition;
        private Vector3 m_currentCamRotation;
        private int m_currentCamIndex;

        public bool CamFollowCharacter {
            get => this.m_camFollowCharacter;
            set => this.m_camFollowCharacter = value;
        }

        public bool CamLookAtCharacterForward {
            get => this.m_camLookAtCharacterForward;
            set {
                this.m_camLookAtCharacterForward = value;
                if (!this.m_camLookAtCharacterForward) { this.SwitchCam(0); }
            }
        }

        protected override void Awake() {
            base.Awake();
            this.m_currentCamIndex = 0;
            this.m_currentCamPosition = this.m_virtualCameras[this.m_currentCamIndex].position;
            this.m_currentCamRotation = this.m_virtualCameras[this.m_currentCamIndex].rotation;
            this.m_ycCamera.transform.position = this.CamFollowCharacter ? m_target.position + this.m_virtualCameras[this.m_currentCamIndex].position : this.m_virtualCameras[this.m_currentCamIndex].position;
            this.m_ycCamera.transform.rotation = Quaternion.Euler(this.m_virtualCameras[this.m_currentCamIndex].rotation);
        }

        private void Start() {
            this.SwitchCam(m_currentCamIndex);
        }

        private void Update() {
            if (this.CamLookAtCharacterForward && this.CamFollowCharacter) { this.RotateAroundTarget(); } 
            else if (this.CamFollowCharacter) { this.FollowTarget(); } 
            else if (this.CamLookAtCharacterForward) { this.m_ycCamera.transform.LookAt(this.m_target, Vector3.up); } 
            else { this.MoveToPosition(); }
        }

        public void SwitchCam(int index, UnityAction _onComplete = null) {
            if (index >= m_virtualCameras.Count) { return; }
            m_currentCamIndex = index;
            this.StartCoroutine(this.Transition(_onComplete));
        }

        public void MoveToPosition() {
            m_ycCamera.transform.position = Vector3.Lerp(m_ycCamera.transform.position, m_currentCamPosition, m_smoothSpeed * Time.deltaTime);
            m_ycCamera.transform.rotation = Quaternion.Lerp(m_ycCamera.transform.rotation, Quaternion.Euler(m_currentCamRotation), m_smoothTurnSpeed * Time.deltaTime);
        }

        public void FollowTarget() {
            m_ycCamera.transform.position = Vector3.Lerp(m_ycCamera.transform.position, m_target.position + m_currentCamPosition, m_smoothSpeed * Time.deltaTime);
            m_ycCamera.transform.rotation = Quaternion.Lerp(m_ycCamera.transform.rotation, Quaternion.Euler(m_currentCamRotation), m_smoothTurnSpeed * Time.deltaTime);
        }

        public void RotateAroundTarget() {
            Vector3 desiredPosition = m_target.position + m_target.rotation * m_currentCamPosition;
            Vector3 smoothedPosition = Vector3.Lerp(m_ycCamera.transform.position, desiredPosition, m_smoothSpeed * Time.deltaTime);
            m_ycCamera.transform.position = smoothedPosition;

            Quaternion desiredrotation = m_target.rotation * Quaternion.Euler(m_currentCamRotation);
            Quaternion smoothedrotation = Quaternion.Lerp(m_ycCamera.transform.rotation, desiredrotation, m_smoothTurnSpeed * Time.deltaTime);
            m_ycCamera.transform.rotation = smoothedrotation;
        }

        public void CamShake(float duration = 1, float strength = 1) {
            this.m_ycCamera.DOShakePosition(duration, strength);
        }

        IEnumerator Transition(UnityAction _onComplete = null) {
            while (Maths.RoughlyEqual(m_currentCamPosition, m_virtualCameras[m_currentCamIndex].position) || Maths.RoughlyEqual(m_currentCamRotation.x, m_virtualCameras[m_currentCamIndex].rotation.x)) {
                m_currentCamPosition = Vector3.Lerp(m_currentCamPosition, m_virtualCameras[m_currentCamIndex].position, m_smoothTransitionSpeed * Time.deltaTime);
                m_currentCamRotation = Vector3.Lerp(m_currentCamRotation, m_virtualCameras[m_currentCamIndex].rotation, m_smoothTransitionSpeed * Time.deltaTime);
                yield return null;
            }
            _onComplete?.Invoke();
        }
    }
}

