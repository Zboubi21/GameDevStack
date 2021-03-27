using UnityEngine;
using GameDevStack.CommonEnums;
using TMPro;

namespace GameDevStack.Optimization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private string m_FpsTxt = "FPS ";
        [SerializeField] private ColorType m_FpsColor = ColorType.Yellow;
        [SerializeField] private float m_UpdateInterval = 0.5f;

        private float m_FramesAccumulated = 0f;
        private float m_FramesDrawnInTheInterval = 0f;
        private float m_TimeLeft;
        private TextMeshProUGUI m_Text;
        private int m_CurrentFPS;

        private void Start()
        {
            m_Text = GetComponent<TextMeshProUGUI>();
            m_TimeLeft = m_UpdateInterval;
        }

        private void Update()
        {
            m_FramesDrawnInTheInterval++;
            m_FramesAccumulated += Time.timeScale / Time.deltaTime;
            m_TimeLeft -= Time.deltaTime;

            if (m_TimeLeft <= 0.0)
            {
                m_CurrentFPS = (int)Mathf.Clamp(m_FramesAccumulated / m_FramesDrawnInTheInterval, 0, 300);
                m_Text.text = string.Concat(m_FpsTxt, "<color=", ConvertColorTypeToString(m_FpsColor), ">", m_CurrentFPS.ToString(), "</color>");
                m_FramesDrawnInTheInterval = 0;
                m_FramesAccumulated = 0f;
                m_TimeLeft = m_UpdateInterval;
            }
        }

        private string ConvertColorTypeToString(ColorType colorType)
        {
            string colorName;
            switch (colorType)
            {
                case ColorType.White:
                    colorName = "white";
                    break;
                case ColorType.Black:
                    colorName = "black";
                    break;
                case ColorType.Red:
                    colorName = "red";
                    break;
                case ColorType.Green:
                    colorName = "green";
                    break;
                case ColorType.Blue:
                    colorName = "blue";
                    break;
                case ColorType.Yellow:
                    colorName = "yellow";
                    break;
                case ColorType.Orange:
                    colorName = "orange";
                    break;
                case ColorType.Purple:
                    colorName = "purple";
                    break;
                default:
                    colorName = "white";
                    break;
            }
            return colorName;
        }
    }
}