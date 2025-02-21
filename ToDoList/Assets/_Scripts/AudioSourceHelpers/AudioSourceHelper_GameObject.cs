using UnityEngine;

public class AudioSourceHelper_GameObject : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [Space]
    [SerializeField] private bool pitchVariation;
    [SerializeField] private bool spatialSound;

    private void OnEnable()
    {
        AudioSourceHelpers_Handler.PlaySound(audioClip, transform.position, pitchVariation, spatialSound, 1);
    }
}