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

    [Header("Buttons")]
    [SerializeField] private Button editButton;

    private TasksListsUI tasksListsUI;
    private TasksList tasksList;

    public TasksList TasksList { get => tasksList; set => tasksList = value; }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        editButton.onClick.AddListener(OnEditButton);
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
        tasksAmountText.text = tasksList.tasks.Count.ToString();
    }

    private void OnClick()
    {
        tasksListsUI.SelectTaskList(this);
    }

    private void OnEditButton()
    {
        tasksListsUI.EditTaskList(this);
    }
}
