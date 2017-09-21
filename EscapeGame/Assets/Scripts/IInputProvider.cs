using UniRx;
using UnityEngine;

public interface IInputProvider
{
    IReadOnlyReactiveProperty<bool> IsJumpButton { get; }
    IReadOnlyReactiveProperty<bool> IsAttackButton { get; }
    IReadOnlyReactiveProperty<Vector3> CharacterMoveDirection { get; }
    IReadOnlyReactiveProperty<Vector3> CameraMoveDirection { get; } 
}

public interface IAdvancedInputProvider : IInputProvider
{
    Subject<Vector3> DashDirection { get; }
    IReadOnlyReactiveProperty<bool> IsDefenceButton { get; }
}