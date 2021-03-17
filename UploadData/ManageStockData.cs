using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SearchOption = System.IO.SearchOption;
using DAL;
using BusinessLayer;
using BusinessLayer.CanadaWeb;
using System.Collections.Generic;

namespace UploadData
{
    public partial class ManageStockData : Form
    {
        public ManageStockData()
        {
            InitializeComponent();
            success.Visible = false;
        }
        

        public static string strDataserver;
        public static string strDatabase;
        public static string dataConnectionString;
        public static string fileDate;
        public static DataTable csvDataWin;


        private void btnUploadData_Click(object sender, EventArgs e)
        {

            success.Visible = false;
            Cursor = Cursors.WaitCursor;
            bool isFolderDate = false;

            dataConnectionString = "Data Source=" + strDataserver + ";Initial Catalog=" + strDatabase + "; Integrated Security=SSPI;";
            var fileDir = new DirectoryInfo(folderPath.Text);
            DirectoryInfo[] diArr = fileDir.GetDirectories();
            for (int i = 0; i < diArr.Length; i++)
            {
                isFolderDate = false;
                DirectoryInfo dri = diArr[i];
                Console.WriteLine(dri.Name);
                var date = dri.Name.Substring(0, 10);
                DateTime myDate;
                if (DateTime.TryParse(date, out myDate))
                {
                    fileDate = date;
                    isFolderDate = true;
                }
                


                try
                {
                    foreach (var file in diArr[i].EnumerateFiles("*.csv", SearchOption.TopDirectoryOnly))
                    {
                        string stockFile = file.FullName.ToLower();
                        if (!isFolderDate)
                        {
                            fileDate = Path.GetFileName(stockFile).Substring(0, 10);
                        }
                        DataTable csvData = ProcessStockData.GetDataTableFromCSVFile(stockFile, fileDate);
                        DatabaseLayer.InsertDataIntoSQLServerUsingSQLBulkCopy(csvData, dataConnectionString, stockFile);
                    }
                    var outputPath = textOutputPath.Text.ToLower() + @"\AllDataRun_" + fileDate + @".csv";
                    var query = @"SELECT [Description],[EX],[Date],[DayOfWeek],[MarketCapNum],[Volume],[Symbol],[%Change],[Last],[EPS],[PE],[Market Cap],
                                [Shares],[Net Chng],[52Low],[Low],[Last],[Close],[Open],[High],[52High],[Bid],[Ask] ,[RSI],[Div. Payout Per Share (% of EPS) - Current],[Open.Int],
                                [Beta],[Delta],[Gamma],[Theta],[Vega],[Rho],[Div],[Div.Freq],[Div. Payout (% of Earnings) - Current (Annual)],[Div. Per Share - Current],
                                [Div. Per Share - TTM - Current (Annual)],[Div. Yield - Current],[Earnings Per Share - Current],[Earnings Per Share - TTM - Current (Annual)],
                                [Sector],[Industry],[Sub-Industry],[Cash Flow Per Share - Current (Annual)],
                                [Free Cash Flow Per Share - Current (Annual)],[Price / Cash Flow Ratio - Current],
                                [Book Value Per Share - Current (Annual)],[Book Value Per Share Growth - Current],[Price / Book Value Ratio - Current],
                                [Price / Earnings Ratio - Current], [Return on Assets (ROA) - Current (Annual)], [ROC], [Return on Equity (ROE) - Current (Annual)], [ROR], 
                                [Revenue - Current], [MoneyFlow], [MoneyFlowIndex], [MoneyFlowIndexCrossover], [MoneyFlowOscillator], [Back Vol], [DailySMA], 
                                [Financial Leverage (Assets/Equity) - Current (Annual)],[Front Vol], [Market Cap / Common Equity Ratio - Current], [Market Maker Move], [Momentum], [MomentumCrossover], [MomentumPercent], [MomentumPercentDiff],
                                [MomentumSMA],[Sizzle Index],[Spreads],[Weighted Back Vol]
                                 FROM [dbo].[WatchList]";
                    DatabaseLayer.SaveResultToFile(dataConnectionString, outputPath, query);
                    SaveSelectedData();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error has occured", ex.StackTrace);
                }
            }
            Cursor = Cursors.Default;
            var t = Task.Delay(2000);//1 second/1000 ms
            t.Wait();
            success.Visible = true;

        }
    
        private void BtnCalc_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");       
        }

        private void FolderPath_Click(object sender, EventArgs e)
        {
            SelectInputFolderPath();
        }

        private void FolderPath_Enter(object sender, EventArgs e)
        {
            SelectInputFolderPath();
        }

        private void SelectInputFolderPath()
        {
            FolderBrowserDialog dialog;
            dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                folderPath.Text = dialog.SelectedPath;

            }
        }

        private void TextOutputPath_Click(object sender, EventArgs e)
        {
            SelectOutputFolderPath();
        }

        private void TextOutputPath_Enter(object sender, EventArgs e)
        {
            SelectOutputFolderPath();
        }

        private void SelectOutputFolderPath()
        {
            FolderBrowserDialog dialog;
            dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textOutputPath.Text = dialog.SelectedPath;

            }
        }

        private void btnBreakEven_Click(object sender, EventArgs e)
        {
            BreakEven be = new BreakEven();

            dataConnectionString = "Data Source=" + strDataserver + ";Initial Catalog=" + strDatabase + "; Integrated Security=SSPI;";
            be.ShowDialog();
        }

        private void btnBackUpDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                var DBBackUpPath = txtPathtoBackUpDB.Text;
                dataConnectionString = "Data Source=" + strDataserver + ";Initial Catalog=" + strDatabase + "; Integrated Security=SSPI;";
                BackupService bkBackupService = new BackupService(dataConnectionString, DBBackUpPath);
                bkBackupService.BackupAllUserDatabases();

            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to backup database: " + exception.Message);
            }
           
        }

        private void DBBackUpPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog;
            dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtPathtoBackUpDB.Text = dialog.SelectedPath;

            }
        }

        private void btnRestoreDB_Click(object sender, EventArgs e)
        {
            try
            {
                var targetDirectory = txtRestoreFilePath.Text; //@"C:\Latha\Personal\UploadData\UploadData";
                var restorePath = txtRestoreDBPath.Text; //@"C:\Latha\Personal\UploadData\UploadData\DBRestore\";
                string[] fileEntries = Directory.GetFiles(targetDirectory, "*.bak");
                strDataserver = textDataServer.Text.ToLower();
                foreach (var fe in fileEntries)
                {
                    var filename = Path.GetFileName(fe);
                    restorePath += filename;
                    string dbname = null;
                    int index = filename.IndexOf('-');
                    if (index > 0)
                    {
                        dbname = filename.Substring(0, index) + "_B";
                    }

                    RestoreDataBase restoreDataBase = new RestoreDataBase();
                    restoreDataBase.RestoreDatabase(dbname, fe, strDataserver, restorePath);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Failed to restore database : " + ex.Message);
            }
            


        }

        private void btnlnkWeb_Click(object sender, EventArgs e)
        {
            Links weblinks = new Links();
            weblinks.ShowDialog();
        }

        private void CAData_Click(object sender, EventArgs e)
        {
            // Just a place holder 
            var folderInputPath = new DirectoryInfo(folderPath.Text);
            //DirectoryInfo(@"C:\Latha\Personal\FormatExcelData\");
            //@"C:\CSV\Canada\In\
            var folderOutputPath = textOutputPath.Text.ToLower();
            dataConnectionString = "Data Source=" + strDataserver + ";Initial Catalog=" + strDatabase + "; Integrated Security=SSPI;";
            //dataConnectionString = ConfigurationManager.ConnectionStrings["RamTest"].ConnectionString;
            var builder = new SqlConnectionStringBuilder(dataConnectionString);
            var databaseName = builder.InitialCatalog;
            //"Data Source=LN452321 ;Initial Catalog=RamTest; Integrated Security=SSPI;";

            foreach (var file in folderInputPath.EnumerateFiles("*.csv"))
            {
                string stockFile = file.FullName.ToLower();
                fileDate = Path.GetFileName(stockFile).Substring(0, 10);
                if (stockFile.Contains("win"))
                {
                    csvDataWin = CreateDataTable_Win.CreateAndFormatDt(stockFile);
                    var dtDataTableWin = ExportToCSV.ConvertToCSV(csvDataWin, folderOutputPath + fileDate + @"_Out_Win.csv");
                    //Write to database table CanadaData
                    // DatabaseLayer.InsertDataIntoSQLServerUsingSQLBulkCopy(dtDataTableWin, dataConnectionString);
                    DatabaseLayer.InsertCAWebDataIntoSQLServerUsingSQLBulkCopy(csvDataWin, dataConnectionString);

                }

                else if (stockFile.Contains("linux"))
                {
                    var csvDataLinux = CreateDataTable_Linux.CreateAndFormatDt(stockFile);
                    //Convert Datatable to CSV
                    ExportToCSV.ConvertToCSV(csvDataLinux, folderOutputPath + fileDate + @"_Out_Linux.csv");
                    DatabaseLayer.InsertCAWebDataIntoSQLServerUsingSQLBulkCopy(csvDataLinux, dataConnectionString);
                }

            }
            //Convert Datatable to CSV
            //var dtDataTableWin = ExportToCSV.ConvertToCSV(csvDataWin, folderOutputPath + fileDate + @"_Out_Win.csv");

            //Write from DB to file
            var query = @"SELECT[ALL], [DAY], [date], [Country], [EX], [Description], [Symbol]
                          , [LP/MA/LC], [% -CHG], [REPORT], [CAT], [CLOSE], [%], [TD], [I], [COM] FROM " + databaseName + ".[dbo].[CanadaData]";

            ExportDataFromDbtoFile.SaveResultToFile(dataConnectionString, folderOutputPath + "\\CanadaData_" + DateTime.Today.Date.ToString("yyyy-MM-dd") + ".csv", query);

            // Create template to upload to get data similar to dashboard
            var qDate = DateTime.Today.ToString("yyyy-MM-dd");
            var q5DaysAgoDate = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");

            var queryTemplateTSXCOMP = @"SELECT DISTINCT(Symbol),  case country when 'Canada' then 'CA' end as country
                                       from[dbo].[CanadaData] WHERE[EX] = 'TSX COMP' AND COUNTRY = 'Canada' AND DATE ='" + qDate + "'  ORDER BY SYMBOL ASC";
            var queryTemplateTSXVENT = @"SELECT DISTINCT(Symbol), case country when 'Canada' then 'CA' end as country 
                                       from [dbo].[CanadaData] WHERE [EX] ='TSX VENT'  AND COUNTRY = 'Canada' AND  DATE = '" + qDate + "' ORDER BY SYMBOL ASC";
            var queryTemplateNYSECOMP = @"SELECT DISTINCT(Symbol), case country when 'United States' then 'US'  end as country 
                                        from [dbo].[CanadaData] WHERE [EX] ='NYSE COMP' AND COUNTRY = 'United States' AND  DATE = '" + qDate + "' ORDER BY SYMBOL ASC";
            var queryTemplateNASDAQ = @"SELECT DISTINCT(Symbol), case country when 'United States' then 'US' end as country 
                                        from [dbo].[CanadaData] WHERE [EX] ='NASDAQ'  AND COUNTRY = 'United States' AND  DATE = '" + qDate + "' ORDER BY SYMBOL ASC";
            var queryTemplateNYSEMKT = @"SELECT DISTINCT(Symbol), case country when 'United States' then 'US' end as country 
                                       from [dbo].[CanadaData] WHERE [EX] ='NYSE MKT' AND COUNTRY = 'United States' AND  DATE = '" + qDate + "' ORDER BY SYMBOL ASC";
            var queryTemplateCAHL = @"SELECT DISTINCT (Symbol), case country when 'Canada' then 'CA' end as country 
                                        from [dbo].[CanadaData]  WHERE COUNTRY in ('Canada') AND EX in ('TSX COMP','TSX VENT')  AND CAT in ('H','L') AND  DATE = '" + qDate + "'  ORDER BY SYMBOL ASC";
            var queryTemplateUSHL = @"SELECT DISTINCT (Symbol), case country when 'United States' then 'US' end as country 
                                      from [dbo].[CanadaData]  WHERE COUNTRY in ('United States') AND EX in ('NYSE COMP','NASDAQ' ,'NYSE MKT')  AND CAT in ('H','L') AND  DATE = '" + qDate + "'  ORDER BY SYMBOL ASC ";
            // Z-101-1D-ALL-HL
            var queryTemplate1DALLHL = @"SELECT DISTINCT (Symbol),case country when 'United States' then 'US' when 'Canada' then 'CA' end as country 
	                                     from [dbo].[CanadaData]  WHERE COUNTRY in ('Canada', 'United States') AND 
                                         EX in ('TSX COMP','TSX VENT','NYSE COMP','NASDAQ' ,'NYSE MKT')  
                                         AND CAT in ('H','L') AND  DATE = '" + qDate + "'  ORDER BY SYMBOL ASC";
            //queries for 5 day date range
            var queryTemplate5DCAHL = @"SELECT DISTINCT (Symbol), case country when 'Canada' then 'CA' end as country 
                                        FROM [dbo].[CanadaData]  WHERE COUNTRY in ('Canada') AND EX in ('TSX COMP','TSX VENT')  AND CAT in ('H','L') 
                                        AND  DATE BETWEEN  '" + q5DaysAgoDate + "' AND '" + qDate + "'  ORDER BY SYMBOL ASC";

            var queryTemplate5DUSHL = @"SELECT DISTINCT(Symbol), case country when 'United States' then 'US' end as country 
                                      from [dbo].[CanadaData] WHERE COUNTRY in ('United States') AND EX in ('NYSE COMP','NASDAQ' ,'NYSE MKT')  AND CAT in ('H','L')
                                      AND  DATE BETWEEN  '" + q5DaysAgoDate + "' AND '" + qDate + "'  ORDER BY SYMBOL ASC";

            // Z-80-5D-ALL-HL
            var queryTemplate5DALLHL = @"SELECT DISTINCT (Symbol),case country when 'United States' then 'US' when 'Canada' then 'CA' end as country 
	                                 from [dbo].[CanadaData]  WHERE COUNTRY in ('Canada', 'United States') AND 
                                     EX in ('TSX COMP','TSX VENT','NYSE COMP','NASDAQ' ,'NYSE MKT')  
                                     AND CAT in ('H','L') AND   DATE BETWEEN  '" + q5DaysAgoDate + "' AND '" + qDate + "'  ORDER BY SYMBOL ASC";


            var queryTemplate5DCAALL = @"SELECT DISTINCT (Symbol), case country when 'Canada' then 'CA' end as country 
                                         FROM [dbo].[CanadaData]  WHERE COUNTRY in ('Canada') AND EX in ('TSX COMP','TSX VENT')  
                                         AND  DATE BETWEEN  '" + q5DaysAgoDate + "' AND '" + qDate + "'  ORDER BY SYMBOL ASC";

            var queryTemplate5DUSALL = @"SELECT DISTINCT (Symbol), case country when 'United States' then 'US' end as country  
                                         FROM [dbo].[CanadaData]  WHERE COUNTRY in ('United States') AND EX in ('NYSE COMP','NASDAQ' ,'NYSE MKT')  
                                         AND  DATE BETWEEN  '" + q5DaysAgoDate + "' AND '" + qDate + "'  ORDER BY SYMBOL ASC";

            // All data query
            var queryTemplateTILLDATECA = @"SELECT DISTINCT (Symbol), case country when 'Canada' then 'CA' end as country 
                                            FROM [dbo].[CanadaData]  WHERE COUNTRY in ('Canada') AND EX in ('TSX COMP','TSX VENT')   ORDER BY SYMBOL ASC";

            var queryTemplateTILLDATEUS = @"SELECT DISTINCT (Symbol), case country when 'United States' then 'US' end as country 
                                            FROM [dbo].[CanadaData]  WHERE COUNTRY in ('United States') AND EX in ('NYSE COMP','NASDAQ' ,'NYSE MKT')    ORDER BY SYMBOL ASC ";

            // 1DAY all
            var queryTemplate1DayCA = @"SELECT DISTINCT (Symbol), case country when 'Canada' then 'CA' end as country FROM [dbo].[CanadaData]  
                                        WHERE COUNTRY in ('Canada') AND EX in ('TSX COMP','TSX VENT') AND  DATE = '" + qDate + "'  ORDER BY SYMBOL ASC";

            var queryTemplate1DayUS = @"SELECT DISTINCT (Symbol), case country when 'United States' then 'US' end as country FROM [dbo].[CanadaData]  
                                        WHERE COUNTRY in ('United States') AND EX in ('NYSE COMP','NASDAQ' ,'NYSE MKT')   AND   DATE = '" + qDate + "'   ORDER BY SYMBOL ASC";

            var queryTemplateTillDateUSCA = @"SELECT DISTINCT (Symbol),case country when 'United States' then 'US' when 'Canada' then 'CA' end as country 
                                              FROM [dbo].[CanadaData]  WHERE COUNTRY in ('Canada','United States') AND EX in ('TSX COMP','TSX VENT','NYSE COMP','NASDAQ' ,'NYSE MKT')    ORDER BY SYMBOL ASC";

            List<CATemplateQueryModel> queriesALL, queriesUSMALCPGPLHLTODAY, queriesCAMALCPGPLHLTODAY, queries5DayAndAll, queriesTillDate, queries1DayAll, queriesMonday, queriesTuesday, queriesWednesday, queriesThrusday, queriesFriday;
            
            QueryList(queryTemplateTSXCOMP, queryTemplateTSXVENT, queryTemplateNYSECOMP,
                queryTemplateNASDAQ, queryTemplateNYSEMKT, queryTemplateCAHL, queryTemplateUSHL, queryTemplate1DALLHL,
                queryTemplate5DCAHL, queryTemplate5DUSHL, queryTemplate5DALLHL, queryTemplate5DCAALL, queryTemplate5DUSALL, 
                queryTemplateTILLDATECA, queryTemplateTILLDATEUS, queryTemplateTillDateUSCA, out queriesALL,
                out queriesUSMALCPGPLHLTODAY, out queriesCAMALCPGPLHLTODAY, out queries5DayAndAll,
                out queriesTillDate, out queries1DayAll, out queriesMonday, out queriesTuesday, 
                out queriesWednesday, out queriesThrusday, out queriesFriday);

            var dayOfWeek = DateTime.Today.DayOfWeek.ToString();
            var outputTemplateDirPath = textOutputPath.Text.ToLower() + @"\";
            switch (dayOfWeek)
            {
                case "Monday":
                    var outputDirMon = Directory.CreateDirectory(outputTemplateDirPath + qDate + @"\1M-45-51");
                    foreach (var fileNames in queriesMonday)
                    {
                        ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + outputDirMon + @"\" + fileNames.FileName, fileNames.Query);
                    }
                    break;
                case "Tuesday":
                    var outputDirTue = Directory.CreateDirectory(outputTemplateDirPath + qDate + @"\2TU-52-58");
                    foreach (var fileNames in queriesTuesday)
                    {
                        ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + outputDirTue + @"\" + fileNames.FileName, fileNames.Query);
                    }
                    break;
                case "Wednesday":
                    var outputDirWed = Directory.CreateDirectory(outputTemplateDirPath + qDate + @"\3W-59-65");
                    foreach (var fileNames in queriesWednesday)
                    {
                        ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + outputDirWed + @"\" + fileNames.FileName, fileNames.Query);
                    }
                    break;
                case "Thursday":
                    var outputDirThur = Directory.CreateDirectory(outputTemplateDirPath + qDate + @"\4TH-66-72");
                    foreach (var fileNames in queriesThrusday)
                    {
                        ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + outputDirThur + @"\" + fileNames.FileName, fileNames.Query);
                    }
                    break;
                case "Friday":
                    var outputDirFri = Directory.CreateDirectory(outputTemplateDirPath + qDate + @"\5F-73-79");
                    foreach (var fileNames in queriesFriday)
                    {
                        ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + outputDirFri + @"\" + fileNames.FileName, fileNames.Query);
                    }
                    break;
                default:
                    break;

            }
            CreateDirectoryAndTemplates(qDate, queriesALL, queriesUSMALCPGPLHLTODAY, queriesCAMALCPGPLHLTODAY,
                                        queries5DayAndAll, queriesTillDate, queries1DayAll,
                                        outputTemplateDirPath);

            Cursor = Cursors.Default;
            var t = Task.Delay(2000);//1 second/1000 ms
            t.Wait();
            success.Visible = true;
        }

        private static void QueryList(string queryTemplateTSXCOMP, string queryTemplateTSXVENT,
                                          string queryTemplateNYSECOMP, string queryTemplateNASDAQ, 
                                      string queryTemplateNYSEMKT, string queryTemplateCAHL,
                                      string queryTemplateUSHL, string queryTemplate1DALLHL, string queryTemplate5DCAHL, 
                                      string queryTemplate5DUSHL, string queryTemplate5DALLHL, string queryTemplate5DCAALL, 
                                      string queryTemplate5DUSALL, string queryTemplateTILLDATECA, 
                                      string queryTemplateTILLDATEUS, string queryTemplateTillDateUSCA,
                                      out List<CATemplateQueryModel> queriesALL,
                                      out List<CATemplateQueryModel> queriesUSMALCPGPLHLTODAY, 
                                      out List<CATemplateQueryModel> queriesCAMALCPGPLHLTODAY, 
                                      out List<CATemplateQueryModel> queries5DayAndAll,
                                      out List<CATemplateQueryModel> queriesTillDate,
                                      out List<CATemplateQueryModel> queries1DayAll,
                                      out List<CATemplateQueryModel> queriesMonday,
                                      out List<CATemplateQueryModel> queriesTuesday,
                                      out List<CATemplateQueryModel> queriesWednesday,
                                      out List<CATemplateQueryModel> queriesThrusday, 
                                      out List<CATemplateQueryModel> queriesFriday)
        {
            queriesALL = new List<CATemplateQueryModel>
            {
                new CATemplateQueryModel() { FileName = "Z-401-5D-CA.csv", Query = TemplateQueriesAll.queryTemplateCA5D },
                new CATemplateQueryModel() { FileName = "Z-402-5D-US.csv", Query = TemplateQueriesAll.queryTemplateUS5D },
                new CATemplateQueryModel() { FileName = "Z-403-1D-CA.csv", Query = TemplateQueriesAll.queryTemplateCA1D },
                new CATemplateQueryModel() { FileName = "Z-404-1D-US.csv", Query = TemplateQueriesAll.queryTemplateUS1D},
                new CATemplateQueryModel() { FileName = "Z-405-tillDate-US.csv", Query = TemplateQueriesAll.queryTemplateUSAllTillDate },
                new CATemplateQueryModel() { FileName = "Z-406-tillDate-CA.csv", Query = TemplateQueriesAll.queryTemplateCAAllTillDate},
                new CATemplateQueryModel() { FileName = "Z-407-TODAY.csv", Query = TemplateQueriesAll.queryTemplateAllWebCAUS},
                new CATemplateQueryModel() { FileName = "Z-408-TILLDATE.csv", Query =  TemplateQueriesAll.queryTemplateCATillDate},
                new CATemplateQueryModel() { FileName = "Z-409-tillDate-US.csv", Query = TemplateQueriesAll.queryTemplateUSTillDate }
            };

            queriesUSMALCPGPLHLTODAY = new List<CATemplateQueryModel>
            {
                new CATemplateQueryModel() { FileName = "Z-301-T-US-DG.csv", Query = TemplateQueriesUSA.queryTemplateUSDG },
                new CATemplateQueryModel() { FileName = "Z-302-T-US-DL.csv", Query = TemplateQueriesUSA.queryTemplateUSDL },
                new CATemplateQueryModel() { FileName = "Z-303-T-US-H.csv", Query = TemplateQueriesUSA.queryTemplateUSH },
                new CATemplateQueryModel() { FileName = "Z-304-T-US-L.csv", Query = TemplateQueriesUSA.queryTemplateUSL},
                new CATemplateQueryModel() { FileName = "Z-305-T-US-MA.csv", Query = TemplateQueriesUSA.queryTemplateUSMA },
                new CATemplateQueryModel() { FileName = "Z-306-T-US-LC.csv", Query = TemplateQueriesUSA.queryTemplateUSLC},
                new CATemplateQueryModel() { FileName = "Z-307-T-US-PG.csv", Query =  TemplateQueriesUSA.queryTemplateUSPG},
                new CATemplateQueryModel() { FileName = "Z-308-T-US-PL.csv", Query = TemplateQueriesUSA.queryTemplateUSPL }
            };
            queriesCAMALCPGPLHLTODAY = new List<CATemplateQueryModel>
            {
                new CATemplateQueryModel() { FileName = "Z-201-T-CA-DG.csv", Query = TemplateQueriesCA.queryTemplateCADG },
                new CATemplateQueryModel() { FileName = "Z-202-T-CA-DL.csv", Query = TemplateQueriesCA.queryTemplateCADL },
                new CATemplateQueryModel() { FileName = "Z-203-T-CA-H.csv", Query = TemplateQueriesCA.queryTemplateCAH },
                new CATemplateQueryModel() { FileName = "Z-204-T-CA-L.csv", Query = TemplateQueriesCA.queryTemplateCAL},
                new CATemplateQueryModel() { FileName = "Z-205-T-CA-MA.csv", Query = TemplateQueriesCA.queryTemplateCALC},
                new CATemplateQueryModel() { FileName = "Z-206-T-CA-LC.csv", Query = TemplateQueriesCA.queryTemplateCAMA },
                new CATemplateQueryModel() { FileName = "Z-207-T-CA-PG.csv", Query =  TemplateQueriesCA.queryTemplateCAPG},
                new CATemplateQueryModel() { FileName = "Z-208-T-CA-PL.csv", Query = TemplateQueriesCA.queryTemplateCAPL }
            };
            queries5DayAndAll = new List<CATemplateQueryModel>
            {
                // RENAME UL TO HL
                new CATemplateQueryModel() { FileName = "Z-89-5DCAHL.csv", Query = queryTemplate5DCAHL },
                new CATemplateQueryModel() { FileName = "Z-90-5DUSHL.csv", Query = queryTemplate5DUSHL },
                // NEW FILE Z-91-1DCA AND Z-92-1DUS
                //new CATemplateQueryModel() { FileName = "Z-89-5DCA.csv", Query = queryTemplate5DCA },
                //new CATemplateQueryModel() { FileName = "Z-90-5DUS.csv", Query = queryTemplate5DUS },
                new CATemplateQueryModel() { FileName = "Z-101-1D-ALL-HL.csv", Query = queryTemplate1DALLHL },
                new CATemplateQueryModel() { FileName = "Z-80-5D-ALL-HL.csv", Query = queryTemplate5DALLHL },

                new CATemplateQueryModel() { FileName = "Z-93-5DCAALL.csv", Query = queryTemplate5DCAALL },
                // DIVIDE BY 710 IF SYMBOL COUNT IS MORE THAN 710 AND CREATE FILES WITH 710 SYMBOL IN EACH FILE
                new CATemplateQueryModel() { FileName = "Z-94-5DUSALL.csv", Query = queryTemplate5DUSALL }
                //new CATemplateQueryModel() { FileName = "Z-95-5DUSALL.csv", Query = queryTemplate5DUSALL }
            };
            queriesTillDate = new List<CATemplateQueryModel>
            {

                new CATemplateQueryModel() { FileName = "Z-88-TILLDATECA.csv", Query = queryTemplateTILLDATECA },
                new CATemplateQueryModel() { FileName = "Z-88-TILLDATEUS.csv", Query = queryTemplateTILLDATEUS }
            };
            queries1DayAll = new List<CATemplateQueryModel>
            {
               // new CATemplateQueryModel() { FileName = "1DayCA.csv", Query = queryTemplate5DCAHL },
               // new CATemplateQueryModel() { FileName = "1DayUS.csv", Query = queryTemplate5DUSHL },
                new CATemplateQueryModel() { FileName = "Z-91-1DCA.csv", Query = TemplateQueriesAll.queryTemplateCA1D },
                new CATemplateQueryModel() { FileName = "Z-92-1DUS.csv", Query = TemplateQueriesAll.queryTemplateUS1D },

                // DIVIDE BY 710 IF SYMBOL COUNT IS MORE THAN 710 AND CREATE FILES WITH 710 SYMBOL IN EACH FILE
                new CATemplateQueryModel() { FileName = "Z-88-TILLDATECAUS.csv", Query = queryTemplateTillDateUSCA }
                
            };
            queriesMonday = new List<CATemplateQueryModel>
            {
                new CATemplateQueryModel() { FileName = "Z-45-1TSX.csv", Query = queryTemplateTSXCOMP },
                new CATemplateQueryModel() { FileName = "Z-46-1VENT.csv", Query = queryTemplateTSXVENT },
                new CATemplateQueryModel() { FileName = "Z-47-1NYSEC.csv", Query = queryTemplateNYSECOMP },
                new CATemplateQueryModel() { FileName = "Z-48-1NASDAQ.csv", Query = queryTemplateNASDAQ },
                new CATemplateQueryModel() { FileName = "Z-49-1NYSEM.csv", Query = queryTemplateNYSEMKT },
                new CATemplateQueryModel() { FileName = "Z-50-1CAHL.csv", Query = queryTemplateCAHL },
                new CATemplateQueryModel() { FileName = "Z-51-1USHL.csv", Query = queryTemplateUSHL }
            };
            queriesTuesday = new List<CATemplateQueryModel>
            {
                new CATemplateQueryModel() { FileName = "Z-52-2TSX.csv", Query = queryTemplateTSXCOMP },
                new CATemplateQueryModel() { FileName = "Z-53-2VENT.csv", Query = queryTemplateTSXVENT },
                new CATemplateQueryModel() { FileName = "Z-54-2NYSEC.csv", Query = queryTemplateNYSECOMP },
                new CATemplateQueryModel() { FileName = "Z-55-2NASDAQ.csv", Query = queryTemplateNASDAQ },
                new CATemplateQueryModel() { FileName = "Z-56-2NYSEM.csv", Query = queryTemplateNYSEMKT },
                new CATemplateQueryModel() { FileName = "Z-57-2CAHL.csv", Query = queryTemplateCAHL },
                new CATemplateQueryModel() { FileName = "Z-58-2USHL.csv", Query = queryTemplateUSHL }
            };
            queriesWednesday = new List<CATemplateQueryModel>
            {
                new CATemplateQueryModel() { FileName = "Z-59-3TSX.csv", Query = queryTemplateTSXCOMP },
                new CATemplateQueryModel() { FileName = "Z-60-3VENT.csv", Query = queryTemplateTSXVENT },
                new CATemplateQueryModel() { FileName = "Z-61-3NYSEC.csv", Query = queryTemplateNYSECOMP },
                new CATemplateQueryModel() { FileName = "Z-62-3NASDAQ.csv", Query = queryTemplateNASDAQ },
                new CATemplateQueryModel() { FileName = "Z-63-3NYSEM.csv", Query = queryTemplateNYSEMKT },
                new CATemplateQueryModel() { FileName = "Z-64-3CAHL.csv", Query = queryTemplateCAHL },
                new CATemplateQueryModel() { FileName = "Z-65-3USHL.csv", Query = queryTemplateUSHL }
            };
            queriesThrusday = new List<CATemplateQueryModel>
            {
                new CATemplateQueryModel() { FileName = "Z-66-4TSX.csv", Query = queryTemplateTSXCOMP },
                new CATemplateQueryModel() { FileName = "Z-67-4VENT.csv", Query = queryTemplateTSXVENT },
                new CATemplateQueryModel() { FileName = "Z-68-4NYSEC.csv", Query = queryTemplateNYSECOMP },
                new CATemplateQueryModel() { FileName = "Z-69-4NASDAQ.csv", Query = queryTemplateNASDAQ },
                new CATemplateQueryModel() { FileName = "Z-70-4NYSEM.csv", Query = queryTemplateNYSEMKT },
                new CATemplateQueryModel() { FileName = "Z-71-4CAHL.csv", Query = queryTemplateCAHL },
                new CATemplateQueryModel() { FileName = "Z-72-4USHL.csv", Query = queryTemplateUSHL }
            };
            queriesFriday = new List<CATemplateQueryModel>
            {
                new CATemplateQueryModel() { FileName = "Z-73-5TSX.csv", Query = queryTemplateTSXCOMP },
                new CATemplateQueryModel() { FileName = "Z-74-5VENT.csv", Query = queryTemplateTSXVENT },
                new CATemplateQueryModel() { FileName = "Z-75-5NYSEC.csv", Query = queryTemplateNYSECOMP },
                new CATemplateQueryModel() { FileName = "Z-76-5NASDAQ.csv", Query = queryTemplateNASDAQ },
                new CATemplateQueryModel() { FileName = "Z-77-5NYSEM.csv", Query = queryTemplateNYSEMKT },
                new CATemplateQueryModel() { FileName = "Z-78-5CAHL.csv", Query = queryTemplateCAHL },
                new CATemplateQueryModel() { FileName = "Z-79-5USHL.csv", Query = queryTemplateUSHL }
            };
        }

        private static void CreateDirectoryAndTemplates(string qDate, List<CATemplateQueryModel>  queriesALL, List<CATemplateQueryModel> queriesUSMALCPGPLHLTODAY, List<CATemplateQueryModel> queriesCAMALCPGPLHLTODAY, List<CATemplateQueryModel> queries5DayAndAll, List<CATemplateQueryModel> queriesTillDate, List<CATemplateQueryModel> queries1DayAll, string outputTemplateDirPath)
        {
            // Create folder and template files for 5day range
            // var outputDir5Day = Directory.CreateDirectory(outputTemplateDirPath + qDate + @"\5DayData");
            foreach (var fileNames in queries5DayAndAll)
            {
                //ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + outputDir5Day + @"\" + fileNames.FileName, fileNames.Query);
                ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + fileNames.FileName, fileNames.Query);
            }
            // Create folder and template files for all data
            //var outputDirTillDate = Directory.CreateDirectory(outputTemplateDirPath + qDate + @"\TillDateData");
            foreach (var fileNames in queriesTillDate)
            {
                //ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + outputDirTillDate + @"\" + fileNames.FileName, fileNames.Query);
                ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + fileNames.FileName, fileNames.Query);
            }
            // Create folder and template files for 1day all data
            //var outputDir1DayAll = Directory.CreateDirectory(outputTemplateDirPath + qDate + @"\1DayAllData");
            foreach (var fileNames in queries1DayAll)
            {
                //ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + outputDir1DayAll + @"\" + fileNames.FileName, fileNames.Query);
                ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + fileNames.FileName, fileNames.Query);
            }

            // Create folder and template files for Canada today CA-MALCPGPLHL-TODAY
            //var outputDirCADGDL = Directory.CreateDirectory(outputTemplateDirPath + qDate + @"\CA-MALCPGPLHL-TODAY");
            foreach (var fileNames in queriesCAMALCPGPLHLTODAY)
            {
                //ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + outputDirCADGDL + @"\" + fileNames.FileName, fileNames.Query);
                ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + fileNames.FileName, fileNames.Query);
            }

            // Create folder and template files for USA today US-MALCPGPLHL-TODAY
            //var outputDirUSDGDL = Directory.CreateDirectory(outputTemplateDirPath + qDate + @"\US-MALCPGPLHL-TODAY");
            foreach (var fileNames in queriesUSMALCPGPLHLTODAY)
            {
                //ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + outputDirUSDGDL + @"\" + fileNames.FileName, fileNames.Query);
                ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + fileNames.FileName, fileNames.Query);
            }

            // Create folder and template files for ALL
            //var outputDirAll = Directory.CreateDirectory(outputTemplateDirPath + qDate + @"\All");
            foreach (var fileNames in queriesALL)
            {
                // ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + outputDirAll + @"\" + fileNames.FileName, fileNames.Query);
                ExportDataFromDbtoFile.CreateCATemplateFromDb(dataConnectionString, outputTemplateDirPath + qDate + @"\" + fileNames.FileName, fileNames.Query);
            }
        }

        private void btnOutputData_Click(object sender, EventArgs e)
        {
            var outputdataToDate = dateTimeOutputData_From.Text;
            var outputdataFromDate = dateTimeOutputData_To.Text;
            lblFileOutput.Text = "";
            dataConnectionString = "Data Source=" + strDataserver + ";Initial Catalog=" + strDatabase + "; Integrated Security=SSPI;";
            var outputPath = textOutputPath.Text.ToLower() + @"\AllDataPrint_"+DateTime.Today.Date.ToString("yyyy-MM-dd") +".csv";
            var query = @"SELECT [Description],[EX],[Date],[DayOfWeek],[MarketCapNum],[Volume],[Symbol],[%Change],[Last],[EPS],[PE],[Market Cap]
                        ,[Shares],[Net Chng],[52Low],[Low],[Last],[Close],[Open],[High],[52High],[Bid],[Ask] ,[RSI],[Div. Payout Per Share (% of EPS) - Current],[Open.Int]
                        FROM [dbo].[WatchList] where [Date] between '" + outputdataToDate +"' and  '"+ outputdataFromDate + "'";

            DatabaseLayer.SaveResultToFile(dataConnectionString, outputPath, query);
            lblFileOutput.Text += @": "+ outputPath;
        }

        private void btnSelectedOutputFile_Click(object sender, EventArgs e)
        {
            var selecteddataFromDate = dateTimeSelectedData_From.Text;
            var selecteddataToDate = dateTimeSelectedData_To.Text; 
            SaveSelectedData(selecteddataFromDate, selecteddataToDate);
            
        }

        private void textDatabase_TextChanged(object sender, EventArgs e)
        {
            strDatabase = textDatabase.Text.ToLower();
        }

        private void textDataServer_TextChanged(object sender, EventArgs e)
        {
            strDataserver = textDataServer.Text.ToLower();
        }

        public void SaveSelectedData()
        {
            dataConnectionString = "Data Source=" + strDataserver + ";Initial Catalog=" + strDatabase + "; Integrated Security=SSPI;";
            var outputPath = textOutputPath.Text.ToLower() + @"\SelectedDataRun_" + DateTime.Today.Date.ToString("yyyy-MM-dd") + ".csv";
            var query = @"select [Description],[EX],[Date],[DayOfWeek],[MarketCapNum],[Volume],[Symbol],[%Change],[Last],[EPS],[PE],[Market Cap]
                        ,[Shares],[Net Chng],[52Low],[Low],[Last],[Close],[Open],[High],[52High],[Bid],[Ask] ,[RSI],[Div. Payout Per Share (% of EPS) - Current],[Open.Int] from watchlist where EX in (
                            'MP',
                             --10
                            '% Change Gainers',
                            '% Change Losers', 
                            'Cash-Settled Futures',
                            'Company Profile',
                            'Economic Indicators', 
                            'Full Session Options',
                            'Futures',
                            'FX',
                            'FX Commission Pairs',
                            'FX Non-Commission Pairs',

                             -- 10
                            'Gap Down'	,
                            'Gap Up	 '		 ,
                            'Interest Rates'	 , 
                            'Market Maker Move Stocks'  , 
                            'MICRO FUTURES',
                            'New Yearly Highs '	 , 
                            'New Yearly Lows'	    , 
                            'Penny Increment Options',
                            'Physicallt Settled Futures',
                            'Preferred Stocks', 

                            --4
                            'Upcoming Dividends',
                            'Upcoming Earnings'	    , 
                            'Upcoming Splits'	 ,
                            'Weeklys'		 , 


                            --4
                            'Analyst Downgrades', 
                            'Analyst Upgrades', 
                            'Post-market Movers', 
                            'Pre-market Movers', 
                             --12
                            'Top10 % Gain NASDAQ ',
                            'Top10 % Gain NYSE'	 , 
                            'Top10 % Loss NASDAQ',	
                            'Top10 % Loss NYSE',
                            'Top10 Active NASDAQ'	 ,
                            'Top10 Active NYSE'	 ,
                            'Top10 Net Gain NASDAQ'	 ,
                            'Top10 Net Gain NYSE'	 ,
                            'Top10 Net Loss NASDAQ'	    ,
                            'Top10 Net Loss NYSE'	    , 
                            'Top10 Sizzling Stocks'	 , 
                            'Top10 Volatility Increase',

                             --- 10
                            '-3To-100M'		 , 
                            '3To100M'		 ,
                            'AAPCGR', 
                            'AAPCLR'		 , 
                            'PCLR', 
                            'PCGR'	,
                            'PCG'			 ,  
                            'PCL',
                            'INV'			 , 
                            'AINV'		 )";

            DatabaseLayer.SaveResultToFile(dataConnectionString, outputPath, query);
            lblSelectedData.Text += @": " + outputPath;
        }

        public void SaveSelectedData(string selecteddataFromDate, string selecteddataToDate)
        {
            dataConnectionString = "Data Source=" + strDataserver + ";Initial Catalog=" + strDatabase + "; Integrated Security=SSPI;";
            var outputPath = textOutputPath.Text.ToLower() + @"\SelectedDataPrint_" + DateTime.Today.Date.ToString("yyyy-MM-dd") + ".csv";
            var query = @"select [Description],[EX],[Date],[DayOfWeek],[MarketCapNum],[Volume],[Symbol],[%Change],[Last],[EPS],[PE],[Market Cap]
                        ,[Shares],[Net Chng],[52Low],[Low],[Last],[Close],[Open],[High],[52High],[Bid],[Ask] ,[RSI],[Div. Payout Per Share (% of EPS) - Current],[Open.Int] 
                        from watchlist where EX in (
                            'MP',
                             --10
                            '% Change Gainers',
                            '% Change Losers', 
                            'Cash-Settled Futures',
                            'Company Profile',
                            'Economic Indicators', 
                            'Full Session Options',
                            'Futures',
                            'FX',
                            'FX Commission Pairs',
                            'FX Non-Commission Pairs',

                             -- 10
                            'Gap Down'	,
                            'Gap Up	 '		 ,
                            'Interest Rates'	 , 
                            'Market Maker Move Stocks'  , 
                            'MICRO FUTURES',
                            'New Yearly Highs '	 , 
                            'New Yearly Lows'	    , 
                            'Penny Increment Options',
                            'Physicallt Settled Futures',
                            'Preferred Stocks', 

                            --4
                            'Upcoming Dividends',
                            'Upcoming Earnings'	    , 
                            'Upcoming Splits'	 ,
                            'Weeklys'		 , 


                            --4
                            'Analyst Downgrades', 
                            'Analyst Upgrades', 
                            'Post-market Movers', 
                            'Pre-market Movers', 
                             --12
                            'Top10 % Gain NASDAQ ',
                            'Top10 % Gain NYSE'	 , 
                            'Top10 % Loss NASDAQ',	
                            'Top10 % Loss NYSE',
                            'Top10 Active NASDAQ'	 ,
                            'Top10 Active NYSE'	 ,
                            'Top10 Net Gain NASDAQ'	 ,
                            'Top10 Net Gain NYSE'	 ,
                            'Top10 Net Loss NASDAQ'	    ,
                            'Top10 Net Loss NYSE'	    , 
                            'Top10 Sizzling Stocks'	 , 
                            'Top10 Volatility Increase',

                             --- 10
                            '-3To-100M'		 , 
                            '3To100M'		 ,
                            'AAPCGR', 
                            'AAPCLR'		 , 
                            'PCLR', 
                            'PCGR'	,
                            'PCG'			 ,  
                            'PCL',
                            'INV'			 , 
                            'AINV'		 )  and 
                            [Date] between '" + selecteddataFromDate + "' and  '" + selecteddataToDate + "'";

            DatabaseLayer.SaveResultToFile(dataConnectionString, outputPath, query);
            lblSelectedData.Text += @": " + outputPath;
        }

        public void SaveSelectedData(string selectedFromDate, string selectedToDate, string filterSymbol)
        {
            dataConnectionString = "Data Source=" + strDataserver + ";Initial Catalog=" + strDatabase + "; Integrated Security=SSPI;";
            var outputPath = textOutputPath.Text.ToLower() + @"\SelectedSymbolDataPrint_" + DateTime.Today.Date.ToString("yyyy-MM-dd") + ".csv";
            var query = @"select [Description],[EX],[Date],[DayOfWeek],[MarketCapNum],[Volume],[Symbol],[%Change],[Last],[EPS],[PE],[Market Cap]
                        ,[Shares],[Net Chng],[52Low],[Low],[Last],[Close],[Open],[High],[52High],[Bid],[Ask] ,[RSI],[Div. Payout Per Share (% of EPS) - Current],[Open.Int] 
                        from watchlist where 
                            [Date] between '" + selectedFromDate + "' and  '" + selectedToDate + "' and" +
                            "[Symbol] like '%" + filterSymbol + "%'  ";

            DatabaseLayer.SaveResultToFile(dataConnectionString, outputPath, query);
            lblSelectedData.Text += @": " + outputPath;
        }
        private void BtnEXSelect_Click(object sender, EventArgs e)
        {
            EXSelect selectEX = new EXSelect();

            dataConnectionString = "Data Source=" + strDataserver + ";Initial Catalog=" + strDatabase + "; Integrated Security=SSPI;";
            selectEX.ShowDialog();

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            var selectedFromDate = dateTimeFrom.Text;
            var selectedToDate = dateTimeTo.Text;
            var filterSymbol = textBoxSelectSymbol.Text;
            SaveSelectedData(selectedFromDate, selectedToDate, filterSymbol);
        }

        private void LinkLabelGoogleFinance_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void LinkLabelGoogleFinance_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData as string);
        }

        private void ManageStockData_Load(object sender, EventArgs e)
        {
            LinkLabel.Link link1 = new LinkLabel.Link();
            link1.LinkData = "https://www.google.ca/finance";
            linkLabelGoogleFinance.Links.Add(link1);
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void txtRestoreFilePath_TextChanged(object sender, EventArgs e)
        {

        }

        private void success_Click(object sender, EventArgs e)
        {

        }

        private void dateTimeOutputData_From_ValueChanged(object sender, EventArgs e)
        {

        }

        private void lblAllDataOutput_From_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void lblRestoreDBPath_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label81_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void btn_CADashboard_Click(object sender, EventArgs e)
        {
            success.Visible = false;
            Cursor = Cursors.WaitCursor;
            bool isFolderDate = false;
            dataConnectionString = "Data Source=" + strDataserver + ";Initial Catalog=" + strDatabase + "; Integrated Security=SSPI;";
            var fileDir = new DirectoryInfo(folderPath.Text);
            DirectoryInfo[] diArr = fileDir.GetDirectories();
            for (int i = 0; i < diArr.Length; i++)
            {
                isFolderDate = false;
                DirectoryInfo dri = diArr[i];
                Console.WriteLine(dri.Name);
                var date = dri.Name.Substring(0, 10);
                DateTime myDate;
                if (DateTime.TryParse(date, out myDate))
                {
                    fileDate = date;
                    isFolderDate = true;
                }


                try
                {
                    foreach (var file in diArr[i].EnumerateFiles("*.csv", SearchOption.AllDirectories))
                    {
                        string stockFile = file.FullName.ToLower();
                        DataTable csvData = ProcessStockData.GetCADashboardDataTableFromCSVFile(stockFile, fileDate);
                        DatabaseLayer.InsertCADashboardDataIntoSQL(csvData, dataConnectionString, stockFile);
                    }
                    var outputPath = textOutputPath.Text.ToLower() + @"\AllDataRun_" + fileDate + @".csv";
                    var query = @"SELECT [Ticker] ,[C],[Market Cap],[Volume] ,[AVG Volume 6 Month] ,[Blk Vol] ,[Name] ,[Symbol] ,[EX] ,[Date] ,[DayOfWeek],
                                [EPS] ,[PE] ,[Yield] ,[% Chg] ,[Change] ,[Price] ,[Ask] ,[Bid] ,[Trd Sz] ,[Ask Sz] 
                                ,[Bid Sz] ,[Close] ,[52 Week Range] ,[52 High] ,[High] ,[Low] ,[52 Low] ,
                                 [Open] ,[Call/Put] ,[Imp Vol] ,[Mark] ,[Exch] ,[Mkt Val] ,
                                 [Shr Os] ,
                                [Opn Int] ,[Day % P/L] ,[Day P/L] ,[% P/L] ,[P/L] ,[Book Cost] ,
                                [Avg Cost] ,[Entry Date] ,[Quantity] ,[Trade] ,[Trend] ,[Opn Int Chg] ,
                                [Curr] ,[Q] ,[Div] , [Div Pay Dt] ,[Ex-Div] ,[Exp Dt] ,[Num Trd] ,
                                [Prc Tck] ,[Quote Trend] ,[Range] ,[Rel Range] ,[Strike]  
                                FROM [dbo].[CADbWatchList]";
                    DatabaseLayer.SaveResultToFile(dataConnectionString, outputPath, query);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error has occured", ex.StackTrace);
                }

            }
            Cursor = Cursors.Default;
            var t = Task.Delay(2000);//1 second/1000 ms
            t.Wait();
            success.Visible = true;
        }

        private void button19_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void textBox55_TextChanged(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
    }
}
