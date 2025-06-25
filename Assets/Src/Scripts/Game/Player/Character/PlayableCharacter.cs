using System;
using UnityEngine;

namespace YsoCorp {
    public class PlayableCharacter : BaseCharacter {
        private Vector3 m_currentInput;
        protected float m_inputMagnitude;

        public Vector3 CurrentInput {
            get => m_currentInput;
            set {
                m_currentInput = value;
                m_inputMagnitude = m_currentInput.magnitude;
            }
        }

        protected override void Awake() {
            base.Awake();
            this.player.m_controller.FingerEvent += this.PlayableCharacterFingerBehaviour;
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            this.player.m_controller.FingerEvent -= this.PlayableCharacterFingerBehaviour;
        }

        private void PlayableCharacterFingerBehaviour(object sender, FingerArgs fingerArgs) {
            switch (fingerArgs.m_fingerState) {
                case FingerState.None:
                    break;
                case FingerState.Began:
                    this.PlayerBegan();
                    break;
                case FingerState.Executing:
                    this.PlayerExecuting(fingerArgs.m_direction);
                    break;
                case FingerState.Ended:
                    this.PlayerEnded();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void PlayerBegan() {
        }

        protected virtual void PlayerExecuting(Vector2 direction) {
            this.CurrentInput = new Vector3(direction.x, 0, direction.y);
        }

        protected virtual void PlayerEnded() {
            this.CurrentInput = Vector3.zero;
        }

        protected override void Rotate() {
            if (this.CurrentInput == Vector3.zero) { return; }
            Quaternion targetRotation = Quaternion.LookRotation(this.CurrentInput.normalized, Vector3.up);
            float step = m_rotationSpeed * Time.fixedDeltaTime * m_inputMagnitude;
            Quaternion rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, step);
            this.transform.rotation = rotation;
        }

        protected override void Translate() {
            Vector3 translation = this.CurrentInput * (m_translationSpeed) * (Time.fixedDeltaTime);
            Vector3 position = this.transform.position + (translation);
            this.transform.position = position;
        }
    }
}