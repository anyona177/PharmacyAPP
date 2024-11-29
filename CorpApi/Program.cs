using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using CorpEntities;
using CorpApi;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// ��������� ��������� ���� ������
builder.Services.AddDbContext<PharmacyContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("AppDb")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// ��������� CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});



// ��������� ��������� JSON �� UTF-8
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
});

// ��������� JwtTokenService
builder.Services.AddSingleton<JwtTokenService>();

var app = builder.Build();

app.UseCors("AllowAll");


// ������������� Content-Type ��� JSON-������� �� UTF-8
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Type", "application/json; charset=utf-8");
    await next();
});
app.MapControllers();
// ��������
app.MapGet("/pharmacy", () =>
    "SalesTrack Med\n������\n�������\n����������"
);
app.MapUserApi();
var pharmacyGroup = app.MapGroup("/pharmacy").RequireAuthorization();

pharmacyGroup.MapGroup("/categories").MapCategoriesApi();
pharmacyGroup.MapGroup("/employees").MapEmployeesApi();
var salesGroup = pharmacyGroup.MapGroup("/sales").MapSalesApi();
var soldItemsGroup = salesGroup.MapGroup("/solditems").MapSoldItemApi();
pharmacyGroup.MapGroup("/products").MapProductsApi();

app.Run();
