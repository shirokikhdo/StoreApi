using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenCustomConfig();
builder.Services.AddPostgreSqlDbContext(builder.Configuration);
builder.Services.AddPostgreSqlIdentityContext();
builder.Services.AddConfigureIdentityOptions();
builder.Services.AddJwtTokenGenerator();
builder.Services.AddAuthentificationConfig(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddShoppingCartService();
builder.Services.AddOrdersService();
builder.Services.AddPaymentService();
builder.Services.AddFileStorageService(builder.Configuration);

var app = builder.Build();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(x =>
    x.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .WithExposedHeaders("*"));
app.UseAuthentication();
app.UseAuthorization();

await app.Services.InitializeRoleAsync();

app.Run();