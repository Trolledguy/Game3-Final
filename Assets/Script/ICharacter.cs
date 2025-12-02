using UnityEngine;
using System.Collections;

public interface ICharacter
{
    int baseHealth { get; set; }
    int buffHealth { get; set; }
    int currentHealth { get; set; }
    int baseAttackDamage { get; set; }
    int buffAttackDamage { get; set; }

    public virtual void TakeDamage(int _amount)
    {
        Entity entity = this as Entity;
        if (entity != null)
        {
            currentHealth -= _amount;
            if (currentHealth <= 0)
            {
                Debug.Log("Entity defeated: " + entity.ename);
                Object.Destroy(entity.gameObject);
            }
        }
    }
    virtual void Move(Vector3 _direction) {}
    IEnumerator Attack();
    void Heal(int _amount);
}