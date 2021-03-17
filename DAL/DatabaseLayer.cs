using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Logger;

namespace DAL
{
    public static class DatabaseLayer
    {
       public static void InsertDataIntoSQLServerUsingSQLBulkCopy(DataTable csvFileData, string DataConnection, string filename)
        {
            // change the data source value as needed LN443397
            using (SqlConnection dbConnection =
                   new SqlConnection(DataConnection))
                // using (SqlConnection dbConnection =
                // new SqlConnection("Data Source=RAMESHI7;Initial Catalog=RAMTEST;Integrated Security=SSPI;"))
                //RAMESHhrd2in1
                //rameshdi5
                try
                {
                    {
                        dbConnection.Open();
                        using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                        {
                            try
                            {
                                s.DestinationTableName = "watchlist";
                                foreach (var column in csvFileData.Columns)
                                    s.ColumnMappings.Add(column.ToString(), column.ToString());
                                s.WriteToServer(csvFileData);
                            }
                            catch (Exception ex)
                            { Console.WriteLine("error", ex.Message); }

                        }
                        var updateCommandText = @"update WatchList set [Market Cap]= replace([Market Cap], '|', ','), [Shares] = REPLACE(Shares,'|',','),
                                          [Volume] = REPLACE(Volume,'|',','), [Description] = REPLACE(Description,'|',','),[Open.Int] = REPLACE([Open.Int],'|',','),
                                          [Sector] = REPLACE(Sector,'|',','), [Industry] = REPLACE(Industry,'|',','), [Sub-Industry] = REPLACE([Sub-Industry],'|',','); 
                                           update WATCHLIST set [%CHANGE] =1 WHERE [%CHANGE] LIKE '<empty>%' AND EX in ('Economic Indicators','Economic IndicatorsCOPY' , 'Interest Rates');
                                            UPDATE WATCHLIST  SET [MarketCapNum] = [market Cap];                                           
                                            DELETE FROM WATCHLIST WHERE [%CHANGE] LIKE '<empty>%'";
                        
                        SqlCommand updateData = new SqlCommand(updateCommandText, dbConnection);
                        updateData.ExecuteNonQuery();

                        var updateMarketCap = @"
                                           UPDATE WATCHLIST SET [MarketCapNum] = REPLACE([MarketCapNum],'M','');
                                           UPDATE WATCHLIST SET [MarketCapNum] = REPLACE([MarketCapNum],',','');
                                           UPDATE WATCHLIST set [MarketCapNum] = cast([MarketCapNum] as bigint)* 1000000;  ";
                        SqlCommand updateMarketCapData = new SqlCommand(updateMarketCap, dbConnection);
                        updateMarketCapData.ExecuteNonQuery();
                    }
                    Logging.Logger("Successfully updated the database on "+ DateTime.Now);
                }
                catch (Exception ex)
                {
                    Logging.Logger("SQL exception with file " + filename + " ; " + ex.Message);
                }

        }

        public static void SaveResultToFile(string ConnectionString, string OutputPath, string query)
        {
           

            using (var sqlConnection = new SqlConnection(ConnectionString))
            using (var sqlCommand = new SqlCommand(query, sqlConnection))
            {
                try
                {
                    sqlConnection.Open();
                    var sqlDataReader = sqlCommand.ExecuteReader();


                    string Delimiter = "|";
                    string Separator = ",";
                    string fileName = OutputPath;
                    StreamWriter writer = new StreamWriter(fileName);

                    // write header number row - start
                    for (int columnCounter = 0; columnCounter < sqlDataReader.FieldCount; columnCounter++)
                    {
                        //add numbers to columns
                        writer.Write((columnCounter + 1) + Delimiter);

                    }

                    writer.WriteLine(string.Empty);
                    //write header number row - end

                    //loop to add column header description
                    for (int columnCounter = 0; columnCounter < sqlDataReader.FieldCount; columnCounter++)
                    {

                        writer.Write(sqlDataReader.GetName(columnCounter) + Delimiter);
                    }

                    writer.WriteLine(string.Empty);

                    // data loop
                    while (sqlDataReader.Read())
                    {
                        // column loop
                        for (int columnCounter = 0; columnCounter < sqlDataReader.FieldCount; columnCounter++)
                        {

                            writer.Write(
                                sqlDataReader.GetValue(columnCounter).ToString().Replace('"', '\'') + Delimiter);
                        } // end of column loop

                        writer.WriteLine(string.Empty);
                    } // data loop
                    writer.Close();
                    writer.Flush();
                    
                }
                catch (Exception ex)
                {
                    Logging.Logger("Exception writing to the file : "+ ex.Message );
                }

            }

        }

        public static void InsertCADashboardDataIntoSQL(DataTable csvFileData, string DataConnection, string filename)
        {
            // change the data source value as needed LN443397
            using (SqlConnection dbConnection =
                   new SqlConnection(DataConnection))
                // using (SqlConnection dbConnection =
                // new SqlConnection("Data Source=RAMESHI7;Initial Catalog=RAMTEST;Integrated Security=SSPI;"))
                //RAMESHhrd2in1
                //rameshdi5
                try
                {
                    {
                        dbConnection.Open();
                        using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                        {
                            try
                            {
                                s.DestinationTableName = "CADbwatchlist";
                                foreach (var column in csvFileData.Columns)
                                    s.ColumnMappings.Add(column.ToString(), column.ToString());
                                s.WriteToServer(csvFileData);
                            }
                            catch (Exception ex)
                            { Console.WriteLine("error", ex.Message); }

                        }
                        var deleteUnlistedCompanies = @" DELETE FROM CADBWATCHLIST WHERE SYMBOL = '-' ";
                        SqlCommand updateMarketCapData = new SqlCommand(deleteUnlistedCompanies, dbConnection);
                        updateMarketCapData.ExecuteNonQuery();

                    }
                    
                    Logging.Logger("Successfully updated the database on " + DateTime.Now);
                }
                catch (Exception ex)
                {
                    Logging.Logger("SQL exception with file " + filename + " ; " + ex.Message);
                }

        }

        //CA Web data
        public static void InsertCAWebDataIntoSQLServerUsingSQLBulkCopy(DataTable csvFileData, string DataConnection)
        {
            // change the data source value as needed LN443397
            using (SqlConnection dbConnection =
                   new SqlConnection(DataConnection))
                // using (SqlConnection dbConnection =
                // new SqlConnection("Data Source=RAMESHI7;Initial Catalog=RAMTEST;Integrated Security=SSPI;"))
                //RAMESHhrd2in1
                //rameshdi5
                try
                {
                    {
                        dbConnection.Open();
                        using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                        {
                            try
                            {
                                s.DestinationTableName = "CanadaData";
                                foreach (var column in csvFileData.Columns)
                                    s.ColumnMappings.Add(column.ToString(), column.ToString());
                                s.WriteToServer(csvFileData);
                            }
                            catch (Exception ex)
                            { Console.WriteLine("error", ex.Message); }

                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex.Message);
                }

        }
    }
}
