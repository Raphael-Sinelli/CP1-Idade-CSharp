namespace CalculadoraIdade;

/// <summary>
/// Representa uma pessoa com nome e data de nascimento, capaz de calcular
/// a própria idade e informar sobre maioridade e habilitação para dirigir.
/// </summary>
public struct Pessoa
{
    /// <summary>Idade mínima, em anos, para ser considerado maior de idade e poder tirar CNH.</summary>
    public const int IdadeMinima = 18;

    public string Nome { get; }
    public DateTime DataNascimento { get; }

    public Pessoa(string nome, DateTime dataNascimento)
    {
        Nome = nome;
        DataNascimento = dataNascimento;
    }

    /// <summary>
    /// Calcula a idade em anos completos na data informada, descontando um ano
    /// caso o aniversário ainda não tenha ocorrido naquele ano.
    /// </summary>
    public int CalcularIdade(DateTime hoje)
    {
        int idade = hoje.Year - DataNascimento.Year;

        bool aniversarioAindaNaoOcorreu = hoje.Month < DataNascimento.Month
            || (hoje.Month == DataNascimento.Month && hoje.Day < DataNascimento.Day);

        if (aniversarioAindaNaoOcorreu)
        {
            idade--;
        }

        return idade;
    }

    /// <summary>Informa se a pessoa é maior de idade, com base na data atual do sistema.</summary>
    public bool EhMaiorDeIdade()
    {
        return CalcularIdade(DateTime.Today) >= IdadeMinima;
    }

    /// <summary>Informa se a pessoa já pode iniciar o processo de habilitação (CNH).</summary>
    public bool PodeTirarCnh()
    {
        return CalcularIdade(DateTime.Today) >= IdadeMinima;
    }

    /// <summary>Quantidade de anos que ainda faltam para a pessoa poder tirar a CNH.</summary>
    public int AnosParaCnh()
    {
        int faltam = IdadeMinima - CalcularIdade(DateTime.Today);
        return faltam > 0 ? faltam : 0;
    }
}
