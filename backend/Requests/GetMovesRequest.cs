namespace Solvation.Requests
{
    public class GetMovesRequest
    {
        public required string[] PlayerCards { get; init; }

        public required string DealerCard { get; init; }
    }
}
