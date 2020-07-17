using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevStack.Animation
{
    public class AnimationDataOptional
    {
        public Coroutine m_AnimCorout;

        [Range(0, 100)] public float m_AnimPercent = 0;

        public Action<Vector3> onUpdateVector3 { get; set; }
        public Action<Quaternion> onUpdateQuaternion { get; set; }
        public Action<float> onUpdateFloat { get; set; }
        public Action<Color> onUpdateColor { get; set; }
        public Action onComplete { get; set; }
    }
}