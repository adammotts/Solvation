namespace Solvation.Models
{
    public class Actions
    {
        public double Hit;
        public double Stand;
        public double Double;
        public double Split;

        public Actions(double hitExpectedValue, double standExpectedValue, double doubleExpectedValue, double splitExpectedValue)
        {
            this.Hit = hitExpectedValue;
            this.Stand = standExpectedValue;
            this.Double = doubleExpectedValue;
            this.Split = splitExpectedValue;
        }

        public override string ToString()
        {
            return $"[Hit: {this.Hit}, Stand: {this.Stand}, Double: {this.Double}, Split: {this.Split}]";
        }
    }
}