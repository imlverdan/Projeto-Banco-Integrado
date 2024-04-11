using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoBanco.DAL
{
    public class BaseDAL
    {
        protected SqlConnection com;

        public BaseDAL()
        {
            com = new SqlConnection(@"Data Source=DESKTOP-90B31F5\MSQLSERVER;Initial Catalog=ProjetoBanco;User ID=sa;Password=34036375");
        }
        protected void AbrirConexao()
        {
            if (com.State != ConnectionState.Open)
            {
                com.Open();
            }
        }
        protected void FecharConexao()
        {
            if (com.State != ConnectionState.Closed)
            {
                com.Close();
            }
        }
    }
}