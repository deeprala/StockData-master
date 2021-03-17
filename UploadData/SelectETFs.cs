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
    public partial class ETFSelect : Form
    {
        public ETFSelect()
        {
            InitializeComponent();
        }

        public void CheckBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSelectAll.Checked == true)
            {
                checkBoxAAPCGR.Checked = true; checkBoxAAPCLR.Checked = true; checkBoxActiveNasdaq.Checked = true; checkBoxActiveNyse.Checked = true;
                checkBoxAINV.Checked = true; checkBoxMICROFUTURES.Checked = true; checkBoxMP.Checked = true; checkBoxCashFutures.Checked = true;
                checkBoxChangeGainers.Checked = true; checkBoxChangeLosers.Checked = true; checkBoxCompanyProfile.Checked = true; 
                checkBoxNewYearlyHighs.Checked = true; checkBoxREITs.Checked = true; checkBoxEconomicInd.Checked = true; checkBoxEnergy.Checked = true;
                checkBoxEquityRealEstate.Checked = true;  checkBoxRussell1000.Checked = true; checkBoxLossNyse.Checked = true;
                checkBoxEuropeanStyleIndices.Checked = true; checkBoxMinus3To100M.Checked = true; checkBoxETNandClosedEnd.Checked = true; checkBoxMP.Checked = true;
                checkBoxNetGainNasdaq.Checked = true; checkBoxRussellIndices.Checked = true; checkBoxRussellMicroCap.Checked = true; checkBoxFinancials.Checked = true;
                checkBoxFullSessionOptions.Checked = true; 
                 checkBoxFUTURES.Checked = true; checkBoxPlus3To100M.Checked = true; checkBoxNASDAQ100.Checked = true;
                checkBoxPreferredStocks.Checked = true; checkBoxNASDAQCOMP.Checked = true;  checkBoxFX.Checked = true;
                checkBoxSP100.Checked = true; checkBoxMarketMakerMoveStocks.Checked = true; checkBoxMaterials.Checked = true; checkBoxUpcomingSplits.Checked = true;
                checkBoxVolatilityIncrease.Checked = true; checkBoxWeeklys.Checked = true; checkBoxRussellMidCap.Checked = true;



            }
            else
            {
                checkBoxAAPCGR.Checked = false; checkBoxAAPCLR.Checked = false; checkBoxActiveNasdaq.Checked = false; checkBoxActiveNyse.Checked = false;
                checkBoxAINV.Checked = false; checkBoxMICROFUTURES.Checked = false; checkBoxMP.Checked = false; checkBoxCashFutures.Checked = false;
                checkBoxChangeGainers.Checked = false; checkBoxChangeLosers.Checked = false; checkBoxCompanyProfile.Checked = false; 
                checkBoxNewYearlyHighs.Checked = false; checkBoxREITs.Checked = false; checkBoxEconomicInd.Checked = false; checkBoxEnergy.Checked = false;
                checkBoxEquityRealEstate.Checked = false;  checkBoxRussell1000.Checked = false; checkBoxLossNyse.Checked = false;
                checkBoxEuropeanStyleIndices.Checked = false; checkBoxMinus3To100M.Checked = false; checkBoxETNandClosedEnd.Checked = false; checkBoxMP.Checked = false;
                checkBoxNetGainNasdaq.Checked = false; checkBoxRussellIndices.Checked = false; checkBoxRussellMicroCap.Checked = false; checkBoxFinancials.Checked = false;
                checkBoxFullSessionOptions.Checked = false; 
                 checkBoxFUTURES.Checked = false; checkBoxPlus3To100M.Checked = false; checkBoxNASDAQ100.Checked = false;
                checkBoxPreferredStocks.Checked = false; checkBoxNASDAQCOMP.Checked = false;  checkBoxFX.Checked = false;
                checkBoxSP100.Checked = false; checkBoxMarketMakerMoveStocks.Checked = false; checkBoxMaterials.Checked = false; checkBoxUpcomingSplits.Checked = false;
                checkBoxVolatilityIncrease.Checked = false; checkBoxWeeklys.Checked = false; checkBoxRussellMidCap.Checked = false;


            }
        }

        private void CheckBoxInternalsIndicators_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
