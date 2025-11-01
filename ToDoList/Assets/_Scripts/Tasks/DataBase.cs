using System.Collections.Generic;
using Saving;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public static List<TasksList> taskLists;
    public static TasksList selectedTaskList;

    [Header("Debug")]
    [SerializeField] private SaveObject saveObject;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (Time.frameCount % 120 == 0)
            saveObject = GetSaveObject();
    }

    public static void CreateTaskList(TasksList taskList)
    {
        taskLists.Add(taskList);

        for (int i = 0; i < taskLists.Count; i++)
        {
            taskLists[i].identifier = i;
        }

        SaveAndLoad.Save();
    }

    public static void DeleteTaskList(TasksList taskList)
    {
        taskLists.RemoveAt(taskList.identifier);

        SaveAndLoad.Save();
    }

    public static void SetTaskList(int index)
    {
        selectedTaskList = taskLists[index];
    }

    public static void UpdateTasksOfSelected(List<Task> tasks)
    {
        selectedTaskList.tasks = tasks;
    }

    public static void Load(SaveObject savedObject)
    {
        taskLists = savedObject.tasksLists;

        if(taskLists == null)
            taskLists = new List<TasksList>();

        GameUI.instance.TasksUI.LastInput = savedObject.lastInput;
        GameManager.Instance.Load(savedObject.tasks, savedObject.tasksLists);
    }

    public static SaveObject GetSaveObject()
    {
        SaveObject saveObject = new SaveObject();

        saveObject.lastInput = GameUI.instance.TasksUI.LastInput;

        if (selectedTaskList != null)
            selectedTaskList.tasks = GameManager.Instance.GetTasksOfCurTaskBars();

        saveObject.tasksLists = taskLists;

        return saveObject;
    }
}

[System.Serializable]
public class TasksList
{
    public int identifier = 0;
    public string taskListName;
    public int spriteName;

    public List<Task> tasks;

    public List<Task> GetTasksDone()
    {
        List < Task > tasksDone = new List <Task> ();   
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].isCompleted)
                tasksDone.Add (tasks[i]);
        }

        return tasksDone;   
    }

    public List<Task> GetTasksPending()
    {
        List<Task> tasksPending = new List<Task>();
        for (int i = 0; i < tasks.Count; i++)
        {
            if (!tasks[i].isCompleted)
                tasksPending.Add(tasks[i]);
        }

        return tasksPending;
    }

    public int GetTasksAmount()
    {
        return tasks.Count;
    }
}
