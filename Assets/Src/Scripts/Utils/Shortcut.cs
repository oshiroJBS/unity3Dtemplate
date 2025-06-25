using UnityEngine;

namespace YsoCorp {
    public class Shortcut : YCBehaviour {

#if UNITY_EDITOR
        private void Update() {
            if (Input.GetKeyDown("w")) {
                this.game.gameState = GameState.Win;
            }
            if (Input.GetKeyDown("l")) {
                this.game.gameState = GameState.Lose;
            }
        }
#endif
    }
}