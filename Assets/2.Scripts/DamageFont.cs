using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class DamageFont : MonoBehaviour
{
    [Tooltip("UI�� ���� �ö� �Ÿ��Դϴ�.")]
    public float MoveDistance = 1f;
    [Tooltip("UI�� �̵��ϰ� ������� �� �ɸ��� �� �ð��Դϴ�.")]
    public float Duration = 1f;
    [Tooltip("�ִϸ��̼��� ������ ����� �����մϴ�.")]
    public Ease MoveEase = Ease.OutQuad;

    RectTransform _rectTransform;
    CanvasGroup _canvasGroup;
    PoolObject _pool;
    TextMeshProUGUI _textMeshProUGUI;


    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _pool = GetComponent<PoolObject>();
        _textMeshProUGUI = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    public void SetDamageFont(float damage, Transform damagedObject)
    {
        _textMeshProUGUI.SetText(damage.ToString());
        _rectTransform.anchoredPosition = damagedObject.position;
        StartEffect();
    }

    void StartEffect()
    {
        _canvasGroup.alpha = 1f;

        Sequence mySequence = DOTween.Sequence();


        mySequence.Join(_rectTransform.DOAnchorPosY(_rectTransform.anchoredPosition.y + MoveDistance, Duration).SetEase(MoveEase));
        mySequence.Join(_canvasGroup.DOFade(0, Duration).SetEase(Ease.Linear));
        mySequence.OnComplete(() =>
        {
            _pool.RelasePool();
        });
    }
}
