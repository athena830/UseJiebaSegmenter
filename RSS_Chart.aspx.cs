using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UseJiebaSegmenter
{
    public partial class RSS_Chart : System.Web.UI.Page
    {
        public static string Chart_data { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page_DataBind();
            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            Page_DataBind();
        }
        private void Page_DataBind()
        {
            double total = JiebaService.getRSSScoreTotal();

            Label2.Text = "總共有 " + total.ToString() + " 筆資料。";
            Label3.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            Chart_data = JiebaService.onCreatePieChart("RSS");
        }
    }
}