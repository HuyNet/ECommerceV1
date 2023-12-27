using Application.Catalog.Products;
using Application.Common;
using Application.Common.System.Users;
using Data.EF;
using Data.Entities;
using Data.IdentityService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

    }).AddInMemoryApiResources(Config.GetApiResources) // bên folder IdentityServer thêm Config
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

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger ECommerce", Version = "v1" });


    ////add authorization identityservice4 to swagger
    //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //{ 
    //    Type=SecuritySchemeType.OAuth2,
    //    Flows = new OpenApiOAuthFlows
    //    {
    //        Implicit = new OpenApiOAuthFlow
    //        {
    //            AuthorizationUrl = new Uri(builder.Configuration["AuthorityUrl"] + "/connect/authorize"),
    //            Scopes = new Dictionary<string, string> { { "api1", "ManageBrans APIs"} }
    //        },
    //    },
    //});
    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme,Id = "Bearer"}
    //        },
    //        new List<string>{ "api1" }
    //    }
    //});



    //add authorization indentity to swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        //// add authorization indentity
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
         {
            new OpenApiSecurityScheme
              {
               Reference = new OpenApiReference
               {
                Type = ReferenceType.SecurityScheme,
                 Id = "Bearer"
               },
               Scheme = "oauth2",
               Name = "Bearer",
               In = ParameterLocation.Header,
            },
            new List<string>()
         }
    });

});

string issuer = builder.Configuration.GetValue<string>("Tokens:Issuer");
string signingKey = builder.Configuration.GetValue<string>("Tokens:Key");
byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer=issuer,
            ValidateAudience= true,
            ValidAudience=issuer,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew=System.TimeSpan.Zero,
            IssuerSigningKey=new SymmetricSecurityKey(signingKeyBytes)

        };
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
//app.UseIdentityServer();
app.UseAuthentication();

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
        //c.OAuthClientId("swagger");
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger ECommerce V1");
    });

app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    });
app.Run();
