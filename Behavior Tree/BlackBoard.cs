using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlackBoard
{
    private Dictionary<string, object> data = new Dictionary<string, object>();

    public void SetValue(string id, object value)
    {
        data[id] = value;
    }

    public bool GetValue(string id, out object value)
    {
        if(data.ContainsKey(id))
        {
            value = data[id];
            return true;
        }
        else
        {
            value = null;
            return false;
        }
    }

    public bool GetValue<T>(string id, out T value)
    {
        if(data.ContainsKey(id))
        {
            value = (T)data[id];
            return true;
        }
        else
        {
            value = default(T);
            return false;
        }
    }

    public List<string> GetAllKeys()
    {
        return data.Keys.ToList();
    }
}
