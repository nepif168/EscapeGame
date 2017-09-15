using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
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
}
