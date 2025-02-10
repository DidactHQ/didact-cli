namespace DidactCli.Services
{
    public class AppSettings
    {
        public required string TestSetting { get; set; }

        public required DidactEngine DidactEngine { get; set; }

        public required DidactUi DidactUi { get; set; }
    }

    public class DidactEngine
    {
        public required string RuntimeEnvironmentVariablesFileName { get; set; }
    }

    public class DidactUi
    {
        public required string RuntimeEnvironmentVariablesFileName { get; set; }
    }
}