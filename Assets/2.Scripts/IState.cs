public interface IState
{
    // 상태에 진입했을 때 한 번 호출
    void Enter();
    // 상태가 활성화된 동안 매 프레임 호출
    void Update();
    // 상태를 빠져나갈 때 한 번 호출
    void Exit();
}