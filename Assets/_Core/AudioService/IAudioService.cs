using Core.Service;

namespace Core.AudioService
{
    public interface IAudioService : IService
    {
        void PlaySfx(string sfxName);
        void StopSfx(string sfxName);
        void PlayMusic(string musicName);
        void StopMusic(string musicName);
        void SetVolume(float val);
        void SetVolumeSfx(float val);
        void SetVolumeMusic(float val);
    }
}