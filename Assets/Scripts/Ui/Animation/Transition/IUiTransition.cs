using Cysharp.Threading.Tasks;

namespace Ui.Animation.Transition
{
    public interface IUiTransition
    {
        UniTask Enable();
        UniTask Disable();
    }
}