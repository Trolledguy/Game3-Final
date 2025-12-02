using UnityEngine;


public class Key : Item
{

    public override void Interact()
    {
        Debug.Log("Key picked up by player.");
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.ishasKey = true;
            Destroy(gameObject);
        }
    }
}