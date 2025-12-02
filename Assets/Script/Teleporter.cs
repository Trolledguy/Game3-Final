using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Collider))]
public class Teleporter : MonoBehaviour
{
    public string targetScene;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Teleporting to " + targetScene);
            //SceneManager.LoadScene(targetScene);
        }
    }
}