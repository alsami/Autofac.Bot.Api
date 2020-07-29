namespace Autofac.Bot.Api.Services.Results
{
    public class BenchmarkRunnerResult
    {
        public BenchmarkRunnerResult(bool succeeded, string output)
        {
            Succeeded = succeeded;
            Output = output;
        }

        public bool Succeeded { get; }

        public string Output { get; }
    }
}