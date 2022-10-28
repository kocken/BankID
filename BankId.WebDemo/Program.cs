using BankId.ServiceClient;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var bankIdIsProduction = bool.Parse(config["BankID:IsProduction"]);
var bankIdCertificateThumbprint = config["BankID:CertificateThumbprint"];

var bankIdService = new BankIdService(bankIdIsProduction, bankIdCertificateThumbprint);
builder.Services.AddSingleton<IBankIdService>(bankIdService);

// Add services to the container.
builder.Services.AddControllersWithViews()/*.AddRazorRuntimeCompilation()*/;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
