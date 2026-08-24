using UnityEngine;

public class PlayerMovementRigidbody : MonoBehaviour {
    [SerializeField] private Rigidbody playerRigidbody;
    private Vector3 move;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpSpeed;
    
    public Vector3 playerMovement;
    public Vector3 playerJump;

    private void Update() {
        playerMovement.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space)) {
            playerJump.y = jumpSpeed;
        }

        else {
            playerJump = Vector3.zero;
        }
    }
    
    private void FixedUpdate() {
        playerRigidbody.AddForce(playerMovement * movementSpeed);
        playerRigidbody.AddForce(playerJump * jumpSpeed);
    }
}
