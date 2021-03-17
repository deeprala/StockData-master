using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Logger;

namespace BusinessLayer
{
    public class ProcessStockData
    {

        //Read CSV file and create a datatable to mimic db table.
        public static DataTable GetDataTableFromCSVFile(string csv_file_path, string fileDate)
        {
            DataTable csvData = new DataTable();
            List<string> fieldData;
            DateTime date = Convert.ToDateTime(fileDate);
            var dayofweek = date.DayOfWeek;


            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    string[] colFields;
                    csvReader.SetDelimiters(new string[] { "|" });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    var line1 = csvReader.ReadLine();
                    var line2 = csvReader.ReadLine();
                    var line3 = csvReader.ReadLine().Trim('|');
                    colFields = csvReader.ReadFields();

                    foreach (string column in colFields)
                    {
                        DataColumn datacolumn = new DataColumn(column);
                        datacolumn.AllowDBNull = true;
                        csvData.Columns.Add(datacolumn);
                    }
                    if (line3 != null)
                    {
                        string ex = "EX";
                        DataColumn datacolumn = new DataColumn(ex);
                        datacolumn.AllowDBNull = true;
                        csvData.Columns.Add(datacolumn);
                        csvData.Columns.Add("Date");
                        csvData.Columns.Add("DayOfWeek");
                    }

                    while (!csvReader.EndOfData)
                    {
                        try
                        {
                            fieldData = csvReader.ReadFields().ToList();

                            //Making empty value as null
                            for (int i = 0; i < fieldData.Count; i++)
                            {
                                if (fieldData[i] == "")
                                {
                                    fieldData[i] = null;
                                }
                            }

                            fieldData.Add(line3);
                            fieldData.Add(date.ToString());
                            fieldData.Add(dayofweek.ToString());
                            string[] arrayList = fieldData.ToArray();
                            csvData.Rows.Add(arrayList);
                        }

                        catch(Exception ex)
                        {
                            Console.WriteLine("error: ", ex.Message);
                        }

                    }
                }

                return csvData;
            }
            catch (Exception ex)
            {
                Logging.Logger("File exception: " + csv_file_path + " ;" + ex.Message);
                return null;
            }


        }

        public static DataTable GetCADashboardDataTableFromCSVFile (string csv_file_path, string fileDate)
        {
            DataTable csvData = new DataTable();
            List<string> fieldData;
             DateTime date = Convert.ToDateTime(fileDate);
            var dayofweek = date.DayOfWeek;
            string EX = "";

            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    string[] colFields;
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    //var line1 = csvReader.ReadLine();
                    //var line2 = csvReader.ReadLine();
                    //var line3 = csvReader.ReadLine().Trim('|');
                    var fileName = Path.GetFileName(csv_file_path)?.Substring(0,1).ToUpper();
                    switch(fileName)
                    {
                        case "A":
                        case "B":
                            EX = "CAADRTSX-412";
                        break;
                        case "C":
                            EX = "CAADRUS-177";
                            break;

                        case "D":
                            EX = "CAADR-US-176";
                            break;
                        case "E":
                            EX = "TSXCOMP-229";
                            break;
                        case "F":
                            EX = "TSXSP-234";
                            break;
                        case "G":
                        case "H":
                        case "I":
                        case "J":
                        case "K":
                            EX = "TSX-860";
                            break;
                        case "L":
                        case "M":
                        case "N":
                        case "O":
                        case "P":
                        case "Q":
                        case "R":
                        case "S":
                        case "T":
                            EX = "TSXV-1654";
                            break;
                        case "U":
                            EX = "CA-REITS";
                            break;
                        case "V":
                            EX = "CANNABIS";
                            break;
                        case "W":
                            EX = "CADRIPS-132";
                            break;
                        case "X":
                            EX = "FINANCE92";
                            break;
                        case "Y":
                            EX = "TSX-TECH-52";
                            break;
                        case "Z":
                            EX = GetExValueForZ(csv_file_path);
                            break;

                    }
                    colFields = csvReader.ReadFields();

                    foreach (string column in colFields)
                    {
                        DataColumn datacolumn = new DataColumn(column);
                        datacolumn.AllowDBNull = true;
                        csvData.Columns.Add(datacolumn);
                    }
                    if (EX != null)
                    {
                        string ex = "EX";
                        DataColumn datacolumn = new DataColumn(ex);
                        datacolumn.AllowDBNull = true;
                        csvData.Columns.Add(datacolumn);
                        csvData.Columns.Add("Date");
                        csvData.Columns.Add("DayOfWeek");
                    }

                    while (!csvReader.EndOfData)
                    {
                        try
                        {
                            fieldData = csvReader.ReadFields().ToList();

                            //Making empty value as null
                            for (int i = 0; i < fieldData.Count; i++)
                            {
                                if (fieldData[i] == "")
                                {
                                    fieldData[i] = null;
                                }
                            }

                            fieldData.Add(EX);
                            fieldData.Add(fileDate.ToString());
                            fieldData.Add(dayofweek.ToString());
                            string[] arrayList = fieldData.ToArray();
                            csvData.Rows.Add(arrayList);
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine("error: ", ex.Message);
                        }

                    }
                }

                return csvData;
            }
            catch (Exception ex)
            {
                Logging.Logger("File exception: " + csv_file_path + " ;" + ex.Message);
                return null;
            }


        }

        private static string GetExValueForZ(string csv_file_path)
        {
            string exchangeCode;
            var fileName = Path.GetFileName(csv_file_path)?.Substring(0, 3).ToUpper();
            switch(fileName)
            {
                case "Z26":
                    exchangeCode = "TSXV-TECH-127";
                    return exchangeCode;
                case "Z27":
                    exchangeCode = "TSX-REALESTATE-64";
                    return exchangeCode; ;
                case "Z28":
                    exchangeCode = "TSXV-REALESTATE-30";
                    return exchangeCode; ;
                case "Z29":
                    exchangeCode = "TSX-OG-80";
                    return exchangeCode; ;
                case "Z30":
                    exchangeCode = "TSXV-OG-139";
                    return exchangeCode; ;
                case "Z31":
                    exchangeCode = "TSX-MINING-228";
                    return exchangeCode; ;
                case "Z32":
                case "Z33":
                case "Z34":
                case "Z35":
                    exchangeCode = "TSXV-MINING-990";
                    return exchangeCode; ;
                case "Z36":
                case "Z37":
                    exchangeCode = "TSX-DIVERSI-IND-400";
                    return exchangeCode; ;
                case "Z38":
                    exchangeCode = "TSXV-DIVERSI-IND-198";
                    return exchangeCode; ;
                case "Z39":
                    exchangeCode = "TSX-ENEGY-SRVS-42";
                    return exchangeCode; ;
                case "Z40":
                    exchangeCode = "TSXV-ENEGY-SRVS-25";
                    return exchangeCode; ;
                case "Z41":
                    exchangeCode = "TSX-CLEAN-TECH-36";
                    return exchangeCode; ;
                case "Z42":
                    exchangeCode = "TSXV-CLEAN-TECH-56";
                    return exchangeCode; ;
                case "Z43":
                case "Z44":
                    exchangeCode = "CSE338";
                    return exchangeCode;
                case "Z45":
                    exchangeCode = "1TSX";
                    return exchangeCode;
                case "Z46":
                    exchangeCode = "1VENT";
                    return exchangeCode;
                case "Z47":
                    exchangeCode = "1NYSEC";
                    return exchangeCode;
                case "Z48":
                    exchangeCode = "1NASDAQ";
                    return exchangeCode;
                case "Z49":
                    exchangeCode = "1NYSEM";
                    return exchangeCode;
                case "Z50":
                    exchangeCode = "1CAHL";
                    return exchangeCode;
                case "Z51":
                    exchangeCode = "1USHL";
                    return exchangeCode;
                //52-58 2TU RAMESH TOGGLE BETWEEN MONDAY 1 TO FRIDAY 5 1.2.3.4.5
                case "Z52":
                    exchangeCode = "2TSX";
                    return exchangeCode;
                case "Z53":
                    exchangeCode = "2VENT";
                    return exchangeCode;
                case "Z54":
                    exchangeCode = "2NYSEC";
                    return exchangeCode;
                case "Z55":
                    exchangeCode = "2NASDAQ";
                    return exchangeCode;
                case "Z56":
                    exchangeCode = "2NYSEM";
                    return exchangeCode;
                case "Z57":
                    exchangeCode = "2CAHL";
                    return exchangeCode;
                case "Z58":
                    exchangeCode = "2USHL";
                    return exchangeCode;

                //59-65 3W RAMESH TOGGLE BETWEEN MONDAY 1 TO FRIDAY 5 1.2.3.4.5
                case "Z59":
                    exchangeCode = "3TSX";
                    return exchangeCode;
                case "Z60":
                    exchangeCode = "3VENT";
                    return exchangeCode;
                case "Z61":
                    exchangeCode = "3NYSEC";
                    return exchangeCode;
                case "Z62":
                    exchangeCode = "3NASDAQ";
                    return exchangeCode;
                case "Z63":
                    exchangeCode = "3NYSEM";
                    return exchangeCode;
                case "Z64":
                    exchangeCode = "3CAHL";
                    return exchangeCode;
                case "Z65":
                    exchangeCode = "3USHL";
                    return exchangeCode;

                //66-72 4TH RAMESH TOGGLE BETWEEN MONDAY 1 TO FRIDAY 5 1.2.3.4.5
                case "Z66":
                    exchangeCode = "4TSX";
                    return exchangeCode;
                case "Z67":
                    exchangeCode = "4VENT";
                    return exchangeCode;
                case "Z68":
                    exchangeCode = "4NYSEC";
                    return exchangeCode;
                case "Z69":
                    exchangeCode = "4NASDAQ";
                    return exchangeCode;
                case "Z70":
                    exchangeCode = "4NYSEM";
                    return exchangeCode;
                case "Z71":
                    exchangeCode = "4CAHL";
                    return exchangeCode;
                case "Z72":
                    exchangeCode = "4USHL";
                    return exchangeCode;

                //73-79 5F RAMESH TOGGLE BETWEEN MONDAY 1 TO FRIDAY 5 1.2.3.4.5 TRY
                case "Z73":
                    exchangeCode = "5TSX";
                    return exchangeCode;
                case "Z74":
                    exchangeCode = "5VENT";
                    return exchangeCode;
                case "Z75":
                    exchangeCode = "5NYSEC";
                    return exchangeCode;
                case "Z76":
                    exchangeCode = "5NASDAQ";
                    return exchangeCode;
                case "Z77":
                    exchangeCode = "5NYSEM";
                    return exchangeCode;
                case "Z78":
                    exchangeCode = "5CAHL";
                    return exchangeCode;
                case "Z79":
                    exchangeCode = "5USHL";
                    return exchangeCode;
                //73-79 5F RAMESH TOGGLE BETWEEN MONDAY 1 TO FRIDAY 5 1.2.3.4.5 TRY

                ///////////// CA-CU MORE SORTEDSYMBOL

                case "Z80":
                    exchangeCode = "5D-ALL-HL";
                    return exchangeCode;
                case "Z81":
                    exchangeCode = "CA1-710";
                    return exchangeCode;
                case "Z82":
                    exchangeCode = "CA2-710-1420-";
                    return exchangeCode;

                case "Z83":
                    exchangeCode = "CA3-1420-19600";
                    return exchangeCode;
                case "Z84":
                    exchangeCode = "US1-710";
                    return exchangeCode;
                case "Z85":
                    exchangeCode = "US2-710-1420";
                    return exchangeCode;
                case "Z86":
                    exchangeCode = "US3-1420-2100";
                    return exchangeCode;
                case "Z87":
                    exchangeCode = "EV";
                    return exchangeCode;
                case "Z88":
                    exchangeCode = "Z-88-TILLDATECA";
                    return exchangeCode;
                // case "Z89":
                //     exchangeCode = "Z-89-NHUS";
                //    return exchangeCode;
                //case "Z90":
                //     exchangeCode = "Z-90-NLUS";
                //     return exchangeCode;

                case "Z89":
                    exchangeCode = "Z-89-5DCAHL";
                    return exchangeCode;
                case "Z90":
                    exchangeCode = "Z-90-5DUSHL";
                    return exchangeCode;
                case "Z91":
                    exchangeCode = "Z-91-1DCA";
                    return exchangeCode;
                case "Z92":
                    exchangeCode = "Z-92-1DUS";
                    return exchangeCode;

                case "Z93":
                    exchangeCode = "Z-93-5DCAALL";
                    return exchangeCode;
                case "Z94":
                    exchangeCode = "Z-94-5DUSALL";
                    return exchangeCode;
                case "Z95":
                    exchangeCode = "Z-95-5DUSALL";
                    return exchangeCode;
                case "Z96":
                    exchangeCode = "Z-96-NHUS";
                    return exchangeCode;
                case "Z97":
                    exchangeCode = "Z-97-NLUS";
                    return exchangeCode;
                    // Z-98-99 FOR SHORT, SHORT1
                case "Z100":
                    exchangeCode = "Z-100-5DUSALL";
                    return exchangeCode;
                case "Z600":
                    exchangeCode = "Z-92-1DUS";
                    return exchangeCode;
                default:
                    return exchangeCode = "TSXT";

            }
            
        }
    }
}
