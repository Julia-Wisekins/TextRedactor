using TextRedactor;
using TextRedactor.Model;
using TextRedactor.Service;
using TextRedactor.Service.Implamentation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adding Transient service so the list can be updated. 
builder.Services.AddTransient<IRedactor>(provider =>
{

    //attempting to get the redaction list 
    var redactionList = builder.Configuration.GetSection(ConfigurationDetails.RedactorConfigLocation)
                                                         .Get<List<string>>();

    //its a little sus but at least there isn't a warning #blissfulIgnorance
    return new RedactorUsingRegex(redactionList!);
});

// Add some file logging in
builder.Logging.AddFileLogger();

// Updated the console log to include more detail and timestamps.
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true;
    options.SingleLine = true;
    options.UseUtcTimestamp = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss.ffff]";
});

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




