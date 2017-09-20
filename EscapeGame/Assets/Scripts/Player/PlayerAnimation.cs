using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerAnimation : MonoBehaviour {

    PlayerCore playerCore;
    IInputProvider playerInput;
    Animator animator;

    private void Awake()
    {
        playerCore = GetComponent<PlayerCore>();
        playerInput = GetComponent<IInputProvider>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerInput.IsJump.Where(j => j).Subscribe(_ => animator.speed = 3);
        playerInput.IsJump.Where(j => !j).Subscribe(_ => animator.speed = 1);
        playerCore.IsDead.Where(d => d).Subscribe(_ => animator.SetBool("dead", true));
    }
}
