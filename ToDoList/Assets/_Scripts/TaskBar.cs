using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskBar : MonoBehaviour
{
    [SerializeField] private int toDoBarIndexer;

    [SerializeField] private Task task;
    [Space]
    [Header("Visuals")]
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textComplete;
    [SerializeField] private TextMeshProUGUI textIndexer;
    [Space]
    [SerializeField] private Button editButton;
    [Space]
    [SerializeField] private Image bgImage;
    [SerializeField] private Color curBgColor;
    [SerializeField] private Color completeColor;
    [SerializeField] private Color uncompleteColor;
    [Space]
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [Space]
    [SerializeField] private Button completeButton;
    [SerializeField] private Color completeButtonColor;
    [SerializeField] private Color uncompleteButtonColor;
    [Space]
    [SerializeField] private Button deleteButton;

    public event Action<TaskBar> OnUpButtonClicked;
    public event Action<TaskBar> OnDownButtonClicked;
    public event Action<TaskBar> OnCompleteButtonClicked;
    public event Action<TaskBar> OnDeleteButtonClicked;

    public static event Action<TaskBar> OnEditButtonClicked;
    public static event Action<TaskBar> OnTaskBarChanged;

    public Task Task { get => task; }
    public bool IsCompleted { get => Task.isCompleted; set
        {
            Task.isCompleted=value;
            SetVisuals();
        }
    }

    public int TaskIdentifier { get => Task.taskIdentifier; set
        {
            Task.taskIdentifier=value;
        } }

    public int ToDoBarIndexer { get => toDoBarIndexer; set
        {
            toDoBarIndexer=value;
            textIndexer.SetText(this.ToDoBarIndexer.ToString());
        }
    }

    public void Init(Task task)
    {
        this.task = task;
        IsCompleted = this.task.isCompleted;

        SetVisuals();
    }

    public void SetVisuals()
    {
        textName.SetText(this.task.taskName);

        completeButton.GetComponent<Image>().color = IsCompleted ?
            completeButtonColor : uncompleteButtonColor;

        curBgColor = IsCompleted ?
            completeColor : uncompleteColor;

        bgImage.color = curBgColor;

        string completeString = !IsCompleted ? "Hecho" : "PorHacer";
        textComplete.SetText(completeString);

        textIndexer.SetText(this.ToDoBarIndexer.ToString());
        textIndexer.gameObject.SetActive(!IsCompleted);

        upButton.gameObject.SetActive(!IsCompleted);
        downButton.gameObject.SetActive(!IsCompleted);
        deleteButton.gameObject.SetActive(IsCompleted);
        editButton.gameObject.SetActive(!IsCompleted);
    }

    private void Awake()
    {
        upButton.onClick.AddListener(UpButtonClicked);
        downButton.onClick.AddListener(DownButtonClicked);
        completeButton.onClick.AddListener(CompleteButtonClicked);
        deleteButton.onClick.AddListener(DeleteButtonClicked);

        editButton.onClick.AddListener(EditButtonClicked);
    }

    private void EditButtonClicked()
    {
        OnEditButtonClicked?.Invoke(this);
    }

    private void UpButtonClicked()
    {

        Action moveAction = () =>
        {
            OnUpButtonClicked?.Invoke(this);
            GameManager.Instance.MoveTaskBar(this, true);
            OnTaskPriorityVisuals();
        };

        FunctionTimer.Create(() => { moveAction(); }, .1f);
        //task.taskPriority++;
        SetMovedVisuals();
    }

    private void DownButtonClicked()
    {
        Action moveAction = () =>
        {
            OnUpButtonClicked?.Invoke(this);
            GameManager.Instance.MoveTaskBar(this, false);
            OnTaskPriorityVisuals();
        };

        FunctionTimer.Create(() => { moveAction(); }, .1f);
        SetMovedVisuals();
    }

    void OnTaskPriorityVisuals()
    {
        //task.taskPriority = Mathf.Clamp(task.taskIdentifier, -20, 20);
        textIndexer.SetText(this.ToDoBarIndexer.ToString());

        OnTaskBarChanged?.Invoke(this);
    }

    private void SetMovedVisuals()
    {
        StartCoroutine(TriggerColorCoroutine(Color.blue));
    }

    private IEnumerator TriggerColorCoroutine(Color from)
    {
        float time = 0;
        float timeTo = .5f;

        while(time < timeTo)
        {
            time += Time.deltaTime;

            Color color = Color.Lerp(from, curBgColor, time / timeTo);
            bgImage.color = color;
            yield return null;
        }
        bgImage.color = curBgColor;
    }

    private void CompleteButtonClicked()
    {
        Color animationColor = IsCompleted ? Color.yellow : Color.green;
        StartCoroutine(TriggerColorCoroutine(animationColor));

        Action completeAction = () =>
        {
            OnCompleteButtonClicked?.Invoke(this);
            IsCompleted = !IsCompleted;

            OnTaskBarChanged?.Invoke(this);
        };

        FunctionTimer.Create(completeAction, .2f);
    }

    private void DeleteButtonClicked()
    {
        Action deleteAction = () =>
        {
            OnDeleteButtonClicked?.Invoke(this);
            IsCompleted = false;

            GameManager.Instance.DeleteTaskBar(this.Task);

            OnTaskBarChanged?.Invoke(this);
        };

        StartCoroutine(TriggerColorCoroutine(Color.red));
        FunctionTimer.Create(deleteAction, .2f);
    }

        

}
