using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity , ICharacter
{
    //[SerializeField]
    //Inputcontroller

    /// <summary>
    /// Stats
    /// </summary>

    public int maxHealth { get; set; } = 100;
    public int currentHealth { get; set; } = 100;
    public int baseAttackDamage { get; set; } = 10;
    public int buffAttackDamage { get; set; } = 0;

    private int currentAttackDamage;
    public int _currentAttackDamage
    {
        get
        {
            return baseAttackDamage +  buffAttackDamage;
        }
    }

    public float moveSpeed = 5f;

    //Weapon position
    public Transform tRightHandPos;
    public Transform tLeftHandPos;

    //Item
    //Invent

    public void Move(Vector3 _direction)
    {
        eAnim.SetFloat("Speed", _direction.magnitude * 5); // Assuming moveSpeed is a float representing speed
        transform.Translate(_direction * Time.deltaTime * moveSpeed, Space.World);
    }
    public IEnumerator Attack()
    {
        eAnim.SetTrigger("Attack");

        AnimatorClipInfo animationInfo = eAnim.GetCurrentAnimatorClipInfo(0)[0];
        float animationLength = animationInfo.clip.length;
        RaycastHit[] hits = null;
    
        //Change to animation time
        for(float t = 0; t < animationLength; t += Time.deltaTime)
        {
            hits = Physics.RaycastAll(tRightHandPos.position, tRightHandPos.up, 2f);
        }
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemyEntity = hit.collider.GetComponent<Enemy>();
                if (enemyEntity != null)
                {
                    Debug.Log($"Player attacked {enemyEntity.ename} for {_currentAttackDamage} damage.");
                    enemyEntity.TakeDamage(_currentAttackDamage);
                    // Here you would typically call a method on the enemy to apply damage
                }
            }
        }
        yield break;
    }


    private void HealPlayer(int _amount)
    {
        currentHealth += _amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        Debug.Log($"Player healed for {_amount}. Current Health: {currentHealth}/{maxHealth}");
    }

    public void Heal(int _amount)
    {
        HealPlayer(_amount);
    }

}
