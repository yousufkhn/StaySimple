using MassTransit;
using StaySimple.Notifications.Consumers;
using StaySimple.Notifications.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ?? Email Service Configuration (conditional based on environment) ??
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IEmailService, MockEmailService>();
    builder.Logging.AddConsole();
}
else
{
    builder.Services.AddScoped<IEmailService, SmtpEmailService>();
}

// ?? RabbitMQ: register all 3 consumers ??
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserRegisteredConsumer>();
    x.AddConsumer<BookingConfirmedConsumer>();
    x.AddConsumer<BookingCancelledConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]!);
            h.Password(builder.Configuration["RabbitMQ:Password"]!);
        });

        // Auto-create queues for each consumer
        cfg.ConfigureEndpoints(ctx);
    });
});

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
