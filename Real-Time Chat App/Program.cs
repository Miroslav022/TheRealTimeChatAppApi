using Real_Time_Chat_App.SignalR;
using FluentValidation;
using RealTimeChatApp.Application.Behaviors;
using RealTimeChatApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MediatR;
using RealTimeChatApp.Application.Abstractions.ValidationRules;
using RealTimeChatApp.Infrastructure.ValidationRules;
using RealTimeChatApp.Application.Abstractions.Repositories.UserRepository;
using RealTimeChatApp.Infrastructure.Repositories.UserRepository;
using RealTimeChatApp.Application.Abstractions.Repositories;
using RealTimeChatApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Real_Time_Chat_App.OptionsSetup;
using RealTimeChatApp.Application.Abstractions.Jwt;
using RealTimeChatApp.Infrastructure.Authentication;
using RealTimeChatApp.Application.Abstractions.Services;
using RealTimeChatApp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(RealTimeChatApp.Application.AssemblyReference.Assembly);
});
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient<IUserValidationRules, UserValidationRules>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IJwtProvider, JwtProvider>();
builder.Services.AddTransient<IJwtRefreshProvider, RefreshJwtProvider>();
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
builder.Services.AddTransient<RealTimeChatApp.Application.Services.AuthenticationService, RealTimeChatApp.Application.Services.AuthenticationService>();
builder.Services.AddValidatorsFromAssembly(RealTimeChatApp.Application.AssemblyReference.Assembly, includeInternalTypes: true);

builder.Services.AddDbContext<AspContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("reactApp", builder =>
    {
        builder.WithOrigins("https://localhost:5173")
               .AllowCredentials()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("chat");
app.UseCors("reactApp");
app.Run();
