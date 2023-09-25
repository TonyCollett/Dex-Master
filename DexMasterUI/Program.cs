var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
});
builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
        options.ClientId = googleAuthNSection["ClientId"] ?? string.Empty;
        options.ClientSecret = googleAuthNSection["ClientSecret"] ?? string.Empty;
        options.CallbackPath = "/signin-google";
    })
    .AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"] ?? string.Empty;
        microsoftOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"] ?? string.Empty;
        microsoftOptions.CallbackPath = "/signin-microsoft";
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextAccessor>();
builder.Services.AddScoped<ProtectedSessionStorage>();

builder.Services.AddSocialMediaSharing();

builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClient>();

builder.Services.AddSingleton<IDbConnection, DbConnection>();
builder.Services.AddTransient<IUserData, MongoUserData>();
builder.Services.AddTransient<IPromptData, MongoPromptData>();
builder.Services.AddTransient<ICommentData, MongoCommentData>();
builder.Services.AddTransient<IFavouriteData, MongoFavouriteData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
