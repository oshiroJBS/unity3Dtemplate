using System.Collections.Generic;

using UnityEngine;

namespace YsoCorp
{
    public class SoundManager : MonoBehaviour
    {

        public class SoundElement
        {
            public string m_key;
            public AudioSource m_audioSource;
        };

        [SerializeField] private bool Mute = false;

        [SerializeField] private AudioClip[] m_effects = null;
        private List<SoundElement> m_soundElements = new List<SoundElement>();

        public static SoundManager m_instance = null;

        private void Awake()
        {
#if !UNITY_EDITOR
            Mute = false;
#endif
            if (m_instance != null)
            {
                Destroy(this);
                return;
            }
            m_instance = this;
        }

        private AudioSource AddAudioSource(string se = "AudioSource-DEFAULT")
        {
            GameObject g = new GameObject(se);
            g.transform.SetParent(this.gameObject.transform);
            return g.AddComponent<AudioSource>();
        }


        public static void PlayEffect(string effect, float volume, float pitch, string key = "", bool loop = false)
        {
            m_instance._PlayEffect(effect, volume, pitch, key, loop);
        }

        private static void StopEffect(string key, bool destroy = false)
        {
            m_instance._StopEffect(key, destroy);
        }

        private void _StopEffect(string key, bool destroy = false)
        {
            for (int i = 0; i < this.m_soundElements.Count; i++)
            {
                if (this.m_soundElements[i].m_key == key)
                {
                    this.m_soundElements[i].m_audioSource.Stop();
                    this.m_soundElements[i].m_audioSource.clip = null;
                    if (destroy)
                    {
                        this.m_soundElements.RemoveAt(i);
                    }
                    return;
                }
            }
        }

        private void _PlayEffect(string effect, float volume, float pitch, string key = "", bool loop = false)
        {
            if (!Mute)
            {
                for (int i = 0; i < this.m_effects.Length; i++)
                {
                    if (this.m_effects[i].name == effect)
                    {
                        SoundElement se = this.SeContain(key);
                        AudioClip clip = this.m_effects[i];
                        if (se == null)
                        {
                            se = new SoundElement();
                            se.m_audioSource = this.AddAudioSource("AudioSource-" + key);
                            se.m_key = key;
                            this.m_soundElements.Add(se);
                        }
                        se.m_audioSource.volume = volume;
                        se.m_audioSource.pitch = pitch;
                        if (loop == true)
                        {
                            se.m_audioSource.clip = clip;
                            se.m_audioSource.loop = true;
                            se.m_audioSource.Play();
                        }
                        else
                        {
                            se.m_audioSource.PlayOneShot(clip);
                        }
                    }
                }
            }
        }

        private SoundElement SeContain(string key)
        {
            for (int i = 0; i < this.m_soundElements.Count; i++)
            {
                if (this.m_soundElements[i].m_key == key)
                {
                    return this.m_soundElements[i];
                }
            }
            return null;
        }
    }
}