using UnityEngine;
using GameDevStack.Patterns;

namespace GameDevStack.Physics
{
    [RequireComponent(typeof(MeshFilter))]
    public class WaterController : MonoBehaviour
    {
        private MeshFilter m_MeshFilter;

        private void Awake()
        {
            m_MeshFilter = GetComponent<MeshFilter>();
        }

        private void Update()
        {
            Vector3[] vertices = m_MeshFilter.mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].y = WaveManager.Instance.GetWaveHeight(transform.position.x + vertices[i].x);
            }
            m_MeshFilter.mesh.vertices = vertices;
            m_MeshFilter.mesh.RecalculateNormals();
        }
    }
}