
using UnityEngine;
using DG.Tweening; // DoTween 사용을 위한 네임스페이스 추가

public class MovingImage : MonoBehaviour
{
    public RectTransform targetBGImg; // 이동시킬 이미지의 RectTransform 참조
    public RectTransform targetStarImg;
    public float scrollDuration = 7.0f; // 스크롤 소요 시간

    void Start()
    {
        // 두 이미지의 초기 위치 설정
        targetBGImg.anchoredPosition = new Vector2(-1080f, 0f);
        targetStarImg.anchoredPosition = new Vector2(0f, 0f);

        // 무한 스크롤 애니메이션 시작
        MoveImages();
    }

    // 무한 스크롤 애니메이션 함수
    void MoveImages()
    {
        // 첫 번째 이미지 이동 애니메이션
        Sequence bgSequence = DOTween.Sequence();
        bgSequence.Append(targetBGImg.DOAnchorPosX(1080f, scrollDuration).SetEase(Ease.Linear));
        bgSequence.Join(targetStarImg.DOAnchorPosX(0f, scrollDuration).SetEase(Ease.Linear));

        // 두 번째 이미지 이동 애니메이션
        Sequence starSequence = DOTween.Sequence();
        starSequence.Append(targetStarImg.DOAnchorPosX(1080f, scrollDuration).SetEase(Ease.Linear));
        starSequence.Join(targetBGImg.DOAnchorPosX(0f, scrollDuration).SetEase(Ease.Linear));

        // 두 애니메이션을 무한 반복
        bgSequence.SetLoops(-1);
        starSequence.SetLoops(-1);
    }
}