using UnityEngine;

namespace GameDevStack.Demos
{
    [RequireComponent(typeof(Rigidbody))]
    public class DEMO_BoatController : MonoBehaviour
    {
        [Header("Movements")]
        [SerializeField] private float m_MaxMovementSpeed = 7.5f;
        [SerializeField] private float m_AccelerationMovementSpeed = 2.5f;
        [SerializeField] private float m_DecelerationMovementSpeed = 2.5f;

        [Header("Rotations")]
        [SerializeField] private float m_MaxRotationSpeed = 1.5f;
        [SerializeField] private float m_AccelerationRotationSpeed = 0.25f;
        [SerializeField] private float m_DecelerationRotationSpeed = 0.25f;

        private Rigidbody m_Rigidbody;
        private Vector2 m_Inputs;
        private Vector2 m_InputsNormalized;
        private float m_CurrentMovementSpeed;
        private float m_CurrentMovementVelocity;
        private float m_CurrentRotationSpeed;
        private float m_CurrentRotationVelocity;
        private Vector3 m_RelativePos;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            m_Inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            m_InputsNormalized = m_Inputs.normalized;

            float targetRotationSpeed = Mathf.Lerp(0, m_MaxRotationSpeed, GetMaxAbsInput(false));
            float targetMovementSpeed = Mathf.Lerp(0, m_MaxMovementSpeed, GetMaxAbsInput(false));

            float accelerationDecelerationRotationSpeed = m_CurrentRotationSpeed < targetRotationSpeed ? m_AccelerationRotationSpeed : m_DecelerationRotationSpeed;
            float accelerationDecelerationMovementSpeed = m_CurrentMovementSpeed < targetMovementSpeed ? m_AccelerationMovementSpeed : m_DecelerationMovementSpeed;

            m_CurrentRotationSpeed = Mathf.SmoothDamp(m_CurrentRotationSpeed, targetRotationSpeed, ref m_CurrentRotationVelocity, accelerationDecelerationRotationSpeed);
            m_CurrentMovementSpeed = Mathf.SmoothDamp(m_CurrentMovementSpeed, targetMovementSpeed, ref m_CurrentMovementVelocity, accelerationDecelerationMovementSpeed);
        }

        private void FixedUpdate()
        {
            Vector3 relativePos = HasInput() ? new Vector3(m_InputsNormalized.x, 0, m_InputsNormalized.y) : m_RelativePos;
            m_RelativePos = relativePos;

            if (relativePos != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
                Quaternion currentRotation = Quaternion.Slerp(transform.rotation, targetRotation, m_CurrentRotationSpeed * Time.deltaTime); // Ne pas utiliser le Slerp !
                m_Rigidbody.MoveRotation(currentRotation);
            }

            m_Rigidbody.MovePosition(m_Rigidbody.position + transform.forward * m_CurrentMovementSpeed * Time.deltaTime);
        }

        private float GetMaxAbsInput(bool normalized)
        {
            Vector2 inputs = normalized ? m_InputsNormalized : m_Inputs;
            float xInput = Mathf.Abs(inputs.x);
            float yInput = Mathf.Abs(inputs.y);
            return xInput > yInput ? xInput : yInput;
        }

        private bool HasInput() => Mathf.Abs(m_Inputs.x) > 0 || Mathf.Abs(m_Inputs.y) > 0;
    }
}