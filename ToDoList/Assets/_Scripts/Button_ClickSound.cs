using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_ClickSound : MonoBehaviour
{
    private void Awake()
    {
        if (TryGetComponent<Button>(out Button button))
        {
            button.onClick.AddListener(() => { AudioManager.PlayUIClip(); });
        }
    }
}
