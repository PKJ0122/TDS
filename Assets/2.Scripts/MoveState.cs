using UnityEngine;

public class MoveState : IState
{
    const float COOL_TIME = 0.5f;

    readonly EnemyAI _enemy;
    float _cooltime;


    public MoveState(EnemyAI enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("��������");
        _cooltime = 0;
    }

    public void Update()
    {
        // �̵�
        _enemy.Move();
        // Ray ��Ÿ��
        _cooltime += Time.deltaTime;

        if (_cooltime < COOL_TIME) return;

        TryJump();
    }

    public void Exit()
    {

    }

    void TryJump()
    {
        _cooltime -= COOL_TIME;

        // 1) �� �Ӹ��� ���� ���� Ȯ�� �� ���� ��������
        bool myHasMonsterAbove = _enemy.HasMonsterAbove();
        if (myHasMonsterAbove) return;

        // 2) ���� ���� üũ ���ٸ� ����
        EnemyAI target = _enemy.FindMonsterAhead();
        if (target == null) return;

        // 3) Ÿ�� �Ӹ��� ���� ���� Ȯ�� �� ���� ��������
        bool targetHasMonsterAbove = target.HasMonsterAbove();
        if (targetHasMonsterAbove) return;

        // 4) ������� �����ϸ� ���� ����, ���� ����
        _enemy.JumpTarget = target;
        _enemy.ChangeState(_enemy.JumpState);
    }
}