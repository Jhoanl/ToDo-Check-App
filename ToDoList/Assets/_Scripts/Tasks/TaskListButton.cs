using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskListButton : MonoBehaviour
{
    private Button button;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI taskListNameText;
    [SerializeField] private TextMeshProUGUI tasksAmountText;
    [SerializeField] private TextMeshProUGUI tasksAmountDoneText;
    [SerializeField] private TextMeshProUGUI tasksAmountPendingText;

    [Header("Buttons")]
    [SerializeField] private Button editButton;
    [SerializeField] private Button deleteButton;

    private TasksListsUI tasksListsUI;
    private TasksList tasksList;

    public TasksList TasksList { get => tasksList; set => tasksList = value; }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        editButton.onClick.AddListener(OnEditButton);
        deleteButton.onClick.AddListener(OnDeletteButton);
    }

    public void Initialize(TasksListsUI tasksListsUI, TasksList tasksList)
    {
        this.tasksListsUI = tasksListsUI;
        this.tasksList = tasksList;

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (tasksList == null)
            return;

        taskListNameText.text = tasksList.taskListName;
        tasksAmountText.text = tasksList.GetTasksAmount().ToString();
        tasksAmountDoneText.text = tasksList.GetTasksDone().Count.ToString();
        tasksAmountPendingText.text = tasksList.GetTasksPending().Count.ToString();
    }

    private void OnClick()
    {
        tasksListsUI.SelectTaskList(this);
    }

    private void OnEditButton()
    {
        tasksListsUI.EditTaskList(this);
    }

    private void OnDeletteButton()
    {
        tasksListsUI.DeleteTaskList(this);
    }

}
