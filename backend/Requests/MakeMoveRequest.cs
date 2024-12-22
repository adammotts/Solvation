namespace Solvation.Requests
{
    public class MakeMoveRequest
    {
        public string Move { get; init; }

        public string Label { get; init; }

        public MakeMoveRequest(string move, string label)
        {
            Move = move;
            Label = label;
        }
    }
}
