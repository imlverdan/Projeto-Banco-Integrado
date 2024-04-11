using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProjetoBanco
{
    public class Cliente
    {
        public string? CPF;
        public string? Nome;
        public string? Senha;
        public DateTime Nascimento;
        public TipoCliente Tipo { get; set; }
        public Conta? Conta { get; set; }
        private static readonly Random rand = new Random();
        private static HashSet<int> idsGerados = new HashSet<int>();

        public string? SenhaCliente
        {
            get { return Senha; }
            set { Senha = value; }
        }
        public string? CpfCliente
        {
            get { return CPF; }
            set { CPF = value; }
        }
        public string? NomeCliente
        {
            get { return Nome; }
            set { Nome = value; }
        }
        public DateTime DataNascimento
        {
            get { return Nascimento; }
            set { Nascimento = value; }
        }
        public void AtualizarTipoCliente()
        {
            if (Conta?.SaldoConta >= 15000)
            {
                Tipo = TipoCliente.Premium;
            }
            else if (Conta?.SaldoConta >= 5000)
            {
                Tipo = TipoCliente.Super;
            }
            else
            {
                Tipo = TipoCliente.Comum;
            }
        }
    }
}