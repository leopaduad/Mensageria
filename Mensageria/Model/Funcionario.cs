namespace Mensageria.Model
{
    public record Funcionario
    {
        public int Id { get; set; }
        public int Matricula { get; set; }
        public DateTime DataInicio { get; set; }
    }
}