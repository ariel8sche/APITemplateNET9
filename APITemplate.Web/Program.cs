using APITemplate.Contracts.Interfaces;
using APITemplate.Data.AuthModels;
using APITemplate.Services.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("NeonDb");

builder.Services.AddDbContext<AuthContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IApiResourceService, ApiResourceService>();
builder.Services.AddScoped<IApiScopeService, ApiScopeService>();
builder.Services.AddScoped<IClientScopeGrantService, ClientScopeGrantService>();
builder.Services.AddScoped<IClientSecretService, ClientSecretService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccessTokenService, AccessTokenService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
