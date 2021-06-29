using UnityEngine;

namespace GameDevStack.Demos
{
    public class DEMO_CameraController : MonoBehaviour
    {
        [SerializeField] private Transform m_FollowingTransform = null;

        private Vector3 m_OffsetPosition;

        private void Awake()
        {
            m_OffsetPosition = transform.position - m_FollowingTransform.transform.position;
        }

        private void LateUpdate()
        {
            Vector3 targetPosition = m_OffsetPosition + m_FollowingTransform.position;
            targetPosition.y = m_OffsetPosition.y;
            transform.position = targetPosition;
        }
    }
}