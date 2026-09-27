using System.Globalization;

namespace CalculadoraIdade;

/// <summary>
/// Ponto de entrada do programa. Cuida apenas da entrada e saída de dados
/// pelo console; toda a lógica de cálculo fica na struct Pessoa.
/// </summary>
internal static class Program
{
    private const int IdadeMaximaAceita = 130;

    private static readonly string[] FormatosDeData = { "dd/MM/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/M/yyyy" };

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        ExibirTitulo();

        bool continuar = true;
        while (continuar)
        {
            Pessoa? pessoa = LerPessoa();

            if (pessoa is null)
            {
                break;
            }

            ExibirResultado(pessoa.Value);

            bool? resposta = PerguntarSeContinua();
            continuar = resposta ?? false;
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
        Console.WriteLine("Encerrando o programa. Até mais!");
    }

    private static void ExibirTitulo()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=========================================");
        Console.WriteLine("      CALCULADORA DE IDADE (CP1 C#)      ");
        Console.WriteLine("=========================================");
        Console.ResetColor();
        Console.WriteLine();
    }

    private static Pessoa? LerPessoa()
    {
        string? nome = LerNome();

        if (nome is null)
        {
            return null;
        }

        DateTime? dataNascimento = LerDataNascimento();

        if (dataNascimento is null)
        {
            return null;
        }

        return new Pessoa(nome, dataNascimento.Value);
    }

    private static string? LerNome()
    {
        while (true)
        {
            Console.Write("Digite o nome completo: ");
            string? entrada = Console.ReadLine();

            if (entrada is null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(entrada))
            {
                return entrada.Trim();
            }

            EscreverErro("O nome não pode ficar em branco. Tente novamente.");
        }
    }

    private static DateTime? LerDataNascimento()
    {
        while (true)
        {
            Console.Write("Digite a data de nascimento (dd/MM/yyyy): ");
            string? entrada = Console.ReadLine();

            if (entrada is null)
            {
                return null;
            }

            bool formatoValido = DateTime.TryParseExact(
                entrada,
                FormatosDeData,
                CultureInfo.GetCultureInfo("pt-BR"),
                DateTimeStyles.None,
                out DateTime data);

            if (!formatoValido)
            {
                EscreverErro("Data inválida. Use o formato dd/MM/yyyy, por exemplo 15/03/2000.");
                continue;
            }

            if (data > DateTime.Today)
            {
                EscreverErro("A data de nascimento não pode estar no futuro.");
                continue;
            }

            int idadeEstimativa = new Pessoa(string.Empty, data).CalcularIdade(DateTime.Today);
            if (idadeEstimativa > IdadeMaximaAceita)
            {
                EscreverErro($"Idade acima de {IdadeMaximaAceita} anos não é aceita. Confira a data digitada.");
                continue;
            }

            return data;
        }
    }

    private static void ExibirResultado(Pessoa pessoa)
    {
        int idade = pessoa.CalcularIdade(DateTime.Today);

        Console.WriteLine();
        Console.WriteLine($"Nome.......: {pessoa.Nome}");
        Console.WriteLine($"Nascimento.: {pessoa.DataNascimento:dd/MM/yyyy}");
        Console.WriteLine($"Idade......: {idade} {Pluralizar(idade, "ano", "anos")}");

        Console.Write("Maior de idade: ");
        EscreverSimOuNao(pessoa.EhMaiorDeIdade());

        Console.Write("Pode tirar CNH: ");

        if (pessoa.PodeTirarCnh())
        {
            EscreverSimOuNao(true);
        }
        else
        {
            EscreverSimOuNao(false);

            int anosParaCnh = pessoa.AnosParaCnh();
            if (anosParaCnh >= 1)
            {
                Console.WriteLine($"Faltam {anosParaCnh} {Pluralizar(anosParaCnh, "ano", "anos")} para poder tirar a CNH.");
            }

            Console.WriteLine($"Poderá iniciar o processo da CNH a partir de {pessoa.DataParaCnh():dd/MM/yyyy}.");
        }

        Console.WriteLine();
    }

    private static string Pluralizar(int quantidade, string singular, string plural)
    {
        return quantidade == 1 ? singular : plural;
    }

    private static void EscreverSimOuNao(bool valor)
    {
        Console.ForegroundColor = valor ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine(valor ? "Sim" : "Não");
        Console.ResetColor();
    }

    private static void EscreverErro(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(mensagem);
        Console.ResetColor();
    }

    private static bool? PerguntarSeContinua()
    {
        while (true)
        {
            Console.Write("Deseja calcular a idade de outra pessoa? (S/N): ");
            string? entrada = Console.ReadLine();

            if (entrada is null)
            {
                return null;
            }

            string resposta = entrada.Trim().ToUpperInvariant();

            if (resposta == "S")
            {
                Console.WriteLine();
                return true;
            }

            if (resposta == "N")
            {
                return false;
            }

            EscreverErro("Resposta inválida. Digite S para sim ou N para não.");
        }
    }
}
