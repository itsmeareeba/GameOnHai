using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlerTarget : MonoBehaviour
{
    [Header(" Settings ")]
    [SerializeField] private bool isBatsmanScene;
    [SerializeField] private Vector2 MinMaxX;
    [SerializeField] private Vector2 MinMaxZ;
    [SerializeField] private Vector2 moveSpeed;

    private Vector3 clickedPosition;
    private Vector3 clickedTargetPosition;

    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove && !isBatsmanScene)
            ManageControl();
    }

    private void ManageControl()
    {
        if(Input.GetMouseButtonDown(0))
        {
            clickedPosition = Input.mousePosition;
            clickedTargetPosition = transform.position;
        }
        else if(Input.GetMouseButton(0))
        {
            Vector3 difference = Input.mousePosition - clickedPosition;

            difference.x /= Screen.width;
            difference.y /= Screen.height;

            Vector3 targetPosition = clickedTargetPosition + new Vector3 (difference.x * moveSpeed.x, 0, difference.y * moveSpeed.y);

            targetPosition.x = Mathf.Clamp(targetPosition.x, MinMaxX.x, MinMaxX.y);
            targetPosition.z = Mathf.Clamp(targetPosition.z, MinMaxZ.x, MinMaxZ.y);

            transform.position = targetPosition;
        }
    }

    public void Move(Vector2 movement)
    {
        float xPosition = Mathf.Lerp(MinMaxX.x, MinMaxX.y, movement.x);
        float zPosition = Mathf.Lerp(MinMaxZ.x, MinMaxZ.y, movement.y);

        transform.position = new Vector3(xPosition, 0, zPosition);
    }

    public void EnableMovement() 
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }
}
