using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;

    public ParticleSystem slashEffect;

    CharacterController controller;
    Animator animator;

    Vector3 inputDirection;

    bool isAttacking;
    bool isAttacking2;
    bool isAttacking3;
    bool isJumping;
    float attackCooldown = 0.6f;
    float attack2Cooldown = 0.6f;
    float attack3Cooldown = 0.6f;
    float jumpCooldown = 0.8f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        UpdateMoveState();
        Attack();
        UpdateAttackState();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        inputDirection = new Vector3(horizontal, 0f, vertical);

        if (inputDirection != Vector3.zero && !isAttacking)
        {
            //calculo para onde o player deve olhar
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);

            //executa a acao de rotacionar o player de forma suave
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            //movendo o player
            controller.Move(inputDirection * moveSpeed * Time.deltaTime);
        }

        animator.SetFloat("Speed", inputDirection.magnitude);

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            animator.SetTrigger("Jump");
            isJumping = true;
            jumpCooldown = 0.8f; // tempo de cooldown do pulo
        }
    }

    void UpdateMoveState()
    {
        if (isJumping)
        {
            jumpCooldown -= Time.deltaTime;
            if (jumpCooldown <= 0f)
                isJumping = false;
        }
    }

    // executa animação de ataque
    void Attack()
    {
        if (Input.GetMouseButtonDown(0)  && !isAttacking)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
            attackCooldown = 0.6f; // tempo de cooldown do ataque
            
            slashEffect.Play();
        }

        if (Input.GetKeyDown(KeyCode.F1) && !isAttacking2)
        {
            animator.SetTrigger("Attack2");
            isAttacking2 = true;
            attack2Cooldown = 0.6f; // tempo de cooldown do ataque
        }

        if (Input.GetKeyDown(KeyCode.F2) && !isAttacking3)
        {
            animator.SetTrigger("Attack3");
            isAttacking3 = true;
            attack3Cooldown = 0.6f; // tempo de cooldown do ataque
        }
    }

    // parar animação de ataque
    void UpdateAttackState()
    {
        if (isAttacking)
        {
            attackCooldown -= Time.deltaTime;

            if (attackCooldown <= 0f)
                isAttacking = false;
        }

        if (isAttacking2)
        {
            attack2Cooldown -= Time.deltaTime;
            if (attack2Cooldown <= 0f)
                isAttacking2 = false;
        }

        if (isAttacking3)
        {
            attack3Cooldown -= Time.deltaTime;
            if (attack3Cooldown <= 0f)
                isAttacking3 = false;
        }
    }
}
