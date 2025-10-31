using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalPanel : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] private Transform _headerArea;
    [SerializeField] private TextMeshProUGUI _tittleField;

    [Header("Content")]
    [SerializeField] private Transform _contentArea;
    [SerializeField] private Image _modalImage;
    [SerializeField] private TextMeshProUGUI modalText;

    [Header("Footer")]
    [SerializeField] private Transform _footerArea;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _declineButton;

    [Space]
    private readonly float defaultShowTime = 2;
    private readonly float enabledAnimDuration = .5f;
    private readonly float disableAnimDuration = 2f;

    private float _showTime = 2;


    private CanvasGroup canvasGroup;
    private Coroutine curAnimCoroutine;

    private Action onConfirmAction;
    private Action onDeclineAction;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        canvasGroup.alpha = 0;
    }

    public void ShowTimed(string text, Sprite modalSprite, float showTime = -1)
    {
        _showTime = showTime == -1 ? defaultShowTime : showTime;
        Show(null, text, true, modalSprite);
    }

    public void ShowAccept(string tittle, string contentText, Sprite modalSprite, bool canBeClosed = true)
    {
        Show(tittle, contentText, false, modalSprite);
        if (!canBeClosed)
            _footerArea.gameObject.SetActive(false);
    }

    public void ShowConfirm(string tittle, string contentText, Sprite modalSprite, Action onConfirm, Action onDecline)
    {
        Show(tittle, contentText, false, modalSprite, onConfirm, onDecline);
    }

    public void Show(string tittle, string contentText, bool timed,
        Sprite modalSprite, Action onConfirm = null, Action onDecline = null)
    {
        gameObject.SetActive(true);
        if (curAnimCoroutine != null)
            StopCoroutine(curAnimCoroutine);

        bool hasTittle = !string.IsNullOrEmpty(tittle);
        _headerArea.gameObject.SetActive(hasTittle);
        _tittleField.SetText(tittle);


        modalText.text = contentText;
        _footerArea.gameObject.SetActive(!timed);

        _modalImage.gameObject.SetActive(modalSprite != null);
        if (modalSprite != null)
            _modalImage.sprite = modalSprite;

        if (timed)
        {
            TimedVisuals();
        }
        else
        {
            Open();

            _confirmButton.onClick.RemoveAllListeners();
            _confirmButton.onClick.AddListener(Confirm);
            onConfirmAction = onConfirm;

            bool hasDecline = onDecline != null;
            _declineButton.onClick.RemoveAllListeners();
            _declineButton.onClick.AddListener(Decline);
            _declineButton.gameObject.SetActive(hasDecline);
            this.onDeclineAction = onDecline;
        }
    }

    #region Animation
    private void Open()
    {
        if (curAnimCoroutine != null)
            StopCoroutine(curAnimCoroutine);
        StartCoroutine(
                HideAnim(false));
    }

    private void Confirm()
    {
        onConfirmAction?.Invoke();
        Close();
    }

    private void Decline()
    {
        onDeclineAction?.Invoke();
        Close();
    }

    private void Close()
    {
        if (curAnimCoroutine != null)
            StopCoroutine(curAnimCoroutine);

        StartCoroutine(HideAnim(true));
    }

    private void TimedVisuals()
    {
        if (curAnimCoroutine != null)
            StopCoroutine(curAnimCoroutine);
        curAnimCoroutine = StartCoroutine(TimedAnim());
    }

    private IEnumerator TimedAnim()
    {
        StartCoroutine(
                HideAnim(true));

        yield return new WaitForSeconds(_showTime);
        StartCoroutine(
                HideAnim(true));
    }

    private IEnumerator HideAnim(bool hide)
    {
        float startValue = hide ? 1 : 0;
        float endValue = hide ? 0 : 1;

        float duration = hide ? disableAnimDuration : enabledAnimDuration;

        canvasGroup.alpha = startValue;
        float curValue = startValue;

        while (curValue  < 1)
        {
            curValue +=(1f / duration) * Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startValue, endValue, curValue);

            yield return null;
        }

        canvasGroup.alpha = endValue;
        canvasGroup.interactable = !hide;
        canvasGroup.blocksRaycasts = !hide;
    }
    #endregion
}
