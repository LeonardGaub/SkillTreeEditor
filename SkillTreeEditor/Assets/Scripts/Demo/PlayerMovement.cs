using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
	public float speed = 7.5f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    
    [SerializeField] CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        rotation.y = transform.eulerAngles.y;
    }

    void Update()
    {
        
        if (characterController.isGrounded)
        {
	        Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        }
        
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
        

        if (canMove)
        {
	        if (Input.GetMouseButton(1))
	        {
		        rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
		        rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
            }

	        rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }
        else
        {
            Cursor.visible = true;
        }
    }

    public void ChangeCanMove(bool _canMove)
    {
        canMove = _canMove;
    }

    public void SetPlayerPosition(Vector3 pos)
    {
       
        characterController.enabled = false;
        transform.position = pos;
        characterController.enabled = true;
    }
}
