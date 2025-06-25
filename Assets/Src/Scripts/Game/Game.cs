using System;
using UnityEngine;

namespace YsoCorp {
    public enum GameState {
        None,
        Home,
        Playing,
        Lose,
        Win,
    }

    public class GameStatesEventArgs : EventArgs {
        public GameState m_gameState;
    }

    [DefaultExecutionOrder(-2)]
    public class Game : YCBehaviour {

        private GameStatesEventArgs m_gameStateEventArgs = new GameStatesEventArgs();
        public event EventHandler<GameStatesEventArgs> GameStatesEvent;


        public GameState gameState {
            get => this.m_gameStateEventArgs.m_gameState;
            set {
                if (value == m_gameStateEventArgs.m_gameState) { return; }
                this.m_gameStateEventArgs.m_gameState = value;
                this.GameStatesEvent?.Invoke(this, this.m_gameStateEventArgs);
            }
        }

        private void Start() {
            this.gameState = GameState.Home;
        }
    }
}