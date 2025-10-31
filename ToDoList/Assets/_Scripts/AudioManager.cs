using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip uiClip;

    public static AudioManager instance;

    void Awake()
    {
        instance = this;
    }

    public static void PlayUIClip()
    {
        AudioClip clip = instance.uiClip;

        AudioSourceHelpers_Handler.PlaySound(clip, default, true, true, 1);
    }
}
