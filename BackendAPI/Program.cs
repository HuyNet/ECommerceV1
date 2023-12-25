using Application.Catalog.Products;
using Application.Common;
using Application.Common.System.Users;
using Data.EF;
using Data.Entities;
using Data.IdentityService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Utilities.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ECommerceDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.MainConnectionString)));

// add Identity approle and app user
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<ECommerceDbContext>()
    .AddDefaultTokenProviders();

// add IdentiyServer
builder.Services.AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;

    }) .AddInMemoryApiResources(Config.GetApiResources) // bên folder IdentityServer thêm Config
                                                   // .AddInMemoryClients(Configuration.GetSection("IdentityServer:Clients"))
    .AddInMemoryClients(Config.Clients) // lấy ra các client
    .AddInMemoryIdentityResources(Config.GetIdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddAspNetIdentity<AppUser>()
    .AddDeveloperSigningCredential();


    //builder.Services.AddTransient<IEmailSender, EmailSenderService>();

    //builder.Services.AddAuthentication()
    // .AddLocalApi("Bearer", option =>
    // {
    //     option.ExpectedScope = "api.WebApp";
    // }
    // );


//derlare DI
builder.Services.AddTransient<IPublicProductService, PublicProductService>();
builder.Services.AddTransient<IPrivateProductService, PrivateProductService>();
builder.Services.AddTransient<IStorageService, FileStorageService>();
builder.Services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
builder.Services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
builder.Services.AddTransient<IUserService, UserService>();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger ECommerce", Version = "v1" });
});


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
app.UseIdentityServer();
app.UseAuthorization();

app.UseRouting();

app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapDefaultControllerRoute();
//    endpoints.MapRazorPages();
//});

app.UseSwagger();
app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger ECommerce V1");
    });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
