using TMPro;

namespace MoveManagement
{
    public class MoveView
    {
        private readonly TextMeshProUGUI _moveCountText;

        public MoveView(TextMeshProUGUI moveCountText)
        {
            _moveCountText = moveCountText;
        }

        public void SetMoveCount(int count)
        {
            if (count < 0)
                return;

            _moveCountText.text = count.ToString();
        }

        public void Start() => _moveCountText.gameObject.SetActive(true);
        public void Stop() => _moveCountText.gameObject.SetActive(false);
    }
}