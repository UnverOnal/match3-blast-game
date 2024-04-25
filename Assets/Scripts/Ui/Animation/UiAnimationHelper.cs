using DG.Tweening;
using UnityEngine;

namespace Ui.Animation
{
    public static class UiAnimationHelper
    {
        public static void FadeIn(float duration, CanvasGroup canvasGroup)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;

            canvasGroup.DOFade(1f, duration);
        }

        public static void FadeOut(float duration, CanvasGroup canvasGroup)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;

            canvasGroup.DOFade(0f, duration).OnComplete(() =>
            {
                canvasGroup.alpha = 0f;
                canvasGroup.gameObject.SetActive(false);
            });
        }
    }
}
