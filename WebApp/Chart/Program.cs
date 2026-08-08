using Chart.Common;
using Chart.Interface;
using Chart.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();//1
// Duplicate here any configuration sources you use.
configurationBuilder.AddJsonFile("AppSettings.json");//2
IConfiguration Configuration = configurationBuilder.Build();//3
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IConfiguration>(Configuration);//4
Global.ConnectionString = Configuration.GetConnectionString("DefaultConnection");//5
builder.Services.AddControllersWithViews();//6
builder.Services.AddDistributedMemoryCache();//7
builder.Services.AddSession(options => { //8
    options.IdleTimeout = TimeSpan.FromMinutes(60);//You can set Time   
});
builder.Services.AddMvc();
builder.Services.AddScoped<ITemperature, TemperatureService>();//9
builder.Services.AddScoped<IUsers, UserService>();

builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>//10
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddCors(options =>
{
    options.AddPolicy(
       name: "AllowOrigin",
       builder =>
       {
           builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();

       });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors("corspolicy");
app.UseSession(); //12
//app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
//app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
