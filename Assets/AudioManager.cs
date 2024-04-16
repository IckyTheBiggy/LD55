using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    private struct SoundItem
    {
        public Sounds Sound;
        public AudioClip Clip;
        public AudioMixerGroup MixerGroup;
        [Range(0, 1)] public float SpatialBlend;
    }

    public enum Sounds
    {
        Sword,
        Damage,
        EnemyAttack
    }

    [SerializeField] private List<SoundItem> SoundCollection;
    private readonly Dictionary<Sounds, AudioSource> _audioSources = new();

    private void Awake()
    {
        AddSources();
    }

    private void AddSources()
    {
        foreach (var sound in SoundCollection)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.clip = sound.Clip;
            src.outputAudioMixerGroup = sound.MixerGroup;
            _audioSources.Add(sound.Sound, src);
        }
    }

    private void SetupAudioSource(AudioSource src, SoundItem soundItem, float pitch)
    {
        src.playOnAwake = false;
        src.clip = soundItem.Clip;
        src.pitch = pitch;
        src.outputAudioMixerGroup = soundItem.MixerGroup;
        src.spatialBlend = soundItem.SpatialBlend;
    }

    public void PlaySFX(Sounds sound, float maxPitchDelta = 0)
    {
        var src = _audioSources[sound];
        src.pitch = Random.Range(1 - maxPitchDelta, 1 + maxPitchDelta);
        src.Play();
    }

    public void PlaySFXOnObject(Sounds sound, GameObject targetObject, float maxPitchDelta = 0)
    {
        var src = targetObject.AddComponent<AudioSource>();
        var soundItem = SoundCollection.Find(x => x.Sound == sound);
        SetupAudioSource(src, soundItem, Random.Range(1 - maxPitchDelta, 1 + maxPitchDelta));
        StartCoroutine(PlaySFXOnObjectRoutine(src));
    }

    private IEnumerator PlaySFXOnObjectRoutine(AudioSource src)
    {
        src.Play();
        yield return new WaitForSeconds(src.clip.length / src.pitch + 0.1f);
        Destroy(src);
    }

    public void PlaySFXAtPosition(Sounds sound, Vector3 pos, float maxPitchDelta = 0)
    {
        var src = Instantiate(new GameObject(), pos, Quaternion.identity).AddComponent<AudioSource>();
        var soundItem = SoundCollection.Find(x => x.Sound == sound);
        SetupAudioSource(src, soundItem, Random.Range(1 - maxPitchDelta, 1 + maxPitchDelta));
        StartCoroutine(PlaySFXAtPos(src));
    }

    private IEnumerator PlaySFXAtPos(AudioSource src)
    {
        src.Play();
        yield return new WaitForSeconds(src.clip.length / src.pitch + 0.1f);
        Destroy(src);
    }
}
