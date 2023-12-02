using AdventOfCode23.Days;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode23
{
    public class AdventRunnerService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _host;

        private Dictionary<AdventDayAttribute, IAdventDay> _days;
        private Dictionary<AdventDayAttribute, string> _outputs;

        public AdventRunnerService(ILogger<AdventRunnerService> logger, IServiceProvider services, IHostApplicationLifetime host) 
        {
            _logger = logger;
            _host = host;

            _logger.LogInformation("Initializing Advent Runner Service...");

            // Initialize dictionary
            _days = new();
            _outputs = new();

            _logger.LogInformation("Gathering all IAdventDay instances that qualify...");
            // Get all days that have been properly qualified
            var dayTypes = Assembly.GetExecutingAssembly().GetTypes()
                // Needs IAdventDay interface and AdventDayAttribute
                .Where(x => x.IsAssignableTo(typeof(IAdventDay)) && x.GetCustomAttribute<AdventDayAttribute>() != null)
                // Needs exaclty 1 constructor.
                .Where(x => x.GetConstructors().Count() == 1)
                // We want the attributes and the types. This way we can sort.
                .Select(x => (Attribute: x.GetCustomAttribute<AdventDayAttribute>(), Type: x))
                .OrderBy(x => x.Attribute!.Day);

            foreach(var day in dayTypes)
            {
                var constructor = day.Type.GetConstructors()[0];
                List<object> parameters = new List<object>();

                foreach(var parameter in constructor.GetParameters())
                {
                    var service = services.GetService(parameter.ParameterType);
                    if(service == null)
                    {
                        throw new ArgumentException($"Service by type {parameter.ParameterType.Name}, as requested by {day.Type.Name}, was not registered as a service!");
                    }

                    parameters.Add(service);
                }

                _days.Add(day.Attribute!, (IAdventDay)Activator.CreateInstance(day.Type!, [.. parameters])!);
                _logger.LogInformation("Registered: {0}", day.Type.Name);
            }
            _logger.LogInformation("Done registering all days!");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Run all days concurrently unless canceled...
            _logger.LogInformation("Running all days concurrently...");

            await Task.WhenAll(_days.Select(x => Task.Run(() =>
            {
                x.Value.Initialize();
                _outputs.Add(x.Key, x.Value.Run());
            }, cancellationToken)));

            _logger.LogInformation("Done running all days concurrently! Stopping Advent Runner Service...");

            _host.StopApplication();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Advent Runner Service was stopped. Gathered the following outputs:");
            foreach (var output in _outputs.OrderBy(x => x.Key.Day))
            {
                _logger.LogInformation("Day: {number} Name: {name} Output: {output}", 
                    output.Key.Day, 
                    output.Key.Name,
                    output.Value);
            }
        }
    }
}
