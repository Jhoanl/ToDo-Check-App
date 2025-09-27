using Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Main")]
    [SerializeField] private GameObject taksBarPrefab;

    [Header("Cur Tasks")]
    [SerializeField] private List<TaskBar> taskBars;

    private bool showCompletedTasks;

    private static int lastTaskIndexer;

    public static event Action<bool> OnCompleteModeChanged;

    public List<TaskBar> TaskBars { get => taskBars; }
    public bool ShowCompletedTasks
    {
        get => showCompletedTasks; set
        {
            showCompletedTasks=value;
            OnCompleteModeChanged?.Invoke(showCompletedTasks);
            ShowTaskBars();
            OrderTaskBars();
        }
    }

    private void Awake()
    {
        Instance = this;

        TaskBar.OnTaskBarChanged +=TaskBar_OnTaskBarChanged;
        TaskBar.OnEditButtonClicked +=TaskBar_OnEditButtonClicked;
    }

    private void TaskBar_OnEditButtonClicked(TaskBar taskBarToEdit)
    {
        Action<string> confirmAction = (newName) =>
        {
            taskBarToEdit.Task.taskName = newName;
            taskBarToEdit.SetVisuals();

            SaveAndLoad.Save();
        };

        GameUI.instance.InputTextPanel.Show("Editar Tarea", taskBarToEdit.Task.taskName, confirmAction);
    }

    private void Start()
    {
        SaveAndLoad.Load();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveAndLoad.Save();
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }

    private void OnDestroy()
    {
        TaskBar.OnTaskBarChanged -=TaskBar_OnTaskBarChanged;
        TaskBar.OnEditButtonClicked -=TaskBar_OnEditButtonClicked;
    }

    private void TaskBar_OnTaskBarChanged(TaskBar obj)
    {
        ShowTaskBars();
        OrderTaskBars();

        SaveAndLoad.Save();
    }

    private void OrderTaskBars()
    {
        //Set completed at Lasts by sibling and toDoBarIndexer

        for (int i = 0; i < taskBars.Count; i++)
        {
            if (!taskBars[i].gameObject.activeInHierarchy)
            {
                taskBars[i].transform.SetSiblingIndex(taskBars.Count);
                taskBars[i].ToDoBarIndexer = taskBars.Count;
            }
            else
            {
                taskBars[i].ToDoBarIndexer = 0;
            }
        }

        OrderActive();
    }

    private void OrderActive()
    {
        //Order by toDoBarIndexer
        taskBars = taskBars.OrderBy(x => x.ToDoBarIndexer).ToList();

        //Order 
        for (int i = 0; i < taskBars.Count; i++)
        {
            if (!taskBars[i].gameObject.activeInHierarchy)
            {
                continue;
            }

            taskBars[i].transform.SetSiblingIndex(i);
            taskBars[i].ToDoBarIndexer = i;
        }
    }

    public void ShowTaskBars()
    {
        for (int i = 0; i < taskBars.Count; i++)
        {
            if (showCompletedTasks)
            {
                taskBars[i].gameObject.SetActive(taskBars[i].IsCompleted);
            }
            else
            {
                taskBars[i].gameObject.SetActive(!taskBars[i].IsCompleted);
            }
        }
    }

    public void RemoveTaskBar(TaskBar taskBar)
    {
        if (taskBars.Contains(taskBar))
            taskBars.Remove(taskBar);
    }

    #region Delete

    public void DeleteTaskBar(Task task)
    {
        TaskBar taskBarToDelete = null;

        for (int i = 0; i < taskBars.Count; i++)
        {
            if (taskBars[i].TaskIdentifier == task.taskIdentifier)
            {
                taskBarToDelete = taskBars[i];

                DeleteSingleTaskBar(taskBarToDelete);
                break;
            }
        }

        SaveAndLoad.Save();
    }

    private void DeleteSingleTaskBar(TaskBar taskBarToDelete)
    {
        Destroy(taskBarToDelete.gameObject);
        RemoveTaskBar(taskBarToDelete);
    }

    public void DeleteAllTasksCompleted()
    {
        List<TaskBar> taskBarsToDelete = new List<TaskBar>();

        for (int i = 0; i < taskBars.Count; i++)
        {
            if (taskBars[i].IsCompleted)
            {
                taskBarsToDelete.Add(taskBars[i]);
            }
        }

        for (int i = 0; i < taskBarsToDelete.Count; i++)
        {
            DeleteSingleTaskBar(taskBarsToDelete[i]);
        }

        SaveAndLoad.Save();
    }

    #endregion

    public void MoveTaskBar(TaskBar taskBar, bool up)
    {
        int taskBarIndex = taskBar.ToDoBarIndexer;
        int targetIndex = taskBar.ToDoBarIndexer;
        targetIndex += up ? -1 : +1;
        if (targetIndex < 0) return;

        for (int i = 0; i < taskBars.Count; i++)
        {
            if (!taskBars[i].gameObject.activeInHierarchy)
            {
                continue;
            }

            if (taskBars[i].ToDoBarIndexer == targetIndex)
            {
                taskBars[i].ToDoBarIndexer = taskBarIndex;
            }
        }

        taskBar.ToDoBarIndexer = targetIndex;
        ShowTaskBars();
        OrderActive();
    }

    public void CreateTaskBar(Task task, bool save = true)
    {
        if (task == null) { return; }

        TaskBar taskBar = null;
        taskBar= Instantiate(taksBarPrefab, GameUI.instance.TasksUI.TaskBarsParent)
            .GetComponent<TaskBar>();

        task.taskIdentifier = lastTaskIndexer;
        taskBar.ToDoBarIndexer = GetHigherTaskBarIndexer();
        taskBar.Init(task);
        taskBars.Add(taskBar);

        lastTaskIndexer++;

        if (save)
            SaveAndLoad.Save();

        OrderTaskBars();
    }

    private int GetHigherTaskBarIndexer()
    {
        int higherIndexer = 0;
        for (int i = 0; i < taskBars.Count; i++)
        {
            TaskBar taskBar = taskBars[i];
            if (taskBar.ToDoBarIndexer >  higherIndexer)
                higherIndexer = taskBar.ToDoBarIndexer;
        }

        return higherIndexer;
    }

    public void Load(List<Task> curTasks)
    {
        if (curTasks == null) return;

        for (int i = 0; i < curTasks.Count; i++)
        {
            CreateTaskBar(curTasks[i], false);
        }

        ShowTaskBars();
        OrderTaskBars();
    }

    public List<Task> GetTasksOfTaskBars(List<TaskBar> taskBars)
    {
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < taskBars.Count; i++)
        {
            tasks.Add(taskBars[i].Task);
        }

        return tasks;
    }

    public SaveObject GetSaveObject()
    {
        SaveObject saveObject = new SaveObject();

        saveObject.tasks = GetTasksOfTaskBars(taskBars);

        return saveObject;
    }
}
