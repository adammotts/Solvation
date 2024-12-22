namespace Solvation.Models
{
    public class Statistics
    {
        public int BestMoves { get; set; } = 0;

        public int Inaccuracies { get; set; } = 0;

        public int Mistakes { get; set; } = 0;

        public int Blunders { get; set; } = 0;

        public Statistics Update(string label)
        {
            switch (label)
            {
                case "Best Move":
                    this.BestMoves++;
                    break;
                case "Inaccuracy":
                    this.Inaccuracies++;
                    break;
                case "Mistake":
                    this.Mistakes++;
                    break;
                case "Blunder":
                    this.Blunders++;
                    break;
                default:
                    throw new System.ArgumentException("Invalid label");
            }

            return this;
        }
    }   
}