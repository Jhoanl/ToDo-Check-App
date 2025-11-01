using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskBar : MonoBehaviour
{
    public static event Action<TaskBar> OnEditButtonClicked;
    public static event Action<TaskBar> OnTaskBarChanged;

    private const float delayToDoAction = .15f;

    public event Action<TaskBar> OnUpButtonClicked;
    public event Action<TaskBar> OnDownButtonClicked;
    public event Action<TaskBar> OnCompleteButtonClicked;
    public event Action<TaskBar> OnDeleteButtonClicked;

    [SerializeField] private int toDoBarIndexer;

    [SerializeField] private Task task;
    [SerializeField] private TaskBarUI taskBarUI;

    [Space]


    private bool destroying = false;

    public Task Task { get => task; }
    public bool IsCompleted { get => Task.isCompleted; set
        {
            Task.isCompleted=value;
            SetVisuals();
        }
    }

    public int TaskIdentifier { get => Task.taskIdentifier; set
        {
            Task.taskIdentifier=value;
        } }

    public int ToDoBarIndexer { get => toDoBarIndexer; set
        {
            toDoBarIndexer=value;
            taskBarUI.TaskPriorityVisuals(toDoBarIndexer);
        }
    }

    public void Init(Task task)
    {
        this.task = task;
        IsCompleted = this.task.isCompleted;

        SetVisuals();
    }

    public void SetVisuals()
    {
        if (destroying) return;

        taskBarUI.SetVisuals(task, IsCompleted, ToDoBarIndexer);
    }

    private void Awake()
    {
        taskBarUI= GetComponent<TaskBarUI>();

        taskBarUI.OnCompleteButtonClicked += CompleteButtonClicked;
        taskBarUI.OnDeleteButtonClicked += DeleteButtonClicked;
        taskBarUI.OnEditButtonClicked += EditButtonClicked;

        taskBarUI.OnDownButtonClicked += DownButtonClicked;
        taskBarUI.OnUpButtonClicked += UpButtonClicked;
    }

    private void EditButtonClicked()
    {
        OnEditButtonClicked?.Invoke(this);
    }

    #region Movement

    private void UpButtonClicked()
    {

        Action moveAction = () =>
        {
            MoveTaskBar(true);
        };

        FunctionTimer.Create(() => { moveAction(); }, delayToDoAction);
        SetMovedVisuals();
    }

    private void DownButtonClicked()
    {
        Action moveAction = () =>
        {
            MoveTaskBar(false);
        };

        FunctionTimer.Create(() => { moveAction(); }, delayToDoAction);
        SetMovedVisuals();
    }

    private void MoveTaskBar(bool up)
    {
        if (up)
        {
            OnUpButtonClicked?.Invoke(this);
        }
        else
        {
            OnDownButtonClicked?.Invoke(this);
        }

        GameManager.Instance.MoveTaskBar(this, up);

        OnTaskBarChanged?.Invoke(this);

        taskBarUI.TaskPriorityVisuals(toDoBarIndexer);
    }

    private void SetMovedVisuals()
    {
        taskBarUI.TriggerColorChange(Color.blue);
    }

    #endregion

    #region State

    private void CompleteButtonClicked()
    {
        if (destroying)
            return;

        Color animationColor = IsCompleted ? Color.yellow : Color.green;
        taskBarUI.TriggerColorChange(animationColor);

        Action completeAction = () =>
        {
            OnCompleteButtonClicked?.Invoke(this);
            IsCompleted = !IsCompleted;

            OnTaskBarChanged?.Invoke(this);
        };

        FunctionTimer.Create(completeAction, delayToDoAction);
    }

    private void DeleteButtonClicked()
    {
        if (destroying)
            return;

        Action deleteAction = () =>
        {
            OnDeleteButtonClicked?.Invoke(this);
            IsCompleted = false;

            GameManager.Instance.DeleteTaskBar(this.Task);

            OnTaskBarChanged?.Invoke(this);
            destroying = true;
        };

        taskBarUI.TriggerColorChange(Color.red);
        FunctionTimer.Create(deleteAction, delayToDoAction);
    }

    #endregion

    public static List<Task> GetTasksOfTaskBars(List<TaskBar> taskBars)
    {
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < taskBars.Count; i++)
        {
            tasks.Add(taskBars[i].Task);
        }

        return tasks;
    }

}
