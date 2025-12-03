using UnityEngine;

public abstract class Weapon : Item
{
    public int damage;
    public float attackSpeed;

    public override void Interact()
    {
        Debug.Log("Weapon picked up by player.");
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.buffAttackDamage += damage;
            gameObject.transform.SetParent(player.tRightHandPos);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
        }
    }

}