using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    [Header("Key Settings")]
    public KeyType keyType;

    public void Interact()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        PlayerInventory inventory =
            player.GetComponent<PlayerInventory>();

        inventory.AddKey(keyType);

        Debug.Log(keyType + " ¿­¼è È¹µæ!");

        Destroy(gameObject);
    }
}