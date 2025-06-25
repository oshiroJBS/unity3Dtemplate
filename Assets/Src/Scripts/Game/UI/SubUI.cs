using System;
using System.Collections.Generic;
using UnityEngine;

namespace YsoCorp {
    public class StepEventArgs : EventArgs {
        public int m_current = -1;
        public GameObject m_step;
    }

    public class SubUI : YCBehaviour {
        [Header("Playable")]
        [SerializeField] private List<GameObject> m_steps;

        private StepEventArgs m_stepEventArgs = new StepEventArgs();
        public event EventHandler<StepEventArgs> StepEvent;

        public int Step {
            get => this.m_stepEventArgs.m_current;
            set {
                if (this.m_stepEventArgs.m_current == value) { return; }
                this.m_stepEventArgs.m_current = value;
                this.m_stepEventArgs.m_step = m_steps[this.m_stepEventArgs.m_current];
                StepEvent?.Invoke(this, m_stepEventArgs);
            }
        }

        protected override void Awake() {
            base.Awake();
            this.StepEvent += this.SubUIBehaviour;
        }

        private void Start() {
            if (m_steps != null) { this.Step = 0; }
        }

        private void SubUIBehaviour(object sender, StepEventArgs stepEventArgs) {
            for (int i = 0; i < m_steps.Count; i++) {
                if (i == stepEventArgs.m_current) { m_steps[i].SetActive(true); } 
                else { m_steps[i].SetActive(false); }
            }
        }
    }
}