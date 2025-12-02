using UnityEngine;

public class Sword : Melee
{

    public override void Interact()
    {
        Debug.Log("Sword picked up by player.");
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.buffAttackDamage += damage;
            Destroy(gameObject);
        }
    }
}