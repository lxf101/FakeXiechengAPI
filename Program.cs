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

// ע�� MVC ����������
builder.Services.AddControllers(setupAction =>
{
    setupAction.ReturnHttpNotAcceptable = true; // ���ͻ��������ʽ��Accept���޷�ƥ��ʱ���Ƿ񷵻�406
}).AddNewtonsoftJson(setupAction =>
{
    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
}).AddXmlDataContractSerializerFormatters();
// ����ע��
// ��������Ŀ����Ҫʹ�� ITouristRouteRepository �ӿڵķ���ʱ��
// ϵͳ���Զ��ṩһ�� TouristRouteRepository ��ʵ�����������ʵ���� ÿһ�� HTTP ������ֻ�ᴴ��һ�Σ�Scoped����
builder.Services.AddScoped<ITouristRouteRepository, TouristRouteRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
    //options.UseSqlite(builder.Configuration["ConnectionStrings:DefaultConnection"])
    options.UseMySql(builder.Configuration["ConnectionStrings:MySQLConnectionString"], ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:MySQLConnectionString"])
)
);

// AutoMapper��������ע��
// ɨ�� profile �ļ�
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ע��http�������
builder.Services.AddHttpClient();

var app = builder.Build();

// ����˭��
app.UseAuthentication();
// ����Ը�ʲô������ʲôȨ�ޣ�
app.UseAuthorization();

//app.MapGet("/", () => "Hello World!");
// �����յ� http����󣬿��Ա�֤����ȷ��·�ɵ���Ӧ�Ŀ�������
app.MapControllers();

app.Run();
