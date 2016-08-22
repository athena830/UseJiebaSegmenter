using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Timers;

namespace UseJiebaSegmenter
{
    public partial class _Default : System.Web.UI.Page
    {
        public static string Chart_data { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Panel2_DataBind();
            }
        }

        private void Panel2_DataBind()
        {
            double total = JiebaService.getFBScoreTotal();

            Label2.Text = "總共有 " + total.ToString() + " 筆資料。";
            Label3.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            Chart_data = JiebaService.onCreatePieChart();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ListView1.DataBind();
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            Panel2_DataBind();
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("CorrectList.aspx");
        }
        protected void Button5_Command(object sender, CommandEventArgs e)
        {
            int score = Int32.Parse(e.CommandArgument.ToString());
            JiebaService.updFBMessages(score, 2, "ALL");
            ListView1.DataBind();
        }

        //protected void Btn_Command(object sender, CommandEventArgs e)
        //{
        //    var arg = e.CommandArgument.ToString();
        //    var cmd= e.CommandName.ToString();
        //    string[] ary = cmd.Split('|');
        //    string id = ary[0];
        //    int score = Int32.Parse(ary[1].ToString());
        //    if (arg == "yes")
        //    {
        //        JiebaService.updFBMessages(score, 1, id);
        //    }
        //    else if (arg == "no")
        //    {
        //        JiebaService.updFBMessages(score, 2, id);
        //    }
        //    //ListView_result.DataSource = JiebaService.onStart(2);
        //    //ListView_result.DataBind();
        //}
    }
}