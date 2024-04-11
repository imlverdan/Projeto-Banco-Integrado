using ProjetoBanco.DAL;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace ProjetoBanco
{
    namespace ProjetoBanco
    {
        class Program
        {
            static BancoDAL bancoDAL = new BancoDAL();
            Conta conta = new Conta();
            Cliente cliente = new Cliente();
            static void Main(string[] args)
            {
                ExibirMenuBanco();
            }
            static void ExibirMenuBanco()
            {
                while (true)
                {
                    Console.WriteLine("———————————————MENU———————————————");
                    Console.WriteLine("1. Cadastrar nova Conta");
                    Console.WriteLine("2. Transferir Dinheiro");
                    Console.WriteLine("3. Depositar Dinheiro");
                    Console.WriteLine("4. Consultar Cliente");
                    Console.WriteLine("5. Encerramento de conta");
                    Console.WriteLine("6. Sair");

                    Console.Write("Opção: ");
                    if (!int.TryParse(Console.ReadLine(), out int opcao))
                    {
                        Console.WriteLine("Opção inválida. Por favor, escolha uma opção válida.");
                        continue;
                    }

                    switch (opcao)
                    {
                        case 1:
                            CadastrarNovaConta();
                            break;
                        case 2:
                            Transferir(bancoDAL);
                            break;
                        case 3:
                            FazerDeposito(bancoDAL);
                            break;
                        case 4:
                            ConsultarSaldo(bancoDAL);
                            break;
                        case 5:
                            Excluir(bancoDAL);
                            break;
                        case 6:
                            Console.WriteLine("Sistema finalizado.");
                            return;
                        default:
                            Console.WriteLine("Opção inválida. Por favor, escolha uma opção válida.");
                            break;
                    }
                }
            }
            static void CadastrarNovaConta()
            {
                while (true)
                {
                    Cliente cliente = new Cliente();
                    Conta conta = new Conta();

                    Console.WriteLine("CPF de cadastro (11 dígitos sem pontos ou traços): ");
                    string? CPF = Console.ReadLine();

                    if (CPF?.Length != 11 || !CPF.All(char.IsDigit))
                    {
                        Console.WriteLine("CPF inválido. Por favor, insira um CPF com 11 dígitos numéricos.");
                        continue;
                    }

                    cliente.CpfCliente = CPF;


                    if (bancoDAL.ConsultarCpf(cliente))
                    {
                        Console.WriteLine("CPF já cadastrado. Por favor, insira um CPF diferente.");
                        continue;
                    }

                    Console.WriteLine("Nome de cadastro: ");
                    string? Nome = Console.ReadLine();
                    cliente.NomeCliente = Nome;

                    Console.WriteLine("Digite sua senha: ");
                    string? Senha = Console.ReadLine();
                    cliente.SenhaCliente = Senha;

                    Console.WriteLine("Qual o tipo da conta? (0 para Conta Corrente ou 1 para Conta Poupanca): ");
                    string? inputTipoConta = Console.ReadLine();

                    if (Enum.TryParse<TipoConta>(inputTipoConta, out TipoConta tipoConta))
                    {
                        Conta novaConta;
                        switch (tipoConta)
                        {
                            case TipoConta.ContaCorrente:
                                novaConta = new Conta(tipoConta);
                                break;
                            case TipoConta.ContaPoupanca:
                                novaConta = new Conta(tipoConta);
                                break;
                            default:
                                Console.WriteLine("Tipo de conta inválido. Por favor, escolha um tipo válido.");
                                continue;
                        }

                        Console.WriteLine("Digite sua data de nascimento (no formato dd/MM/yyyy): ");
                        string? inputNascimento = Console.ReadLine();

                        DateTime dataNascimento;
                        if (DateTime.TryParseExact(inputNascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataNascimento))
                        {
                            cliente.DataNascimento = dataNascimento;
                        }
                        else
                        {
                            Console.WriteLine("Formato de data inválido. Por favor, digite no formato dd/MM/yyyy.");
                            continue;
                        }

                        cliente.Conta = novaConta;
                        bancoDAL.AdicionarCliente(cliente, novaConta);

                        Console.WriteLine("Cliente cadastrado com sucesso!");
                        Console.WriteLine($"Número da conta: {novaConta.NumeroConta}");
                        Console.WriteLine($"Tipo de conta: {tipoConta}");
                        Console.WriteLine($"Saldo: {cliente.Conta.SaldoConta}");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Tipo de conta inválido. Por favor, escolha um tipo válido.");
                        continue;
                    }
                }
            }
            static void ConsultarSaldo(BancoDAL bancoDAL)
            {
                Console.WriteLine("Digite o CPF do cliente: ");
                string? cpf = Console.ReadLine();

                Console.WriteLine("Digite a senha: ");
                string? senha = Console.ReadLine();

                Cliente cliente = bancoDAL.EncontrarCliente(cpf, senha);

                if (cliente != null)
                {
                    cliente.AtualizarTipoCliente();
                    Console.WriteLine($"Nome: {cliente.NomeCliente.ToUpper()}");
                    Console.WriteLine($"Tipo de conta: {cliente.Conta.Tipo}");
                    Console.WriteLine($"Número da conta: {cliente.Conta.NumeroConta}");
                    Console.WriteLine($"Saldo atual: {cliente.Conta.SaldoConta.ToString("C2")}");
                    Console.WriteLine($"Cliente do tipo: {cliente.Tipo}");
                }
                else
                {

                    Console.WriteLine("Credenciais inválidas. Por favor, verifique o CPF e a senha e tente novamente.");
                }
            }
            static void FazerDeposito(BancoDAL bancoDAL)
            {
                Console.WriteLine("Digite o CPF do cliente: ");
                string? cpf = Console.ReadLine();

                Console.WriteLine("Digite a senha: ");
                string? senha = Console.ReadLine();

                Cliente cliente = bancoDAL.EncontrarCliente(cpf, senha);

                if (cliente != null)
                {
                    Console.WriteLine("Digite o valor a ser depositado: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal valorDeposito))
                    {
                        bancoDAL.FazerDeposito(cliente, valorDeposito);

                    }
                    else
                    {
                        Console.WriteLine("Valor inválido. Digite um valor numérico válido.");
                    }
                }
                else
                {

                    Console.WriteLine("Credenciais inválidas. Por favor, verifique o CPF e a senha e tente novamente.");
                }
            }
            static void Transferir(BancoDAL bancoDAL)
            {
                Console.WriteLine("Digite o CPF do remetente: ");
                string? cpf = Console.ReadLine();

                Console.WriteLine("Digite a senha: ");
                string? senha = Console.ReadLine();

                Cliente clienteOrigem = bancoDAL.EncontrarCliente(cpf, senha);

                if (clienteOrigem != null)
                {
                    Console.WriteLine("Digite o CPF do destinatário");
                    string? cpfDestinatario = Console.ReadLine();

                    Console.WriteLine("Digite o valor a ser transferido: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal valorTransferencia) || valorTransferencia <= 0)
                    {
                        Console.WriteLine("Valor inválido. Digite um valor numérico válido e maior que zero.");
                        return;
                    }

                    Cliente clienteDestino = bancoDAL.EncontrarCliente(cpfDestinatario, senha);

                    if (clienteDestino == null)
                    {
                        Console.WriteLine("Cliente destinatário não encontrado.");
                        return;
                    }

                    bancoDAL.Transferir(clienteOrigem, clienteDestino, valorTransferencia);
                }
                else
                {
                    Console.WriteLine("Credenciais inválidas. Verifique o CPF e a senha e tente novamente.");
                }
            }
            static void Excluir(BancoDAL bancoDAL)
            {
                Console.WriteLine("Digite o CPF do cliente para excluir a conta: ");
                string? cpf = Console.ReadLine();


                Console.WriteLine("Confirme sua senha para exclusão da conta: ");
                string? senha = Console.ReadLine();

                bool contaExcluida = bancoDAL.Excluir(cpf, senha);

                if (contaExcluida)
                {
                    Console.WriteLine("Conta excluida com sucesso!");
                }
                else
                {
                    Console.WriteLine("Erro de credenciais, CPF ou SENHA inválidos.");
                }
            }
        }
    }
}