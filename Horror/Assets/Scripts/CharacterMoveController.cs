using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    public CharacterController CharacterController;
    private Vector3 playerVelocity;
    private bool groundedPlayer = true;
    public float playerSpeed = 2.0f;
    public float jumppower = 1.0f;
    public float gravityValue = -9.81f;
    
    public Animator Animator;

    // �O�̑��x
    private Vector3 oldVelocity;

    public FootStepsSoundManager FootStepsSoundManager;

    // Camera
    public Camera mainCamera;

    // �X�^�~�i�̍ő�l
    public float MaxStamina = 6f;

    // ���̃X�^�~�i
    public float NowStamina = 0f;

    // ��ꂽ�t���O
    public bool Tired = false;

    private void Start()
    {
        NowStamina = MaxStamina;
    }


    // Update is called once per frame
    void Update()
    {
        groundedPlayer = CharacterController.isGrounded;

        // �ڒn���Ă��邩������B
        if(groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

    // �X�^�~�i���O��艺�̏ꍇ�A������󂯕t���Ȃ��B
    if (NowStamina <= 0 && !Tired)
    {
        Tired = true;
        Animator.SetBool("Tired", true);
    }
    if (Tired)
    {
            NowStamina += Time.deltaTime / 2;
            if (NowStamina > MaxStamina)
            {
                Tired = false;
                NowStamina = MaxStamina;
                Animator.SetBool("Tired", false);
            }
            return;
    }

        var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

 
        // X�L�[���������Ƃ��Ɖ����������Ƃ���PlayerSpeed��ύX����

        if (Input.GetKey(KeyCode.X))
        {
            playerSpeed = 3.5f;
            NowStamina -= Time.deltaTime;
        }
        else
        {
            playerSpeed = 2.0f;
            if (NowStamina < MaxStamina)
            {
                NowStamina += Time.deltaTime;
            }
        }

        Animator.SetFloat("MovePower", move.magnitude * playerSpeed);
        playerVelocity = move;

        if (move.magnitude > 0)
        {
            FootStepsSoundManager.PlayFootStepSE();
        }
        else
        {
            FootStepsSoundManager.StopFootStepSE();
        }

   

        // player�̉����x��move�̒l����
        playerVelocity = move;

        var horizontalRotation = Quaternion.AngleAxis(mainCamera.transform.eulerAngles.y,Vector3.up);

        playerVelocity = horizontalRotation * move;

        // S
        // 
        playerVelocity = Vector3.Slerp(oldVelocity, playerVelocity, playerSpeed * Time.deltaTime);

        // 
        oldVelocity = playerVelocity;

        // 
        if (playerVelocity.magnitude > 0f)
        {
            //
            transform.LookAt(transform.position + playerVelocity);
        }


        playerVelocity.y += gravityValue * Time.deltaTime;
        CharacterController.Move(playerVelocity * Time.deltaTime * playerSpeed );
    }
}
