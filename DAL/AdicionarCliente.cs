using ProjetoBanco;
using System;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace ProjetoBanco.DAL
{
    public class BancoDAL : BaseDAL
    {
        public void AdicionarCliente(Cliente cliente, Conta conta)
        {
            try
            {
                AbrirConexao();
                string queryAdicionarCliente = @"INSERT INTO TB_CLIENTE (ds_cliente, nm_cliente, pw_cliente, dn_cliente, nm_conta, sd_conta, tp_contaid)
                                               VALUES (@ds_cliente, @nm_cliente, @pw_cliente, @dn_cliente, @nm_conta, @sd_conta, @tp_contaid)";
                SqlCommand sqlAdicionarCliente = new SqlCommand(queryAdicionarCliente, com);

                sqlAdicionarCliente.Parameters.AddWithValue("@ds_cliente", cliente.CPF);
                sqlAdicionarCliente.Parameters.AddWithValue("@nm_cliente", cliente.Nome);
                sqlAdicionarCliente.Parameters.AddWithValue("@pw_cliente", cliente.Senha);
                sqlAdicionarCliente.Parameters.AddWithValue("@dn_cliente", cliente.Nascimento);
                sqlAdicionarCliente.Parameters.AddWithValue("@nm_conta", conta.NumeroConta);
                sqlAdicionarCliente.Parameters.AddWithValue("@sd_conta", conta.SaldoConta);
                sqlAdicionarCliente.Parameters.AddWithValue("@tp_contaid", conta.Tipo);

                sqlAdicionarCliente.ExecuteNonQuery();
                Console.WriteLine("Cliente cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no SQL ao adicionar cliente: {ex.Message}");
            }
            finally
            {
                FecharConexao();
            }
        }
        public bool ConsultarCpf(Cliente cliente)
        {
            try
            {

                if (cliente == null)
                {
                    throw new ArgumentNullException(nameof(cliente), "Cliente não pode ser nulo.");
                }

                if (string.IsNullOrWhiteSpace(cliente.CpfCliente))
                {
                    throw new ArgumentException("CPF do cliente inválido.", nameof(cliente.CpfCliente));
                }

                AbrirConexao();

                string queryVerificarCPF = "SELECT COUNT(*) FROM TB_CLIENTE WHERE ds_cliente = @ds_cliente";
                using (SqlCommand sqlVerificarCPF = new SqlCommand(queryVerificarCPF, com))
                {
                    sqlVerificarCPF.Parameters.AddWithValue("@ds_cliente", cliente.CpfCliente);

                    int count = (int)sqlVerificarCPF.ExecuteScalar();

                    if (count > 0)
                    {

                        return true;
                    }
                    else
                    {

                        return false;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erro SQL ao consultar cliente: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao consultar cliente: {ex.Message}");
                return false;
            }
            finally
            {
                FecharConexao();
            }
        }
        public Cliente EncontrarCliente(string cpf, string senha)
        {
            Cliente cliente = new Cliente();
            Conta conta = new Conta();

            try
            {
                AbrirConexao();

                string queryEncontrarCliente = "SELECT * FROM TB_CLIENTE WHERE ds_cliente = @ds_cliente AND pw_cliente = @pw_cliente";

                using (SqlCommand sqlEncontrarCliente = new SqlCommand(queryEncontrarCliente, com))
                {
                    sqlEncontrarCliente.Parameters.AddWithValue("@ds_cliente", cpf);
                    sqlEncontrarCliente.Parameters.AddWithValue("@pw_cliente", senha);
                    SqlDataReader reader = sqlEncontrarCliente.ExecuteReader();

                    if (reader.Read())
                    {
                        cliente = new Cliente
                        {
                            CpfCliente = reader["ds_cliente"].ToString(),
                            NomeCliente = reader["nm_cliente"].ToString(),
                            SenhaCliente = reader["pw_cliente"].ToString(),
                            DataNascimento = Convert.ToDateTime(reader["dn_cliente"]),
                            Conta = new Conta
                            {
                                NumeroConta = reader["nm_conta"].ToString(),
                                SaldoConta = Convert.ToDecimal(reader["sd_conta"]),
                                Tipo = (TipoConta)Convert.ToInt32(reader["tp_contaid"])
                            }
                        };
                       
                        if (cliente.Conta.SaldoConta >= 15000)
                        {
                            cliente.Conta.Tipo = (TipoConta)TipoCliente.Premium;
                        }
                        else if (cliente.Conta.SaldoConta >= 5000)
                        {
                            cliente.Tipo = TipoCliente.Super;
                        }
                        else
                        {
                            cliente.Tipo = TipoCliente.Comum;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erro SQL ao consultar cliente: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao consultar cliente: {ex.Message}");
            }
            finally
            {
                FecharConexao();
            }

            return cliente;
        }

        public void FazerDeposito(Cliente cliente, decimal valorDeposito)
        {
            try
            {
                AbrirConexao();

                string queryAtualizarSaldo = @"UPDATE TB_CLIENTE
                                             SET sd_conta = sd_conta + @valorDeposito
                                             WHERE ds_cliente = @ds_cliente";

                SqlCommand sqlAtualizarSaldo = new SqlCommand(queryAtualizarSaldo, com);
                sqlAtualizarSaldo.Parameters.AddWithValue("@valorDeposito", valorDeposito);
                sqlAtualizarSaldo.Parameters.AddWithValue("@ds_cliente", cliente.CpfCliente);
                sqlAtualizarSaldo.ExecuteNonQuery();

                cliente.Conta.SaldoConta += valorDeposito;

                string queryConsultarSaldo = "SELECT sd_conta FROM TB_CLIENTE WHERE ds_cliente = @ds_cliente";
                SqlCommand sqlConsultarSaldo = new SqlCommand(queryConsultarSaldo, com);
                sqlConsultarSaldo.Parameters.AddWithValue("@ds_cliente", cliente.CpfCliente);
                decimal saldoAtualizado = Convert.ToDecimal(sqlConsultarSaldo.ExecuteScalar());

                Console.WriteLine($"Saldo atual: {saldoAtualizado.ToString("C2")}");
                Console.WriteLine("Depósito realizado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao fazer depósito: {ex.Message}");
            }
            finally
            {
                FecharConexao();
            }
        }
        public void Transferir(Cliente clienteOrigem, Cliente clienteDestino, decimal valorTransferencia)
        {
            try
            {
                AbrirConexao();

                string queryDescontarSaldo = @"UPDATE TB_CLIENTE
                                             SET sd_conta = sd_conta - @valorTransferencia
                                             WHERE ds_cliente = @cpfClienteOrigem";

                SqlCommand sqlDescontarSaldo = new SqlCommand(queryDescontarSaldo, com);
                sqlDescontarSaldo.Parameters.AddWithValue("@valorTransferencia", valorTransferencia);
                sqlDescontarSaldo.Parameters.AddWithValue("@cpfClienteOrigem", clienteOrigem.CpfCliente);
                sqlDescontarSaldo.ExecuteNonQuery();

                string queryAdicionarSaldo = @"UPDATE TB_CLIENTE
                                             SET sd_conta = sd_conta + @valorTransferencia
                                             WHERE ds_cliente = @cpfClienteDestino";

                SqlCommand sqlAdicionarSaldo = new SqlCommand(queryAdicionarSaldo, com);
                sqlAdicionarSaldo.Parameters.AddWithValue("@valorTransferencia", valorTransferencia);
                sqlAdicionarSaldo.Parameters.AddWithValue("@cpfClienteDestino", clienteDestino.CpfCliente);
                sqlAdicionarSaldo.ExecuteNonQuery();

                Console.WriteLine("Transferência realizada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao transferir: {ex.Message}");
            }
            finally
            {
                FecharConexao();
            }
        }
        public bool Excluir(string cpf, string senha)
        {
            try
            {
                AbrirConexao();

                string queryVerificarCliente = "SELECT COUNT(1) FROM TB_CLIENTE " +
                                               "WHERE ds_cliente = @ds_cliente AND pw_cliente = @pw_cliente";
                SqlCommand sqlVerificarCliente = new SqlCommand(queryVerificarCliente, com);
                sqlVerificarCliente.Parameters.AddWithValue("@ds_cliente", cpf);
                sqlVerificarCliente.Parameters.AddWithValue("@pw_cliente", senha);
                int clienteExistente = (int)sqlVerificarCliente.ExecuteScalar();

                if (clienteExistente == 0)
                {
                    Console.WriteLine("Cliente não encontrado ou senha incorreta.");
                    return false;
                }

                string queryExcluirConta = "DELETE FROM TB_CLIENTE WHERE ds_cliente = @ds_cliente";
                SqlCommand sqlExcluirConta = new SqlCommand(queryExcluirConta, com);
                sqlExcluirConta.Parameters.AddWithValue("@ds_cliente", cpf);
                sqlExcluirConta.ExecuteNonQuery();

                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir conta: {ex.Message}");
                return false;
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}