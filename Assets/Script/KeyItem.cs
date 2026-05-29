using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        PlayerInventory inventory =
            player.GetComponent<PlayerInventory>();

        inventory.hasKey = true;

        Debug.Log("¿­¼è È¹µæ!");

        Destroy(gameObject);
    }
}