using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AuthCore.Services;
using System.Security.Claims;
using Request;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
//add for testing
//for api testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add JWT Bearer configuration
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer' [space] and then your token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"  // optional
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "Bearer",
            Name = "Bearer",
            In = ParameterLocation.Header
        },
        new List<string>()  // no scopes required
    }});
});



builder.WebHost.UseUrls("http://0.0.0.0:5011");


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
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])
                        ),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero, 
                };
        });

builder.Services.AddScoped<AuthService>();





//build app
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("swagger/v1/swagger.json", "My Api v1");});
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


//------------------ authentification ------------------//
app.MapPost("/user/signup", async ([FromServices]api.Db db, [FromBody]UserCreateRequest request) =>
{
        //------------------ secure password -----------------//
        string password = Utils.Password.Hash(request.password);
        
        //-------------- add user to database ----------------//
        Data.User user = new Data.User(request.email,
                                       request.username,
                                       password,
                                       request.preference,
                                       "user");
              
        try {
              await db.Users.AddAsync(user);
              await db.SaveChangesAsync();

              return Results.Created($"User created {user.username}",user.IntoDto());
        } catch(DbUpdateException ex) {
        
              return Results.Problem($"Email already taken {ex.ToString()}");
        } catch {

              return Results.Problem("internal error");        
        }
}).AllowAnonymous();


app.MapPost("/user/login", async (
      [FromServices]api.Db db, 
      [FromServices]AuthService authservice, 
      [FromBody]UserLoginRequest request) =>
{       
        //---------------------- get user ---------------------// 
        Data.User? user = await db.Users
                    .FirstOrDefaultAsync(u => u.email == request.email);
        
        if ( user is null ) {
              return Results.Problem("Invalid Credentials"); 
        };
        //------------------- match password ------------------//
        bool Valid = Utils.Password.Validate(request.password,user.password);
            
        if(!Valid) {
              return Results.Problem("Invalid Credentials");
        };
        
        //--------------------- create JWT --------------------//
        string token = authservice.GenerateToken(user.IntoDto());
        if (token is null) {
              return Results.Problem("Unable to generate token");
        }
        return Results.Ok(new { token });
}).AllowAnonymous();



//------------------ user routes ------------------//

app.MapGet("/user/{username}", async (
      [FromServices]api.Db db, 
      string username) =>
{
        var user = await db.Users.FirstOrDefaultAsync(u => u.username == username );
        if (user is null) {
              return Results.NotFound("user not found");
        }

        return Results.Ok(user.IntoDto());
}).RequireAuthorization(p => p.RequireRole("user"));


app.MapDelete("/user/delete", async (
      [FromServices]api.Db db, 
      ClaimsPrincipal principal, 
      [FromBody]UserDeleteRequest request) =>
{
        //------------ get user from token------------//
        string? email = principal.Identity?.Name;
        if (email is null) {
              return Results.Unauthorized(); 
        }

        Data.User? user = await db.Users.FirstOrDefaultAsync(u => u.email == email);
        if (user is null) {
              return Results.NotFound("user not found"); 
        }
        
        //------------- validate password ------------//
        bool valid = Utils.Password.Validate(request.password,user.password);
        if (!valid) {
            return Results.Problem("invalid credentials");
        }
        //------------- delete the user --------------//
        db.Users.Remove(user); 
        await db.SaveChangesAsync();

        return Results.Ok();
}).RequireAuthorization(p => p.RequireRole("user"));



app.MapPatch("/user/update",async (
      [FromServices]api.Db db, 
      ClaimsPrincipal principal, 
      [FromBody]UserUpdateRequest request) =>
{
        //------------ get user from token------------//
        string? email = principal.Identity?.Name;
        if (email is null) {
              return Results.Unauthorized(); 
        }

        Data.User? user = await db.Users.FirstOrDefaultAsync(u => u.email == email);
        if (user is null) {
              return Results.NotFound("user not found"); 
        }
        
        user.preference = request.preference; 

        //------------- delete the user --------------//
        db.Users.Update(user); 
        await db.SaveChangesAsync();

        return Results.Ok();
}).RequireAuthorization(p => p.RequireRole("user"));





//------------------ task routes ------------------//

app.MapPost("/task/create", async (
      [FromServices]api.Db db,
       ClaimsPrincipal principal,
      [FromBody]TaskCreateRequest request)=>
{
      if (request.desc is null) {
            return Results.Problem("invalid request");
      }      
      if (request.title is null) {
            return Results.Problem("invalid request");
      }
      string? email = principal.Identity?.Name;
      if (email is null) {
            return Results.Problem("invalid token");
      }

      //trop paresseux pour changer
      Data.User? user = await db.Users.FirstOrDefaultAsync(u => u.email == email);
      if (user is null) {
            return Results.NotFound("user not found");
      }
      //---------------- create task ------------------//
      Data.Tasks task = new Data.Tasks( request.title, request.desc ,user.id,false, DateTime.UtcNow); 
      
      await db.Tasks.AddAsync(task);
      await db.SaveChangesAsync();

      return Results.Ok();

 }).RequireAuthorization(p => p.RequireRole("user"));




app.MapPatch("/task/completed/{id}", async ([FromServices]api.Db db, string id) =>
{
        if (!int.TryParse(id, out int output)) {
              return Results.NotFound("Invalid ID");
        }

        int row = await db.Tasks
                          .Where(t => t.id == output)
                          .ExecuteUpdateAsync(s => s.SetProperty(t=>t.completed, true)); 
        return row == 0 
            ? Results.Problem("no rows have been affected")
            : Results.Ok();

}).RequireAuthorization(p => p.RequireRole("user"));

app.MapDelete("/task/delete/{id}", async ([FromServices]api.Db db, string id) =>
{
        if (!int.TryParse(id, out int output)) {
              return Results.NotFound("Invalid ID");
        }

        Data.Tasks? task = await db.Tasks.FindAsync(output);
        if (task is null) {
                return Results.Problem("null task");
        }

        db.Tasks.Remove(task);
        await db.SaveChangesAsync();

        return Results.Ok();
}).RequireAuthorization(p => p.RequireRole("user"));




app.MapGet("/task/all/noncompleted", async ([FromServices]api.Db db) => 
{
      List<Data.Tasks> tasks = await db.Tasks
                                  .Where(t => t.completed == false)
                                  .ToListAsync();
      return Results.Ok(tasks);
}).RequireAuthorization(p => p.RequireRole("user"));






app.MapGet("/task/all/completed", async ([FromServices]api.Db db) => 
{      
      List<Data.Tasks> tasks = await db.Tasks
                                  .Where(t => t.completed == true)
                                  .ToListAsync();

      return Results.Ok(tasks);
}).RequireAuthorization(p => p.RequireRole("user"));






app.MapGet("/task/email/{email}", async ([FromServices]api.Db db,string email)=> 
{
      Data.User? user = await db.Users.FirstOrDefaultAsync(u => u.email == email);
      if(user is null) {
            return Results.Problem("Invalid email");
      }

      List<Data.Tasks> tasks = await db.Tasks
                                  .Where(t => t.user_id == user.id)
                                  .ToListAsync(); 

      return Results.Ok(tasks);
}).RequireAuthorization(p => p.RequireRole("user"));




app.MapGet("/task/all/{min}/{max}", async ([FromServices]api.Db db, int min, int max) => 
{
      List<Data.Tasks> tasks = await db.Tasks
                                 .Where(t => t.id>=min && t.id<=max)
                                 .ToListAsync();

      return Results.Ok(tasks);
}).RequireAuthorization(p => p.RequireRole("user"));




app.MapGet("/task/date/weekly", async ([FromServices]api.Db db) =>
{     
    DateTime oneWeekAgo = DateTime.UtcNow.Subtract(TimeSpan.FromDays(7));

    List<Data.Tasks> tasks = await db.Tasks
        .Where(t => t.task_date >= oneWeekAgo)
        .OrderByDescending(t => t.task_date)
        .ToListAsync();

    return Results.Ok(tasks);
}).RequireAuthorization(p => p.RequireRole("user"));


app.Run();


