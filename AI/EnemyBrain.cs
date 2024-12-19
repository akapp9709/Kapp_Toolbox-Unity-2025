using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AIModels
{
    public class EnemyBrain : EnemyFSM
    {
        public EnemyStats enemyStats;
        protected Dictionary<string, object> knowledge = new Dictionary<string, object>();

        public void AddToDictionary(string name, object value)
        {
            if (knowledge.ContainsKey(name))
                return;

            knowledge[name] = value;
        }

        public object GetValue(string name)
        {
            if (knowledge.ContainsKey(name))
            {
                return knowledge[name];
            }
            else
                return null;
        }

        public bool TryGetValue<T>(string name, out T value)
        {
            if (knowledge.ContainsKey(name))
            {
                value = (T)knowledge[name];
                return true;
            }

            value = default(T);
            return false;
        }
    }
}