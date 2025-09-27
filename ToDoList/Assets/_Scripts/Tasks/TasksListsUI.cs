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

    private List<TaskListButton> taskListButtons;

    private void Awake()
    {
        taskListButtons = new List<TaskListButton>();
        createListButton.onClick.AddListener(CreateTaskList);
    }

    public void Initialize(List<TasksList> tasksLists)
    {
        Clear();
    }

    public void Seek(List<TasksList> tasksLists)
    {
        Clear();
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

        //Load Set Game Mananger tasks
    }

    public void EditTaskList(TaskListButton taskListButton)
    {

    }

    private void CreateTaskList()
    {
        
    }
}
