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
        _entity.maxHealth += healthBuff;
    }

    private void Setup()
    {
        if(instance != this)
        {
            instance = this;
        }
    }
}