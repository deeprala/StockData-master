using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Logger;


namespace UploadData
{
    public partial class BreakEven : Form
    {
        public double amountSpent = 0.0;
        public double currentAmount = 0.0;

        public static string strDataserver;
        public static string strDatabase;
        public static string dataConnectionString;

        public BreakEven()
        {
            InitializeComponent();
        }

        private void txtOldPrice_TextChanged(object sender, EventArgs e)
        {

            amountSpent = Convert.ToDouble(txtOldStockNumbers.Text) * Convert.ToDouble(txtOldPrice.Text);
            txtAmountSpent.Text = amountSpent.ToString();
        }

        private void txtCurrentPrice_TextChanged(object sender, EventArgs e)
        {
            double longPercentUpDownAmt = 0.0;
            double longPresentUpDownStock = 0.0;

            currentAmount = Convert.ToDouble(txtOldStockNumbers.Text) * Convert.ToDouble(txtCurrentPrice.Text);
            txtCurrentAmount.Text = currentAmount.ToString("F");

            longPercentUpDownAmt = -(1 - currentAmount / amountSpent) * 100;
            txtLngUpDownAmt.Text = longPercentUpDownAmt.ToString("F");

            longPresentUpDownStock = -(1 - Convert.ToDouble(txtCurrentPrice.Text) / Convert.ToDouble(txtOldPrice.Text)) * 100;
            txtLngUpDownStock.Text = longPresentUpDownStock.ToString("F");

            txtShortUpDownAmount.Text  = (-longPercentUpDownAmt).ToString("F");
            txtShortUpDownStock.Text  = (-longPresentUpDownStock).ToString("F");

            txtLongProfitLoss.Text = (currentAmount - amountSpent).ToString("F");


            txtShortProfitLoss.Text = (-(currentAmount - amountSpent)).ToString("F");

        }

        private void txtBreakEvenMoney_TextChanged(object sender, EventArgs e)
        {
            

        }

        public void Calculate()
        {
            txtUnroundedStockstoBuy.Text =
                (Convert.ToDouble(txtBreakEvenMoney.Text) / Convert.ToDouble(txtCurrentPrice.Text)).ToString("F");

            txtRoundedStockstoBuy.Text = Math.Round(Convert.ToDecimal(txtUnroundedStockstoBuy.Text)).ToString();

            txtAdjBreakEvenMoney.Text =
                (Convert.ToDouble(txtCurrentPrice.Text) * Convert.ToInt64(txtRoundedStockstoBuy.Text)).ToString("F");

            txtTotalShares.Text =
                (Convert.ToDecimal(txtOldStockNumbers.Text) + Convert.ToDecimal(txtUnroundedStockstoBuy.Text)).ToString("####");

            txtTotalAmount.Text = (amountSpent + Convert.ToDouble(txtAdjBreakEvenMoney.Text)).ToString("F");

            txtAvgPrice.Text = (Convert.ToDouble(txtTotalAmount.Text) / Convert.ToDouble(txtTotalShares.Text)).ToString("F");

            txtLongAvgPercent.Text = (-(1 - Convert.ToDouble(txtCurrentPrice.Text) / Convert.ToDouble(txtAvgPrice.Text)) *
                                      100).ToString("F") ;
            //txtLongAvgPercent.Text = txtLongAvgPercent.Text.Contains("-") ? txtLongAvgPercent.Font. ;
            txtLngAvgReductionPercent.Text =
                (-(-Convert.ToDouble(txtLngUpDownStock.Text) - -Convert.ToDouble(txtLongAvgPercent.Text))).ToString("F") ;

            txtShortAvgPercent.Text = ((1 - Convert.ToDouble(txtCurrentPrice.Text) / Convert.ToDouble(txtAvgPrice.Text)) *
                                       100).ToString("F") ;

            txtShortAvgReductionPercent.Text =
                (-Convert.ToDouble(txtLngUpDownStock.Text) - -Convert.ToDouble(txtLongAvgPercent.Text)).ToString("F") ;

            txtLongAvgProfitLoss.Text =
                ((currentAmount + Convert.ToDouble(txtBreakEvenMoney.Text) - Convert.ToDouble(txtTotalAmount.Text)))
                .ToString("F");

            txtShortAvgProfitLoss.Text =
                (-(currentAmount + Convert.ToDouble(txtBreakEvenMoney.Text) - Convert.ToDouble(txtTotalAmount.Text)))
                .ToString("F");
        }

        private void Save_Click(object sender, EventArgs e)
        {
            //BreakEvenData.InsertDataIntoBreakEven(dataConnectionString);

            // change the data source value as needed LN443397
            using (SqlConnection dbConnection =
                    new SqlConnection(ManageStockData.dataConnectionString))
                //using (SqlConnection dbConnection =
                //new SqlConnection("Data Source=LN443397\\LOCAL;Initial Catalog=RAMTEST;Integrated Security=SSPI;"))
                //RAMESHhrd2in1
                //rameshdi5
                try
                {
                    {
                        dbConnection.Open();

                        var insertCommandText = @"INSERT INTO [dbo].[BreakEven]
          ([LongShortProfitLoss] ,[OldStockNumbers]          ,[OldPrice]          ,[AmountSpent]          ,[CurrentPrice]          ,[CurrentAmount]
          ,[Long%UpDownAmount]          ,[Long%UpDownStock]          ,[Short%UpDownAmount]          ,[Short%UpDownStock]
          ,[LongProfitLoss]          ,[LongAvgProfitLoss]
          ,[ShortProfitLoss]
          ,[ShortAvgProfitLoss]
          ,[BreakEvenMoney]
          ,[UnroundedStocktoBuy]
          ,[RoundedSTockstoBuy]
          ,[AdjBreakEvenMoney]
          ,[TotalShares]
          ,[TotalAmount] 
          ,[AvgPrice]
          ,[LongAvg%]
          ,[LongAvg%Reduction]
          ,[ShortAvg%]
          ,[ShortAvg%Reduction])
    VALUES
          ('" + txtStockProfitLoss.Text.Replace("'", "''") + "'" +  "," + txtOldStockNumbers.Text + "," + txtOldPrice.Text + "," + txtAmountSpent.Text
         + "," + txtCurrentPrice.Text + "," + txtCurrentAmount.Text + "," + txtLngUpDownAmt.Text + "," + txtLngUpDownStock.Text
         + "," + txtShortUpDownAmount.Text + "," + txtShortUpDownStock.Text + "," + txtLongProfitLoss.Text + "," + txtLongAvgProfitLoss.Text + "," +
               txtShortProfitLoss.Text + "," + txtShortAvgProfitLoss.Text + "," + txtBreakEvenMoney.Text + "," + txtUnroundedStockstoBuy.Text
         + "," + txtRoundedStockstoBuy.Text + "," + txtAdjBreakEvenMoney.Text + "," + txtTotalShares.Text + "," + txtTotalAmount.Text
         + "," + txtAvgPrice.Text + "," + txtLongAvgPercent.Text + "," + txtLngAvgReductionPercent.Text + "," + txtShortAvgPercent.Text
         + "," + txtShortAvgReductionPercent.Text + ")";
                        SqlCommand insertData = new SqlCommand(insertCommandText, dbConnection);
                        insertData.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    //Logging.Logger("SQL exception " + ex.Message);
                }


        }


        private void btnCalculate_Click(object sender, EventArgs e)
        {

           // txtStockProfitLoss.Text 
            Calculate();
        }

        private void labelMarginCalculation_Click(object sender, EventArgs e)
        {

        }
    }
}
