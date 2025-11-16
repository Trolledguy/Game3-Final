using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public string ename;
    public string entityID;

    public EClasses eClass;

    public Collider eColli;
    public Rigidbody eRigi;

    void Start()
    {
        SetUP();
    }

    protected virtual void SetUP()
    {
        eColli = GetComponent<Collider>();
        eRigi = GetComponent<Rigidbody>();
        Debug.Log($"Setup Entity {ename}");
    }

    


}
