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
    [Header("UI")]
    [SerializeField] private TasksUI tasksUI;
    [SerializeField] private TasksListsUI taskListsUI;
    [SerializeField] private GameObject tasksListsPanel;
    [SerializeField] private GameObject tasksPanel;
    //[SerializeField] private GameObject taskLists;
    //[SerializeField] private GameObject taskListPrefab;

    //[SerializeField] private Transform taskListsParent;


    #region Getters

    public ModalPanel CurModalPanel { get => modalPanel; }
    public InputTextPanel InputTextPanel { get => inputTextPanel; }
    public TasksUI TasksUI { get => tasksUI; set => tasksUI=value; }
    public TasksListsUI TaskListsUI { get => taskListsUI; set => taskListsUI = value; }
    #endregion

    private void Awake()
    {
        instance = this;
    }

    private void ShowCompletedTasks()
    {
        GameManager.Instance.ShowCompletedTasks = !GameManager.Instance.ShowCompletedTasks;
        //SetUITasks();
    }

    public void SetScreen(int screenId)
    {
        tasksListsPanel.SetActive(screenId == 0);
        tasksPanel.SetActive(screenId == 1);
    }

}
