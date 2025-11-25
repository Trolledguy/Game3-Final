using UnityEngine;

public class InputController : MonoBehaviour
{
    public Player targetPlayer;
    private void Update()
    {
        if (targetPlayer == null)
            return;
        HandleMovement();
        HandleActions();
        HandleRotation();
    }
    private void HandleRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 pointToLook = ray.GetPoint(rayDistance);
            Vector3 heightCorrectedPoint = new Vector3(pointToLook.x, targetPlayer.transform.position.y, pointToLook.z);
            targetPlayer.transform.LookAt(heightCorrectedPoint);
        }
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        targetPlayer.Move(movement);
    }
        
    private void HandleActions()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            targetPlayer.StartCoroutine(targetPlayer.Attack());
        }
    }
}