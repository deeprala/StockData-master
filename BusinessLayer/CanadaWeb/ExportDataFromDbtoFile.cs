using System;
using System.Data.SqlClient;
using System.IO;

namespace BusinessLayer.CanadaWeb
{
    public static class ExportDataFromDbtoFile
    {
        private const int fieldCount = 56;

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
                    // writer.Flush();

                }
                catch (Exception ex)
                {

                }

            }

        }

        //Create CA template from CA web data
        public static void CreateCATemplateFromDb(string ConnectionString, string OutputPath, string query)
        {
            CreateTextTemplate(ConnectionString, OutputPath, query);

            using (var sqlConnection = new SqlConnection(ConnectionString))
            using (var sqlCommand = new SqlCommand(query, sqlConnection))
            {
                try
                {
                    sqlConnection.Open();
                    var sqlDataReader = sqlCommand.ExecuteReader();


                    string Delimiter = ",";
                    string fileName = OutputPath;
                    
                    using (StreamWriter writer = new StreamWriter(fileName))
                    {

                        //Add text
                        writer.WriteLine("As of Date" + Delimiter + DateTime.Now);
                        writer.WriteLine(string.Empty);
                        writer.WriteLine("Watchlist" + Delimiter + "List 01");
                        writer.WriteLine(string.Empty);
                        writer.WriteLine("Stocks and ETFs");

                        var rowHeader = "Ticker" + Delimiter + "C" + Delimiter + "Name" + Delimiter + "Symbol" + Delimiter + "EPS" + Delimiter +
                            "PE" + Delimiter + "Yield" + Delimiter + "% Chg" + Delimiter + "Change" + Delimiter + "Price" + Delimiter +
                            "Ask" + Delimiter + "Bid" + Delimiter + "Trd Sz" + Delimiter + "Ask Sz" + Delimiter + "Bid Sz" + Delimiter +
                            "Close" + Delimiter + "52 Week Range" + Delimiter + "52 High" + Delimiter + "High" + Delimiter + "Low" + Delimiter +
                            "52 Low" + Delimiter + "Open" + Delimiter + "Call/Put" + Delimiter + "Imp Vol" + Delimiter + "Mark" + Delimiter +
                            "Exch" + Delimiter + "Mkt Val" + Delimiter + "Market Cap" + Delimiter + "Shr Os" + Delimiter + "Volume" + Delimiter +
                            "AVG Volume 6 Month" + Delimiter + "Blk Vol" + Delimiter + "Opn Int" + Delimiter + "Day % P/L" + Delimiter +
                            "Day P/L" + Delimiter + "% P/L" + Delimiter + "P/L" + Delimiter + "Book Cost" + Delimiter + "Avg Cost" + Delimiter +
                            "Entry Date" + Delimiter + "Quantity" + Delimiter + "Trade" + Delimiter + "Trend" + Delimiter +
                            "Opn Int Chg" + Delimiter + "Curr" + Delimiter + "Q" + Delimiter +
                            "Div" + Delimiter + "Div Pay Dt" + Delimiter + "Ex-Div" + Delimiter + "Exp Dt" + Delimiter +
                            "Num Trd" + Delimiter + "Prc Tck" + Delimiter + "Quote Trend" + Delimiter + "Range" + Delimiter +
                            "Rel Range" + Delimiter + "Strike" + Delimiter;
                        //write header number row - end
                        writer.WriteLine(rowHeader);


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
                        }
                        // data loop
                        writer.Close();
                    }


                }
                catch (Exception ex)
                {

                }

            }

        }

        private static void CreateTextTemplate(string ConnectionString, string OutputPath, string query)
        {
            string textFileName = OutputPath.Replace(".csv", ".txt");
            using (var sqlConnection = new SqlConnection(ConnectionString))
            using (var sqlCommand = new SqlCommand(query, sqlConnection))
            {
                try
                {
                    sqlConnection.Open();
                    var sqlDataReader = sqlCommand.ExecuteReader();
                    // Write only the first column to text file
                    using (StreamWriter txtwriter = new StreamWriter(textFileName))
                    {
                        // data loop
                        while (sqlDataReader.Read())
                        {
                            if (textFileName.Contains("US"))
                            {
                                var formatValue = sqlDataReader.GetValue(0).ToString().Contains(".") ? sqlDataReader.GetValue(0).ToString().Replace(".", "/") : sqlDataReader.GetValue(0).ToString();
                                txtwriter.Write(formatValue);
                            }
                            // check if the textfilename contains US/USA; if yes replace . with /
                            else
                            {
                                txtwriter.Write(
                                    sqlDataReader.GetValue(0).ToString());
                            }

                            txtwriter.WriteLine(string.Empty);
                        }
                        // data loop
                        txtwriter.Close();
                    }
                }
                catch (Exception ex)
                { }
            }
        }
    }
}
