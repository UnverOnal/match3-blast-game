using Services.PoolingService;
using VContainer;

namespace Services.CommandService
{
    public class CommandService : ICommandService
    {
        private readonly IPoolService _poolService;
        private readonly ObjectPool<CommandInvoker> _invokerPool;

        [Inject]
        public CommandService(IPoolService poolService)
        {
            _poolService = poolService;

            _invokerPool = poolService.GetPoolFactory().CreatePool(() => new CommandInvoker());
        }

        public CommandInvoker GetCommandInvoker()
        {
            return _invokerPool.Get();
        }

        public void ReturnInvoker(CommandInvoker invoker)
        {
            invoker.Reset();
            _invokerPool.Return(invoker);
        }
    }
}