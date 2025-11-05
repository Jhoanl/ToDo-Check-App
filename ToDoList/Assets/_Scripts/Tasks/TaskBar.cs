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
    public bool IsCompleted
    {
        get => Task.isCompleted; set
        {
            Task.isCompleted=value;
            SetVisuals();
        }
    }

    public int TaskIdentifier
    {
        get => Task.taskIdentifier; set
        {
            Task.taskIdentifier=value;
        }
    }

    public int ToDoBarIndexer
    {
        get => toDoBarIndexer; set
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

        taskBarUI.OnTaskAction +=HandleTaskAction;

    }

    // Un solo método para manejar todas las acciones de botones
    private void HandleTaskAction(TaskBarUI.TaskActionType action)
    {
        switch (action)
        {
            case TaskBarUI.TaskActionType.Up:
                MoveTaskUp();
                break;

            case TaskBarUI.TaskActionType.Down:
                MoveTaskDown();
                break;

            case TaskBarUI.TaskActionType.Complete:
                CompleteTask();
                break;

            case TaskBarUI.TaskActionType.Delete:
                DeleteTask();
                break;

            case TaskBarUI.TaskActionType.Edit:
                EditTask();
                break;

            case TaskBarUI.TaskActionType.Copy:
                CopyTask();
                break;

            case TaskBarUI.TaskActionType.Paste:
                PasteTask();
                break;
            case TaskBarUI.TaskActionType.ColorChange:
                OnTaskBarChanged?.Invoke(this);
                break;
            default:
                Debug.LogWarning("Acción no manejada: " + action);
                break;
        }
    }

    private void PasteTask()
    {
        if (!TaskBarHandlers.hasTaskCopied)
        {
            return;
        }

        GameUI.instance.CurModalPanel.ShowConfirm("Pegar tarea", "¿Deseas sobreescribir esta tarea?",
            null, () =>
            {

                Init(TaskBarHandlers.GetTaskCopied());
                TaskBarHandlers.SetTaskPasted();

                OnTaskBarChanged?.Invoke(this);

            }, null);
    }

    private void CopyTask()
    {
        TaskBarHandlers.CopyTaskBar(task);
    }

    private void EditTask()
    {
        OnEditButtonClicked?.Invoke(this);
    }

    #region Movement

    private void MoveTaskUp()
    {

        Action moveAction = () =>
        {
            MoveTaskBar(true);
        };

        FunctionTimer.Create(() => { moveAction(); }, delayToDoAction);
        SetMovedVisuals();
    }

    private void MoveTaskDown()
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

    private void CompleteTask()
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

    private void DeleteTask()
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
