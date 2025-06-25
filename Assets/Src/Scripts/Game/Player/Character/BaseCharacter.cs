using System;
using UnityEngine;

namespace YsoCorp {
    public class BaseCharacter : YCBehaviour {
        public float m_translationSpeed = 10;
        public float m_rotationSpeed = 1000;
        private bool m_canMove;
        private bool m_isPlaying;

        public bool CanMove {
            get => m_canMove;
            set => m_canMove = value;
        }

        public bool IsPlaying {
            get => m_isPlaying;
            set => m_isPlaying = value;
        }


        protected override void Awake() {
            base.Awake();
            this.game.GameStatesEvent += this.BaseCharacterGameBehaviour;
        }

        protected virtual void Start() {
            this.CanMove = true;
        }

        protected virtual void FixedUpdate() {
            this.UpdatePlaying();
            this.UpdateAnimation();
        }

        protected virtual void UpdatePlaying() {
            if (this.IsPlaying) {
                this.HandleMovement();
            }
        }

        protected virtual void HandleMovement() {
            if (this.CanMove) {
                this.Rotate();
                this.Translate();
            }
        }

        protected virtual void Rotate() {
        }

        protected virtual void Translate() {
        }

        protected virtual void UpdateAnimation() {
        }

        public virtual void Stop() {
            this.CanMove = false;
        }


        protected virtual void OnDestroy() {
            this.game.GameStatesEvent -= this.BaseCharacterGameBehaviour;
        }

        private void BaseCharacterGameBehaviour(object sender, GameStatesEventArgs gameStatesEventArgs) {
            switch (gameStatesEventArgs.m_gameState) {
                case GameState.None:
                    break;
                case GameState.Home:
                    this.BaseCharacterHome();
                    break;
                case GameState.Playing:
                    this.BaseCharacterPlaying();
                    break;
                case GameState.Win:
                    this.BaseCharacterWin();
                    break;
                case GameState.Lose:
                    this.BaseCharacterLoose();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void BaseCharacterHome() {
            this.IsPlaying = false;
        }

        protected virtual void BaseCharacterPlaying() {
            this.IsPlaying = true;
        }

        protected virtual void BaseCharacterWin() {
            this.IsPlaying = false;
        }

        protected virtual void BaseCharacterLoose() {
            this.IsPlaying = false;
        }
    }
}