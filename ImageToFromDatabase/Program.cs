using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace ImageToFromDatabase
{
    internal class Program
    {
        readonly static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ImageToFromDatabase.Properties.Settings.LifelongLearningConnectionString"].ConnectionString;

        static void Main(string[] args)
        {
            //SaveImagesToDb("ImagesIn");

            DatabaseFileRead("ImagesOut");

            Console.WriteLine("Press any key.. Hey, where is any key?");
            Console.ReadKey();
        }


        private static void SaveImagesToDb(string path)
        {
            foreach (string filePath in Directory.EnumerateFiles(path, "*.*"))
            {
                Console.WriteLine(filePath);
                DatabaseFilePut(filePath);
            }
        }

        public static void DatabaseFilePut(string filePath)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                byte[] file;

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        file = reader.ReadBytes((int)stream.Length);
                    }
                }

                using (var sqlWrite = new SqlCommand("INSERT INTO [dbo].[Picture] Values(@Name,@Type,@Hash,@PicData)", connection))
                {
                    sqlWrite.Parameters.Add("@Name", SqlDbType.VarChar).Value = Path.GetFileName(filePath);
                    sqlWrite.Parameters.Add("@Type", SqlDbType.VarChar).Value = Path.GetExtension(filePath);
                    sqlWrite.Parameters.Add("@Hash", SqlDbType.VarChar).Value = DBNull.Value;
                    sqlWrite.Parameters.Add("@PicData", SqlDbType.VarBinary, file.Length).Value = file;
                    sqlWrite.ExecuteNonQuery();
                }
            }
        }
        
        
        //This method is to get file from database and save it on drive:
        public static void DatabaseFileRead(string folderPath)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var sqlCommand = new SqlCommand(
                @"SELECT TOP (1000) 
	               [ID]
                  ,[Name]
                  ,[Type]
                  ,[Hash]
                  ,[PicData]
              FROM [LifelongLearning].[dbo].[Picture]", connection))
            {
                connection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}", reader[0], reader[1], reader[2], reader[3], reader[4]));

                        var blob = new Byte[(reader.GetBytes(4, 0, null, 0, int.MaxValue))];
                        reader.GetBytes(4, 0, blob, 0, blob.Length);

                        string fileName = reader[1].ToString();
                        string filePath = folderPath + "\\" + fileName;

                        using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            fs.Write(blob, 0, blob.Length);

                    }
                }

            }
        }



    }
}
