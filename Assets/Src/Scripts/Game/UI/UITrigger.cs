using System.Collections.Generic;
using UnityEngine;
using YsoCorp;

public class UITrigger : YCBehaviour {
    [SerializeField] List<GameObject> m_unlockable;
    [SerializeField] GameState m_targetState = GameState.None;
    [SerializeField] int m_targetStep = 0;

    private bool m_isTrigger = false;

    private void OnTriggerStay(Collider other) {
        //if (other.gameObject.CompareTag("Player") && !m_isTrigger) {
        //    if (this.m_targetStep >= 0) { this.uiManager.SubUI.Step = m_targetStep; } 
        //    else { this.gameObject.SetActive(false); }
        //    foreach (GameObject g in m_unlockable) {
        //        g.SetActive(true);
        //    }
        //    if (m_targetState != GameState.None) { this.game.gameState = m_targetState; }
        //    m_isTrigger = true;
        //}
    }
}
