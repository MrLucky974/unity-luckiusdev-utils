using UnityEngine;

namespace LuckiusDev.Utils.FSM
{
    public class StateMachine : MonoBehaviour
    {
        private BaseState m_currentState;

        protected virtual void Start() {
            m_currentState = GetInitialState();
            m_currentState?.Enter();
        }

        protected virtual void Update()
        {
            m_currentState?.UpdateLogic();
        }

        protected virtual void FixedUpdate()
        {
            m_currentState?.UpdatePhysics();
        }

        protected virtual BaseState GetInitialState() {
            return null;
        }

        public void ChangeState(BaseState newState) {
            m_currentState.Exit();

            m_currentState = newState;
            newState.Enter();
        }

        public void DrawGUI()
        {
            GUILayout.BeginArea(new Rect(10f, 10f, 200f, 100f));
            var content = m_currentState != null ? m_currentState.Name : "(no current state)";
            GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
            GUILayout.EndArea();
        }
    }
}