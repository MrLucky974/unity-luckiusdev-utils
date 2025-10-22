using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace LuckiusDev.Utils.DampedOscillator
{
    [DefaultExecutionOrder(-1000)]
    public static class DampedOscillator
    {
        private static readonly List<IAnimationInstance> s_instances = new();
        private static readonly List<IAnimationInstance> s_deletion = new();
        
        public delegate void AnimationProcessDelegate<T>(in T originalValue, in float displacement, in float scaleFactor);
        public delegate void AnimationEndDelegate<T>(in T originalValue);
        
        private interface IAnimationInstance
        {
            public void Process();
        }
        
        private class AnimationInstance<T> : IAnimationInstance
        {
            public event Action<AnimationInstance<T>> onAnimationFinished;
            
            private readonly T m_originalValue;
            private readonly AnimationProcessDelegate<T> m_animation;
            private readonly AnimationEndDelegate<T> m_reset;
            
            private readonly float m_springForce;
            private readonly float m_damp;

            private float m_velocity;
            private float m_displacement;
            private readonly float m_scaleFactor;
            
            public AnimationInstance(AnimationProcessDelegate<T> animation, AnimationEndDelegate<T> reset, T originalValue, float springForce, float damp, float velocity, float scaleFactor)
            {
                m_animation = animation;
                m_originalValue = originalValue;
                
                m_springForce = springForce;
                m_damp = damp;

                m_velocity = velocity;
                m_scaleFactor = scaleFactor;
            }

            public void Process()
            {
                var force = -m_springForce * m_displacement - m_damp * m_velocity;
                m_velocity += force * Time.deltaTime;
                m_displacement += m_velocity * Time.deltaTime;
                
                m_animation?.Invoke(m_originalValue, m_displacement, m_scaleFactor);

                if (Mathf.Abs(m_velocity) < Mathf.Epsilon)
                {
                    m_reset?.Invoke(m_originalValue);
                    onAnimationFinished?.Invoke(this);
                }
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Awake()
        {
            // Clear the lists
            s_deletion.Clear();
            s_instances.Clear();
            
            // Add the Update method to the player loop
            #region Player Loop

            var systemRoot = PlayerLoop.GetDefaultPlayerLoop();
            var updateSystem = new PlayerLoopSystem()
            {
                subSystemList = null,
                updateDelegate = Update,
                type = typeof(DampedOscillator)
            };
            
            PlayerLoopSystem dampedOscillatorLoop = new()
            {
                loopConditionFunction = systemRoot.loopConditionFunction,
                type = systemRoot.type,
                updateDelegate = systemRoot.updateDelegate,
                updateFunction = systemRoot.updateFunction
            };
            List<PlayerLoopSystem> subSystemList = new();
            foreach (var subSystem in systemRoot.subSystemList)
            {
                subSystemList.Add(subSystem);
                
                // Add the new update system after the physics update
                if (subSystem.type == typeof(Update))
                    subSystemList.Add(updateSystem);
            }
            dampedOscillatorLoop.subSystemList = subSystemList.ToArray();
            
            PlayerLoop.SetPlayerLoop(dampedOscillatorLoop);

            #endregion
        }

        private static void Update()
        {
            // Prevent the loop from updating in the Editor
            if (Application.isPlaying is false)
            {
                // Clear everything to be sure nothing gets updated
                s_instances.Clear();
                s_deletion.Clear();
                
                return;
            }
            
            // Remove every element queued for deletion from the Instances list
            foreach (var instance in s_deletion)
            {
                if (s_instances.Contains(instance))
                {
                    s_instances.Remove(instance);
                }
            }
            s_deletion.Clear(); // Clear the Deletion list
            
            // Update each animation instance
            foreach (var instance in s_instances)
            {
                instance.Process();
            }
        }
        
        public static void Animate<T>(AnimationProcessDelegate<T> animation, AnimationEndDelegate<T> reset, T originalValue, 
            float springForce, float damp, float scaleFactor, float velocity)
        {
            var instance = new AnimationInstance<T>(animation, reset, originalValue, springForce, damp, velocity, scaleFactor);
            s_instances.Add(instance);
            
            instance.onAnimationFinished += (i) => s_deletion.Add(i);
        }

        public static void Animate<T>(AnimationProcessDelegate<T> animation, AnimationEndDelegate<T> reset, T originalValue,
            DampedOscillatorParameters parameters, float velocity)
        {
            Animate(animation, reset, originalValue, parameters.SpringForce, parameters.Damp, parameters.ScaleFactor, velocity);
        }

        public static Vector3 Squash(Vector3 originalValue, float displacement, float factor)
        {
            return originalValue + new Vector3(displacement, -displacement, displacement) * factor;
        }
        
        public static Vector3 Stretch(Vector3 originalValue, float displacement, float factor)
        {
            return originalValue + new Vector3(-displacement, displacement, -displacement) * factor;
        }
    }
}