using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyAI : MonoBehaviour
{
    [Tooltip("몬스터 이동속도")]
    public float moveSpeed = 2f;
    [Tooltip("몬스터 탐지범위")]
    public float detectionRange = 0.5f;

    public LayerMask TargetLayer { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public Collider2D Cd { get; private set; }
    public EnemyAI JumpTarget { get; set; }

    public MoveState MoveState { get; set; }
    public JumpState JumpState { get; set; }

    SpriteRenderer[] _srs;
    IState _currentState;
    Vector2 _baseOffset;


    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Cd = GetComponent<Collider2D>();
        _srs = GetComponentsInChildren<SpriteRenderer>();
        _baseOffset = Cd.offset;
        StateCacheCreate();

        moveSpeed = Random.Range(moveSpeed * 0.8f, moveSpeed * 1.5f);
    }

    void OnEnable()
    {
        ChangeState(MoveState);
    }

    void FixedUpdate()
    {
        _currentState?.Update();
    }

    /// <summary>
    /// 라인 설정
    /// </summary>
    /// <param name="laneIndex">라인 인덱스</param>
    public void SetLane(int laneIndex)
    {
        string laneLayerName = $"{EnemySpawner.LANE_LAYER_NAME}{laneIndex}";
        float laneOffsetY = EnemySpawner.Instance.laneOffsetY;

        int laneLayer = LayerMask.NameToLayer(laneLayerName);

        if (laneLayer == -1) return;

        Cd.offset = _baseOffset + new Vector2(0, -laneIndex * laneOffsetY);
        gameObject.layer = laneLayer;
        TargetLayer = 1 << laneLayer;

        foreach (SpriteRenderer sr in _srs)
        {
            sr.sortingLayerName = laneLayerName;
        }
    }

    /// <summary>
    /// 상태 변경
    /// </summary>
    /// <param name="newState">변경 상태</param>
    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    #region 상태머신 행동
    public void Move()
    {
        Rb.velocity = new Vector2(-moveSpeed, Rb.velocity.y);
    }

    public void Jump()
    {
        if (JumpTarget == null) return;

        Collider2D targetCol = JumpTarget.Cd;

        // 발끝과 머리위 좌표
        Vector2 startPos = new(Cd.bounds.center.x, Cd.bounds.min.y);
        Vector2 targetPos = new(targetCol.bounds.center.x, targetCol.bounds.max.y + 0.1f);

        float g = Physics2D.gravity.magnitude * Rb.gravityScale;

        // 목표 지점까지 상대 좌표
        float diffX = targetPos.x - startPos.x;
        float diffY = targetPos.y - startPos.y;

        // 원하는 비행 시간 (짧을수록 빠른 점프)
        float desiredTime = 0.3f;

        // 속도 계산
        float Vx = diffX / desiredTime;
        float Vy = (diffY + 0.5f * g * desiredTime * desiredTime) / desiredTime;

        Vector2 velocity = new(Vx, Vy);
        Rb.AddForce(velocity * Rb.mass, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 머리 위 몬스터 존재 여부 확인
    /// </summary>
    /// <returns>존재 여부</returns>
    public bool HasMonsterAbove()
    {
        Vector2 headPos = new(Cd.bounds.center.x, Cd.bounds.max.y + 0.01f);
        float checkDistance = Cd.bounds.size.y * 0.5f;
        RaycastHit2D hitOnHead = Physics2D.Raycast(headPos, Vector2.up, checkDistance, TargetLayer);
        if (hitOnHead.collider != null && hitOnHead.collider.gameObject != gameObject)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 전방 몬스터 찾기
    /// </summary>
    /// <returns>전방 몬스터</returns>
    public EnemyAI FindMonsterAhead()
    {
        Vector2 bodyPos = new(Cd.bounds.min.x - 0.01f, Cd.bounds.center.y);
        RaycastHit2D hit = Physics2D.Raycast(bodyPos, Vector2.left, detectionRange, TargetLayer);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.TryGetComponent(out EnemyAI hitEnemy))
            {
                return hitEnemy;
            }
        }

        return null;
    }
    #endregion

    /// <summary>
    /// 상태머신 캐싱
    /// </summary>
    void StateCacheCreate()
    {
        MoveState = new MoveState(this);
        JumpState = new JumpState(this);
    }
}