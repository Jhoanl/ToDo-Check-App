using Saving;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TasksListsUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform taskListsParent;
    [SerializeField] private GameObject taskListsGoPrefab;
    [Space]
    [SerializeField] private Button createListButton;

    [Header("Edit Menu")]
    [SerializeField] private TaskListsEditMenu editMenu;

    private List<TaskListButton> taskListButtons;

    private void Awake()
    {
        taskListButtons = new List<TaskListButton>();
        createListButton.onClick.AddListener(CreateTaskList);

        editMenu.Initialize(this);
    }

    private void Start()
    {
        editMenu.CloseEditMenu();
    }

    private void OnEnable()
    {
        RecalculateUI();
    }

    public void Initialize(List<TaskList> tasksLists)
    {
        Clear();

        Populate(tasksLists);
    }

    public void UpdateUI()
    {
        RecalculateUI();
    }

    private void RecalculateUI()
    {
        for (int i = 0; i < taskListButtons.Count; i++)
        {
            taskListButtons[i].UpdateUI();
        }
    }


    private void Populate(List<TaskList> tasksLists)
    {
        for (int i = 0; i < tasksLists.Count; i++)
        {
            InstantiateTaskList(tasksLists[i], false);
        }
    }

    
    private void Clear()
    {
        if (taskListButtons == null)
            taskListButtons = new List<TaskListButton>();

        for (int i = 0; i < taskListButtons.Count; i++)
        {
            Destroy(taskListButtons[i].gameObject);
        }

        taskListButtons.Clear();
    }

    public void SelectTaskList(TaskListButton taskListButton)
    {
        Debug.Log("Task list selected");

        //Load Set Game Manager tasks
        GameManager.Instance.SetTaskList(taskListButton.TasksList.identifier);
    }

    public void EditTaskList(TaskListButton taskListButton)
    {
        Debug.Log("Edit task Lists");
        //Open Edit Menu of task lists

        if (editMenu != null)
        {
            editMenu.Seek(taskListButton);
        }
        else
        {

            GameUI.instance.InputTextPanel.Show("Change task lists Name",
                taskListButton.TasksList.taskListName,
                (x) =>
                {
                    taskListButton.TasksList.taskListName = x;
                    taskListButton.UpdateUI();
                    Save();
                });

        }

    }

    public void DeleteTaskList(TaskListButton taskListButton)
    {
        Debug.Log($"Deleting task List : {taskListButton.TasksList.identifier}");

        DataBase.DeleteTaskList(taskListButton.TasksList);

        Initialize(DataBase.taskLists);

        Save();
    }

    private void CreateTaskList()
    {
        Debug.Log("Create task list");

        //Create New Task List
        TaskList tasksList = new TaskList();
        tasksList.taskListName = "New";
        tasksList.tasks = new List<Task>();

        DataBase.CreateTaskList(tasksList);

        InstantiateTaskList(tasksList);

        //Save

        Save();

        //Select
    }

    private void InstantiateTaskList(TaskList tasksLists, bool autoSetName = true)
    {
        GameObject go = Instantiate(taskListsGoPrefab, taskListsParent);

        if (autoSetName)
        {
            int index = taskListsParent.childCount;
            tasksLists.taskListName = "Lista " + index;
        }

        TaskListButton taskListBut = go.GetComponent<TaskListButton>();
        taskListBut.Initialize(this, tasksLists);

        taskListButtons.Add(taskListBut);
    }

    private List<TaskList> GetTaskLists()
    {
        List<TaskList> taskLists = new List<TaskList>();

        for (int i = 0; i < taskListButtons.Count; i++)
        {
            taskLists.Add(taskListButtons[i].TasksList);
        }

        return taskLists;
    }

    public void Save()
    {
        DataBase.taskLists = GetTaskLists();

        SaveAndLoad.Save();
    }


}
