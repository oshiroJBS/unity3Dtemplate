using UnityEngine;

namespace YsoCorp
{

    public class projectile : YCBehaviour
    {
        public GameObject fxExplosion = null;

        [Range(0.01f, 50)] public float liveSpan = 5f;
        [Range(0, 50)] public float speed = 1f;
        [Range(0, 50)] public float size = 1f;


        protected override void Awake()
        {
            base.Awake();

            this.transform.localScale = Vector3.one * size;

            if (this.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddForce(this.transform.forward * speed * 100);
            }
            else
            {
                Debug.LogWarning("no Rigidbody on projectile " + this.transform.name);
            }

            Invoke("die", liveSpan);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                this.die();
            }

        }
        private void die()
        {
            if (this.fxExplosion != null)
            {
                this.fxExplosion.transform.localScale = Vector3.one * size;
                SoundManager.PlayEffect("BulletImpact", 0.7f, Random.Range(.9f, 1.2f));
                Instantiate(this.fxExplosion, this.transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}