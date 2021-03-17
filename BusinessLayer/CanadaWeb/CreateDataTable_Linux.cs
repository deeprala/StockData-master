using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace BusinessLayer.CanadaWeb
{
    public static class CreateDataTable_Linux
    {
        public static DataTable CreateAndFormatDt(string path)
        {
            //Create datatable to monipulate data
            DataTable csvData = new DataTable();
            DataTable csvTblDesc = new DataTable();
            List<string> fieldData;

            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;


                string[] colFields;
                colFields = csvParser.ReadFields();
                foreach (string column in colFields)
                {
                    DataColumn datacolumn = new DataColumn(column);
                    datacolumn.AllowDBNull = true;

                    csvData.Columns.Add(datacolumn);
                    csvTblDesc.Columns.Add("temp" + datacolumn);
                }


                csvData.Columns.Add("COUNTRY");
                csvData.Columns.Add("SYMBOL");
                csvData.Columns.Add("DESCRIPTION");
                // csvData.Columns.Add("SerialNo");

                while (!csvParser.EndOfData)
                {
                    string[] arrayList;
                    fieldData = csvParser.ReadFields().ToList();
                    var count = fieldData.Count;

                    //Check for Bear and Bull Category - CAT column
                    if (!(fieldData[8].Equals("BE") || fieldData[8].Equals("BU")))
                    {
                        //country
                        if (fieldData[4].StartsWith("Canada"))
                        {
                            //Convert date string to Date format
                            fieldData[2] = (DateTime.TryParseExact(fieldData[2], "dddd, MMMM d, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date.ToShortDateString() : (DateTime.TryParseExact(fieldData[2], "ddd, MMMM d, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date2) ? date2.ToShortDateString() : fieldData[2]));

                            fieldData.Add(fieldData[4].Substring(0, 6));
                            //symbol length
                            var symbol = Regex.Match(fieldData[4].Substring(6), @"[ A-Z ]+").Value;
                            if (symbol.Length > 0)
                            {
                                //symbol length
                                fieldData.Add(symbol.Substring(0, symbol.Length - 1));
                                //Desc
                                var descIndex = 6 + symbol.Length - 1;
                                fieldData.Add(fieldData[4].Substring(descIndex));
                            }
                            else
                            {
                                fieldData.Add("ref desc");
                            }

                        }
                        else if (fieldData[4].StartsWith("United States"))
                        {
                            //Convert date string to Date format
                            fieldData[2] = DateTime.TryParseExact(fieldData[2], "dddd, MMMM d, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date.ToShortDateString() : (DateTime.TryParseExact(fieldData[2], "ddd, MMMM d, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date2) ? date2.ToShortDateString() : fieldData[2]);

                            //United States
                            fieldData.Add(fieldData[4].Substring(0, 13));
                            //symbol length
                            var symbol = Regex.Match(fieldData[4].Substring(13), @"[ A-Z ]+").Value;
                            if (symbol.Length > 0)
                            {
                                //symbol length
                                fieldData.Add(symbol.Substring(0, symbol.Length - 1));
                                //Desc
                                var descIndex = 13 + symbol.Length - 1;
                                fieldData.Add(fieldData[4].Substring(descIndex));
                            }
                            else
                            {
                                fieldData.Add("ref desc");
                            }


                        }
                    }
                    else
                    {
                        //country
                        if (fieldData[4].StartsWith("Canada"))
                        {
                            //Convert date string to Date format
                            fieldData[2] = DateTime.TryParseExact(fieldData[2], "dddd, MMMM d, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date.ToShortDateString() : (DateTime.TryParseExact(fieldData[2], "ddd, MMMM d, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date2) ? date2.ToShortDateString() : fieldData[2]);
                            //Country
                            fieldData.Add(fieldData[4].Substring(0, 6));
                            //Symbol
                            fieldData.Add("");
                            //Desc
                            fieldData.Add(fieldData[4].Substring(6));
                        }
                        else if (fieldData[4].StartsWith("United States"))
                        {
                            //Convert date string to Date format
                            fieldData[2] = DateTime.TryParseExact(fieldData[2], "dddd, MMMM d, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date.ToShortDateString() : (DateTime.TryParseExact(fieldData[2], "ddd, MMMM d, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date2) ? date2.ToShortDateString() : fieldData[2]);
                            //United States
                            fieldData.Add(fieldData[4].Substring(0, 13));
                            //Symbol
                            fieldData.Add("");
                            //Desc
                            fieldData.Add(fieldData[4].Substring(13));
                        }
                    }
                    arrayList = fieldData.ToArray();
                    csvData.Rows.Add(arrayList);
                }
                csvData.AcceptChanges();

                //update symbols which are null by reading the description which has symbols associated wit it
                //var emptySymbols = csvData.AsEnumerable()
                //                   .Where(x => x.Field<string>("Symbol") == null); 

                // Rearrange Column Order
                //DataTableExtensions.SetColumnsOrder(csvData, "ALL", "DAY", "date", "Country", "EX",  "Symbol", "LP/MA/LC", "% -CHG", "REPORT", "CAT", "CLOSE", "%", "TD", "I", "Description", "COM");
                DataTableExtensions.SetColumnsOrder(csvData, "ALL", "DAY", "DATE", "COUNTRY", "EX", "DESCRIPTION", "COM", "SYMBOL", "LP/MA/LC", "% -CHG", "REPORT", "CAT", "CLOSE", "%", "TD", "I");
            }

            return csvData;
        }
    }
}
