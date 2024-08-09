using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using IMPOLAssistant.KernelMemory;
using IMPOLAssistant.SemanticKernel;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.SemanticKernel;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);
var openAiApiKey = builder.Configuration["OpenAI:ApiKey"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddScoped(container =>
{
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion("gpt-3.5-turbo", openAiApiKey)
    .AddOpenAITextEmbeddingGeneration("text-embedding-ada-002", openAiApiKey)
    .Build();
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    return kernel;
});


builder.Services.AddScoped<IKernelMemory>(container =>
{
    return new KernelMemoryBuilder()
        .WithOpenAIDefaults(openAiApiKey)
        .Build<MemoryServerless>();
});

builder.Services.AddTransient<IKernelMemoryService, KernelMemoryService>();
builder.Services.AddScoped<ISemanticKernelService, SemanticKernelService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
