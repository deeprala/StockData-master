using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UploadData
{
    public partial class EXSelect : Form
    {
        public EXSelect()
        {
            InitializeComponent();
        }

        public void CheckBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSelectAll.Checked == true)
            {
                checkBoxAAPCGR.Checked = true; checkBoxAAPCLR.Checked = true; checkBoxActiveNasdaq.Checked = true; checkBoxActiveNyse.Checked = true;
                checkBoxAINV.Checked = true; checkBoxAnalystDowngrades.Checked = true; checkBoxAnalystUpgrades.Checked = true; checkBoxCashFutures.Checked = true;
                checkBoxChangeGainers.Checked = true; checkBoxChangeLosers.Checked = true; checkBoxCompanyProfile.Checked = true; checkBoxEconomicIindicators.Checked = true;
                checkBoxFutures.Checked = true; checkBoxFX.Checked = true; checkBoxFXCommPairs.Checked = true; checkBoxFxNonCommPairs.Checked = true;
                checkBoxGainNasdaq.Checked = true; checkBoxGainNyse.Checked = true; checkBoxGapDown.Checked = true; checkBoxGapUp.Checked = true;
                checkBoxInterestRates.Checked = true; checkBoxINV.Checked = true; checkBoxLossNasdaq.Checked = true; checkBoxLossNyse.Checked = true;
                checkBoxMicroFutures.Checked = true; checkBoxMinus3To100M.Checked = true; checkBoxMoveStocks.Checked = true; checkBoxMP.Checked = true;
                checkBoxNetGainNasdaq.Checked = true; checkBoxNetGainNyse.Checked = true; checkBoxNetLossNasdaq.Checked = true; checkBoxNewYearlyHoghs.Checked = true;
                checkBoxNewYearlyLows.Checked = true; checkBoxPCG.Checked = true; checkBoxPCGR.Checked = true; checkBoxPCL.Checked = true;
                checkBoxPCLR.Checked = true; checkBoxPennyIncrementOptions.Checked = true; checkBoxPlus3To100M.Checked = true; checkBoxPostmarketMovers.Checked = true;
                checkBoxPreferredStocks.Checked = true; checkBoxPremarketMovers.Checked = true; checkBoxSessionOptions.Checked = true; checkBoxSettledFutures.Checked = true;
                checkBoxSizzlingStocks.Checked = true; checkBoxUpcomingDvd.Checked = true; checkBoxUpcomingEarnings.Checked = true; checkBoxUpcomingSplits.Checked = true;
                checkBoxVolatilityIncrease.Checked = true; checkBoxWeeklys.Checked = true; checkBoxNetLossNyse.Checked = true;



            }
            else
            {
                checkBoxAAPCGR.Checked = false; checkBoxAAPCLR.Checked = false; checkBoxActiveNasdaq.Checked = false; checkBoxActiveNyse.Checked = false;
                checkBoxAINV.Checked = false; checkBoxAnalystDowngrades.Checked = false; checkBoxAnalystUpgrades.Checked = false; checkBoxCashFutures.Checked = false;
                checkBoxChangeGainers.Checked = false; checkBoxChangeLosers.Checked = false; checkBoxCompanyProfile.Checked = false; checkBoxEconomicIindicators.Checked = false;
                checkBoxFutures.Checked = false; checkBoxFX.Checked = false; checkBoxFXCommPairs.Checked = false; checkBoxFxNonCommPairs.Checked = false;
                checkBoxGainNasdaq.Checked = false; checkBoxGainNyse.Checked = false; checkBoxGapDown.Checked = false; checkBoxGapUp.Checked = false;
                checkBoxInterestRates.Checked = false; checkBoxINV.Checked = false; checkBoxLossNasdaq.Checked = false; checkBoxLossNyse.Checked = false;
                checkBoxMicroFutures.Checked = false; checkBoxMinus3To100M.Checked = false; checkBoxMoveStocks.Checked = false; checkBoxMP.Checked = false;
                checkBoxNetGainNasdaq.Checked = false; checkBoxNetGainNyse.Checked = false; checkBoxNetLossNasdaq.Checked = false; checkBoxNewYearlyHoghs.Checked = false;
                checkBoxNewYearlyLows.Checked = false; checkBoxPCG.Checked = false; checkBoxPCGR.Checked = false; checkBoxPCL.Checked = false;
                checkBoxPCLR.Checked = false; checkBoxPennyIncrementOptions.Checked = false; checkBoxPlus3To100M.Checked = false; checkBoxPostmarketMovers.Checked = false;
                checkBoxPreferredStocks.Checked = false; checkBoxPremarketMovers.Checked = false; checkBoxSessionOptions.Checked = false; checkBoxSettledFutures.Checked = false;
                checkBoxSizzlingStocks.Checked = false; checkBoxUpcomingDvd.Checked = false; checkBoxUpcomingEarnings.Checked = false; checkBoxUpcomingSplits.Checked = false;
                checkBoxVolatilityIncrease.Checked = false; checkBoxWeeklys.Checked = false; checkBoxNetLossNyse.Checked = false;


            }
        }
    }
}
