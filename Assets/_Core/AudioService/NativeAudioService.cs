using System.Collections.Generic;
using UnityEngine;

namespace Core.AudioService
{
    public class NativeAudioService : MonoBehaviour, IAudioService
    {
        private readonly Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();

        private void Awake()
        {
            AudioSource[] sources = GetComponentsInChildren<AudioSource>();
            foreach (AudioSource source in sources)
            {
                this.audioSources.Add(source.name, source);
            }
        }

        public void PlaySfx(string sfxName)
        {
            string sfxKey = $"Sfx_{sfxName}";
            if (this.audioSources.TryGetValue(sfxKey, out var sfx))
            {
                sfx.Play();
            }
        }

        public void StopSfx(string sfxName)
        {
            string sfxKey = $"Sfx_{sfxName}";
            if (this.audioSources.TryGetValue(sfxKey, out var sfx))
            {
                sfx.Stop();
            }
        }

        public void PlayMusic(string musicName)
        {
            string musicKey = $"Music_{musicName}";
            if (this.audioSources.TryGetValue(musicKey, out var music))
            {
                music.Play();
            }
        }

        public void StopMusic(string musicName)
        {
            string musicKey = $"Music_{musicName}";
            if (this.audioSources.TryGetValue(musicKey, out var music))
            {
                music.Stop();
            }
        }

        public void SetVolume(float val)
        {
            foreach (AudioSource source in this.audioSources.Values)
            {
                source.volume = val;
            }
        }

        public void SetVolumeSfx(float val)
        {
            
        }

        public void SetVolumeMusic(float val)
        {
            
        }
    }
}