using UnityEngine;

namespace YsoCorp
{
    public class PlayableCharacterAdvanced : PlayableCharacter
    {
        public Rigidbody m_characterRigidbody;
        public Animator m_characterAnimator;

        private bool m_isAlive = true;

        private static readonly int Moving = Animator.StringToHash("IsMoving");
        private static readonly int Death = Animator.StringToHash("Death");


        protected override void Awake()
        {
            base.Awake();
        }

        protected override void FixedUpdate()
        {
            if (this.m_isAlive)
            {
                base.FixedUpdate();
            }
        }

        protected override void UpdateAnimation()
        {
            // this.m_characterAnimator.SetBool(Moving, this.m_inputMagnitude > 0);
        }

        protected override void Rotate()
        {
            if (this.CurrentInput == Vector3.zero) { return; }

            if (this.cam.CamLookAtCharacterForward)
            {
                this.m_characterRigidbody.transform.Rotate(Vector3.up, this.CurrentInput.x * this.m_rotationSpeed * Time.fixedDeltaTime * this.m_inputMagnitude);
            }
            else
            {
                Quaternion targetRotation = Quaternion.LookRotation(this.CurrentInput.normalized, Vector3.up);
                float step = this.m_rotationSpeed * 10f * Time.fixedDeltaTime * m_inputMagnitude;
                Quaternion rotation = Quaternion.RotateTowards(this.m_characterRigidbody.rotation, targetRotation, step);
                this.m_characterRigidbody.MoveRotation(rotation);
            }
        }

        protected override void Translate()
        {
            Vector3 translation;

            if (this.cam.CamLookAtCharacterForward)
            {
                translation = this.transform.forward * this.CurrentInput.z * this.m_translationSpeed * Time.fixedDeltaTime;
            }
            else
            {
                translation = this.CurrentInput * (this.m_translationSpeed) * (Time.fixedDeltaTime);
            }

            this.m_characterRigidbody.velocity = translation;
        }

        public override void Stop()
        {
            base.Stop();
            this.m_characterRigidbody.Sleep();
        }

        public void timer()
        {
            Invoke("die", 120f);
        }

        public void Die()
        {
            this.Stop();
            this.m_isAlive = false;
            this.game.gameState = GameState.Lose;

            //this.m_characterAnimator.SetTrigger(Death);
            //this.m_characterRigidbody.isKinematic = true;

            //foreach (Collider col in this.GetComponentsInChildren<Collider>()) {
            //    Destroy(col);
            //}
        }
    }
}