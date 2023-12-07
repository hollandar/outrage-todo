using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;

namespace Gatekeeper.OpenId;


public class ClientSeeder : IHostedService
{
    private readonly IOptions<OpenIdOptions> openidOptions;
    private readonly IServiceProvider _serviceProvider;

    public ClientSeeder(IOptions<OpenIdOptions> openidOptions, IServiceProvider serviceProvider)
    {
        this.openidOptions = openidOptions;
        _serviceProvider = serviceProvider;
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        await PopulateScopes(scope, cancellationToken);

        await PopulateInternalApps(scope, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async ValueTask PopulateScopes(IServiceScope scope, CancellationToken cancellationToken)
    {
        var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        foreach (var scopeDescriptor in this.openidOptions.Value.Scopes)
        {
            var scopeInstance = await scopeManager.FindByNameAsync(scopeDescriptor.Name, cancellationToken);

            if (scopeInstance == null)
            {
                await scopeManager.CreateAsync(scopeDescriptor, cancellationToken);
            }
            else
            {
                await scopeManager.UpdateAsync(scopeInstance, scopeDescriptor, cancellationToken);
            }
        }
    }

    private async ValueTask PopulateInternalApps(IServiceScope scopeService, CancellationToken cancellationToken)
    {
        var appManager = scopeService.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        foreach (var appDescriptor in this.openidOptions.Value.Clients)
        {
            var client = await appManager.FindByClientIdAsync(appDescriptor.ClientId, cancellationToken);

            if (client == null)
            {
                await appManager.CreateAsync(appDescriptor, cancellationToken);
            }
            else
            {
                await appManager.UpdateAsync(client, appDescriptor, cancellationToken);
            }
        }
    }
}
