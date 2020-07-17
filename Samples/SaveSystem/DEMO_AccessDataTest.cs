using UnityEngine;
using TMPro;
using GameDevStack.SaveSystem;

namespace GameDevStack.DEMO_AccessDataTest
{
    public class DEMO_AccessDataTest : MonoBehaviour
    {
        
        [SerializeField] private TextMeshProUGUI m_DebugText = null;

        public bool m_Booling = false;
        public Vector3 m_Position = Vector3.zero;
        public Vector3 m_Rotation = Vector3.zero;
        public string m_name = "toto";

        private float m_Score = 0;
        private int m_LevelNbr = 0;
        

        private void Start()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            m_DebugText.text = "score = " + m_Score + " | lvlNbr = " + m_LevelNbr;
        }
        
        public void AddScore(bool addScore)
        {
            if (addScore)
                m_Score += 0.5f;
            else
                m_Score -= 0.5f;

            UpdateText();
        }
        public void AddLevel(bool addLevel)
        {
            if (addLevel)
                m_LevelNbr ++;
            else
                m_LevelNbr --;

            UpdateText();
        }
        public void On_Save()
        {
            SaveSystemManager.Instance.SaveInt(this, SaveSystemManager.GetVariableName(() => m_LevelNbr), m_LevelNbr);
            SaveSystemManager.Instance.SaveFloat(this, SaveSystemManager.GetVariableName(() => m_Score), m_Score);
            SaveSystemManager.Instance.SaveBool(this, SaveSystemManager.GetVariableName(() => m_Booling), m_Booling);
            SaveSystemManager.Instance.SaveVector3(this, SaveSystemManager.GetVariableName(() => m_Position), m_Position);
            SaveSystemManager.Instance.SaveVector3(this, SaveSystemManager.GetVariableName(() => m_Rotation), m_Rotation);
            SaveSystemManager.Instance.SaveString(this, SaveSystemManager.GetVariableName(() => m_name), m_name);

            SaveSystemManager.Instance.SaveAllDataInFile();
        }
        public void On_Load()
        {
            // SaveSystemManager.s_instance.TestLoadInt(this, "m_levelNbr", ref m_levelNbr);

            if (SaveSystemManager.Instance.SaveContains(this, SaveSystemManager.GetVariableName(() => m_LevelNbr), DataType.Int))
                m_LevelNbr = SaveSystemManager.Instance.LoadInt(this, SaveSystemManager.GetVariableName(() => m_LevelNbr));

            // if (SaveSystemManager.s_instance.SaveContains(this, "m_score", DataType.Float))
            //     m_score = SaveSystemManager.s_instance.LoadFloat(this, "m_score");

            // m_booling = SaveSystemManager.s_instance.LoadBool(this, "m_booling");

            if (SaveSystemManager.Instance.SaveContains(this, SaveSystemManager.GetVariableName(() => m_Position), DataType.Vector3))
                m_Position = SaveSystemManager.Instance.LoadVector3(this, SaveSystemManager.GetVariableName(() => m_Position));

            m_Rotation = SaveSystemManager.Instance.LoadVector3(this, SaveSystemManager.GetVariableName(() => m_Rotation));

            // m_name = SaveSystemManager.s_instance.LoadString(this, "m_name");

            UpdateText();
        }
        public void On_DeleteSave()
        {
            SaveSystemManager.Instance.DeleteSaveDataFile();
            UpdateText();
        }
    }
}