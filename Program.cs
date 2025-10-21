using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using Request;

var builder = WebApplication.CreateBuilder(args);

//for api testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// database services 
var connectionString = builder.Configuration
                              .GetConnectionString("DefaultConnection");
builder.Services
       .AddDbContext<api.Db>(options => options.UseNpgsql(connectionString));

//JWT token 
builder.Services.AddAuthorization();
builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
        .AddJwtBearer(o => 
        {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
                        ValidIssuer = builder.Configuration["Jwt:Issuers"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero, 
                };
        });









//build app
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();


//------------------ authentification / security route -------------------//

//as to fetch a public key
//to after encrypt it using symmetric key
//if not it will break
app.MapGet("/security/publickey", async () =>
{

});



//------------------ user routes ------------------//
app.MapGet("/user/{username}", async (api.Db db, string username) =>
{
        var user = await db.Users.FirstOrDefaultAsync(u => u.username == username );
        return user is null
               ? Results.NotFound("user not found")
               : Results.Ok(user.IntoDto());
});


app.MapPost("/user/create", async (api.Db db, UserCreateRequest request) =>
{       //---------------------- secure password ---------------------//

        //create a user
        var user = new Data.User(request.email,request.username,request.password,request.preference);  
        try {
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();

                return Results.Created($"User created {user.username}",user.IntoDto());
        } catch {
                return Results.Problem("internal error");        
        }
});



app.MapPost("/user/login", (UserLoginRequest request) => 
{       
        Console.WriteLine($"user request: {request}");
        return Results.Ok(new { message = $"User {request.username} logged in" });
});


app.MapDelete("/user/{id, password}", (int id, String password) =>
{
        return $"user delete: {id}, {password}";
});


app.MapPatch("/user/update", (int id, String preference) =>
{
        return $"/user updated: {id}, {preference}";
});





//------------------ task routes ------------------//
/*
app.MapGet("/task/all", () => 
{


});

app.MapGet("/task/date?={da", () =>
{


});

app.MapGet("/task/user?={id}", () =>
{

});
*/
app.Run();


