using UnityEngine;

[System.Serializable]
public class Audio_Data
{
    [Header("Audio")]

    [SerializeField] private AudioClip clip;
    public AudioClip Clip { get => clip; }

    [Range(0, 1)]
    [SerializeField] private float audioVolume = .7f;
    public float AudioVolume { get => audioVolume; }
}
