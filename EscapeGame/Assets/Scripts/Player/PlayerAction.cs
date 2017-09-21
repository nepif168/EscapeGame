using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class PlayerAction : MonoBehaviour {

    /// <summary>
    /// 攻撃のクールダウン
    /// </summary>
    public float AttackCooldown;
    /// <summary>
    /// 守り（無敵）のクールダウン
    /// </summary>
    public float DefenceCooldown;
    bool isInAttackCooldown = false, isInDefenceCooldown = false;
    PlayerInput playerInput;

    private void Awake()
    {
        playerInput.GetComponent<PlayerInput>();
    }

    private void Start()
    {
        playerInput.IsAttackButton.Where(a => a).Where(_=> !isInAttackCooldown).Subscribe(_ =>
        {
            isInAttackCooldown = true;
            // TODO: 攻撃処理
            Observable.Timer(TimeSpan.FromSeconds(AttackCooldown)).Subscribe(__ => isInAttackCooldown = false);
        });

        playerInput.IsDefenceButton.Where(d => d).Where(_ => !isInAttackCooldown).Subscribe(_ =>
        {
            isInDefenceCooldown = true;
            // TODO: ディフェンス処理
            // ex: var go = Instantiate(barrier, transform); 下のクールダウン解消のとこに Destroy(go);
            // ex: in barrier objectでIDamageableを継承 TakeDamageの中身を何も書かない -> 無敵
            Observable.Timer(TimeSpan.FromSeconds(DefenceCooldown)).Subscribe(__ => isInDefenceCooldown = false);
        });
    }
}
