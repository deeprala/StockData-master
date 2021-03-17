using System;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Logger;

namespace DAL
{
    public class RestoreDataBase
    {

        public void RestoreDatabase(string databaseName, string backUpFile, string serverName, string restorePath)
        {
            try
            {
                ServerConnection connection = new ServerConnection(serverName);
                Server sqlServer = new Server(connection);
                Restore rstDatabase = new Restore();
                rstDatabase.Action = RestoreActionType.Database;
                rstDatabase.Database = databaseName;
                BackupDeviceItem bkpDevice = new BackupDeviceItem(backUpFile, DeviceType.File);
                rstDatabase.Devices.Add(bkpDevice);
                var dataFilePath = restorePath;
                var logFilePath = restorePath;

                //Create The Restore Database Ldf & Mdf file name
                string dataFileLocation = dataFilePath + databaseName + ".mdf";
                string logFileLocation = logFilePath + databaseName + "_Log.ldf";
               
                RelocateFile rf = new RelocateFile(databaseName, dataFileLocation);

                // Replace ldf, mdf file name of selected Backup file 
                System.Data.DataTable logicalRestoreFiles = rstDatabase.ReadFileList(sqlServer);
                rstDatabase.RelocateFiles.Add(new RelocateFile(logicalRestoreFiles.Rows[0][0].ToString(), dataFileLocation));
                rstDatabase.RelocateFiles.Add(new RelocateFile(logicalRestoreFiles.Rows[1][0].ToString(), logFileLocation));


                rstDatabase.ReplaceDatabase = true;
                rstDatabase.SqlRestore(sqlServer);

            }
            catch (Exception e)
            {           
                Console.WriteLine("Failed to restore database" + e.Message);
                throw e;
            }
            
        }
    }
}
