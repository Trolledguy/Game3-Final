using UnityEngine;
using System.Collections;

public interface ICharacter
{
    int maxHealth { get; set; }
    int currentHealth { get; set; }
    int baseAttackDamage { get; set; }


    virtual void Move(Vector3 _direction) {}
    IEnumerator Attack();
    void Heal(int _amount);
}