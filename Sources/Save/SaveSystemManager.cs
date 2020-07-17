using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq.Expressions;
using UnityEngine;
using Sirenix.OdinInspector;
using GameDevStack.Patterns;
using GameDevStack.CommonEnums;

namespace GameDevStack.SaveSystem
{
    public class SaveSystemManager : SingletonSerializedMonoBehaviour<SaveSystemManager>
    {

#region Debug Dictionnary Variables
        [Header("Debug"), Tooltip("Show the SaveData for easier debugging?")]
        [SerializeField] private bool m_UseDebugDictionnary = false;

        [ShowIf("m_UseDebugDictionnary")]
        [SerializeField] private Dictionary<string, int> m_DebugSavedInt = new Dictionary<string, int>();
        [ShowIf("m_UseDebugDictionnary")]
        [SerializeField] private Dictionary<string, float> m_DebugSavedFloat = new Dictionary<string, float>();
        [ShowIf("m_UseDebugDictionnary")]
        [SerializeField] private Dictionary<string, bool> m_DebugSavedBool = new Dictionary<string, bool>();
        [ShowIf("m_UseDebugDictionnary")]
        [SerializeField] private Dictionary<string, Vector3> m_DebugSavedVector3 = new Dictionary<string, Vector3>();
        [ShowIf("m_UseDebugDictionnary")]
        [SerializeField] private Dictionary<string, string> m_DebugSavedString = new Dictionary<string, string>();

        private void DebugDataSaved()
        {
            if (!m_UseDebugDictionnary) return;
            m_DebugSavedInt = m_CurrentData.m_Int;
            m_DebugSavedFloat = m_CurrentData.m_Float;
            m_DebugSavedBool = m_CurrentData.m_Bool;

    #region Debug Vector3
            int currentInt = 1;
            Vector3 value = new Vector3();
            m_DebugSavedVector3.Clear();
            foreach (var floatValue in m_CurrentData.m_Vector3)
            {
                if (currentInt % 3 == 1) 
                    value.x = floatValue.Value;
                if (currentInt % 3 == 2) 
                    value.y = floatValue.Value;
                if (currentInt % 3 == 0)
                {
                    value.z = floatValue.Value;
                    m_DebugSavedVector3.Add(floatValue.Key.Substring(0, floatValue.Key.Length - 1), value);
                }
                currentInt ++;
            }
    #endregion

            m_DebugSavedString = m_CurrentData.m_String;
        }

    [SerializeField] private DebugType m_DebugType = DebugType.LogWarning;

    private void WriteDebugMessage(DebugType type, string message)
    {
        #if UNITY_EDITOR
        switch (type)
        {
            case DebugType.None:
                // Don't write message.
            break;
            case DebugType.Log:
                Debug.Log(message);
            break;
            case DebugType.LogWarning:
                Debug.LogWarning(message);
            break;
            case DebugType.LogError:
                Debug.LogError(message);
            break;
        }
        #endif
    }
#endregion
        
        private SaveData m_CurrentData = new SaveData();

#region Manage Saved File
        [HorizontalGroup("Datas1"), Button("Save"), GUIColor(0, 1, 0)]
        public void SaveAllDataInFile() // Call SaveAllDataInFile when the player quit the game or save it
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(GetSavePath());
            bf.Serialize(file, m_CurrentData);
            file.Close();
        }
        [HorizontalGroup("Datas2"), Button("Load"), GUIColor(1, 1, 0)]
        public void LoadAllDataFromFile()
        {
            if(File.Exists(GetSavePath()))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(GetSavePath(), FileMode.Open);
                SaveData data = bf.Deserialize(file) as SaveData;
                file.Close();
                LoadAllData(data);
                On_LoadData?.Invoke();
            }
            else
                On_CanNotLoadData?.Invoke();
        }
        private void LoadAllData(SaveData data)
        {
            m_CurrentData.m_Int = data.m_Int;
            m_CurrentData.m_Float = data.m_Float;
            m_CurrentData.m_Bool = data.m_Bool;
            m_CurrentData.m_Vector3 = data.m_Vector3;
            m_CurrentData.m_String = data.m_String;
            DebugDataSaved();
        }
        [HorizontalGroup("Datas1"), Button("Clear"), GUIColor(1, 0.5f, 0)]
        public void ClearData()
        {
            m_DebugSavedInt.Clear();
            m_DebugSavedFloat.Clear();
            m_DebugSavedBool.Clear();
            m_DebugSavedVector3.Clear();
            m_DebugSavedString.Clear();
        }
        [HorizontalGroup("Datas2"), Button("Delete"), GUIColor(1, 0, 0)]
        public void DeleteSaveDataFile()
        {
            File.Delete(GetSavePath());
            m_CurrentData = new SaveData();
            DebugDataSaved();
        }
#endregion

#region Save And Load Functions
    
    #region Int
        public void SaveInt(MonoBehaviour script, string key, int value)
        {
            string path = ConvertKeyToFilePath(script, key);
            if (m_CurrentData.m_Int.ContainsKey(path))
                m_CurrentData.m_Int.Remove(path);
            m_CurrentData.m_Int.Add(path, value);
            DebugDataSaved();
        }
        public int LoadInt(MonoBehaviour script, string key)
        {
            string path = ConvertKeyToFilePath(script, key);
            int value;
            if (m_CurrentData.m_Int.TryGetValue(path, out value))
                return value;

            ShowDebugMessage(path);
            return 0;
        }
        public void SaveInts(MonoBehaviour script, string key, int[] values)
        {
            if (values == null)
                return;
            
            for (int i = 0, l = values.Length; i < l; ++i)
            {
                string path = ConvertKeyToFilePath(script, key, i);
                if (m_CurrentData.m_Int.ContainsKey(path))
                    m_CurrentData.m_Int.Remove(path);

                m_CurrentData.m_Int.Add(path, values[i]);
            }
            DebugDataSaved();
        }
        public int[] LoadInts(MonoBehaviour script, string key, int length)
        {
            int[] values = new int[length];

            for (int i = 0, l = length; i < l; ++i)
            {
                string path = ConvertKeyToFilePath(script, key, i);

                int value;
                if (m_CurrentData.m_Int.TryGetValue(path, out value))
                    values[i] = value;
                ShowDebugMessage(path);
            }
            return values;
        }
    #endregion

    #region Float
        public void SaveFloat(MonoBehaviour script, string key, float value)
        {
            string path = ConvertKeyToFilePath(script, key);
            if (m_CurrentData.m_Float.ContainsKey(path))
                m_CurrentData.m_Float.Remove(path);
            m_CurrentData.m_Float.Add(path, value);
            DebugDataSaved();
        }
        public float LoadFloat(MonoBehaviour script, string key)
        {
            string path = ConvertKeyToFilePath(script, key);
            float value;
            if (m_CurrentData.m_Float.TryGetValue(path, out value))
                return value;

            ShowDebugMessage(path);
            return 0;
        }
    #endregion
    
    #region Bool
        public void SaveBool(MonoBehaviour script, string key, bool value)
        {
            string path = ConvertKeyToFilePath(script, key);
            if (m_CurrentData.m_Bool.ContainsKey(path))
                m_CurrentData.m_Bool.Remove(path);
            m_CurrentData.m_Bool.Add(path, value);
            DebugDataSaved();
        }
        public bool LoadBool(MonoBehaviour script, string key)
        {
            string path = ConvertKeyToFilePath(script, key);
            bool value;
            if (m_CurrentData.m_Bool.TryGetValue(path, out value))
                return value;

            ShowDebugMessage(path);
            return false;
        }
    #endregion

    #region Vector3
        public void SaveVector3(MonoBehaviour script, string key, Vector3 value)
        {
            string path = ConvertKeyToFilePath(script, key);

            if (m_CurrentData.m_Vector3.ContainsKey(path + "x"))
                m_CurrentData.m_Vector3.Remove(path + "x");
            if (m_CurrentData.m_Vector3.ContainsKey(path + "y"))
                m_CurrentData.m_Vector3.Remove(path + "y");
            if (m_CurrentData.m_Vector3.ContainsKey(path + "z"))
                m_CurrentData.m_Vector3.Remove(path + "z");

            m_CurrentData.m_Vector3.Add(path + "x", value.x);
            m_CurrentData.m_Vector3.Add(path + "y", value.y);
            m_CurrentData.m_Vector3.Add(path + "z", value.z);

            DebugDataSaved();
        }
        public Vector3 LoadVector3(MonoBehaviour script, string key)
        {
            string path = ConvertKeyToFilePath(script, key);

            float xValue;
            float yValue;
            float zValue;
            
            if (m_CurrentData.m_Vector3.TryGetValue(path + "x", out xValue))
            {
                m_CurrentData.m_Vector3.TryGetValue(path + "y", out yValue);
                m_CurrentData.m_Vector3.TryGetValue(path + "z", out zValue);
                return new Vector3(xValue, yValue, zValue);
            }

            ShowDebugMessage(path);
            return Vector3.zero;
        }
    #endregion

    #region String
        public void SaveString(MonoBehaviour script, string key, string value)
        {
            string path = ConvertKeyToFilePath(script, key);
            if (m_CurrentData.m_String.ContainsKey(path))
                m_CurrentData.m_String.Remove(path);
            m_CurrentData.m_String.Add(path, value);
            DebugDataSaved();
        }
        public string LoadString(MonoBehaviour script, string key)
        {
            string path = ConvertKeyToFilePath(script, key);
            string value;
            if (m_CurrentData.m_String.TryGetValue(path, out value))
                return value;

            ShowDebugMessage(path);
            return "";
        }
    #endregion

        public bool SaveContains(MonoBehaviour script, string key, DataType dataType)
        {
            string path = ConvertKeyToFilePath(script, key);
            bool dataContainsKey = false;

            switch (dataType)
            {
                case DataType.Int:
                    dataContainsKey = m_CurrentData.m_Int.ContainsKey(path);
                break;
                case DataType.Float:
                    dataContainsKey = m_CurrentData.m_Float.ContainsKey(path);
                break;
                case DataType.Bool:
                    dataContainsKey = m_CurrentData.m_Bool.ContainsKey(path);
                break;
                case DataType.Vector3:
                    if(m_CurrentData.m_Vector3.ContainsKey(path + "x") && m_CurrentData.m_Vector3.ContainsKey(path + "y") && m_CurrentData.m_Vector3.ContainsKey(path + "z"))
                        dataContainsKey = true;
                break;
                case DataType.String:
                    dataContainsKey = m_CurrentData.m_String.ContainsKey(path);
                break;
            }
            return dataContainsKey;
        }
        public bool SaveContains(MonoBehaviour script, string key, DatasType dataType, int length)
        {
            string path;
            bool dataContainsKey = true;

            switch (dataType)
            {
                case DatasType.Ints:
                    for (int i = 0, l = length; i < l; ++i)
                    {
                        path = ConvertKeyToFilePath(script, key, i);
                        bool containsKey = m_CurrentData.m_Int.ContainsKey(path);
                        if (!containsKey)
                            dataContainsKey = false;
                    }
                break;
            }
            return dataContainsKey;
        }

        public static string GetVariableName<T>(Expression<Func<T>> expr)
        {
            var body = (MemberExpression)expr.Body;
            return body.Member.Name;
        }

#endregion
    
#region Utilities
        private string GetSavePath()
        {
            // Debug.Log("Application.persistentDataPath = " + Application.persistentDataPath);
            return Application.persistentDataPath + "/playerSave.dat";
        }
        private string ConvertKeyToFilePath(MonoBehaviour script, string key)
        {
            return script.GetType() + "/" + key;
        }
        private string ConvertKeyToFilePath(MonoBehaviour script, string key, int i)
        {
            return script.GetType() + "/" + key + "." + i;
        }
        private void ShowDebugMessage(string path)
        {
            WriteDebugMessage(m_DebugType, "The path value " + path + " dosen't exist!");
        }
#endregion

#region Observer Pattern
        public delegate void DataIsLoaded();
        public event DataIsLoaded On_LoadData;
        public delegate void DataIsNotLoaded();
        public event DataIsNotLoaded On_CanNotLoadData; // Essayer de juste passer une bool en argument de l'event DataIsLoaded plutôt que d'avoir 2 event !!!
#endregion

    }
}