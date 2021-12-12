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

    // 前の速度
    private Vector3 oldVelocity;

    public FootStepsSoundManager FootStepsSoundManager;

    // Camera
    public Camera mainCamera;

    // スタミナの最大値
    public float MaxStamina = 6f;

    // 今のスタミナ
    public float NowStamina = 0f;

    // 疲れたフラグ
    public bool Tired = false;

    private void Start()
    {
        NowStamina = MaxStamina;
    }


    // Update is called once per frame
    void Update()
    {
        groundedPlayer = CharacterController.isGrounded;

        // 接地しているかを見る。
        if(groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

    // スタミナが０より下の場合、操作を受け付けない。
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

 
        // Xキーを押したときと押し続けたときにPlayerSpeedを変更する

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

   

        // playerの加速度にmoveの値を代入
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
