using LuckiusDev.Utils.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace LuckiusDev.Utils.Blackboard
{
    public class BlackboardManager : Singleton<BlackboardManager>
    {
        readonly Dictionary<GameObject, Blackboard> Blackboards = new Dictionary<GameObject, Blackboard>();

        public Blackboard GetBlackboard(GameObject requestor)
        {
            if (!Blackboards.ContainsKey(requestor))
                Blackboards[requestor] = new Blackboard();

            return Blackboards[requestor];
        }
    }
}
