using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskBarUI : MonoBehaviour
{
    // Enum para las distintas acciones de los botones
    public enum TaskActionType
    {
        Up,
        Down,
        Complete,
        Delete,
        Edit,
        Copy,
        Paste,
        ColorChange
    }

    // Evento genérico que emite el tipo de acción
    public event Action<TaskActionType> OnTaskAction;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textComplete;
    [SerializeField] private TextMeshProUGUI textIndexer;

    [Header("BG")]
    [SerializeField] private Image bgImage;
    [SerializeField] private Image taskBarColorImage;
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
    [SerializeField] private Image completeTick;
    [SerializeField] private Color completeButtonColor;
    [SerializeField] private Color uncompleteButtonColor;
    [Space]
    [SerializeField] private Button deleteButton;
    [Space]
    [SerializeField] private Button editButton;
    [Space]
    [SerializeField] private Button copyButton;
    [SerializeField] private Button pasteButton;

    private Task task;
    private void Awake()
    {
        // Suscribir cada botón al mismo método con su acción correspondiente
        upButton.onClick.AddListener(() => RaiseAction(TaskActionType.Up));
        downButton.onClick.AddListener(() => RaiseAction(TaskActionType.Down));
        completeButton.onClick.AddListener(() => RaiseAction(TaskActionType.Complete));
        deleteButton.onClick.AddListener(() => RaiseAction(TaskActionType.Delete));
        editButton.onClick.AddListener(() => RaiseAction(TaskActionType.Edit));
        copyButton.onClick.AddListener(() => RaiseAction(TaskActionType.Copy));
        pasteButton.onClick.AddListener(() => RaiseAction(TaskActionType.Paste));

        TaskBarHandlers.OnCopyOrPaste +=TaskBarHandlers_OnCopyOrPaste;
    }

    private void OnEnable()
    {
        TaskBarHandlers_OnCopyOrPaste();
    }

    private void OnDestroy()
    {
        TaskBarHandlers.OnCopyOrPaste -=TaskBarHandlers_OnCopyOrPaste;
    }


    // Método que lanza el evento
    private void RaiseAction(TaskActionType actionType)
    {
        OnTaskAction?.Invoke(actionType);
    }

    private void TaskBarHandlers_OnCopyOrPaste()
    {
        pasteButton.gameObject.SetActive(TaskBarHandlers.hasTaskCopied);
    }

    public void SetVisuals(Task task, bool isCompleted, int toDoBarIndexer)
    {
        this.task = task;

        textName.SetText(task.taskName);
        TaskPriorityVisuals(toDoBarIndexer);

        //Completed
        textIndexer.gameObject.SetActive(!isCompleted);

        string completeString = !isCompleted ? "Hecho" : "PorHacer";

        curBgColor = isCompleted ?
            completeColor : uncompleteColor;

        TaskColor taskColor = (TaskColor)task.taskColorIndex;
        taskBarColorImage.color = TaskBarHandlers.GetTaskColor(taskColor);

        bgImage.color = curBgColor;

        completeButton.GetComponent<Image>().color = isCompleted ?
            completeButtonColor : uncompleteButtonColor;
        completeTick.color = isCompleted ? Color.white : Color.black;

        textComplete.SetText(completeString);
        upButton.gameObject.SetActive(!isCompleted);
        downButton.gameObject.SetActive(!isCompleted);
        deleteButton.gameObject.SetActive(isCompleted);
        editButton.gameObject.SetActive(!isCompleted);
    }

    public void ChangeTaskColor()
    {
        task.taskColorIndex = TaskBarHandlers.ChangeTaskColor(task);

        TaskColor taskColor = (TaskColor)task.taskColorIndex;
        taskBarColorImage.color = TaskBarHandlers.GetTaskColor(taskColor);

        RaiseAction(TaskActionType.ColorChange);
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
