using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerInputProvider : MonoBehaviour, IInputProvider
{
    public IReadOnlyReactiveProperty<bool> IsJump => isJump;
    public IReadOnlyReactiveProperty<bool> IsAttack => isAttack;
    public IReadOnlyReactiveProperty<Vector3> CharacterMoveDirection => characterMoveDirection;
    public IReadOnlyReactiveProperty<Vector3> CameraMoveDirection => cameraMoveDirection;

    BoolReactiveProperty isJump = new BoolReactiveProperty(false);
    BoolReactiveProperty isAttack = new BoolReactiveProperty(false);
    Vector3ReactiveProperty characterMoveDirection = new Vector3ReactiveProperty();
    Vector3ReactiveProperty cameraMoveDirection = new Vector3ReactiveProperty();

    private void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ => characterMoveDirection.Value = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized);

        this.UpdateAsObservable().Where(_ => isJump.Value == false && Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ => isJump.Value = true);

        this.UpdateAsObservable()
            .Subscribe(_ => cameraMoveDirection.Value = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0));
    }

    public void Land()
    {
        isJump.Value = false;
    }
}
