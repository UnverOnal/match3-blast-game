namespace GamePlay.Mediator
{
    public class Colleague
    {
        protected MoveMediator moveMediator;
        
        public void SetMediator(MoveMediator mediator) => moveMediator = mediator;
    }
}
