using UnityEngine;
using GameDevStack.Patterns;

namespace GameDevStack.Physics
{
    public class WaveManager : SingletonMonoBehaviour<WaveManager>
    {
        [SerializeField] private float m_Amplitude = 1f;
        [SerializeField] private float m_Length = 2f;
        [SerializeField] private float m_Speed = 1f;
        [SerializeField] private float m_Offset = 0f;

        private void Update()
        {
            m_Offset += Time.deltaTime * m_Speed;
        }

        public float GetWaveHeight(float xPosition)
        {
            return m_Amplitude * Mathf.Sin(xPosition / m_Length + m_Offset);
        }
    }
}