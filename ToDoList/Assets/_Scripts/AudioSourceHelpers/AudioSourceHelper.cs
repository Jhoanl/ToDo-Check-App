using UnityEngine;
using UnityEngine.Audio;

public class AudioSourceHelper
{
    private const float MAX_AUDIO_RANGE = 300;
    private const float MIN_AUDIO_RANGE = 50;
    private const float MAX_PITCH_RANGE = 0.1f;
    private AudioSource aSource;


    private bool pitchVariation = false;
    private bool playOnEnable = true;

    private GameObject parent;

    public bool PlayOnEnable { get => playOnEnable; }
    public bool IsPlaying { get => aSource.isPlaying; }
    public AudioSource AudioSource { get => aSource; }

    public AudioSourceHelper(GameObject parent, bool worldSound, AudioClip initialClip, bool pitchVariation, float volume = 1)
    {
        Create(parent, worldSound, initialClip, pitchVariation, volume);
    }

    public AudioSourceHelper(GameObject parent, AudioClip initialClip, bool pitchVariation, float volume = 1)
    {
        Create(parent, true, initialClip, pitchVariation, volume);
    }

    private void Create(GameObject parent, bool worldSound, AudioClip initialClip, bool pitchVariation, float volume)
    {
        string parentName = parent != null ? parent.name : "";

        GameObject newGameObject = new GameObject(parentName + "_AudioSourceHelper");

        this.parent = parent;
        if (parent != null)
        {
            newGameObject.transform.parent = parent.transform;
            newGameObject.transform.position = parent.transform.position;
        }
        else
        {
            newGameObject.transform.parent = null;
            newGameObject.transform.position = Vector3.zero;
        }

        aSource = newGameObject.AddComponent<AudioSource>();

        aSource.maxDistance = MAX_AUDIO_RANGE;
        aSource.minDistance = MIN_AUDIO_RANGE;
        aSource.rolloffMode = AudioRolloffMode.Linear;
        aSource.loop = false;
        aSource.playOnAwake = false;
        aSource.dopplerLevel = 0;

        Seek(worldSound, initialClip, pitchVariation, volume);
    }

    public void Play()
    {
        if (aSource.clip == null)
            return;

        aSource.gameObject.SetActive(true);
        if(pitchVariation)
            aSource.pitch = GetPitchValue();
        aSource.Play();

        if (parent == null)
            FunctionTimer.Create(() => Disable(), aSource.clip.length);
    }

    private static float GetPitchValue()
    {
        return 1 + UnityEngine.Random.Range(-MAX_PITCH_RANGE, MAX_PITCH_RANGE);
    }

    public void PlayOneShotClip(AudioClip clipToPlay, float volume = 1)
    {
        if (clipToPlay == null)
            return;

        aSource.gameObject.SetActive(true);
        aSource.pitch = GetPitchValue();
        aSource.PlayOneShot(clipToPlay, volume);

        if (parent == null)
            FunctionTimer.Create(() => Disable(), aSource.clip.length);
    }

    public void SetPosition(Vector3 position)
    {
        aSource.transform.position = position;
    }

    public void Seek(bool worldSound, AudioClip initialClip, bool pitchVariation, float volume)
    {
        if (worldSound)
            aSource.spatialBlend = 1;
        else
            aSource.spatialBlend = 0;

        this.aSource.volume = volume;
        this.pitchVariation = pitchVariation;
        this.aSource.clip = initialClip;
    }

    public void SetMixerGroup(AudioMixerGroup audioMixerGroup)
    {
        aSource.outputAudioMixerGroup = audioMixerGroup;
    }

    public void SetLoopState(bool loopState)
    {
        aSource.loop = loopState;
    }

    private void Disable()
    {
        aSource.gameObject.SetActive(false);
    }
}
