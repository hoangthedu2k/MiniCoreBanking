using CoreBanking.Application;
using CoreBanking.Application.Services;
using CoreBanking.Infrastructure;
using CoreBanking.Infrastructure.Hubs;
using Hangfire;
using Hangfire.PostgreSql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(option => option.UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))));
builder.Services.AddHangfireServer();
// 1. ??ng ký các Layer (Clean Architecture)
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// 2. ??ng ký Controllers
builder.Services.AddControllers();

// 3. ??ng ký Swagger (Dùng Swashbuckle - Giao di?n chu?n d? dùng)
// L?u ý: N?u báo l?i ?? ? ?ây, b?n c?n cài gói: dotnet add package Swashbuckle.AspNetCore
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// 1. CẤU HÌNH CORS (Sửa lại đoạn này nếu chưa chuẩn)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // URL Angular
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // <--- Bắt buộc cho SignalR
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 4. Kích ho?t giao di?n Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization(); // (Nên có dòng này dù ch?a dùng, ?? sau này ?? quên)
app.UseHangfireDashboard("/hangfire");
// 5. QUAN TR?NG: Ph?i có dòng này thì AccountsController m?i ch?y ???c!
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub"); // Đường dẫn kết nối
var recurringJobManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<IRecurringJobManager>();

// Đăng ký Job: Chạy hàm CalculateDailyInterest mỗi ngày
// Cron.Daily(23, 55) nghĩa là 23h55 phút hàng ngày
recurringJobManager.AddOrUpdate<IInterestService>(
    "tinh-lai-cuoi-ngay",
    service => service.CalculateDailyInterest(),
    Cron.Daily(23, 55));
app.Run();