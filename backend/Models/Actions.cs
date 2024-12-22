namespace Solvation.Models
{
    public class Actions
    {
        public double? Hit { get; set; }
        public double? Stand { get; set; }
        public double? Double { get; set; }
        public double? Split { get; set; }

        public Actions(double? hitExpectedValue, double? standExpectedValue, double? doubleExpectedValue, double? splitExpectedValue)
        {
            this.Hit = hitExpectedValue;
            this.Stand = standExpectedValue;
            this.Double = doubleExpectedValue;
            this.Split = splitExpectedValue;
        }

        public double BestMoveEV()
        {
            double bestMoveEV = double.MinValue;
            if (this.Hit != null && this.Hit > bestMoveEV)
            {
                bestMoveEV = (double)this.Hit;
            }
            if (this.Stand != null && this.Stand > bestMoveEV)
            {
                bestMoveEV = (double)this.Stand;
            }
            if (this.Double != null && this.Double > bestMoveEV)
            {
                bestMoveEV = (double)this.Double;
            }
            if (this.Split != null && this.Split > bestMoveEV)
            {
                bestMoveEV = (double)this.Split;
            }
            return bestMoveEV;
        }

        public double MoveEV(string move)
        {
            double defaultEV = double.MinValue;
            
            switch (move)
            {
                case "hit":
                    return this.Hit ?? defaultEV;
                case "stand":
                    return this.Stand ?? defaultEV;
                case "double":
                    return this.Double ?? defaultEV;
                case "split":
                    return this.Split ?? defaultEV;
                default:
                    throw new System.ArgumentException("Invalid move");
            }
        }

        public override string ToString()
        {
            return $"[Hit: {(this.Hit != null ? this.Hit.ToString() : "Invalid")}, Stand: {(this.Stand != null ? this.Stand.ToString() : "Invalid")}, Double: {(this.Double != null ? this.Double.ToString() : "Invalid")}, Split: {(this.Split != null ? this.Split.ToString() : "Invalid")}]";
        }
    }
}