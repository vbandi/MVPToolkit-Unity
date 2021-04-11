namespace LightswitchExample
{
    public class MainModel
    {
        private static MainModel _instance;
        public static MainModel Instance => _instance ?? (_instance = new MainModel());

#if UNITY_EDITOR
        public static MainModel CreateInstanceForTesting() => new MainModel();
#endif


        public readonly LightswitchModel Lightswitch1 = new LightswitchModel();
        public readonly LightswitchModel Lightswitch2 = new LightswitchModel();
        public readonly LightModel Light;

        private MainModel()
        {
            Light = new LightModel(this);
        }
    }
}


