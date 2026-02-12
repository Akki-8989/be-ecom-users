var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

var users = new List<User>
{
    new("user-1", "Akash Kumar", "akash@example.com", "Mumbai", DateTime.Parse("2025-12-01")),
    new("user-2", "Priya Singh", "priya@example.com", "Delhi", DateTime.Parse("2026-01-15")),
    new("user-3", "Rahul Verma", "rahul@example.com", "Bangalore", DateTime.Parse("2026-02-01"))
};

app.MapGet("/api/users", () => Results.Ok(users))
   .WithName("GetUsers")
   .WithOpenApi();

app.MapGet("/api/users/{id}", (string id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
})
.WithName("GetUserById")
.WithOpenApi();

app.MapGet("/api/users/health", () => Results.Ok(new { service = "be-ecom-users", status = "healthy" }));

app.Run();

record User(string Id, string Name, string Email, string City, DateTime JoinedAt);
