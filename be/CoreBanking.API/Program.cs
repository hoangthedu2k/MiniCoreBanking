using CoreBanking.Application;
using CoreBanking.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1. ??ng ký các Layer (Clean Architecture)
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// 2. ??ng ký Controllers
builder.Services.AddControllers();

// 3. ??ng ký Swagger (Dùng Swashbuckle - Giao di?n chu?n d? dùng)
// L?u ý: N?u báo l?i ?? ? ?ây, b?n c?n cài gói: dotnet add package Swashbuckle.AspNetCore
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 4. Kích ho?t giao di?n Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization(); // (Nên có dòng này dù ch?a dùng, ?? sau này ?? quên)

// 5. QUAN TR?NG: Ph?i có dòng này thì AccountsController m?i ch?y ???c!
app.MapControllers();

app.Run();