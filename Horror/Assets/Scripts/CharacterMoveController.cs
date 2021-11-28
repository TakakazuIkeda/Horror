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
    
    public Animator Animator;

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = CharacterController.isGrounded;

        // 設置しているかを見る。
        if(groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Abs …… Absolute の略。すなわち絶対値となる。負の方向への移動も正のパワーとして扱える
        var movePower = Mathf.Abs(move.z) + Mathf.Abs(move.x);

        Animator.SetFloat("MovePower", movePower);

        CharacterController.Move(move * Time.deltaTime * playerSpeed);

        // 何か入力されていれば（　Vector3(0,0,0)以外ならば　）
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

    }
}
