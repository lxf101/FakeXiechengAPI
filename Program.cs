using AutoMapper;
using FakeXiechengAPI.Database;
using FakeXiechengAPI.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
// �����յ� http����󣬿��Ա�֤����ȷ��·�ɵ���Ӧ�Ŀ�������
app.MapControllers();

app.Run();
