using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IAdvancedInputProvider
{
    public Subject<Vector3> DashDirection { get; } = new Subject<Vector3>();
    public IReadOnlyReactiveProperty<bool> IsJumpButton => isJumpButton;
    public IReadOnlyReactiveProperty<bool> IsAttackButton => isAttackButton;
    public IReadOnlyReactiveProperty<Vector3> CharacterMoveDirection => characterMoveDirection;
    public IReadOnlyReactiveProperty<Vector3> CameraMoveDirection => cameraMoveDirection;
    public IReadOnlyReactiveProperty<bool> IsDefenceButton => IsDefenceButton;

    Vector3ReactiveProperty characterMoveDirection = new Vector3ReactiveProperty();
    Vector3ReactiveProperty cameraMoveDirection = new Vector3ReactiveProperty();
    ReactiveProperty<bool> isJumpButton;
    ReactiveProperty<bool> isAttackButton;
    ReactiveProperty<bool> isDefenceButton;

    private void Start()
    {
        // キャラクターの移動方向CharacterMoveDirection
        this.UpdateAsObservable().Subscribe(_ =>
            characterMoveDirection.Value = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized);

        // カメラの移動量
        this.UpdateAsObservable().Subscribe(_ =>
            cameraMoveDirection.Value = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0));

        // ジャンプボタン
        isJumpButton = this.UpdateAsObservable().Select(_ => Input.GetKeyDown(KeyCode.Space)).ToReactiveProperty();

        // アタックボタン
        isAttackButton = this.UpdateAsObservable().Select(_ => Input.GetMouseButtonDown(0)).ToReactiveProperty();

        // ディフェンスボタン
        isDefenceButton = this.UpdateAsObservable().Select(_ => Input.GetMouseButton(1)).ToReactiveProperty();

        // ダッシュボタン
        // ダブルタップと感知する時間
        double doubleTapInterval = 250d;
        // W
        this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.W)).TimeInterval()
            .Select(t => t.Interval.TotalMilliseconds).Buffer(2, 1)
            .Where(list => list[0] > doubleTapInterval).Where(list => list[1] <= doubleTapInterval)
            .Subscribe(_ => DashDirection.OnNext(Vector3.forward));
        // A
        this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.A)).TimeInterval()
            .Select(t => t.Interval.TotalMilliseconds).Buffer(2, 1)
            .Where(list => list[0] > doubleTapInterval).Where(list => list[1] <= doubleTapInterval)
            .Subscribe(_ => DashDirection.OnNext(Vector3.left));
        // S
        this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.S)).TimeInterval()
            .Select(t => t.Interval.TotalMilliseconds).Buffer(2, 1)
            .Where(list => list[0] > doubleTapInterval).Where(list => list[1] <= doubleTapInterval)
            .Subscribe(_ => DashDirection.OnNext(Vector3.back));
        // D
        this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.D)).TimeInterval()
            .Select(t => t.Interval.TotalMilliseconds).Buffer(2, 1)
            .Where(list => list[0] > doubleTapInterval).Where(list => list[1] <= doubleTapInterval)
            .Subscribe(_ => DashDirection.OnNext(Vector3.right));
    }
}
