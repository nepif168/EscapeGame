using UniRx;
using UnityEngine;

public interface IInputProvider
{
    IReadOnlyReactiveProperty<bool> IsJump { get; }
    IReadOnlyReactiveProperty<bool> IsAttack { get; }
    IReadOnlyReactiveProperty<Vector3> CharacterMoveDirection { get; }
    IReadOnlyReactiveProperty<Vector3> CameraMoveDirection { get; } 
}

public interface IAdvancedInputProvider : IInputProvider
{
    IReadOnlyReactiveProperty<Vector3> DashDirection { get; }
}