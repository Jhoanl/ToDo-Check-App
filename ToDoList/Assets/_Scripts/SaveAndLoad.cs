using Saving;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Saving
{
    public class SaveAndLoad : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.K))
            {
                Save();
            }
        }
#endif

        public static void Save()
        {
            Debug.Log("Save");
            SaveObject saveObject = CreateSaveObject();

            SaveSystem.Save(saveObject);
        }

        public static bool Load()
        {
            Debug.Log("Load");
            SaveObject saveObject = SaveSystem.Load();

            if (saveObject != null)
            {
                SetSaveObject(saveObject);
                return true;
            }
            return false;
        }

        private static void SetSaveObject(SaveObject savedObject)
        {
            //To Save old version data
            if (savedObject.tasks.Count > 0)
            {
                savedObject.tasksLists = new List<TaskList>();

                TaskList tasksList = new TaskList();
                tasksList.tasks = savedObject.tasks;
                tasksList.taskListName = "Main";
                tasksList.identifier = 0;

                savedObject.tasks = new List<Task>();
                savedObject.tasksLists.Add(tasksList);

                FunctionTimer.Create(() => Save(), .1f);
            }

            for (int i = 0; i < savedObject.tasksLists.Count; i++)
            {
                savedObject.tasksLists[i].identifier = i;
            }

            DataBase.Load(savedObject);
        }

        private static SaveObject CreateSaveObject()
        {
            SaveObject saveObject = DataBase.GetSaveObject();

            saveObject.lastInput =GameUI.instance.TasksUI.LastInput;

            //Debug.Log("Create SaveObject Needed");
            return saveObject;
        }
    }

    [System.Serializable]
    public class SaveObject
    {
        public List<TaskList> tasksLists;

        public List<Task> tasks;
        public string lastInput;
    }
}