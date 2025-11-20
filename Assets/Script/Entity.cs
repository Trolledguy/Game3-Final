using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public string ename;
    public string entityID;

    public EClasses eClass;

    public Collider eColli;
    public Rigidbody eRigi;
    public Animator eAnim;

    void Start()
    {
        SetUP();
    }
    protected virtual void SetUP()
    {
        eColli = GetComponent<Collider>();
        eRigi = GetComponent<Rigidbody>();
        eAnim = GetComponent<Animator>();
        if(gameObject.tag == "Untagged")
        {
            Debug.LogWarning("Entity " + ename + " has no tag assigned!");
        }
        Debug.Log($"Setup Entity {ename}");
    }
}

