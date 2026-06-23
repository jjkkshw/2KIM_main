using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    private List<KeyType> keys = new List<KeyType>();

    public void AddKey(KeyType key)
    {
        if (!keys.Contains(key))
        {
            keys.Add(key);
            KeyPickupMessageUI.Show(key);
            Debug.Log(key + " 열쇠 획득!");
        }
    }

    public bool HasKey(KeyType key)
    {
        return keys.Contains(key);
    }
}
