//using System;
//using System.Data.SqlClient;
//using Logger;

//namespace BreakEvenDAL
//{
//   public class BreakEvenData
//    {
       
//        public static void InsertDataIntoBreakEven(string DataConnection)
//        {
//            // change the data source value as needed LN443397
//            using (SqlConnection dbConnection =
//                    new SqlConnection(DataConnection))
//                // using (SqlConnection dbConnection =
//                // new SqlConnection("Data Source=RAMESHI7;Initial Catalog=RAMTEST;Integrated Security=SSPI;"))
//                //RAMESHhrd2in1
//                //rameshdi5
//                try
//                {
//                    {
//                        dbConnection.Open();

//                        var insertCommandText = @"INSERT INTO [dbo].[BreakEven]
//          ([LongShortProfitLoss]
//          ,[OldStockNumbers]
//          ,[OldPrice]
//          ,[AmountSpent]
//          ,[CurrentPrice]
//          ,[CurrentAmount]
//          ,[Long%UpDownAmount]
//          ,[Long%UpDownStock]
//          ,[Short%UpDownAmount]
//          ,[Short%UpDownStock]
//          ,[LongProfitLoss]
//          ,[LongAvgProfitLoss]
//          ,[ShortProfitLoss]
//          ,[ShortAvgProfitLoss]
//          ,[BreakEvenMoney]
//          ,[UnroundedStocktoBuy]
//          ,[RoundedSTockstoBuy]
//          ,[AdjBreakEvenMoney]
//          ,[TotalShares]
//          ,[TotalAmount]
//          ,[AvgPrice]
//          ,[LongAvg%]
//          ,[LongAvg%Reduction]
//          ,[ShortAvg%]
//          ,[ShortAvg%Reduction])
//    VALUES
//          (" txtStockProfitLoss.Text + ","+ txtOldStockNumbers.Text +","+ txtOldPrice.Text +","+ txtAmountSpent.Text 
//             +","+ txtCurrentPrice.Text +","+ txtCurrentAmount.Text +","+ txtLngUpDownAmt.Text +","+ txtLngUpDownStock.Text 
//             +","+ txtShortUpDownAmount.Text +","+ txtShortUpDownStock.Text +","+ txtLongProfitLoss.Text +","+ txtLongAvgProfitLoss.Text +","+ 
//                   txtShortProfitLoss.Text +","+ txtShortAvgProfitLoss.Text +","+ txtBreakEvenMoney.Text +","+ txtUnroundedStockstoBuy.Text 
//             +","+ txtRoundedStockstoBuy.Text +","+ txtAdjBreakEvenMoney.Text +","+ txtTotalShares.Text +","+ txtTotalAmount.Text 
//             +","+ txtAvgPrice.Text +","+ txtLongAvgPercent.Text +","+ txtLngAvgReductionPercent.Text +","+ txtShortAvgPercent.Text 
//             +","+ txtShortAvgReductionPercent.Text +")";
//                        SqlCommand updateData = new SqlCommand(insertCommandText, dbConnection);
//                        updateData.ExecuteNonQuery();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Logging.Logger("SQL exception " + ex.Message);
//                }

//        }

//    }
//}
