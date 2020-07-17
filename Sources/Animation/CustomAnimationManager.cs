using System.Collections;
using UnityEngine;
using GameDevStack.Patterns;
using GameDevStack.CommonEnums;

namespace GameDevStack.Animation
{
    public class CustomAnimationManager : SingletonMonoBehaviour<CustomAnimationManager>
    {
    #region Public Animation Functions
        
        // Anim Position
        public static AnimationData AnimPositionWithSpeed(Transform trans, Vector3 toPos, float animSpeed, SpaceType transType = SpaceType.Local)
        {
            return AnimTransformPosition(trans, toPos, animSpeed, transType, false);
        }
        public static AnimationData AnimPositionWithTime(Transform trans, Vector3 toPos, float animTime, SpaceType transType = SpaceType.Local)
        {
            return AnimTransformPosition(trans, toPos, animTime, transType, true);
        }

        // Anim Rotation
        public static AnimationData AnimRotationWithSpeed(Transform trans, Vector3 toRot, float animSpeed, SpaceType transType = SpaceType.Local)
        {
            return AnimTransformRotation(trans, Quaternion.Euler(toRot), animSpeed, transType, false);
        }
        public static AnimationData AnimRotationWithTime(Transform trans, Vector3 toRot, float animTime, SpaceType transType = SpaceType.Local)
        {
            return AnimTransformRotation(trans, Quaternion.Euler(toRot), animTime, transType, true);
        }
        public static AnimationData AnimRotationWithSpeed(Transform trans, Quaternion toRot, float animSpeed, SpaceType transType = SpaceType.Local)
        {
            return AnimTransformRotation(trans, toRot, animSpeed, transType, false);
        }
        public static AnimationData AnimRotationWithTime(Transform trans, Quaternion toRot, float animTime, SpaceType transType = SpaceType.Local)
        {
            return AnimTransformRotation(trans, toRot, animTime, transType, true);
        }

        // Anim Float
        public static AnimationData AnimFloatWithSpeed(float fromValue, float toValue, float animSpeed)
        {
            return AnimFloat(fromValue, toValue, animSpeed, false);
        }
        public static AnimationData AnimFloatWithTime(float fromValue, float toValue, float animTime)
        {
            return AnimFloat(fromValue, toValue, animTime, true);
        }

        // Stop Animation
        public static void StopAnimation(AnimationData animData)
        {
            if (animData != null)
                if (animData.Optional.m_AnimCorout != null)
                    Instance.StopCoroutine(animData.Optional.m_AnimCorout);
        }
    #endregion Public Functions

    #region Animations
        
        #region Position animation
            private static AnimationData AnimTransformPosition(Transform trans, Vector3 toPos, float animTimeOrSpeed, SpaceType transType = SpaceType.Local, bool useTime = true)
            {
                AnimationData animationData = new AnimationData();
                float distance = transType == SpaceType.Local ? Vector3.Distance(trans.localPosition, toPos) : Vector3.Distance(trans.position, toPos);
                float speed = useTime ? distance / animTimeOrSpeed : animTimeOrSpeed;
                animationData.Optional.m_AnimCorout = Instance.StartCoroutine(ChangeTransformPosition(animationData, trans, toPos, distance, speed, transType));
                return animationData;
            }
            private static IEnumerator ChangeTransformPosition(AnimationData animationData, Transform changeTrans, Vector3 toValue, float distance, float changeSpeed, SpaceType transType)
            {
                yield return new WaitForEndOfFrame();
                yield return new WaitForSeconds(animationData.Delay);
                Vector3 fromValue = transType == SpaceType.Local ? changeTrans.localPosition : changeTrans.position;
                if (IsEqualValues(fromValue, toValue))
                {
                    On_AnimComplete(animationData);
                    yield break;
                }
                Vector3 currentValue = fromValue;
                float fracJourney = 0;
                while (fracJourney < 1)
                {
                    fracJourney += (Time.deltaTime) * changeSpeed / distance;
                    currentValue = animationData.Curve == null ? Vector3.Lerp(fromValue, toValue, fracJourney) : Vector3.Lerp(fromValue, toValue, animationData.Curve.Evaluate(fracJourney));
                    
                    if (transType == SpaceType.Local)
                        changeTrans.localPosition = currentValue;
                    else
                        changeTrans.position = currentValue;

                    On_AnimUpdate(animationData, currentValue);
                    yield return null;
                }
                On_AnimComplete(animationData);
            }
        #endregion Position animation

        #region Rotation animation
            private static AnimationData AnimTransformRotation(Transform trans, Quaternion toRot, float animTimeOrSpeed, SpaceType transType = SpaceType.Local, bool useTime = true)
            {
                AnimationData animationData =  new AnimationData();
                float distance = transType == SpaceType.Local ? Quaternion.Angle(trans.localRotation, toRot) : Quaternion.Angle(trans.rotation, toRot);
                float speed = useTime ? distance / animTimeOrSpeed : animTimeOrSpeed;
                animationData.Optional.m_AnimCorout = Instance.StartCoroutine(ChangeTransformRotation(animationData, trans, toRot, distance, speed, transType));
                return animationData;
            }
            private static IEnumerator ChangeTransformRotation(AnimationData animationData, Transform changeTrans, Quaternion toValue, float distance, float changeSpeed, SpaceType transType)
            {
                yield return new WaitForEndOfFrame();
                yield return new WaitForSeconds(animationData.Delay);
                Quaternion fromValue = transType == SpaceType.Local ? changeTrans.localRotation : changeTrans.rotation;
                if (IsEqualValues(fromValue, toValue))
                {
                    On_AnimComplete(animationData);
                    yield break;
                }
                Quaternion currentValue = fromValue;
                float fracJourney = 0;
                while (fracJourney < 1)
                {
                    fracJourney += (Time.deltaTime) * changeSpeed / distance;
                    currentValue = animationData.Curve == null ? Quaternion.Slerp(fromValue, toValue, fracJourney) : Quaternion.Slerp(fromValue, toValue, animationData.Curve.Evaluate(fracJourney));
                    
                    if (transType == SpaceType.Local)
                        changeTrans.localRotation = currentValue;
                    else
                        changeTrans.rotation = currentValue;

                    On_AnimUpdate(animationData, currentValue);
                    yield return null;
                }
                On_AnimComplete(animationData);
            }    
        #endregion Rotation animation

        #region Color animation
            private static AnimationData AnimColor(Color fromColor, Color toColor, float animTimeOrSpeed, bool useTime = true)
            {
                AnimationData animationData = new AnimationData();
                float distance = GetDistanceFromColors(fromColor, toColor);
                float speed = useTime ? distance / animTimeOrSpeed : animTimeOrSpeed;
                animationData.Optional.m_AnimCorout = Instance.StartCoroutine(ChangeColor(animationData, fromColor, toColor, distance, speed));
                return animationData;
            }
            private static IEnumerator ChangeColor(AnimationData animationData, Color fromValue, Color toValue, float distance, float changeSpeed)
            {
                yield return new WaitForEndOfFrame();
                yield return new WaitForSeconds(animationData.Delay);
                if (IsEqualValues(fromValue, toValue))
                {
                    On_AnimComplete(animationData);
                    yield break;
                }
                Color currentValue = fromValue;
                float fracJourney = 0;
                while (fracJourney < 1)
                {
                    fracJourney += (Time.deltaTime) * changeSpeed / distance;
                    currentValue = animationData.Curve == null ? Color.Lerp(fromValue, toValue, fracJourney) : Color.Lerp(fromValue, toValue, animationData.Curve.Evaluate(fracJourney));
                    animationData.ColorData = currentValue;
                    On_AnimUpdate(animationData, currentValue);
                    yield return null;
                }
                On_AnimComplete(animationData);
            }
        #endregion Color animation

        #region Float animation
            private static AnimationData AnimFloat(float fromValue, float toValue, float animTimeOrSpeed, bool useTime = true)
            {
                AnimationData animationData = new AnimationData();
                float distance = GetDistanceFromFloats(fromValue, toValue);
                float speed = useTime ? distance / animTimeOrSpeed : animTimeOrSpeed;
                animationData.Optional.m_AnimCorout = Instance.StartCoroutine(ChangeFloatValue(animationData, fromValue, toValue, distance, speed));
                return animationData;
            }
            private static IEnumerator ChangeFloatValue(AnimationData animationData, float fromValue, float toValue, float distance, float changeSpeed)
            {
                yield return new WaitForEndOfFrame();
                yield return new WaitForSeconds(animationData.Delay);
                if (IsEqualValues(fromValue, toValue))
                {
                    On_AnimComplete(animationData);
                    yield break;
                }
                float currentValue = fromValue;
                float fracJourney = 0;
                while (fracJourney < 1)
                {
                    fracJourney += (Time.deltaTime) * changeSpeed / distance;
                    currentValue = animationData.Curve == null ? Mathf.Lerp(fromValue, toValue, fracJourney) : Mathf.Lerp(fromValue, toValue, animationData.Curve.Evaluate(fracJourney));
                    animationData.FloatData = currentValue;
                    On_AnimUpdate(animationData, currentValue);
                    yield return null;
                }
                On_AnimComplete(animationData);
            }
        #endregion Float animation

    #endregion Animations

    #region Actions
        private static void On_AnimUpdate(AnimationData moveObjectData, Vector3 currentValue)
        {
            // moveObjectData.m_fracJourney = Mathf.Clamp01(fracJourney);    // Utile de clamp juste pour retourner 1 à coups sûr ?
            if (moveObjectData.Optional.onUpdateVector3 != null)
                moveObjectData.Optional?.onUpdateVector3(currentValue);
        }
        private static void On_AnimUpdate(AnimationData moveObjectData, Quaternion currentValue)
        {
            if (moveObjectData.Optional.onUpdateQuaternion != null)
                moveObjectData.Optional?.onUpdateQuaternion(currentValue);
        }
        private static void On_AnimUpdate(AnimationData moveObjectData, float currentValue)
        {
            if (moveObjectData.Optional.onUpdateFloat != null)
                moveObjectData.Optional?.onUpdateFloat(currentValue);
        }
        private static void On_AnimUpdate(AnimationData moveObjectData, Color currentValue)
        {
            if (moveObjectData.Optional.onUpdateColor != null)
                moveObjectData.Optional?.onUpdateColor(currentValue);
        }
        private static void On_AnimComplete(AnimationData moveObjectData)
        {
            if (moveObjectData.Optional.onComplete != null)
                moveObjectData.Optional?.onComplete();
        }
    #endregion

    #region Utilities
        // Equal Values?
        private static bool IsEqualValues(Vector3 fromValue, Vector3 toValue)
        {
            return fromValue == toValue;
        }
        private static bool IsEqualValues(Quaternion fromValue, Quaternion toValue)
        {
            return fromValue == toValue;
        }
        private static bool IsEqualValues(float fromValue, float toValue)
        {
            return fromValue == toValue;
        }
        private static bool IsEqualValues(Color fromValue, Color toValue)
        {
            return fromValue == toValue;
        }

        // Distances
        private static float GetDistanceFromFloats(float f1, float f2)
        {
            return Mathf.Abs(f1 - f2);
        }
        private static float GetDistanceFromColors(Color color1, Color color2)
        {
            return Mathf.Abs(color1.r - color2.r) + Mathf.Abs(color1.g - color2.g) + Mathf.Abs(color1.b - color2.b) + Mathf.Abs(color1.a - color2.a);
        }
    #endregion
    }
}