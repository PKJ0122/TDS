public interface IState
{
    // ���¿� �������� �� �� �� ȣ��
    void Enter();
    // ���°� Ȱ��ȭ�� ���� �� ������ ȣ��
    void Update();
    // ���¸� �������� �� �� �� ȣ��
    void Exit();
}