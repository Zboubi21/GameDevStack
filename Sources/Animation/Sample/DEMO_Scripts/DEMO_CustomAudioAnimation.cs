using System;
using UnityEngine;
using GameDevStack.Animation;

namespace GameDevStack.DEMO
{
    public class DEMO_CustomAudioAnimation : DEMO_CustomAnimation
    {

        [SerializeField] private Parameters m_FadeInAnim = null;
        [SerializeField] private Parameters m_FadeOutAnim = null;
        [Serializable] private class Parameters
        {
            [Range(0, 1)] public float m_Volume = 0.5f;
            public float m_TimeToReachPos = 1;
            public AnimationCurve m_Curve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));
        }

        private AudioSource m_AudioSource;

        private void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        protected override void On_NeedToFadeIn()
        {
            if (m_AudioSource != null)
                m_AnimData = CustomAnimationManager.AnimFloatWithTime(m_AudioSource.volume, m_FadeInAnim.m_Volume, m_FadeInAnim.m_TimeToReachPos).SetCurve(m_FadeInAnim.m_Curve).SetOnUpdate(SetVolume);
        }
        protected override void On_NeedToFadeOut()
        {
            if (m_AudioSource != null)
                m_AnimData = CustomAnimationManager.AnimFloatWithTime(m_AudioSource.volume, m_FadeOutAnim.m_Volume, m_FadeOutAnim.m_TimeToReachPos).SetCurve(m_FadeOutAnim.m_Curve).SetOnUpdate(SetVolume);
        }
        
        private void SetVolume(float volume)
        {
            if (m_AudioSource != null)
                m_AudioSource.volume = volume;
        }
    }
}