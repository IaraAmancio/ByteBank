using System;
using System.IO;


public class Cliente
{
    public string cpf { get; set; }
    public string titular { get; set; }
    public string senha { get; set; }
    public double saldo { get; set; }
    public int numConta { get; set; }

    public Cliente()
    {
    }

    public Cliente(string cpf, string titular, string senha, double saldo)
    {
        this.cpf = cpf;
        this.titular = titular;
        this.senha = senha;
        this.saldo = saldo;
    }

    public void Depositar(double valor)
    {
        this.saldo += valor;
    }

    public void Sacar(double valor)
    {
        this.saldo -= valor;
    }
    // Metodo que verifica se um cpf é valido
    public bool ValidaCpf(string cpf)
    { 

        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf;
        string digito;

        int soma = 0;
        int resto;

        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");

        if (cpf.Length != 11)
            return false;

        tempCpf = cpf.Substring(0, 9);
        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        resto = soma % 11;
        if (resto < 2)
              resto = 0;
        else
            resto = 11 - resto;

        digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = digito + resto.ToString();
        return cpf.EndsWith(digito);
    }
    
}


class Program
{
    public static void MostrarMenu()
    {
        Console.WriteLine("-------------------------");
        Console.WriteLine("---------- ByteBank -----------\n");
        Console.WriteLine("Qual opção você deseja? \n");
        Console.WriteLine(
                          "1 - Inserir novo usuário\n" +
                          "2 - Deletar um usuário\n" +
                          "3 - Listar todas as contas registradas\n" +
                          "4 - Detalhes de um usuário\n" +
                          "5 - Total armazenado no banco\n" +
                          "6 - Manipular a conta\n" +
                          "0 - Sair");
        Console.WriteLine("--------------------------------\n");
    }

    public static void InserirCliente(List<Cliente> clientes)
    {       
        Cliente cliente = new Cliente();
        
        try
        {
            Console.WriteLine("\nDigite o cpf do cliente: ");
            cliente.cpf = Console.ReadLine();

            if (cliente.ValidaCpf(cliente.cpf))
            {
                Console.Write("\n\tCPF válido!\n");  
                Console.WriteLine("\nDigite o titular do cliente: ");
                cliente.titular = Console.ReadLine();
                Console.WriteLine("\nDigite o saldo do cliente: ");
                cliente.saldo = Double.Parse(Console.ReadLine());
                Console.WriteLine("\nDigite a senha do cliente: ");
                cliente.senha = Console.ReadLine();

                cliente.numConta = (clientes.Count + 1);
                clientes.Add(cliente);
                Console.Clear();
                Console.WriteLine("\nUsuário cadastrado com sucesso!\n\n");
            
            }
            else
            {
                Console.Write("\nCPF inválido! Tente novamente!\n");
            }          
        }
        catch (Exception)
        {
            Console.Clear();
            Console.WriteLine("\nEntrada inválida! Tente novamente!\n\n");
        }
    }
    public static void DeletarCliente(List<Cliente> clientes)
    {
        if (clientes.Count == 0)
        {
            Console.WriteLine("\nSem clientes cadastrados!");
            return;
        }
          
        try
        {
            Console.WriteLine("Digite o cpf do cliente a ser excluído  ");
            int index = clientes.FindIndex(cliente => cliente.cpf == Console.ReadLine());
            clientes.RemoveAt(index);
            Console.WriteLine("\nUsuário deletado com sucesso!\n");
        }
        catch (Exception)
        {
            Console.WriteLine("CPF inexistente!\n");
            DeletarCliente(clientes);
            
        }
    }

    public static void DetalhesCliente(List<Cliente> clientes)
    {
        Cliente cliente = new Cliente();
        Console.WriteLine("Qual o cpf do cliente:  ");
        int index = clientes.FindIndex(a => a.cpf == Console.ReadLine());
        Console.WriteLine($"Numero da conta:  {clientes[index].numConta.ToString().PadLeft(4, '0')}");
        Console.WriteLine($"CPF:  {clientes[index].cpf}  |  Titular:  {clientes[index].titular}");
        Console.WriteLine($"Saldo:  R$ {clientes[index].saldo:F2}");
        Console.WriteLine($"Senha:  {clientes[index].senha}");
    }
    public static void ListarContas(List<Cliente> clientes)
    {
        if (clientes.Count == 0)
        {
            Console.WriteLine("\nSem clientes cadastrados!\n");
            return;
        }
        foreach (Cliente cliente in clientes)
        {
            Console.WriteLine($"Numero da conta:  {cliente.numConta.ToString().PadLeft(4,'0')}");
            Console.WriteLine($"CPF:  {cliente.cpf}  |  Titular:  {cliente.titular}");
            Console.WriteLine($"Saldo:  R$ {cliente.saldo:F2}");
            Console.WriteLine($"Senha:  {cliente.senha}");           
            Console.WriteLine();      
        }     
    }
    public static void TotalArmazenado(List<Cliente> clientes)
    {
        double soma = 0;
        for(int i = 0; i < clientes.Count() ; i++)
        {
            soma += clientes[i].saldo;
        }
        Console.WriteLine($"Valor armazenado no ByteBank:  R$ {soma:F2}\n");
    }

    // Metodo que verifica se o cpf e senha inseridos pelo usuario estao corretos
    // antes de efetuar alguma manipulacao com o saldo da conta
    public static int Seguranca(List<Cliente> clientes)
    {      
        Console.Clear();
        Console.WriteLine("---------- ByteBank -----------");
        Console.WriteLine("-------------------------\n");
        string cpf, senha;
        Console.WriteLine("Digite o CPF: ");
        cpf = Console.ReadLine();
        Console.WriteLine("\nDigite a senha: ");
        senha = Console.ReadLine();
        if (clientes.Any(cliente => cliente.cpf == cpf))
        {
            int index = clientes.FindIndex(cliente => cliente.cpf == cpf);
            if (clientes[index].senha == senha)
            {
                Console.WriteLine("\nOs dados estao corretos!\n");
                Console.WriteLine($"Numero da conta:  {clientes[index].numConta.ToString().PadLeft(4, '0')} |  Titular:  {clientes[index].titular}");
                Console.WriteLine();
                return index;
            }
        }
        Console.Clear();
        Console.WriteLine("\nOs dados estao incorretos!\n");
        
        return -1;
    }
    public static void Depositar(List<Cliente> clientes)
    {
        int index = Seguranca(clientes);
        if( index == -1)
            return;
        double valor;
        
        try
        {
            Console.WriteLine("Qual o valor do deposito?");
            valor = Double.Parse(Console.ReadLine());
            clientes[index].Depositar(valor);
            Console.Clear();
            Console.WriteLine("\nDeposito efetudado com sucesso!\n");
        }
        catch (Exception)
        {
            Console.Clear();
            Console.WriteLine("Houve algum erro. Tente Novamente!"); 
        }
    }

    public static void Sacar(List<Cliente> clientes)
    {
        int index = Seguranca(clientes);
        if (index == -1)
            return;
        double valor;
        try
        {
            Console.WriteLine("Qual o valor do Saque?");
            valor = Double.Parse(Console.ReadLine());
            if (clientes[index].saldo < valor)
            {
                Console.Clear();
                Console.WriteLine("Não é possivel efetuar o Saque! Saldo insuficiente!\n");
                return;
            }

            clientes[index].Sacar(valor);
            Console.Clear();
            Console.WriteLine("\nSaque efetudado com sucesso!\n");
        }
        catch (Exception)
        {
            Console.Clear();
            Console.WriteLine("Houve algum erro. Tente Novamente!");
        }      
    }

    public static void Transferir(List<Cliente> clientes)
    {
        int index1 = Seguranca(clientes);
        int index2 = 0;
        Cliente cliente = new Cliente();     
        string cpf_secundario;
        double valor;
        if (index1 == -1)
            return;       
        try
        {
            Console.WriteLine("Para qual CPF voce deseja efetuar a transferencia?");
            cpf_secundario =  Console.ReadLine();
            if (!clientes.Any(cliente => cliente.cpf == cpf_secundario))
            {
                Console.WriteLine("CPF invalido!");
                return;
            }
            index2 = clientes.FindIndex(cliente => cliente.cpf == cpf_secundario);
            int opcao;
            do
            {
                Console.WriteLine($"\nDeseja transferir um certo valor para {clientes[index2].titular} ?\n\n" +
                                    "0 - Nao\n" +
                                    "1 - Sim\n");
                opcao = int.Parse(Console.ReadLine());
                switch (opcao)
                {
                    case 0:
                        Console.Clear();
                        return;
                        break;
                    case 1:
                        break;
                    default:
                        Console.WriteLine("Opcao invalida! Tente Novamente!");
                        break;
                }
            } while (opcao != 1);
           
            Console.WriteLine("Qual o valor da Transferencia?");
            valor = Double.Parse(Console.ReadLine());

            if (clientes[index1].saldo < valor)
            {
                Console.Clear();
                Console.WriteLine("Não é possivel efetuar a Tranferencia! Saldo insuficiente!");
                return;
            }

            clientes[index1].Sacar(valor);
            clientes[index2].Depositar(valor);
            Console.Clear();
            Console.WriteLine("\nTransferencia efetudada com sucesso!\n");
        }
        catch (Exception)
        {
            Console.Clear();
            Console.WriteLine("Houve algum erro. Tente Novamente!");
        }

    }
    public static void MenuOperacoes()
    {
        Console.WriteLine("-------------------------");
        Console.WriteLine("---------- ByteBank -----------\n");
        Console.WriteLine("Qual opção você deseja? \n");
        Console.WriteLine(
                          "1 - Depositar\n" +
                          "2 - Sacar\n" +
                          "3 - transferir\n" +
                          "0 - Voltar");
        Console.WriteLine("--------------------------------\n");
    }
    public static void ManipularConta(List<Cliente> clientes)
    {
        int opcao;
        do
        {
            MenuOperacoes();
            if (clientes.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("\nSem clientes cadastrados!");
                return;
            }
            opcao = int.Parse(Console.ReadLine());
            switch(opcao)
            {
                case 0:
                    break;
                case 1:
                    Depositar(clientes);
                    break;
                case 2:
                    Sacar(clientes);
                    break;
                case 3:
                    Transferir(clientes);
                    break;
                default:
                    Console.WriteLine("Opcao inexistente!");
                    break;
            }
        } while (opcao != 0);
        Console.Clear();      
    }

    static void Main(string[] args)
    {
        int opcao = -1;
        List<Cliente> clientes = new List<Cliente>();
        while (true)
        {
            do
            {
                MostrarMenu();
                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    Console.Clear();
                    Console.WriteLine("\n-------------------------");
                    Console.WriteLine("---------- ByteBank -----------\n");
                    switch (opcao)
                    {
                        case 1:
                            InserirCliente(clientes);
                            break;
                        case 2:
                            DeletarCliente(clientes);
                            break;
                        case 3:
                            ListarContas(clientes);
                            break;
                        case 4:
                            DetalhesCliente(clientes);
                            break;
                        case 5:
                            TotalArmazenado(clientes);
                            break;
                        case 6:
                            Console.Clear();
                            ManipularConta(clientes);
                            break;
                        case 0:
                            Console.WriteLine("\nEstou encerrando o programa!\n");
                            break;
                        default:
                            Console.WriteLine("\nOpção inválida! Digite novamente! \n");
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("\nEntrada inválida! Tente novamente!\n");   
                }

            } while (opcao != 0);
            if (opcao == 0) break;
           
        }
        Console.WriteLine("-------------------------");
        Console.WriteLine("---------- ByteBank -----------");
    }
}
