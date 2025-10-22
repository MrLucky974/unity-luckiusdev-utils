using System;
using System.Collections;
using UnityEngine;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// A utility class for managing a coroutine-based timer in Unity.
    /// Allows you to start and stop timed callbacks using any MonoBehaviour instance.
    /// </summary>
    public class CoroutineTimer
    {
        /// <summary>
        /// Event triggered when the timer has completed.
        /// </summary>
        public event Action onTimerComplete;

        /// <summary>
        /// Time remaining (in seconds) until the timer completes.
        /// </summary>
        public float TimeLeft { get; private set; }

        private readonly float m_duration;
        private Coroutine m_timerCoroutine;
        private bool m_isRunning;

        /// <summary>
        /// Initializes a new instance of the CoroutineTimer.
        /// </summary>
        /// <param name="duration">The delay duration in seconds before the timer completes.</param>
        public CoroutineTimer(float duration)
        {
            m_duration = duration;
        }

        /// <summary>
        /// Starts the timer using the given MonoBehaviour as the coroutine runner.
        /// </summary>
        public void Start(MonoBehaviour runner)
        {
            if (m_isRunning) return; // Prevent double starts

            m_timerCoroutine = runner.StartCoroutine(nameof(TimerCoroutine));
            m_isRunning = true;
        }

        /// <summary>
        /// Stops the timer if it is running.
        /// </summary>
        public void Stop(MonoBehaviour runner)
        {
            if (!m_isRunning || m_timerCoroutine == null) return;

            runner.StopCoroutine(m_timerCoroutine);
            
            TimeLeft = 0;
            m_timerCoroutine = null;
            m_isRunning = false;
        }

        /// <summary>
        /// The internal coroutine that tracks elapsed time and triggers the completion event.
        /// </summary>
        private IEnumerator TimerCoroutine()
        {
            float endTime = Time.time + m_duration;

            while (Time.time < endTime)
            {
                TimeLeft = endTime - Time.time;
                yield return null;
            }

            TimeLeft = 0;
            m_isRunning = false;
            m_timerCoroutine = null;

            onTimerComplete?.Invoke();
        }
    }
}
