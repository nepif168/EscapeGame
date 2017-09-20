using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerAnimation : MonoBehaviour {

    PlayerCore playerCore;
    IInputProvider playerInput;
    Animator animator;
    DashInputProvider dashInputProvider;
    PlayerInputProvider playerInputProvider;

    private void Awake()
    {
        playerCore = GetComponent<PlayerCore>();
        playerInput = GetComponent<IInputProvider>();
        animator = GetComponent<Animator>();
        dashInputProvider = GetComponent<DashInputProvider>();
        playerInputProvider = GetComponent<PlayerInputProvider>();
    }

    private void Start()
    {
        playerInput.IsJump.Where(j => j).Subscribe(_ => animator.speed = 3);
        playerInput.IsJump.Where(j => !j).Subscribe(_ => animator.speed = 1);
        playerCore.IsDead.Where(d => d).Subscribe(_ => animator.SetBool("dead", true));


        // 前方ダッシュ
        dashInputProvider.wDoubleTapStream
             .Where(_ => !playerCore.IsDead.Value && !playerInputProvider.IsJump.Value)
             .Subscribe(_ =>
             {
                 animator.SetTrigger("rollForward");
             });

        // 左方ダッシュ
        dashInputProvider.aDoubleTapStream
             .Where(_ => !playerCore.IsDead.Value && !playerInputProvider.IsJump.Value)
            .Subscribe(_ =>
            {
                animator.SetTrigger("rollLeft");
            });

        // 後方ダッシュ
        dashInputProvider.sDoubleTapStream
            .Where(_ => !playerCore.IsDead.Value && !playerInputProvider.IsJump.Value)
            .Subscribe(_ =>
            {
                animator.SetTrigger("rollBackward");
            });

        // 右方ダッシュ
        dashInputProvider.dDoubleTapStream
            .Where(_ => !playerCore.IsDead.Value && !playerInputProvider.IsJump.Value)
            .Subscribe(_ =>
            {
                animator.SetTrigger("rollRight");
            });
    }
}
