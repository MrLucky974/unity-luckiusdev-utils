using LuckiusDev.Utils.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace LuckiusDev.Utils.Blackboard
{
    public class BlackboardManager : Singleton<BlackboardManager>
    {
        private readonly Dictionary<GameObject, Blackboard> m_blackboards = new();

        public Blackboard GetBlackboard(GameObject requestor)
        {
            if (!m_blackboards.ContainsKey(requestor))
                m_blackboards[requestor] = new Blackboard();

            return m_blackboards[requestor];
        }
    }
}
