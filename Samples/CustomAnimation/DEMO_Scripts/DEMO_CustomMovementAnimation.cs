using System;
using UnityEngine;
using GameDevStack.Animation;

namespace GameDevStack.DEMO
{
    public class DEMO_CustomMovementAnimation : DEMO_CustomAnimation
    {

        [SerializeField] private Parameters m_FadeInAnim = null;
        [SerializeField] private Parameters m_FadeOutAnim = null;
        [Serializable] private class Parameters
        {
            public Vector3 m_Position = new Vector3();
            public float m_TimeToReachPos = 1;
            public AnimationCurve m_Curve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));
        }
        
        protected override void On_NeedToFadeIn()
        {
            m_AnimData = CustomAnimationManager.AnimPositionWithTime(transform, m_FadeInAnim.m_Position, m_FadeInAnim.m_TimeToReachPos).SetCurve(m_FadeInAnim.m_Curve);
        }
        protected override void On_NeedToFadeOut()
        {
            m_AnimData = CustomAnimationManager.AnimPositionWithTime(transform, m_FadeOutAnim.m_Position, m_FadeOutAnim.m_TimeToReachPos).SetCurve(m_FadeOutAnim.m_Curve);
        }
    }
}