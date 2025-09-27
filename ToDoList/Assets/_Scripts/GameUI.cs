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
    [SerializeField] private TasksUI tasksUI;


    #region Getters

    public ModalPanel CurModalPanel { get => modalPanel; }
    public InputTextPanel InputTextPanel { get => inputTextPanel; }
    public TasksUI TasksUI { get => tasksUI; set => tasksUI=value; }
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

}
