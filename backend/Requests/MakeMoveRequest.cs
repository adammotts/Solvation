namespace Solvation.Requests
{
    public class MakeMoveRequest
    {
        public string Move { get; init; }

        public MakeMoveRequest(string move)
        {
            Move = move;
        }
    }
}
