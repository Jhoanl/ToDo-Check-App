using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    [Header("Systems")]
    [SerializeField] private ModalPanel modalPanel;
    [SerializeField] private InputTextPanel inputTextPanel;
    [Space]
    [Header("TaskLists")]
    [SerializeField] private GameObject taskLists;
    [SerializeField] private GameObject taskListPrefab;
    [SerializeField] private Transform taskListsParent;

    [Header("Tasks")]
    [SerializeField] private Transform taskBarsParent;
    [Header("Create")]
    [SerializeField] private TaskListsUI taskListsUI;
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


    public ModalPanel CurModalPanel { get => modalPanel; }
    public InputTextPanel InputTextPanel { get => inputTextPanel; } 
    #endregion

    private void Awake()
    {
        instance = this;

        createToDoTaskButton.onClick.AddListener(OnTaskBarCreateButton);
        showCompletedTasks.onClick.AddListener(ShowCompletedTasks);
        deleteAllCompletedTasksButton.onClick.AddListener(DeleteAllTasksButton);

        inputFieldTaskInput.onEndEdit.AddListener(OnEndEditCreateTaskInputField);
        SetUITasks();
    }

    private void ShowCompletedTasks()
    {
        GameManager.Instance.ShowCompletedTasks = !GameManager.Instance.ShowCompletedTasks;
        SetUITasks();
    }

    private void SetUITasks()
    {
        bool show = GameManager.Instance.ShowCompletedTasks;

        string showString = !show ? "Mostrar Completadas" : "Mostrar Por Hacer";
        showCompletedTasksText.SetText(showString);

        showCompletedTasksImage.color = !show ? showCompletedColor : showToDoColor;

        createTasksPanel.SetActive(!show);
        deleteAllCompletedTasksButton.gameObject.SetActive(show);
    }

    private void OnEndEditCreateTaskInputField(string text)
    {
        OnTaskBarCreateButton();
    }

    private void OnTaskBarCreateButton()
    {
        if(inputFieldTaskInput.text == string.Empty) { return; }

        Task task = new Task();

        task.taskPriority = 0;
        task.taskName = inputFieldTaskInput.text;
        inputFieldTaskInput.text = "";

        GameManager.Instance.CreateTaskBar(task);
    }

    private void DeleteAllTasksButton()
    {
        Action deleteAll = () => GameManager.Instance.DeleteAllTasksCompleted();

        CurModalPanel.ShowConfirm(null, "¿Deseas Borrar todas las tareas completadas?",
            null, deleteAll, () => { });
    }
}
