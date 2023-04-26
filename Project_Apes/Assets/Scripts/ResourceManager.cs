using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Send
{
    public int amount;
    public string drop;

    public Send(string _drop, int _amount)
    {
        drop = _drop;
        amount = _amount;
    }
}

public class ResourceManager : MonoBehaviour
{
    //make ui to visualize
    public Dictionary<string, int> Resources;

    private void Awake()
    {
        Resources = new Dictionary<string, int>() 
        {
            { "gold", 0 }

        };
    }

    public void AddResources(List<Send> sends)
    {
        Send[] drops = sends.ToArray();
        for (int i = 0; i < drops.Length; i++)
        {
            Resources[drops[i].drop] += drops[i].amount;
        }
    }

    public void AddResource(string r, int amount)
    {
        Resources[r] += amount;
    }
}
