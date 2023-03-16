using Mensageria;
using Mensageria.Model;
using System.Diagnostics;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapPost("/EscalaFuncionarios", 
    async Task<IResult> (HttpRequest request) =>
{
    if (!request.HasFormContentType)
        return Results.BadRequest();

    var form = await request.ReadFormAsync();

    if (form.Files.Any() == false)
        return Results.BadRequest("Não há arquivos");

    var file = form.Files.FirstOrDefault();

    if (file is null || file.Length == 0 || file.ContentType != "text/csv")
        return Results.BadRequest("O arquivo não pode estar vazio ou diferente do formato .CSV.");

    var sw = new Stopwatch();
    sw.Start();
    var total = 0;
    string line;
    using (var reader = new StreamReader(file.OpenReadStream()))
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split(',');

            var EscalaFuncionarios = new Funcionario { Id = int.Parse(parts[0]), Matricula = int.Parse(parts[1]), DataInicio = Convert.ToDateTime(parts[2]) };
            RabbitMqPublish.EnviarMensagem(EscalaFuncionarios);
            total++;
        }

    return Results.Ok("Total: " + total);
}).Accepts<IFormFile>("multipart/form-data");

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.Run();