namespace PSY.Innovative
{
    public enum AppState
    {
        Resumed,
        Suspended
    }

    public static class Innovative
    {
        static Innovative()
        {
            AppState = AppState.Resumed;
        }

        public static AppState AppState { get; set; }
    }
}