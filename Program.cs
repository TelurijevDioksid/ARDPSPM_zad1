using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Span.Culturio.Api.Data;
using Span.Culturio.Api.Models.CultureObject;
using Span.Culturio.Api.Services;
using Span.Culturio.Api.Services.CultureObject;
using Span.Culturio.Api.Services.Package;
using Span.Culturio.Api.Services.Subscriptions;
using Span.Culturio.Api.Services.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    foreach (string file in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly))
        c.IncludeXmlComments(file);

    c.UseAllOfForInheritance();
    c.DocInclusionPredicate((_, _) => true);

});

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCultureObjectValidator>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICultureObjectService, CultureObjectService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPackageServices, PackageServices>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
