using Gatekeeper.Components;
using Gatekeeper.Components.Account;
using Gatekeeper.Data;
using Gatekeeper.OpenId;
using Gatekeeper.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<OpenIdOptions>().Bind(builder.Configuration.GetSection("OpenId"));

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMvc();
builder.Services.AddControllers();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

// OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
// (like pruning orphaned authorizations/tokens from the database) at regular intervals.
builder.Services.AddQuartz(options =>
{
    options.UseSimpleTypeLoader();
    options.UseInMemoryStore();
});

builder.Services.AddOpenIddict()

           // Register the OpenIddict core components.
           .AddCore(options =>
           {
               // Configure OpenIddict to use the Entity Framework Core stores and models.
               // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
               options.UseEntityFrameworkCore()
                       .UseDbContext<ApplicationDbContext>();

               // Enable Quartz.NET integration.
               options.UseQuartz();
           })

           // Register the OpenIddict server components.
           .AddServer(options =>
           {
               // Enable the authorization, logout, token and userinfo endpoints.
               options.SetDeviceEndpointUris("connect/device")
                      .SetVerificationEndpointUris("connect/verify")
                      .SetAuthorizationEndpointUris("connect/authorize")
                      .SetLogoutEndpointUris("connect/logout")
                      .SetTokenEndpointUris("connect/token")
                      .SetUserinfoEndpointUris("connect/userinfo");

               // Mark the "email", "profile" and "roles" scopes as supported scopes.
               options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

               // Note: the sample uses the code and refresh token flows but you can enable
               // the other flows if you need to support implicit, password or client credentials.
               options
                    .AllowDeviceCodeFlow()
                    .AllowAuthorizationCodeFlow()
                    .AllowRefreshTokenFlow();

               // Register the signing and encryption credentials.
               options.AddDevelopmentEncryptionCertificate()
                      .AddDevelopmentSigningCertificate();

               // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
               options.UseAspNetCore()
                      .EnableAuthorizationEndpointPassthrough()
                      .EnableLogoutEndpointPassthrough()
                      .EnableStatusCodePagesIntegration()
                      .EnableTokenEndpointPassthrough()
                      .EnableVerificationEndpointPassthrough()
                      .EnableUserinfoEndpointPassthrough();
           })

           // Register the OpenIddict validation components.
           .AddValidation(options =>
           {
               // Import the configuration from the local OpenIddict server instance.
               options.UseLocalServer();

               // Register the ASP.NET Core host.
               options.UseAspNetCore();
           });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.UseOpenIddict();
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityEmailSenderService>();

builder.Services.AddHostedService<ClientSeeder>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

// Add additional endpoints required by the Identity /Account Razor components.
//app.MapAdditionalIdentityEndpoints();

app.Run();
