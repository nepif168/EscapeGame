﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerMove : MonoBehaviour {

    [SerializeField]
    float movementSpeed;

    [SerializeField]
    float cameraSpeed;

    [SerializeField]
    float jumpPower;

    PlayerInputProvider playerInputProvider;
    PlayerCore playerCore;
    Rigidbody rb;

    private void Awake()
    {
        playerInputProvider = GetComponent<PlayerInputProvider>();
        playerCore = GetComponent<PlayerCore>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        this.FixedUpdateAsObservable().Where(_=>!playerCore.IsDead.Value).Subscribe(_ =>
            rb.AddForce(transform.rotation *playerInputProvider.CharacterMoveDirection.Value * -movementSpeed, ForceMode.Impulse));

        playerInputProvider.CharacterMoveDirection
            .Where(_ => !playerCore.IsDead.Value)
            .Where(x => x == Vector3.zero && !playerInputProvider.IsJump.Value).Subscribe(_ => rb.velocity = Vector3.zero);

        playerInputProvider.IsJump
            .Where(_ => !playerCore.IsDead.Value)
            .Where(j => j).Subscribe(_ =>
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse));

        playerInputProvider.IsJump
            .Where(_ => !playerCore.IsDead.Value)
            .Where(j => !j).Subscribe(_ => rb.velocity = Vector3.zero);
        

        this.OnCollisionEnterAsObservable().Where(t => t.gameObject.tag == "Ground").Subscribe(_ => playerInputProvider.Land());
    }
}
