using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameDevStack.Animation
{
    [System.Serializable]
    public class AnimationData
    {
#region Private Variables
        private AnimationDataOptional m_Optional = new AnimationDataOptional();
        private Vector3 m_Vector3Data;
        private Quaternion m_QuaternionData;
        private float m_FloatData;
        private Color m_ColorData;
        private float m_Delay = 0;
        private AnimationCurve m_Curve;
#endregion

#region Public Get
        public AnimationDataOptional Optional => m_Optional;
        public float Delay => m_Delay;
        public AnimationCurve Curve => m_Curve;
#endregion

#region Public Set
        public Vector3 Vector3Data { set => m_Vector3Data = value; }
        public Quaternion QuaternionData { set => m_QuaternionData = value; }
        public float FloatData { set => m_FloatData = value; }
        public Color ColorData { set => m_ColorData = value; }
#endregion

        public AnimationData SetDelay(float delay)
        {
            m_Delay = delay;
            return this;
        }

        public AnimationData SetCurve(AnimationCurve curve)
        {
            m_Curve = curve;
            return this;
        }

        // On anim is update
        public AnimationData SetOnUpdate(Action<float> onUpdate)
        {
            m_Optional.onUpdateFloat = onUpdate;
            return this;
        }
        public AnimationData SetOnUpdate(Action<Color> onUpdate)
        {
            m_Optional.onUpdateColor = onUpdate;
            return this;
        }

        // On anim is finished
        public AnimationData SetOnComplete(Action onComplete)
        {
            m_Optional.onComplete = onComplete;
            return this;
        }
    }
}