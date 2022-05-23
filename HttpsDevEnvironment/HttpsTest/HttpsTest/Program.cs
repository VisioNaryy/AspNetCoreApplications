using HttpsTest.Configuration;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureServices((context, services) =>
{
    HostConfig.CertPath = context.Configuration["CertPath"];
    HostConfig.CertPassword = context.Configuration["CertPassword"]; // from user secrets
})
.ConfigureKestrel(opt =>
    {
        var host = Dns.GetHostEntry("weather.io");
        var address = host.AddressList.FirstOrDefault();

        //opt.ListenAnyIP(5007);
        //opt.ListenAnyIP(5001, listOpt =>
        //{
        //    listOpt.UseHttps(HostConfig.CertPath, HostConfig.CertPassword);
        //});
        opt.Listen(address, 5007);
        opt.Listen(address, 5001, listOpt =>
        {
            listOpt.UseHttps(HostConfig.CertPath, HostConfig.CertPassword);
        });
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();