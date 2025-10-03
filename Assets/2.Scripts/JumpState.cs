using UnityEngine;

public class JumpState : IState
{
    readonly EnemyAI _enemy;

    public JumpState(EnemyAI enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("점프 진입");
        _enemy.Jump();
    }

    public void Update()
    {
        // 포물선 운동 최고점 도달 or Y축 운동 멈춤시 상태변경
        if (_enemy.Rb.velocity.y < 0)
        {
            _enemy.ChangeState(_enemy.MoveState);
        }
    }

    public void Exit()
    {

    }
}
