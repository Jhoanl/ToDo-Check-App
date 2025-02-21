using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceHelpers_Handler
{
    private static List<AudioSourceHelper> souceHelpers;

    public static void PlaySound(AudioClip clip, Vector3 position, bool pitchVariation, bool worldSound, float volume)
    {
        AudioSourceHelper audioSouceHelper = GetAudioSouceHelper(clip, pitchVariation, worldSound, volume);
        audioSouceHelper.SetPosition(position);
        audioSouceHelper.Play();
    }

    private static AudioSourceHelper GetAudioSouceHelper(AudioClip clip, bool pitchVariation, bool worldSound, float volume)
    {
        if (souceHelpers == null)
            souceHelpers = new List<AudioSourceHelper>();
        foreach (var helper in souceHelpers)
        {
            if (helper == null)
                continue;
            else if (helper.AudioSource == null)
                continue;

            if (helper.IsPlaying)
                continue;

            helper.Seek(worldSound, clip, pitchVariation, volume);

            return helper;
        }

        ClearAudioSourceHelpers();

        return CreateAudioSourceHelper(clip, pitchVariation, worldSound, volume);
    }

    private static AudioSourceHelper CreateAudioSourceHelper(AudioClip clip, bool pitchVariation, bool worldSound, float volume)
    {
        //Debug.Log("<color=yellow>CreatingAudioSource Proyectile</color>");
        AudioSourceHelper audioSouceHelper = new AudioSourceHelper(null,
            worldSound, clip, pitchVariation, volume);
        souceHelpers.Add(audioSouceHelper);
        return audioSouceHelper;
    }

    private static void ClearAudioSourceHelpers()
    {
        souceHelpers = RemoveNull(souceHelpers);
        souceHelpers.RemoveAll(item => item.AudioSource == null);
    }

    public static List<T> RemoveNull<T>(List<T> parameterList)
    {
        parameterList.RemoveAll(item => item == null);
        return parameterList;
    }
}
