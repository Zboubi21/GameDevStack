using UnityEngine;
using GameDevStack.CommonEnums;
using Sirenix.OdinInspector;

namespace GameDevStack.Animation
{
    public class CustomTransformAnimation : MonoBehaviour
    {
        [Header("Common parameters")]
        [SerializeField] private bool m_ActiveAtStart = true;
        [SerializeField] protected AnimationType m_AnimationType = AnimationType.OneShot;
        [SerializeField] protected SpeedType m_SpeedType = SpeedType.Time;
        [SerializeField] protected SpaceType m_SpaceType = SpaceType.Local;
        [SerializeField] protected Vector3 m_Target = new Vector3();
        [SerializeField] protected float m_speedValue = 1;
        [SerializeField] protected AnimationCurve m_Curve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));

        // [TabGroup("Position")] public Test m_test1;
        // [TabGroup("Rotation")] public Test m_test2;
        // [TabGroup("Scale")] public Test m_test3;
        // [System.Serializable] public class Test
        // {
        //     [Title("Yo")]
        //     public SpaceType m_SpaceType = SpaceType.Local;
        // }

        // [Header("Animations")]
        // [SerializeField] private Vector3AnimationParameters m_PositionAnimation;
        // [SerializeField] private Vector3AnimationParameters m_RotationAnimation;
        // [SerializeField] private Vector3AnimationParameters m_ScaleAnimation;

        protected AnimationData m_AnimData;

        private void Start()
        {
            if (m_ActiveAtStart)
                StartAnim();
        }

        public virtual void StartAnim()
        {
            StopAnim();
        }
        public void StopAnim()
        {
            CustomAnimationManager.StopAnimation(m_AnimData);
        }
    }
}