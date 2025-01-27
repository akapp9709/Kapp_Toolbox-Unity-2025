using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Brain : MonoBehaviour
    {
        public BehaviorTreeClass tree;
        // Start is called before the first frame update
        protected virtual void Awake()
        {
            tree = tree.Clone();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // GatherData();
            tree.Update();
        }

        public void WriteToBlackBoard(string id, object value)
        {
            tree.WriteData(id, value);
        }

        protected virtual void GatherData()
        {

        }
    }
}
