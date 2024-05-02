namespace GamePlay.Mediator
{
    public class Colleague
    {
        protected MatchMediator matchMediator;
        
        public void SetMediator(MatchMediator mediator) => matchMediator = mediator;
    }
}
