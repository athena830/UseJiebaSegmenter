using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UseJiebaSegmenter
{
    public partial class Calculate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            JiebaService.onStartCalculate("FB");
            //JiebaService.onRunCalculate();
            //double total = JiebaService.getFBScoreTotal();

            //Label2.Text = "總共有 " + total.ToString() + " 筆資料。";
            //Label3.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //Chart_data = JiebaService.onCreatePieChart();


        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            JiebaService.onStartCalculate("RSS");
        }
    }
}