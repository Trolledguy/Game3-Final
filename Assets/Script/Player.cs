using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    //[SerializeField]
    //Inputcontroller

    public Transform tRightHandPos;
    public Transform tLeftHandPos;

    //Item
    //Invent

    public void Move(Vector3 _direction)
    {
        
    }
    public IEnumerator Attack(int _damage)
    {
        yield break;
    }

    public void Heal(int _amount)
    {
        
    }

}