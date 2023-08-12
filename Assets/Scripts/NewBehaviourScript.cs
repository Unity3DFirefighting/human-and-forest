using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float speed = 6.0F;
    public float turnSpeed = 10.0F; // 转身速度
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

            // 计算移动方向
            moveDirection = new Vector3(horizontalInput, 0, verticalInput);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

            // 设置移动速度参数，控制动画切换
            float moveSpeed = moveDirection.magnitude;
            if (moveSpeed > 0.1f && !isWalking)
            {
                // 切换到Walk动画
                animator.SetBool("IsWalking", true);
                isWalking = true;
            }
            else if (moveSpeed <= 0.1f && isWalking)
            {
                // 切换到Idle动画
                animator.SetBool("IsWalking", false);
                isWalking = false;
            }

            // 转身
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
