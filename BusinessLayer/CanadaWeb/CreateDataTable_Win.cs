using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BusinessLayer.CanadaWeb
{
    public static class CreateDataTable_Win
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
                csvParser.SetDelimiters(new string[] { "|" });
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

                    if (!(fieldData[4].ToLower().Contains("data delayed") || fieldData[4].ToLower().Trim().Equals("company") ||
                        fieldData[5].ToLower().Contains("Price") || fieldData[5].ToLower().Trim().Equals("Volume") ||
                        fieldData[5].ToLower().Trim().Equals("Market Cap") || fieldData[4].ToString().Length == 0 ||
                        fieldData[4].ToString().Contains("2020")))
                    {
                        if (fieldData[5].ToString().Length > 0)
                        {
                            //country
                            if (fieldData[4].StartsWith("Canada"))
                            {
                                fieldData.Add(fieldData[4].Substring(0, 6));
                                //symbol
                                var symbolLength = fieldData[4].Substring(6).Length;
                                if (symbolLength > 0)
                                { fieldData.Add(fieldData[4].Substring(6)); }
                                else
                                {
                                    fieldData.Add("ref desc");
                                }

                            }
                            else if (fieldData[4].StartsWith("United"))
                            {
                                //United States
                                fieldData.Add(fieldData[4].Substring(0, 13));
                                //symbol
                                var symbolLength = fieldData[4].Substring(13).Length;
                                if (symbolLength > 0)
                                { fieldData.Add(fieldData[4].Substring(13)); }
                                else
                                {
                                    fieldData.Add("ref desc");
                                }

                            }
                            else
                            {

                                if (fieldData[3].Contains("TSX"))
                                {
                                    fieldData.Add("Canada");
                                    fieldData.Add(fieldData[4].Substring(0));

                                }
                                else if (fieldData[3].Contains("NYSE") || fieldData[3].Contains("NASDAQ"))
                                {
                                    fieldData.Add("United States");
                                    fieldData.Add(fieldData[4].Substring(0));
                                }
                                else
                                {
                                    //For all other
                                    fieldData.Add("");
                                    fieldData.Add(fieldData[4].Substring(0));
                                }

                            }


                            arrayList = fieldData.ToArray();
                            csvData.Rows.Add(arrayList);
                        }
                        else
                        {

                            csvTblDesc.Rows.Add(fieldData.ToArray());
                        }
                    }
                    else
                    { continue; }

                }

                //Update Desc value from csvTblDesc to csvData
                for (int i = 0; i < csvTblDesc.Rows.Count; i++)
                {
                    var count = i + 1;
                    csvData.Rows[i]["Description"] = csvTblDesc.Rows[i]["tempCOM"];
                    // csvData.Rows[i]["SerialNo"] = count;
                }

                csvData.AcceptChanges();

                // Rearrange Column Order
                DataTableExtensions.SetColumnsOrder(csvData, "ALL", "DAY", "DATE", "COUNTRY", "EX", "DESCRIPTION", "COM", "SYMBOL", "LP/MA/LC", "% -CHG", "REPORT", "CAT", "CLOSE", "%", "TD", "I");
            }

            return csvData;
        }
    }
}
