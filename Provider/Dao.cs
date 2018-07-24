namespace SS.GovPublic.Provider
{
    public static class Dao
    {
        public static int GetIntResult(string sqlString)
        {
            var count = 0;

            using (var conn = Main.Instance.DatabaseApi.GetConnection(Main.Instance.ConnectionString))
            {
                conn.Open();
                using (var rdr = Main.Instance.DatabaseApi.ExecuteReader(conn, sqlString))
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        count = rdr.GetInt32(0);
                    }
                    rdr.Close();
                }
            }

            return count;
        }
    }
}
