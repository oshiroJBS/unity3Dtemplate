using System;
using System.Collections;

using UnityEngine;

namespace YsoCorp {
    public class YCBehaviour : MonoBehaviour {
        private static Game GAME = null;
        private static Player PLAYER = null;
        private static UIManager UIMANAGER = null;
        private static Cam CAMERA = null;

        public Game game {
            get => GAME;
            private set { }
        }
        public Player player {
            get => PLAYER;
            private set { }
        }
        public UIManager uiManager {
            get => UIMANAGER;
            private set { }
        }
        public Cam cam {
            get => CAMERA;
            private set { }
        }

        protected virtual void Awake() {
            this.Init();
        }

        private void Init() {
            if (GAME == null) { GAME = FindObjectOfType<Game>(); }
            if (PLAYER == null) { PLAYER = FindObjectOfType<Player>(); }
            if (UIMANAGER == null) { UIMANAGER = FindObjectOfType<UIManager>(); }
            if (CAMERA == null) { CAMERA = FindObjectOfType<Cam>(); }
        }

        private IEnumerator _invokeCallBack(float delay, Action action) {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

        public Coroutine InvokeCallback(float delay, Action action) {
            return this.StartCoroutine(this._invokeCallBack(delay, action));
        }

        private IEnumerator _invokeRepeatingCoroutine(float delay, float repeatRate, Action action, int repeat = -1) {
            yield return new WaitForSeconds(delay);
            while (repeat > 0 || repeat <= -1) {
                repeat--;
                action.Invoke();
                yield return new WaitForSeconds(repeatRate);
            }
        }

        public Coroutine InvokeCallBackRepeating(float delay, float repeatRate, Action action) {
            return this.StartCoroutine(this._invokeRepeatingCoroutine(delay, repeatRate, action));
        }
    }
}