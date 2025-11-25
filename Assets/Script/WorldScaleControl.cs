using UnityEngine;

public class WorldScaleControl : MonoBehaviour
{
    public static WorldScaleControl instance;

    private int healthBuff = 0;
    private int damageBuff = 0;

    void Start()
    {
        Setup();
    }

    public void ApplyBuffs(AIAutoControl _entity)
    {
        healthBuff = Mathf.FloorToInt(Vector3.Distance(Vector3.zero, _entity.transform.position) / 10) * 10;
        damageBuff = Mathf.FloorToInt(Vector3.Distance(Vector3.zero, _entity.transform.position) / 10) * 5;

        _entity.baseHealth += healthBuff;
        _entity.buffAttackDamage = damageBuff;

    }

    private void Setup()
    {
        if(instance != this)
        {
            instance = this;
        }
    }
}