using System.Data.SQLite;

namespace HealthApp.Database
{
    class DatabaseSource
    {
        public static readonly SQLiteConnection SQLiteConn =
            new SQLiteConnection($"Data Source=UserData.db;Version=3;");

        public static SQLiteConnection? CreateConnection()
        {
            try
            {
                SQLiteConn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database error " + ex.Message);
                Environment.Exit(1);
            }

            return SQLiteConn;
        }

        public static void CloseConnection()
        {
            try
            {
                SQLiteConn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database error " + ex.Message);
                Environment.Exit(0);
            }
        }


    }
}
