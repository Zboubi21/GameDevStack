using UnityEngine;

namespace GameDevStack.Animation
{
    [RequireComponent(typeof(Transform))]
    public class CustomPositionAnimation : CustomTransformAnimation
    {
        private Vector3 m_LastValue;

        public override void StartAnim()
        {
            base.StartAnim();
            StartPosAnimationAccordingToSpeedType(m_Target);
        }

        private void StartPosAnimationAccordingToSpeedType(Vector3 targetPos)
        {
            m_LastValue = transform.position;
            if (m_SpeedType == SpeedType.Time)
                m_AnimData = CustomAnimationManager.AnimPositionWithTime(transform, targetPos, m_speedValue, m_SpaceType).SetCurve(m_Curve).SetOnComplete(OnPosAnimIsFinished);
            else
                m_AnimData = CustomAnimationManager.AnimPositionWithSpeed(transform, targetPos, m_speedValue, m_SpaceType).SetCurve(m_Curve).SetOnComplete(OnPosAnimIsFinished);
        }

        private void OnPosAnimIsFinished()
        {
            if (m_AnimationType == AnimationType.PingPong)
                StartPosAnimationAccordingToSpeedType(m_LastValue);
        }
    }
}