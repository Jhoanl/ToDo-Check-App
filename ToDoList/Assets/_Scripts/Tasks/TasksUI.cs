using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TasksUI : MonoBehaviour
{
    private GameUI ui;

    [Header("Tasks")]
    [SerializeField] private Transform taskBarsParent;

    [Header("Create")]
    [SerializeField] private GameObject createTasksPanel;
    [SerializeField] private TMP_InputField inputFieldTaskInput;
    [SerializeField] private Button createToDoTaskButton;
    [Space]
    [Header("Completed")]
    [SerializeField] private Button showCompletedTasks;
    [SerializeField] private Image showCompletedTasksImage;
    [SerializeField] private Color showCompletedColor;
    [SerializeField] private Color showToDoColor;
    [SerializeField] private TextMeshProUGUI showCompletedTasksText;
    [Space]
    [SerializeField] private Button deleteAllCompletedTasksButton;

    public static event Action OnCreateTaskButton;
    public static event Action OnDeleteTaskButton;
    public static event Action<bool> OnShowCompletedTasks;

    private bool showCompleteTasks;

    #region Getters

    public Transform TaskBarsParent { get => taskBarsParent; }

    public string LastInput
    {
        get => inputFieldTaskInput.text;
        set
        {
            string lastInput = value;
            inputFieldTaskInput.SetTextWithoutNotify(lastInput);
        }
    }

    #endregion


    private void Awake()
    {
        createToDoTaskButton.onClick.AddListener(OnTaskBarCreateButton);
        showCompletedTasks.onClick.AddListener(ShowCompletedTasks);
        deleteAllCompletedTasksButton.onClick.AddListener(DeleteAllTasksButton);

        inputFieldTaskInput.onEndEdit.AddListener(OnEndEditCreateTaskInputField);
        SetUITasks();
    }

    private void Start()
    {
        ui=GameUI.instance;
    }

    private void ShowCompletedTasks()
    {
        showCompleteTasks = !showCompleteTasks;
        GameManager.Instance.ShowCompletedTasks = !GameManager.Instance.ShowCompletedTasks;
        OnShowCompletedTasks?.Invoke(showCompleteTasks);

        SetUITasks();
    }

    private void SetUITasks()
    {
        string showString = !showCompleteTasks ? "Mostrar Completadas" : "Mostrar Por Hacer";
        showCompletedTasksText.SetText(showString);
        showCompletedTasksImage.color = !showCompleteTasks ? showCompletedColor : showToDoColor;

        createTasksPanel.SetActive(!showCompleteTasks);
        deleteAllCompletedTasksButton.gameObject.SetActive(showCompleteTasks);
    }

    private void OnEndEditCreateTaskInputField(string text)
    {
        OnTaskBarCreateButton();
    }

    private void OnTaskBarCreateButton()
    {
        if (inputFieldTaskInput.text == string.Empty) { return; }

        Task task = new Task();

        task.taskPriority = 0;
        task.taskName = inputFieldTaskInput.text;
        inputFieldTaskInput.text = "";

        GameManager.Instance.CreateTaskBar(task);
        OnCreateTaskButton?.Invoke();
    }

    private void DeleteAllTasksButton()
    {
        Action deleteAll = () =>
        {
            GameManager.Instance.DeleteAllTasksCompleted();
            OnDeleteTaskButton?.Invoke();
        };

        ui.CurModalPanel.ShowConfirm(null, "¿Deseas Borrar todas las tareas completadas?",
            null, deleteAll, () => { });
    }

}
