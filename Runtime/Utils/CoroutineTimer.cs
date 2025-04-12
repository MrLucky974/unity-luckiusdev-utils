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
        public event Action TimerComplete;

        /// <summary>
        /// Time remaining (in seconds) until the timer completes.
        /// </summary>
        public float TimeLeft { get; private set; }

        private readonly float _duration;
        private IEnumerator _timerCoroutine;
        private bool _isRunning;

        /// <summary>
        /// Initializes a new instance of the CoroutineTimer.
        /// </summary>
        /// <param name="duration">The delay duration in seconds before the timer completes.</param>
        public CoroutineTimer(float duration)
        {
            _duration = duration;
        }

        /// <summary>
        /// Starts the timer using the given MonoBehaviour as the coroutine runner.
        /// </summary>
        public void Start(MonoBehaviour runner)
        {
            if (_isRunning) return; // Prevent double starts

            _timerCoroutine = TimerCoroutine();
            runner.StartCoroutine(_timerCoroutine);
            _isRunning = true;
        }

        /// <summary>
        /// Stops the timer if it is running.
        /// </summary>
        public void Stop(MonoBehaviour runner)
        {
            if (!_isRunning || _timerCoroutine == null) return;

            runner.StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
            _isRunning = false;
            TimeLeft = 0;
        }

        /// <summary>
        /// The internal coroutine that tracks elapsed time and triggers the completion event.
        /// </summary>
        private IEnumerator TimerCoroutine()
        {
            float endTime = Time.time + _duration;

            while (Time.time < endTime)
            {
                TimeLeft = endTime - Time.time;
                yield return null;
            }

            TimeLeft = 0;
            _isRunning = false;

            TimerComplete?.Invoke();
        }
    }
}
