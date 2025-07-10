using AutoMapper;
using FakeXiechengAPI.Database;
using FakeXiechengAPI.Models;
using FakeXiechengAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretByte = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]);
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Authentication:Audience"],

            ValidateLifetime = true,

            IssuerSigningKey = new SymmetricSecurityKey(secretByte)
        };
    });

// 注册 MVC 控制器服务
builder.Services.AddControllers(setupAction =>
{
    setupAction.ReturnHttpNotAcceptable = true; // 当客户端请求格式（Accept）无法匹配时，是否返回406
}).AddNewtonsoftJson(setupAction =>
{
    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
}).AddXmlDataContractSerializerFormatters();
// 依赖注入
// 当你在项目中需要使用 ITouristRouteRepository 接口的服务时，
// 系统会自动提供一个 TouristRouteRepository 的实例，而且这个实例在 每一个 HTTP 请求中只会创建一次（Scoped）。
builder.Services.AddScoped<ITouristRouteRepository, TouristRouteRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
    //options.UseSqlite(builder.Configuration["ConnectionStrings:DefaultConnection"])
    options.UseMySql(builder.Configuration["ConnectionStrings:MySQLConnectionString"], ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:MySQLConnectionString"])
)
);

// AutoMapper服务依赖注入
// 扫描 profile 文件
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// 注册http请求服务
builder.Services.AddHttpClient();

var app = builder.Build();

// 你是谁？
app.UseAuthentication();
// 你可以干什么？你有什么权限？
app.UseAuthorization();

//app.MapGet("/", () => "Hello World!");
// 当接收到 http请求后，可以保证能正确的路由到相应的控制器中
app.MapControllers();

app.Run();
