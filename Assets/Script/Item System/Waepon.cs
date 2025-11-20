using UnityEngine;

public abstract class Weapon : Item
{
    public int damage;
    public float attackSpeed;

    protected override void Setup()
    {
        base.Setup();
        // Additional setup for weapons can be added here
    }
}