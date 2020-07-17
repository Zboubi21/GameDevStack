using System;
using System.Collections.Generic;

namespace GameDevStack.SaveSystem
{
    public enum DataType
    {
        Int,
        Float,
        Bool,
        Vector3,
        String,
    }
    public enum DatasType
    {
        Ints,
    }

    [Serializable]
    public class SaveData
    {
        public Dictionary<string, int> m_Int = new Dictionary<string, int>();
        public Dictionary<string, float> m_Float = new Dictionary<string, float>();
        public Dictionary<string, bool> m_Bool = new Dictionary<string, bool>();
        public Dictionary<string, float> m_Vector3 = new Dictionary<string, float>();
        public Dictionary<string, string> m_String = new Dictionary<string, string>();
    }
}