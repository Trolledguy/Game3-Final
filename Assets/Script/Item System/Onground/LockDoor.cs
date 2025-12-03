using UnityEngine;

public class LockDoor : MovableObject
{
    public override void Interact()
    {
        Debug.Log("Attempting to unlock the door.");
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            if (player.ishasKey)
            {
                Debug.Log("Door unlocked!");
                player.ishasKey = false;
                // Add logic to open the door
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Player does not have a key to unlock the door.");
            }
        }
    }
}
