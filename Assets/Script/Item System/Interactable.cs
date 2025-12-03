using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();

    void Start()
    {
        IterSetup();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interact();
        }
    }

    private void IterSetup()
    {
        try
        {
            Collider col = GetComponent<BoxCollider>();
            if (!col.isTrigger)
            {
                col.isTrigger = true;
            }   
        }
        catch (MissingComponentException)
        {
            return;
        }
    }
}
