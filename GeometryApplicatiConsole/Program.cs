using GeometryApplicationlibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

class Program
{
    static async Task Main(string[] args)
    {
        // Step 2: Setup Feature Management
        var featureManagement = new Dictionary<string, string> {
            { "FeatureManagement:Square", "true"},
            { "FeatureManagement:Rectangle", "false"},
            { "FeatureManagement:Triangle", "true"}
        };

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(featureManagement)
            .Build();

        // Step 3: Setup Dependency Injection
        var services = new ServiceCollection();
        services.AddFeatureManagement(configuration);
        var serviceProvider = services.BuildServiceProvider();

        // Step 4: Control Access to Shapes
        var featureManager = serviceProvider.GetRequiredService<IFeatureManagerSnapshot>();
            if (await featureManager.IsEnabledAsync("Square"))
            {
                // Step 5: Accept User Input
                Console.WriteLine("Enter the side length of the square:");
                string? input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    double sideLength = double.Parse(input);
                    var square = new Square(sideLength);
                    Console.WriteLine($"Area of the square: {square.CalculateArea()}");
                    Console.WriteLine($"Perimeter of the square: {square.CalculatePerimeter()}");
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }
            else
            {
                Console.WriteLine("Square feature is disabled.");
            }
    }
}