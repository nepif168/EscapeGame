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

    bool isDash = false;

    private void Awake()
    {
        playerInputProvider = GetComponent<PlayerInputProvider>();
        dashInputProvider = GetComponent<DashInputProvider>();
        playerCore = GetComponent<PlayerCore>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        this.FixedUpdateAsObservable().Where(_ => !playerCore.IsDead.Value && !isDash).Subscribe(_ =>
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

        //
        // 以下PlayerMoveから条件丸々パクってきた
        //

        // 前方ダッシュ
        dashInputProvider.wDoubleTapStream
             .Where(_ => !playerCore.IsDead.Value && !playerInputProvider.IsJump.Value)
             .Subscribe(_ =>
             {
                 isDash = true;
                 rb.velocity = Vector3.forward * -dashPower;
                 Observable.Timer(TimeSpan.FromSeconds(0.2f)).Subscribe(__ => isDash = false);
             });

        // 左方ダッシュ
        dashInputProvider.aDoubleTapStream
             .Where(_ => !playerCore.IsDead.Value && !playerInputProvider.IsJump.Value)
            .Subscribe(_ =>
            {
                isDash = true;
                rb.velocity = Vector3.left * -dashPower;
                 Observable.Timer(TimeSpan.FromSeconds(0.2f)).Subscribe(__ => isDash = false);
            });

        // 後方ダッシュ
        dashInputProvider.sDoubleTapStream
            .Where(_ => !playerCore.IsDead.Value && !playerInputProvider.IsJump.Value)
            .Subscribe(_ =>
            {
                isDash = true;
               rb.velocity = Vector3.back * -dashPower;
                Observable.Timer(TimeSpan.FromSeconds(0.2f)).Subscribe(__ => isDash = false);
            });

        // 右方ダッシュ
        dashInputProvider.dDoubleTapStream
            .Where(_ => !playerCore.IsDead.Value && !playerInputProvider.IsJump.Value)
            .Subscribe(_ =>
            {
                isDash = true;
                rb.velocity = Vector3.right * -dashPower;
                Observable.Timer(TimeSpan.FromSeconds(0.3f)).Subscribe(__ => isDash = false);
            });

        this.OnCollisionEnterAsObservable().Where(t => t.gameObject.tag == "Ground").Subscribe(_ => playerInputProvider.Land());
    }
}
