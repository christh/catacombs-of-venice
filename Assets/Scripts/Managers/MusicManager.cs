using System;
using UnityEngine;

namespace CV
{
    public class MusicManager : Singleton<MusicManager>
    {
        public Sound[] sounds;

        public static MusicManager instance;
        public string currentTrack = "Start";
        [SerializeField] private Sound currentClip;

        private void Start()
        {
            foreach (var sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.loop = true;
            }

            Play(currentTrack);
        }

        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.trackName == name);
            if (s == null)
            {
                Debug.LogWarning($"Sound: {s.trackName} not found!");
                return;
            }

            if (currentClip.trackName == "")
            {
                currentClip = s;
            }

            if (currentClip.trackName != "" && s.source.clip.name != currentClip.trackName)
            {
                currentClip.source.Stop();
                s.source.loop = true;
                s.source.Play();
                currentClip = s;
            }
        }

        public void StopPlayingCurrentTrack()
        {
            currentClip.source.Stop();
        }
    }
}