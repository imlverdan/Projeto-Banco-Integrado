using System;

namespace ProjetoBanco
{
    public class Conta : IContaCorrente, IContaPoupanca
    {     
        public string NumeroConta { get; set; }
        public decimal SaldoConta { get; set; }
        public TipoConta Tipo { get; set; }
        decimal IContaCorrente.TaxaManutencao { get; set; }
        decimal IContaPoupanca.TaxaRendimento { get; set; }
        private static readonly Random rand = new Random();

         public Conta(TipoConta tipoConta)
        {
            Tipo = tipoConta;
            
            NumeroConta = GerarNumeroContaAleatorio();
            SaldoConta = 0;
            ((IContaCorrente)this).TaxaManutencao = 10;
        }
        public Conta()
        {
        }
        private string GerarNumeroContaAleatorio()
        {
            string numeroConta = "";

            for (int i = 0; i < 6; i++)
            {
                numeroConta += rand.Next(0, 10);
            }

            return numeroConta;
        }
        public decimal DescontarTaxa(decimal taxaManutencao)
        {   
                    if (SaldoConta >= taxaManutencao)
                    {
                        SaldoConta -= taxaManutencao;
                        
                        return taxaManutencao;
                    }
                    else
                    {
                        throw new InvalidOperationException("Saldo insuficiente para descontar a taxa de manutenção.");
                    }         
        }
        public void AcrescentarRendimento()
        {
            decimal rendimento = SaldoConta * ((IContaPoupanca)this).TaxaRendimento;
            SaldoConta += rendimento;

        }
    }
}