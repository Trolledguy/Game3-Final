using UnityEngine;

public class Destroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Entity>())
        {
            Destroy(other.gameObject);
        }
    }
}
