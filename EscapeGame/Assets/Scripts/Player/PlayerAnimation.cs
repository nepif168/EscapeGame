using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerAnimation : MonoBehaviour {

    PlayerMove playerMove;
    PlayerCore playerCore;
    Animator animator;

    bool isIdle = true;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerCore = GetComponent<PlayerCore>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerMove.DashDirection.Subscribe(direction =>
        {
            if (direction == Vector3.forward)
                animator.SetTrigger("rollForward");
            else if (direction == Vector3.left)
                animator.SetTrigger("rollLeft");
            else if (direction == Vector3.back)
                animator.SetTrigger("rollBackward");
            else if (direction == Vector3.right) 
                animator.SetTrigger("rollRight");
        });

        playerMove.MoveDirection.Where(_=> !playerMove.IsJumping.Value).Subscribe(direction =>
        {
            if (direction == Vector3.zero)
                animator.SetTrigger("idle");
            else
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                //移動方向が変わるたびに呼ばれるので、現在の状態が走ってる状態でなかったら変える
                if (stateInfo.fullPathHash != Animator.StringToHash("Base Layer.Unarmed-Run-Forward"))
                {
                    animator.SetTrigger("running");
                }
            }
        });

        playerMove.IsJumping.Where(j => j).Subscribe(_ =>
        {
            animator.SetTrigger("running");
            animator.speed = 4;
        });
        playerMove.IsJumping.Where(j => !j).Subscribe(_ =>
        {
            animator.SetTrigger("idle");
            animator.speed = 1;
        });

        playerCore.IsDead.Where(d => d).Subscribe(_ => animator.SetTrigger("dead"));

    }
}
