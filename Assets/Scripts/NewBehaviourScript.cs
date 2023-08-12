using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float speed = 6.0F;
    public float turnSpeed = 10.0F; // ת���ٶ�
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;

    private Vector3 moveDirection = Vector3.zero;
    private Animator animator;
    private CharacterController controller;
    private bool isWalking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // �����ƶ�����
            moveDirection = new Vector3(horizontalInput, 0, verticalInput);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

            // �����ƶ��ٶȲ��������ƶ����л�
            float moveSpeed = moveDirection.magnitude;
            if (moveSpeed > 0.1f && !isWalking)
            {
                // �л���Walk����
                animator.SetBool("IsWalking", true);
                isWalking = true;
            }
            else if (moveSpeed <= 0.1f && isWalking)
            {
                // �л���Idle����
                animator.SetBool("IsWalking", false);
                isWalking = false;
            }

            // ת��
            if (horizontalInput != 0 || verticalInput != 0)
            {
                Vector3 targetDirection = new Vector3(horizontalInput, 0, verticalInput);
                if (targetDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                }
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
