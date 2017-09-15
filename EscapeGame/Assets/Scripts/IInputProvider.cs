using UniRx;
using UnityEngine;

public interface IInputProvider
{
    IReadOnlyReactiveProperty<bool> IsJump { get; }
    IReadOnlyReactiveProperty<bool> IsAttack { get; }
    IReadOnlyReactiveProperty<Vector3> CharacterMoveDirection { get; }
    IReadOnlyReactiveProperty<Vector3> CameraMoveDirection { get; } 
}