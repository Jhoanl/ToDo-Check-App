using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputTextPanel : MonoBehaviour
{
    [SerializeField] private GameObject inputPanel;
    [Space]
    [SerializeField] private TextMeshProUGUI tittleText;
    [SerializeField] private TMP_InputField inputField;
    [Space]
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button declineButton;

    private Action<string> onConfirmCallback;
    private Action onDeclineCallback;

    private void Awake()
    {
        confirmButton.onClick.AddListener(Confirm);
        declineButton.onClick.AddListener(Close);

        inputField.onEndEdit.AddListener(InputField_OnEndEdit);
    }

    public void Show(string tittle, string curInput, Action<string> confirmAction)
    {
        inputPanel.SetActive(true);

        inputField.SetTextWithoutNotify(curInput);
        tittleText.SetText(tittle);

        onConfirmCallback = confirmAction;
    }

    private void InputField_OnEndEdit(string text)
    {
        Confirm();
    }

    private void Confirm()
    {
        onConfirmCallback?.Invoke(inputField.text);
        Close();
    }

    private void Close()
    {
        inputPanel.SetActive(false);
    }
}
