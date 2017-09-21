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
    float jumpPower;
    [SerializeField]
    float dashSpeed;

    /// <summary>
    /// ジャンプのクールダウン
    /// </summary>
    public float JumpCooldown;
    /// <summary>
    /// Dashのクールダウン
    /// </summary>
    public float DashCooldown;

    /// <summary>
    /// 移動方向のリアクティブプロパティ
    /// </summary>
    public IReadOnlyReactiveProperty<Vector3> MoveDirection => moveDirection;
    /// <summary>
    /// ダッシュ方向のリアクティブプロパティ
    /// </summary>
    public Subject<Vector3> DashDirection = new Subject<Vector3>();
    /// <summary>
    /// ジャンプのリアクティブプロパティ
    /// </summary>
    public IReadOnlyReactiveProperty<bool> IsJumping => isJumping;

    Vector3ReactiveProperty moveDirection = new Vector3ReactiveProperty();
    BoolReactiveProperty isJumping = new BoolReactiveProperty(false);

    Rigidbody rb;
    PlayerInput playerInput;

    bool isDash = false;

    bool isInDashCooldown, isInJumpCooldown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        // 移動量の設定
        playerInput.CharacterMoveDirection.Subscribe(v => moveDirection.Value = v);

        // ジャンプ
        playerInput.IsJumpButton.Where(j => j && !isJumping.Value).Where(_=> !isInJumpCooldown).Subscribe(_ =>
         {
             isJumping.Value = true;
             isInJumpCooldown = true;
             rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
             Observable.Timer(TimeSpan.FromSeconds(JumpCooldown)).Subscribe(__ => isInJumpCooldown = false);
         });

        // ダッシュ
        playerInput.DashDirection.Where(d => d != Vector3.zero).Where(d=> !isInDashCooldown).Subscribe(d =>
        {
            // ダッシュ中かどうか
            isDash = true;
            // クールダウンに入ってるかどうか 
            isInDashCooldown = true;
            rb.velocity = new Vector3(d.x * -dashSpeed, rb.velocity.y, d.z * -dashSpeed);
            DashDirection.OnNext(d);
            Observable.Timer(TimeSpan.FromSeconds(DashCooldown)).Subscribe(_ => isInDashCooldown = false);
            Observable.Timer(TimeSpan.FromSeconds(0.2f)).Subscribe(_ => isDash = false);
        });

        // 移動
        this.FixedUpdateAsObservable().Where(_=> !isDash).Subscribe(_ => {
            rb.velocity = new Vector3(moveDirection.Value.x* -movementSpeed, rb.velocity.y, moveDirection.Value.z * -movementSpeed);
        });

        //着地判定
        this.OnCollisionEnterAsObservable().Where(_=> IsJumping.Value).Subscribe(c => isJumping.Value = c.gameObject.tag != "Ground");
    }
}
