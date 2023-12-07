var builder = DistributedApplication.CreateBuilder(args);

var gatekeeper = builder.AddProject<Projects.Gatekeeper>("gatekeeper").WithLaunchProfile("https");

var turbulence = builder.AddProject<Projects.Turbulence>("turbulence").WithLaunchProfile("https");

builder.Build().Run();
