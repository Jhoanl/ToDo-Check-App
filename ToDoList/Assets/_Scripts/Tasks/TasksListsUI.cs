using Saving;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    }

    public void Initialize(List<TasksList> tasksLists)
    {
        Clear();

        Populate(tasksLists);
    }

    public void Seek(List<TasksList> tasksLists)
    {
        Clear();

        Populate(tasksLists);
    }

    private void Populate(List<TasksList> tasksLists)
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
        Debug.Log("Edit task Lists");
    }

    private void CreateTaskList()
    {
        Debug.Log("Create task list");

        //Create New Task List
        TasksList tasksList = new TasksList();
        tasksList.taskListName = "New";
        tasksList.tasks = new List<Task>();

        InstantiateTaskList(tasksList);

        //Save

        Save();

        //Select
    }

    private void InstantiateTaskList(TasksList tasksLists, bool autoSetName = true)
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

    private List<TasksList> GetTaskLists()
    {
        List<TasksList> taskLists = new List<TasksList>();

        for (int i = 0; i < taskListButtons.Count; i++)
        {
            taskLists.Add(taskListButtons[i].TasksList);
        }

        return taskLists;
    }

    public void Save()
    {
        GameManager.Instance.TaskLists = GetTaskLists();

        SaveAndLoad.Save();
    }


}
