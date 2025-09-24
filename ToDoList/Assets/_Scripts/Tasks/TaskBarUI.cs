using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskBarUI : MonoBehaviour
{
    //Events
    public event Action OnUpButtonClicked;
    public event Action OnDownButtonClicked;
    public event Action OnCompleteButtonClicked;
    public event Action OnDeleteButtonClicked;
    public event Action OnEditButtonClicked;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textComplete;
    [SerializeField] private TextMeshProUGUI textIndexer;
        
    [Header("BG")]
    [SerializeField] private Image bgImage;
    [SerializeField] private Color curBgColor;
    [SerializeField] private Color completeColor;
    [SerializeField] private Color uncompleteColor;
    [Space]
    [Header("Movement")]
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [Space]
    [Header("Edit")]
    [SerializeField] private Button completeButton;
    [SerializeField] private Color completeButtonColor;
    [SerializeField] private Color uncompleteButtonColor;
    [Space]
    [SerializeField] private Button deleteButton;
    [Space]
    [SerializeField] private Button editButton;


    private void Awake()
    {
        upButton.onClick.AddListener(UpButtonClicked);
        downButton.onClick.AddListener(DownButtonClicked);

        completeButton.onClick.AddListener(CompleteButtonClicked);
        deleteButton.onClick.AddListener(DeleteButtonClicked);

        editButton.onClick.AddListener(EditButtonClicked);
    }

    #region Buttons

    private void UpButtonClicked()
    {
        OnUpButtonClicked?.Invoke();
    }

    private void DownButtonClicked()
    {
        OnDownButtonClicked?.Invoke();
    }

    private void DeleteButtonClicked()
    {
        OnDeleteButtonClicked?.Invoke();
    }

    private void CompleteButtonClicked()
    {
        OnCompleteButtonClicked?.Invoke();
    }

    private void EditButtonClicked()
    {
        OnEditButtonClicked?.Invoke();
    }

    #endregion

    public void SetVisuals(Task task, bool isCompleted, int toDoBarIndexer)
    {
        textName.SetText(task.taskName);
        TaskPriorityVisuals(toDoBarIndexer);

        //Completed
        textIndexer.gameObject.SetActive(!isCompleted);

        string completeString = !isCompleted ? "Hecho" : "PorHacer";

        curBgColor = isCompleted ?
            completeColor : uncompleteColor;

        bgImage.color = curBgColor;

        completeButton.GetComponent<Image>().color = isCompleted ?
            completeButtonColor : uncompleteButtonColor;

        textComplete.SetText(completeString);
        upButton.gameObject.SetActive(!isCompleted);
        downButton.gameObject.SetActive(!isCompleted);
        deleteButton.gameObject.SetActive(isCompleted);
        editButton.gameObject.SetActive(!isCompleted);
    }

    public void TaskPriorityVisuals(int toDoBarIndexer)
    {
        textIndexer.SetText(toDoBarIndexer.ToString());
    }

    public void TriggerColorChange(Color from)
    {
        StartCoroutine(TriggerColorCoroutine(from));
    }

    private IEnumerator TriggerColorCoroutine(Color from)
    {
        float time = 0;
        float timeTo = .5f;

        while (time < timeTo)
        {
            time += Time.deltaTime;

            Color color = Color.Lerp(from, curBgColor, time / timeTo);
            bgImage.color = color;
            yield return null;
        }
        bgImage.color = curBgColor;
    } 
}
