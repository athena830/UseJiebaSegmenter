using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UseJiebaSegmenter
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListView_result.DataSource = JiebaService.onStart(2);
                ListView_result.DataBind();
            }
        }

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    //JiebaSegmenter segmenter = new JiebaSegmenter();
        //    ////var txt = Label1.Text;
        //    //string txt = "7年整治未成 6億形同丟水溝";
        //    //var segments = segmenter.Cut(txt);
        //    //Label2.Text = string.Join("/ ", segments);
        //    //Label2.Text = JiebaService.onStart(2);

        //    ListView_result.DataSource = JiebaService.onStart(2);
        //    ListView_result.DataBind();


        //}

        protected void Btn_Command(object sender, CommandEventArgs e)
        {
            var arg = e.CommandArgument.ToString();
            var cmd= e.CommandName.ToString();
            string[] ary = cmd.Split('|');
            string id = ary[0];
            int score = Int32.Parse(ary[1].ToString());
            if (arg == "yes")
            {
                JiebaService.updFBMessages(score, 1, id);
            }
            else if (arg == "no")
            {
                JiebaService.updFBMessages(score, 2, id);
            }
            ListView_result.DataSource = JiebaService.onStart(2);
            ListView_result.DataBind();
        }
    }
}