using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Peliculas.Api;
using Peliculas.Api.Contexts;
using Peliculas.Api.Filters;
using Peliculas.Api.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), sqlServer => sqlServer.UseNetTopologySuite());
});

builder.Services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

//builder.AddAutoMapper()
builder.Services.AddSingleton(provider =>
    new MapperConfiguration(config =>
    {
        var geometry = provider.GetRequiredService<GeometryFactory>();
        config.AddProfile(new PeliculaMapper(geometry));
    }).CreateMapper()
);

//var mapperConfig = new MapperConfiguration(mapperConfig =>
//{
//    mapperConfig.AddProfile<PeliculaMapper>();
//});
//IMapper mapper = mapperConfig.CreateMapper();
//builder.Services.AddSingleton(mapper);


builder.Services.AddTransient<IAlmacenadorDeArchivos, AlmacenadorDeArchivosLocal>(c => new AlmacenadorDeArchivosLocal(builder.Environment, new HttpContextAccessor()));
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(FiltroDeExcepcion));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        //string frontEndUrl;

        //colocar la url en una variable uqe se lea desde appsetiings
        builder.WithOrigins("*")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders(new string[] { "cantidadTotalDeRegistros" });
    });
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    using (var swapStream = new MemoryStream())
    {
        var respuestaOriginal = context.Response.Body;
        context.Response.Body = swapStream;

        await next.Invoke();

        swapStream.Seek(0, SeekOrigin.Begin);
        string respuesta = new StreamReader(swapStream).ReadToEnd();
        swapStream.Seek(0, SeekOrigin.Begin);

        await swapStream.CopyToAsync(respuestaOriginal);
        context.Response.Body = respuestaOriginal;

        app.Logger.LogInformation(respuesta);
    }
});

app.Map("/mapa1", (app) =>
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Interceptando el pipeline");
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
