using UnityEngine;

public class EndPortal : Teleporter
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if(player.ishasKey)
            {
                Debug.Log("Player reached the end portal. Loading Win Screen.");
                UIManager.instance.ShowWinScreen();
            }
            else
            {
                Debug.Log("Player needs a key to use the end portal.");
            }
            
        }
    }
}