using Microsoft.EntityFrameworkCore;
using ASCO.DbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ASCO.Repositories;
using ASCO.Services;
//using ASCO.Repositories.Implementations;
using Microsoft.Extensions.FileProviders;
using ASCP.Repositories;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", builder =>
    {
        builder.WithOrigins("http://localhost:3000") //can be any but for now lets just say the frontend is at port 3000
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
//all class configurations and services should be added here
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CrewRepository>();
builder.Services.AddScoped<CrewService>();
builder.Services.AddScoped<VesselRepository>();
builder.Services.AddScoped<VesselService>();
builder.Services.AddScoped<DocumentRepository>();
builder.Services.AddScoped<DocumentService>();
builder.Services.Configure<DocumentStorageOptions>(
    builder.Configuration.GetSection("DocumentStorage")
);
//jwt add ons go here.
builder.Services.AddScoped<JwtServices>();
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var key = Convert.FromHexString(jwtKey!); // i know its not null niga.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//authentication for jwts on logins.
builder.Services.AddAuthentication ( options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer( options => {
    options.RequireHttpsMetadata = false; //for now 
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {   
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidIssuer = jwtIssuer
    };
    options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Auth Failed: " + context.Exception.ToString());
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine("OnChallenge: " + context.AuthenticateFailure?.ToString());
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                Console.WriteLine("Token received: " + context.Token?.Substring(0, Math.Min(10, context.Token?.Length ?? 0)));
                return Task.CompletedTask;
            }
        };
});

//database configuation goes here.
 var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ASCODbContext>(options =>
        options.UseNpgsql(connectionString)
    );

builder.Services.AddAuthorization();
builder.Services.AddControllers();
var app = builder.Build();
app.UseCors("AllowLocalhost3000");
//automatic migration application
if (!app.Environment.IsEnvironment("Test"))
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ASCODbContext>();
        db.Database.Migrate();
    }
}
// app.UseStaticFiles(new StaticFileOptions
// {
//     FileProvider = new PhysicalFileProvider(
//         Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
//     RequestPath = "/documents"
// }); //do not need to use for now. Shifted to another filesytem approach for better handling.
////////////////////////////////////////
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


app.Run();
