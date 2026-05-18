namespace KiranaStoreUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddControllersWithViews();

            
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //Local Api Link

            //builder.Services.AddHttpClient("api", client =>
            //{
            //    client.BaseAddress = new Uri("https://localhost:7262/api/");
            //});

            //Server Api Link

            builder.Services.AddHttpClient("api", client =>
            {
                client.BaseAddress = new Uri("https://kirana-store-repo.onrender.com/api/");
            });

            builder.Services.AddAuthentication("Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.LoginPath = "/User/Login";
                    options.LogoutPath = "/User/Logout";
                });

            builder.Services.AddAuthorization();


            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();   // MUST be here
            app.UseSession();
            app.UseAuthorization();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
