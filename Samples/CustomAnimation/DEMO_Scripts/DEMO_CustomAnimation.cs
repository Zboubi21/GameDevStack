using UnityEngine;
using GameDevStack.Animation;
using GameDevStack.CommonEnums;

namespace GameDevStack.DEMO
{
    public class DEMO_CustomAnimation : MonoBehaviour
    {
        [SerializeField] protected StartType m_StartType;

        protected AnimationData m_AnimData;
        protected bool m_NeedToFadeIn = true;

        private void Awake()
        {
            m_NeedToFadeIn = m_StartType == StartType.FromValue ? true : false;
        }
        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
                StartAnimation();
        }

        public virtual void StartAnimation()
        {
            CustomAnimationManager.StopAnimation(m_AnimData);
            if (m_NeedToFadeIn)
                On_NeedToFadeIn();
            else
                On_NeedToFadeOut();
            m_NeedToFadeIn =! m_NeedToFadeIn;
        }
        protected virtual void On_NeedToFadeIn() { }
        protected virtual void On_NeedToFadeOut() { }
    }
}