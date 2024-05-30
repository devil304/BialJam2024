public partial class GameManager
{
    public class StatsModel
    {
        public float Code { get; private set; }
        public float Design { get; private set; }
        public float Art { get; private set; }
        public float Audio { get; private set; }
        public float QA { get; private set; }

        public StatsModel((float, float, float, float, float) stats)
        {
            Code = stats.Item1;
            Design = stats.Item2;
            Art = stats.Item3;
            Audio = stats.Item4;
            QA = stats.Item5;
        }

        public StatsModel()
        {
            Reset();
        }

        public void Reset()
        {
            Code = 0;
            Design = 0;
            Art = 0;
            Audio = 0;
            QA = 0;
        }

        public void StatsModify((float, float, float, float, float) statsMod)
        {
            Code += statsMod.Item1;
            Design += statsMod.Item2;
            Art += statsMod.Item3;
            Audio += statsMod.Item4;
            QA += statsMod.Item5;
        }

        public void StatsModify(StatsModel statsMod)
        {
            Code += statsMod.Code;
            Design += statsMod.Design;
            Art += statsMod.Art;
            Audio += statsMod.Audio;
            QA += statsMod.QA;
        }
    }
}
