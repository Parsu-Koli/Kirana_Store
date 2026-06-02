using BLL.Services;
using DAL.Data;
using DAL.Repository.Implementation;
using DAL.Repository.Implimentation;
using DAL.Repository.Interface;
using DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace KiranaStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // 🔐 Swagger JWT Support
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "KiranaStore API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter ONLY the JWT token. Do NOT add 'Bearer'"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });


            //For the local DB Connection 

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            //Server Connection
            //builder.Services.AddDbContext<AppDbContext>(options =>
            //          options.UseNpgsql(
            //     builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IPurchaseItemRepository, PurchaseItemRepository>();
            builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            builder.Services.AddScoped<ISaleItemRepository, SaleItemRepository>();
            builder.Services.AddScoped<ISaleRepository, SaleRepository>();
            builder.Services.AddScoped<IStockRepository, StockRepository>();
            builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

            // Services
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<CustomerService>();
            builder.Services.AddScoped<OrderItemService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<PaymentService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<PurchaseItemService>();
            builder.Services.AddScoped<PurchaseService>();
            builder.Services.AddScoped<SaleItemService>();
            builder.Services.AddScoped<SaleService>();
            builder.Services.AddScoped<StockService>();
            builder.Services.AddScoped<SupplierService>();

            // 🔐 JWT Authentication (SAFE FIXES ADDED)
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

                        // ✅ Prevent clock-time mismatch issues
                        ClockSkew = TimeSpan.Zero
                    };

                    // ✅ Helpful for debugging invalid_token
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("JWT AUTH FAILED: " +
                                context.Exception.Message);
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

            var app = builder.Build();

            // if (app.Environment.IsDevelopment())
            // {
            //     app.UseSwagger();
            //     app.UseSwaggerUI();
            // }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            // 🔴 ORDER IS CORRECT (do not change)
            app.UseAuthentication();
            app.UseCors("AllowMVC");
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
