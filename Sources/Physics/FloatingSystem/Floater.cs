using UnityEngine;

namespace GameDevStack.Physics
{
    public class Floater : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_Rigidbody = null;
        [SerializeField, Min(0)] private float m_DepthBeforeSubmerged = 1f;
        [SerializeField, Min(0)] private float m_DisplacementAmount = 3f;
        [SerializeField] private int m_FloaterCount = 1;
        [SerializeField] private float m_WaterDrag = 0.99f;
        [SerializeField] private float m_WaterAngularDrag = 0.5f;

        private void FixedUpdate()
        {
            m_Rigidbody.AddForceAtPosition(UnityEngine.Physics.gravity / m_FloaterCount, transform.position, ForceMode.Acceleration);

            float waveHeight = WaveManager.Instance.GetWaveHeight(transform.position.x);

            if (transform.position.y < waveHeight)
            {
                float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / m_DepthBeforeSubmerged) * m_DisplacementAmount;
                m_Rigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(UnityEngine.Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
                m_Rigidbody.AddForce(displacementMultiplier * -m_Rigidbody.velocity * m_WaterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
                m_Rigidbody.AddTorque(displacementMultiplier * -m_Rigidbody.angularVelocity * m_WaterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }
    }
}