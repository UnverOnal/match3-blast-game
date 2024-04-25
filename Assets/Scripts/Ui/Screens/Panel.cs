using DG.Tweening;
using UnityEngine;

namespace UI.Screens
{
    public class Panel
    {
        private readonly CanvasGroup _canvasGroup;
        private readonly GameObject _panelGameObject;

        public Panel(CanvasGroup canvasGroup, GameObject panelGameObject)
        {
            _canvasGroup = canvasGroup;
            _panelGameObject = panelGameObject;
        }

        public void EnablePanel(float duration = 0.5f, bool instant = false)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
            _panelGameObject.SetActive(true);

            if (!instant)
                _canvasGroup.DOFade(1f, duration);
            else
                _canvasGroup.alpha = 1f;
        }

        public void DisablePanel(float duration = 0.5f, bool instant = false)
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;

            if (!instant)
                _canvasGroup.DOFade(0f, duration).OnComplete(() =>
                {
                    _canvasGroup.alpha = 0f;
                    _panelGameObject.SetActive(false);
                });
            else
            {
                _canvasGroup.alpha = 0f;
                _panelGameObject.SetActive(false);
            }
        }
    }
}