using VContainer;
using VContainer.Unity;

namespace Services.InputService
{
    public class InputEntryPoint : ITickable
    {
        [Inject] private InputService _inputService;
        
        public void Tick()
        {
            _inputService.Update();
        }
    }
}
