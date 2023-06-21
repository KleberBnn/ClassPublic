using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ERP_V1
{
    class PreencheComboBox
    {
        internal static void CarregaDataSet(ComboBox _cbn, string _Sql, string _nameSet)
        {
            if (Db.sqlConnection.State == System.Data.ConnectionState.Closed) Db.sqlConnection.Open();
            SqlDataAdapter _dtaAdp = new SqlDataAdapter(_Sql, Db.sqlConnection);
            System.Data.DataSet _dtaSet = new System.Data.DataSet();

            _dtaAdp.Fill(_dtaSet, _nameSet);
            _cbn.DataSource = _dtaSet.Tables[_nameSet];
            _cbn.DisplayMember = "DISPLAY";
            _cbn.ValueMember = "COD";
            _cbn.SelectedIndex = -1;

            _dtaSet.Dispose();
            _dtaAdp.Dispose();
        }

        internal static void CarregaDataReder(ComboBox _combo, string _select, string _selectCount, ToolStripProgressBar prgBar)
        {
            try
            {
                _combo.Items.Clear();
                if (Db.sqlConnection.State == System.Data.ConnectionState.Closed) Db.sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(_selectCount, Db.sqlConnection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.SingleResult);

                float r = 0;
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    r = Convert.ToSingle(String.Format("{0:n2}", 100 / Convert.ToSingle(sqlDataReader[0])));
                }
                sqlCommand.Dispose();
                sqlDataReader.Close();
                sqlCommand = new SqlCommand(_select, Db.sqlConnection);
                sqlDataReader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.SingleResult);
                prgBar.Value = 0;

                int n = 0;
                float r1 = 0;

                while (sqlDataReader.Read())
                {
                    try
                    {
                        r1 = (r1 + r);
                        r = 1;
                        n = Convert.ToInt32(r1);
                    }
                    catch
                    {
                        n = (n + 1);
                    }
                    _combo.Items.Add(sqlDataReader[0].ToString());
                    prgBar.Value = n > 100 ? 100 : n;
                }
                sqlCommand.Dispose();
                sqlDataReader.Close();
                prgBar.Value = 0;
            }
            catch
            {
                if (Db.sqlConnection.State == System.Data.ConnectionState.Open)
                    Db.sqlConnection.Close();
            }
        }
    }
}
