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

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = CharacterController.isGrounded;

        // �ڒn���Ă��邩������B
        if(groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Abs �c�c Absolute �̗��B���Ȃ킿��Βl�ƂȂ�B���̕����ւ̈ړ������̃p���[�Ƃ��Ĉ�����
        var movePower = Mathf.Abs(move.z) + Mathf.Abs(move.x);

        Animator.SetFloat("MovePower", movePower);

        if (movePower > 0)
        {
            FootStepsSoundManager.PlayFootStepSE();
        }
        else
        {
            FootStepsSoundManager.StopFootStepSE();
        }

        // player�̉����x��move�̒l����
        playerVelocity = move;
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
        CharacterController.Move(playerVelocity * Time.deltaTime);
    }
}
