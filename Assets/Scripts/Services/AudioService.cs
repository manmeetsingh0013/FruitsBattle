using CardMatch.Services.Base;
using UnityEngine;

namespace CardMatch.Services
{
    public class AudioService : ServiceBase
    {
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private AudioSource bgSource;
        [SerializeField] private AudioSource sfxSource;

        protected override void Awake()
        {
            base.Awake();
        }

        public void PlayAudio(AudioTag audioTag)
        {
            AudioFile audioScriptable = gameConfig.audios.Find(x => x.AudioTag.Equals(audioTag));

            if (audioScriptable != null)
            {
                if (audioScriptable.AudioTag == AudioTag.BG)
                {
                    bgSource.clip = audioScriptable.AudioClip;
                    bgSource.loop = audioScriptable.loop;
                    bgSource.Play();
                }
                else
                {
                    sfxSource.clip = audioScriptable.AudioClip;
                    sfxSource.Play();
                }
            }

        }

        protected override void RegisterService() =>
            Bootstrap.RegisterService(this);
    }
}