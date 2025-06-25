using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace YsoCorp {
    public class Player : YCBehaviour {
        public Controller m_controller;
        public PlayableCharacterAdvanced m_character;

        protected override void Awake() {
            base.Awake();
            this.game.GameStatesEvent += this.PlayerGameBehaviour;
        }

        private void OnDestroy() {
            this.game.GameStatesEvent += this.PlayerGameBehaviour;
        }

        private void PlayerGameBehaviour(object sender, GameStatesEventArgs gameStatesEventArgs) {
            switch (gameStatesEventArgs.m_gameState) {
                case GameState.None:
                    break;
                case GameState.Home:
                    this.PlayerHome();
                    break;
                case GameState.Playing:
                    this.PlayerPlaying();
                    break;
                case GameState.Win:
                    this.PlayerWin();
                    break;
                case GameState.Lose:
                    this.PlayerLoose();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayerHome() {
            m_controller.CanUse = false;
        }

        private void PlayerPlaying() {
            m_controller.CanUse = true;
        }

        private void PlayerWin() {
            m_controller.CanUse = false;
        }

        private void PlayerLoose() {
            m_controller.CanUse = false;
        }
    }
}