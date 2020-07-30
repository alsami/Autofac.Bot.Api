using System.Text;
using System.Threading.Tasks;
using Autofac.Bot.Api.Controllers.Presentation;
using Autofac.Bot.Api.Controllers.Services;
using Autofac.Bot.Api.UseCases.Abstractions.Commands;
using Autofac.Bot.Api.UseCases.Abstractions.Enums;
using Autofac.Bot.Api.UseCases.Abstractions.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Autofac.Bot.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/benchmarks")]
    public class BenchmarksController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ExecuteAsync(
            [FromServices] IMediator mediator,
            [FromBody] BenchmarkRequestDto benchmarkRequest)
        {
            var targetExecutionCommand = new ExecuteBenchmarkCommand(benchmarkRequest.Benchmark,
                new Repository(benchmarkRequest.TargetRepository.Ref, benchmarkRequest.TargetRepository.Url),
                RepositoryTarget.Target);

            var targetResult = await mediator.Send(targetExecutionCommand);

            var sourceExecutionCommand = new ExecuteBenchmarkCommand(benchmarkRequest.Benchmark,
                new Repository(benchmarkRequest.SourceRepository.Ref, benchmarkRequest.SourceRepository.Url),
                RepositoryTarget.Source);

            var sourceResult = await mediator.Send(sourceExecutionCommand);

            var bytes = MarkdownGenerator.Generate(benchmarkRequest.Benchmark, targetResult, sourceResult);

            return File(Encoding.UTF8.GetBytes(bytes), "text/html");
        }
    }
}