namespace GreedyAlgorithms.MST
{
    public class LoadEdge
    {
        public LoadEdge(int first, int second, decimal cost)
        {
            Cost = cost;
            From = first;
            To = second;
        }

        public int From { get; set; }
        public int To { get; set; }
        public decimal Cost { get; set; }
    }
}