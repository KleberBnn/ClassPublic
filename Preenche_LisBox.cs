using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ER_V1
{
    class PreencheListBox
    {
        public static void CarregaDataReader(ListBox _pesquisaList, string _pesquisa, string _pesquisaCount, ToolStripProgressBar _progress)
        {
            _pesquisaList.Items.Clear();
            if (Db.sqlConnection.State == System.Data.ConnectionState.Closed) Db.sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(_pesquisaCount, Db.sqlConnection);
            System.Data.SqlClient.SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(SingleResult);

            float r = 0;
            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                r = int.Parse(sqlDataReader[0].ToString()) > 0 ? Convert.ToSingle(String.Format("{0:n2}", 100 / Convert.ToSingle(sqlDataReader[0]))) : 0;
            }

            sqlCommand.Dispose();
            sqlDataReader.Close();

            sqlCommand = new SqlCommand(_pesquisa, Db.sqlConnection);
            sqlDataReader = sqlCommand.ExecuteReader();
            _progress.Value = 0;
            int n = 0;
            float r1 = 0;

            if (sqlDataReader.HasRows)
                while (sqlDataReader.Read())
                {
                    r1 = r1 + r;
                    n = Convert.ToInt32(r1);
                    _pesquisaList.Items.Add(Convert.ToString(sqlDataReader[0]));
                    _progress.Value = n > 100 ? 100 : n;
                }
            sqlCommand.Dispose();
            sqlDataReader.Close();
            Db.sqlConnection.Close();
            _progress.Value = 0;
        }

        internal static void CarregaDataSet(ListBox _Lst, string _Sql, string _nameSet)
        {
            if (Db.sqlConnection.State == System.Data.ConnectionState.Closed) Db.sqlConnection.Open();
            System.Data.SqlClient.SqlDataAdapter _dtaAdp = new System.Data.SqlClient.SqlDataAdapter(_Sql, Db.sqlConnection);
            System.Data.DataSet _dtaSet = new System.Data.DataSet();

            _dtaAdp.Fill(_dtaSet, _nameSet);
            _Lst.DataSource = _dtaSet.Tables[_nameSet];
            _Lst.DisplayMember = "DISPLAY";
            _Lst.ValueMember = "COD";

            _dtaSet.Dispose();
            _dtaAdp.Dispose();
            Db.sqlConnection.Close();
        }
    }
}
