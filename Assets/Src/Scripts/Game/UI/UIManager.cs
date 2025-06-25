using System;
using UnityEngine;
using UnityEngine.UI;

namespace YsoCorp {
    public class UIManager : YCBehaviour {
        [Header("Playable")]
        [SerializeField] private Button m_bEndcard = null;
        [SerializeField] private GameObject m_menuHome;
        [SerializeField] private GameObject m_menuPlaying;
        [SerializeField] private GameObject m_menuWin;
        [SerializeField] private GameObject m_menuLoose;

        private GameObject m_currentMenu;
        private Coroutine m_end;

        private SubUI m_subUI;
        public SubUI SubUI {
            get => this.m_subUI;
            set => this.m_subUI = value;
        }

        protected override void Awake() {
            base.Awake();
            this.game.GameStatesEvent += this.UIManagerGameBehaviour;
            this.m_bEndcard.onClick.AddListener(() => YsoPlayable.Network.Install());
        }

        private void UIManagerGameBehaviour(object sender, GameStatesEventArgs gameStatesEventArgs) {
            switch (gameStatesEventArgs.m_gameState) {
                case GameState.None:
                    break;
                case GameState.Home:
                    this.UIManagerHome();
                    break;
                case GameState.Playing:
                    this.UIManagerPlaying();
                    break;
                case GameState.Win:
                    this.UIManagerWin();
                    break;
                case GameState.Lose:
                    this.UIManagerLoose();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UIManagerHome() {
            YsoPlayable.Network.GameReady();
            this.HideAllMenus();
            this.SwitchMenu(this.m_menuHome);
        }

        private void UIManagerPlaying() {
            this.SwitchMenu(this.m_menuPlaying);
            //this.m_end = this.InvokeCallback(30f, () => this.game.gameState = GameState.Lose);
        }

        private void UIManagerWin() {
            this.SwitchMenu(this.m_menuWin);
            this.EndCard();
        }

        private void UIManagerLoose() {
            this.SwitchMenu(this.m_menuLoose);
            this.EndCard();
        }

        private void SwitchMenu(GameObject menuToDisplay) {
            if (this.m_currentMenu) { this.m_currentMenu.SetActive(false); }
            this.m_currentMenu = menuToDisplay;
            this.m_currentMenu.SetActive(true);
        }

        private void HideAllMenus() {
            this.m_menuHome.SetActive(false);
            this.m_menuPlaying.SetActive(false);
            this.m_menuWin.SetActive(false);
            this.m_menuLoose.SetActive(false);
        }

        public void Tutorial() {
            this.game.gameState = GameState.Playing;
        }

        private void EndCard() {
            if (this.m_end != null) { this.StopCoroutine(this.m_end); }
            YsoPlayable.Network.GameEnd();
            this.m_bEndcard.gameObject.SetActive(true);
        }

        private void OnDestroy() {
            this.game.GameStatesEvent -= this.UIManagerGameBehaviour;
        }
    }
}