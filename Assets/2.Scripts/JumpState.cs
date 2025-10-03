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
        Debug.Log("���� ����");
        _enemy.Jump();
    }

    public void Update()
    {
        // ������ � �ְ��� ���� or Y�� � ����� ���º���
        if (_enemy.Rb.velocity.y < 0)
        {
            _enemy.ChangeState(_enemy.MoveState);
        }
    }

    public void Exit()
    {

    }
}
