using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(50);
    options.SlidingExpiration = false;
}).AddOpenIdConnect(options =>
{
    // **************************************************************************************************************
    // TODO and ATTENTION: these settings must match the WebApp client secret craeted on puship dashboard details

    options.ClientId = "<you client id here>"; //example: AHU47J-example-api
    options.ClientSecret = "<your client secret here>"; //example: "b40b7f81-16b0-46a4-b065-d33d5230f2f6"
    // **************************************************************************************************************


    options.RequireHttpsMetadata = false;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.SaveTokens = true;

        // Use the authorization code flow.
        options.ResponseType = OpenIdConnectResponseType.Code;
    options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;

    // Note: setting the Authority allows the OIDC client middleware to automatically
    // retrieve the identity provider's configuration and spare you from setting
    // the different endpoints URIs or the token validation parameters explicitly.
    options.Authority = "https://auth.puship.com/";

    //options.Scope.Add("email");
    //options.Scope.Add("roles");
    options.Scope.Add("Tags");
    options.Scope.Add("Apps");
    options.Scope.Add("Devices");
    options.Scope.Add("PushMessages");
    //options.Scope.Add("Admin");


    // Disable the built-in JWT claims mapping feature.
    options.MapInboundClaims = false;

    options.TokenValidationParameters.NameClaimType = "name";
    options.TokenValidationParameters.RoleClaimType = "role";
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
