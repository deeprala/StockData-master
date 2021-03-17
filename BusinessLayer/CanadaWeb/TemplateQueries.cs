using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.CanadaWeb
{
   public static class TemplateQueriesCA
    {
        public static string qDate = DateTime.Today.ToString("yyyy-MM-dd");
        public static string q5DaysAgoDate = DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd");

        public static string queryTemplateCADG = @"SELECT DISTINCT(Symbol),  case country when 'Canada' then 'CA' end as country
                                       from[dbo].[CanadaData] WHERE COUNTRY in ('Canada') AND CAT in ('DG')   AND DATE ='" + qDate + "'  ORDER BY SYMBOL ASC";
        public static string queryTemplateCADL = @"SELECT DISTINCT(Symbol), case country when 'Canada' then 'CA' end as country 
                                       from [dbo].[CanadaData] WHERE COUNTRY in ('Canada') AND CAT in ('DL') AND  DATE = '" + qDate + "' ORDER BY SYMBOL ASC";
        public static string queryTemplateCAH = @"SELECT DISTINCT(Symbol), case country when 'Canada' then 'CA' end as country
                                        from [dbo].[CanadaData] WHERE COUNTRY in ('Canada') AND CAT in ('H') AND  DATE = '" + qDate + "' ORDER BY SYMBOL ASC";
        public static string queryTemplateCAL = @"SELECT DISTINCT(Symbol), case country when 'Canada' then 'CA' end as country
                                        from [dbo].[CanadaData] WHERE COUNTRY in ('Canada') AND CAT in ('L')  AND  DATE = '" + qDate + "' ORDER BY SYMBOL ASC";
        public static string queryTemplateCAMA = @"SELECT DISTINCT(Symbol), case country when 'Canada' then 'CA' end as country
                                        from [dbo].[CanadaData] WHERE COUNTRY in ('Canada') AND CAT in ('MA') AND  DATE = '" + qDate + "' ORDER BY SYMBOL ASC";
        public static string queryTemplateCALC = @"SELECT DISTINCT (Symbol), case country when 'Canada' then 'CA' end as country 
                                        from [dbo].[CanadaData]  WHERE COUNTRY in ('Canada') AND CAT in ('LC') AND  DATE = '" + qDate + "'  ORDER BY SYMBOL ASC";
        public static string queryTemplateCAPG = @"SELECT DISTINCT (Symbol), case country when 'Canada' then 'CA' end as country 
                                        from [dbo].[CanadaData]  WHERE COUNTRY in ('Canada') AND CAT in ('PG') AND  DATE = '" + qDate + "'  ORDER BY SYMBOL ASC";
        public static string queryTemplateCAPL = @"SELECT DISTINCT (Symbol), case country when 'Canada' then 'CA' end as country 
                                        from [dbo].[CanadaData]  WHERE COUNTRY in ('Canada') AND CAT in ('PL') AND  DATE = '" + qDate + "'  ORDER BY SYMBOL ASC";


    }

    public static class TemplateQueriesUSA
    {
        public static string qDate = DateTime.Today.ToString("yyyy-MM-dd");
        public static string q5DaysAgoDate = DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd");

        public static string queryTemplateUSDG = @"SELECT DISTINCT(Symbol),  case country when 'United States' then 'US' end as country
                                       from[dbo].[CanadaData] WHERE COUNTRY in ('United States') AND CAT in ('DG')   AND DATE ='" + qDate + "'  ORDER BY SYMBOL ASC";
        public static string queryTemplateUSDL = @"SELECT DISTINCT(Symbol), case country when 'United States' then 'US' end as country 
                                       from [dbo].[CanadaData] WHERE COUNTRY in ('United States') AND CAT in ('DL') AND  DATE = '" + qDate + "' ORDER BY SYMBOL ASC";
        public static string queryTemplateUSH = @"SELECT DISTINCT(Symbol), case country when 'United States' then 'US' end as country
                                        from [dbo].[CanadaData] WHERE COUNTRY in ('United States') AND CAT in ('H') AND  DATE = '" + qDate + "' ORDER BY SYMBOL ASC";
        public static string queryTemplateUSL = @"SELECT DISTINCT(Symbol), case country when 'United States' then 'US' end as country
                                        from [dbo].[CanadaData] WHERE COUNTRY in ('United States') AND CAT in ('L')  AND  DATE = '" + qDate + "' ORDER BY SYMBOL ASC";
        public static string queryTemplateUSMA = @"SELECT DISTINCT(Symbol), case country when 'United States' then 'US' end as country
                                        from [dbo].[CanadaData] WHERE COUNTRY in ('United States') AND CAT in ('MA') AND  DATE = '" + qDate + "' ORDER BY SYMBOL ASC";
        public static string queryTemplateUSLC = @"SELECT DISTINCT (Symbol), case country when 'United States' then 'US' end as country 
                                        from [dbo].[CanadaData]  WHERE COUNTRY in ('United States') AND CAT in ('LC') AND  DATE = '" + qDate + "'  ORDER BY SYMBOL ASC";
        public static string queryTemplateUSPG = @"SELECT DISTINCT (Symbol), case country when 'United States' then 'US' end as country 
                                        from [dbo].[CanadaData]  WHERE COUNTRY in ('United States') AND CAT in ('PG') AND  DATE = '" + qDate + "'  ORDER BY SYMBOL ASC";
        public static string queryTemplateUSPL = @"SELECT DISTINCT (Symbol), case country when 'United States' then 'US' end as country 
                                        from [dbo].[CanadaData]  WHERE COUNTRY in ('United States') AND CAT in ('PL') AND  DATE = '" + qDate + "'  ORDER BY SYMBOL ASC";


    }

    public static class TemplateQueriesAll
    {
        public static string qDate = DateTime.Today.ToString("yyyy-MM-dd");
        public static string q5DaysAgoDate = DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd");

        // 5D CA ALL DIVIDE 710 IF MORE THAN 710 -
        public static string queryTemplateCA5D = @"SELECT DISTINCT (Symbol), case country when 'Canada' then 'CA' end as country 
                                        from [dbo].[CanadaData]  WHERE COUNTRY in ('Canada') AND EX in ('TSX COMP','TSX VENT') AND  DATE BETWEEN  '" + q5DaysAgoDate + "' AND '" + qDate + "'  ORDER BY SYMBOL ASC";
        
        //5D USA ALL DIVIDE 710 IF MORE THAN 710 -
        public static string queryTemplateUS5D = @"SELECT DISTINCT(Symbol),  case country when 'United States' then 'US' end as country
                                       from[dbo].[CanadaData] WHERE COUNTRY in ('United States') AND EX in ('NYSE COMP','NASDAQ' ,'NYSE MKT')  AND DATE BETWEEN  '" + q5DaysAgoDate + "' AND '" + qDate + "'  ORDER BY SYMBOL ASC";

        // TODAY CA D CA ALL DIVIDE 710 IF MORE THAN 710 -
        public static string queryTemplateCA1D = @"SELECT DISTINCT(Symbol), case country when 'Canada' then 'CA' end as country 
                                       from [dbo].[CanadaData] WHERE COUNTRY in ('Canada') AND EX in ('TSX COMP','TSX VENT')  AND  DATE = '" + qDate + "' ORDER BY SYMBOL ASC";
        

        public static string queryTemplateUS1D = @"SELECT DISTINCT(Symbol), case country when 'United States' then 'US' end as country
                                        from [dbo].[CanadaData] WHERE COUNTRY in ('United States') AND EX in ('NYSE COMP','NASDAQ' ,'NYSE MKT') AND  DATE = '" + qDate + "' ORDER BY SYMBOL ASC";
        // US TILLDATE 
        public static string queryTemplateUSAllTillDate = @"SELECT DISTINCT(Symbol), case country when 'United States' then 'US' end as country
                                        from [dbo].[CanadaData] WHERE COUNTRY in ('United States') AND EX in ('NYSE COMP','NASDAQ' ,'NYSE MKT') ORDER BY SYMBOL ASC";
        // CA TILLDATE 
        public static string queryTemplateCAAllTillDate = @"SELECT DISTINCT(Symbol), case country when 'Canada' then 'CA' end as country
                                        from [dbo].[CanadaData] WHERE COUNTRY in ('Canada') AND EX in ('TSX COMP','TSX VENT') ORDER BY SYMBOL ASC";

        // ALL WEB 5 EXS AND CA USA  DISTINCT NR
        public static string queryTemplateAllWebCAUS = @"SELECT DISTINCT (Symbol), case country when 'United States' then 'US' when 'Canada' then 'CA' end as country  
                                        from [dbo].[CanadaData]  WHERE COUNTRY in ('Canada','United States') AND EX in ('TSX COMP','TSX VENT','NYSE COMP','NASDAQ' ,'NYSE MKT')  AND  DATE = '" + qDate + "'  ORDER BY SYMBOL ASC";
       // CA TillDate
        public static string queryTemplateCATillDate = @"SELECT DISTINCT (Symbol), case country when 'Canada' then 'CA' end as country
                                        from [dbo].[CanadaData]  WHERE COUNTRY in ('Canada') AND EX in ('TSX COMP')  ORDER BY SYMBOL ASC";
       // US TillDate
        public static string queryTemplateUSTillDate = @"SELECT DISTINCT (Symbol), case country when 'United States' then 'US' end as country 
                                        from [dbo].[CanadaData]  WHERE COUNTRY in ('United States') AND EX in ('NYSE COMP','NASDAQ' ,'NYSE MKT')  ORDER BY SYMBOL ASC";


    }

}


