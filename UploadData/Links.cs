using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UploadData
{
    public partial class Links : Form
    {
        public Links()
        {
            InitializeComponent();
        }

        private void Links_Load(object sender, EventArgs e)
        {
            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = "https://www.officeholidays.com/countries/usa/2019";
            linkUSAHols.Links.Add(link);

            LinkLabel.Link link1 = new LinkLabel.Link();
            link1.LinkData = "https://www.statutoryholidays.com/2019.php";
            linkCanadaHols.Links.Add(link1);

            LinkLabel.Link link2 = new LinkLabel.Link();
            link2.LinkData = "https://www.calendarlabs.com/holidays/india/2019";
            linkIndiaHols.Links.Add(link2);

            //News
            LinkLabel.Link link3 = new LinkLabel.Link();
            link3.LinkData = "https://economictimes.indiatimes.com/";
            linkIndiaEconomicTimes.Links.Add(link3);

            LinkLabel.Link link4 = new LinkLabel.Link();
            link4.LinkData = "https://www.moneycontrol.com/";
            linkMoneyControl.Links.Add(link4);

            LinkLabel.Link link14 = new LinkLabel.Link();
            link14.LinkData = "https://www.bloomberg.com/canada";
            linkbloombergCA.Links.Add(link14);

            LinkLabel.Link link5 = new LinkLabel.Link();
            link5.LinkData = "https://www.bnnbloomberg.ca/";
            linkbnnbloomberg.Links.Add(link5);

            LinkLabel.Link link6 = new LinkLabel.Link();
            link6.LinkData = "https://www.foxbusiness.com/";
            linkfoxbusiness.Links.Add(link6);

            LinkLabel.Link link7 = new LinkLabel.Link();
            link7.LinkData = "https://www.wsj.com/";
            linkwsj.Links.Add(link7);

            LinkLabel.Link link8 = new LinkLabel.Link();
            link8.LinkData = "https://www.ft.com/";
            linkft.Links.Add(link8);

            LinkLabel.Link link9 = new LinkLabel.Link();
            link9.LinkData = "https://earth.google.com/web/@0,0,-24018.82718741a,36750128.22569847d,35y,0h,0t,0r/data=CgAoAQ?authuser=0";
            linkgoogleearth.Links.Add(link9);

            LinkLabel.Link link10 = new LinkLabel.Link();
            link10.LinkData = "https://translate.google.ca/?hl=en&authuser=0";
            linkgoogletranslate.Links.Add(link10);

            LinkLabel.Link link11 = new LinkLabel.Link();
            link11.LinkData = "https://news.google.com/?tab=ln&hl=en-CA&gl=CA&ceid=CA:en";
            linkgooglenews.Links.Add(link11);

            LinkLabel.Link link12 = new LinkLabel.Link();
            link12.LinkData = "https://calendar.google.com/calendar/r?tab=rc&pli=1";
            linkgooglecalendar.Links.Add(link12);

            LinkLabel.Link link13 = new LinkLabel.Link();
            link13.LinkData = "https://www.google.ca/maps?hl=en&tab=cl&authuser=0";
            linkgooglemaps.Links.Add(link13);

            LinkLabel.Link link15 = new LinkLabel.Link();
            link15.LinkData = "https://www.iiroc.ca/Pages/default.aspx";
            linkLabelIIROC.Links.Add(link15);

            LinkLabel.Link link16 = new LinkLabel.Link();
            link16.LinkData = "https://www.bseindia.com/";
            linkLabelBSEIndia.Links.Add(link16);

            LinkLabel.Link link17 = new LinkLabel.Link();
            link17.LinkData = "https://www.nseindia.com/";
            linkLabelNSEIndia.Links.Add(link17);

            LinkLabel.Link link18 = new LinkLabel.Link();
            link18.LinkData = "https://www.nyse.com/index";
            linkLabelNYSE.Links.Add(link18);

            LinkLabel.Link link19 = new LinkLabel.Link();
            link19.LinkData = "https://www.nasdaq.com/";
            linkLabelNasdaq.Links.Add(link19);

            LinkLabel.Link link20 = new LinkLabel.Link();
            link20.LinkData = "https://www.tsx.com/";
            linkLabelTSX.Links.Add(link20);

            LinkLabel.Link link21 = new LinkLabel.Link();
            link21.LinkData = "https://www.google.ca/finance";
            linkLabel1.Links.Add(link21);

        }

        private void linkUSAHols_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkCanadaHols_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkIndiaHols_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkIndiaEconomicTimes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkMoneyControl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkNDTVBusiness_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkbnnbloomberg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkbloombergCA_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkfoxbusiness_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkft_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkwsj_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkgoogleearth_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkgoogletranslate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkgooglenews_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkgooglecalendar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkgooglemaps_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void LinkLabelIIROC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void LinkLabelBSEIndia_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void LinkLabelNSEIndia_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void LinkLabelNYSE_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void LinkLabelNasdaq_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void LinkLabelTSX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }
    }
}
