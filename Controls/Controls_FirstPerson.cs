using UnityEngine;

public enum JumpStatus
{
    Queued,
    Midair,
    QueuedSecond,
    MidairSecond,
    Grounded
}

public class Controls_FirstPerson : MonoBehaviour
{
    public Rigidbody rb;

    public KeyCode keyJump = KeyCode.Space;
    public KeyCode keySprint = KeyCode.LeftShift;

    public bool canDoubleJump = true;
    private JumpStatus jumpStatus = JumpStatus.Grounded;
    private float airResistance = 1f;

    private Vector3 movementDirection;

    private float currentSpeed = 1f;
    private float targetSpeed = 1f;
    private float maxSpeed = 5f;

    private const float gravity = 9.81f;

    private void Start()
    {
        rb.freezeRotation = true;
    }

    void Update()
    {

        if (Input.GetKeyDown(keySprint) && jumpStatus == JumpStatus.Grounded)
            targetSpeed = 2f;
        if (Input.GetKeyUp(keySprint))
            targetSpeed = 1f;

        if (Input.GetKeyDown(keyJump))
        {
            if (jumpStatus == JumpStatus.Grounded)
                jumpStatus = JumpStatus.Queued;
            else if (jumpStatus == JumpStatus.Midair && canDoubleJump)
                jumpStatus = JumpStatus.QueuedSecond;
        }

        if (targetSpeed > currentSpeed)
            currentSpeed = Mathf.SmoothStep(currentSpeed, targetSpeed, Time.deltaTime * 50f);
        else if (targetSpeed < currentSpeed)
            currentSpeed = Mathf.SmoothStep(currentSpeed, targetSpeed, Time.deltaTime * 25f);

    }

    void FixedUpdate()
    {

        if (jumpStatus == JumpStatus.Queued || jumpStatus == JumpStatus.QueuedSecond)
        {
            if (jumpStatus == JumpStatus.Queued)
                jumpStatus = JumpStatus.Midair;
            else if (jumpStatus == JumpStatus.QueuedSecond)
                jumpStatus = JumpStatus.MidairSecond;

            //Getting rid of gravity in case of double jump
            Vector3 currentVelocity = rb.velocity;
            currentVelocity.y = 0f;
            rb.velocity = currentVelocity;

            rb.AddForce(Vector3.up * (15f - gravity), ForceMode.Impulse);
        }
        else if (jumpStatus == JumpStatus.Midair || jumpStatus == JumpStatus.MidairSecond)
        {
            airResistance = 4f;
        }

        movementDirection = Camera.main.transform.forward * Input.GetAxisRaw("Vertical") + Camera.main.transform.right * Input.GetAxisRaw("Horizontal");
        MovePlayer(movementDirection, currentSpeed);

    }

    private void MovePlayer(Vector3 direction, float speed)
    {
        if (rb.velocity.magnitude < maxSpeed * targetSpeed)
        {
            direction = direction.normalized;
            direction.y = 0f;

            rb.AddForce(direction * speed * 0.5f / airResistance, ForceMode.Impulse);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            jumpStatus = JumpStatus.Grounded;
            airResistance = 1f;
        }
    }

}
