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
        Debug.Log("무브진입");
        _cooltime = 0;
    }

    public void Update()
    {
        // 이동
        _enemy.Move();
        // Ray 쿨타임
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

        // 1) 내 머리위 몬스터 존재 확인 후 점프 가능판정
        bool myHasMonsterAbove = _enemy.HasMonsterAbove();
        if (myHasMonsterAbove) return;

        // 2) 전방 몬스터 체크 없다면 리턴
        EnemyAI target = _enemy.FindMonsterAhead();
        if (target == null) return;

        // 3) 타겟 머리위 몬스터 존재 확인 후 점프 가능판정
        bool targetHasMonsterAbove = target.HasMonsterAbove();
        if (targetHasMonsterAbove) return;

        // 4) 여기까지 도달하면 점프 가능, 실제 점프
        _enemy.JumpTarget = target;
        _enemy.ChangeState(_enemy.JumpState);
    }
}