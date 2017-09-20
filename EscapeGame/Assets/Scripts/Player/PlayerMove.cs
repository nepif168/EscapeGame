using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class PlayerMove : MonoBehaviour {

    [SerializeField]
    float movementSpeed;

    [SerializeField]
    float cameraSpeed;

    [SerializeField]
    float jumpPower;

    [SerializeField]
    float dashPower = 5;

    PlayerInputProvider playerInputProvider;
    DashInputProvider dashInputProvider;
    PlayerCore playerCore;
    Rigidbody rb;

    private void Awake()
    {
        playerInputProvider = GetComponent<PlayerInputProvider>();
        dashInputProvider = GetComponent<DashInputProvider>();
        playerCore = GetComponent<PlayerCore>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        this.FixedUpdateAsObservable().Where(_ => !playerCore.IsDead.Value).Subscribe(_ =>
        {
            Vector3 dir = transform.rotation * playerInputProvider.CharacterMoveDirection.Value.normalized * -movementSpeed;
            rb.velocity = new Vector3(dir.x, rb.velocity.y, dir.z);
        });
    

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

        dashInputProvider.wDoubleTapStream
             .Where(_ => !playerCore.IsDead.Value)
             .Subscribe(_ =>
             {
             });

        this.OnCollisionEnterAsObservable().Where(t => t.gameObject.tag == "Ground").Subscribe(_ => playerInputProvider.Land());
    }
}
