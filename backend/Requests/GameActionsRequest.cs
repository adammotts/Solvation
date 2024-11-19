using Solvation.Enums;
using Solvation.Models;

namespace Solvation.Requests
{
    public class GameActionsRequest
    {
        public double? Hit { get; set; }
        public double? Stand { get; set; }
        public double? Double { get; set; }
        public double? Split { get; set; }
    }
}