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
            GameManager.Instance.Load(savedObject.tasks);
            GameUI.instance.LastInput = savedObject.lastInput;
            //Debug.Log("SetSaveObject Needed");
        }

        private static SaveObject CreateSaveObject()
        {
            SaveObject saveObject = GameManager.Instance.GetSaveObject();

            saveObject.lastInput =GameUI.instance.LastInput;

            //Debug.Log("Create SaveObject Needed");
            return saveObject;
        }
    }

    [System.Serializable]
    public class SaveObject
    {
        public List<Task> tasks;
        public string lastInput;
    }
}