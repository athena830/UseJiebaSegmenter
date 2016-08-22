using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UseJiebaSegmenter
{
    public partial class RSS_Detail : System.Web.UI.Page
    {
        public static int score { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["score"] != null)
                {
                    score = Int32.Parse(Request["score"]);
                    Hidden_score.Value = score.ToString();
                    //JiebaStart();
                }
                else
                {
                    Response.Redirect("RSS_List.aspx");
                }
            }
        }
        protected void LinkButton1_Command(object sender, CommandEventArgs e)
        {
            var btn = (LinkButton)sender;
            if (btn.Attributes["disabled"] == null)
            {
                var id = e.CommandArgument.ToString();
                JiebaService.updFBMessages(score, 1, id);
                string cid = btn.UniqueID.Substring(0, btn.UniqueID.Length - 1);
                LinkButton btn1 = (LinkButton)Page.FindControl(cid + "1");
                LinkButton btn2 = (LinkButton)Page.FindControl(cid + "2");
                btn1.Attributes.Add("disabled", "true");
                btn2.Attributes.Add("disabled", "true");
                btn1.CssClass = "btn btn-warning";
                btn2.CssClass = "btn btn-warning";
            }
        }
        protected void LinkButton2_Command(object sender, CommandEventArgs e)
        {
            var btn = (LinkButton)sender;
            if (btn.Attributes["disabled"] == null)
            {
                var id = e.CommandArgument.ToString();
                JiebaService.updFBMessages(score, 2, id);
                string cid = btn.UniqueID.Substring(0, btn.UniqueID.Length - 1);
                LinkButton btn1 = (LinkButton)Page.FindControl(cid + "1");
                LinkButton btn2 = (LinkButton)Page.FindControl(cid + "2");
                btn1.Attributes.Add("disabled", "true");
                btn2.Attributes.Add("disabled", "true");
                btn1.CssClass = "btn btn-warning";
                btn2.CssClass = "btn btn-warning";
            }
        }
        private void JiebaStart()
        {
            var data = JiebaService.onStart(2, score);
            if (data != null)
            {
                //var num = JiebaService.getFBMessagesNumbers();
                //Label2.Text = "成功。目前還有 " + num.ToString() + " 筆尚未處理。";
                ListView_result.DataSource = data;
                ListView_result.DataBind();
            }
            else
            {
                //Label2.Text = "100筆內沒有>=4分的資料，請重新整理。";
            }

        }
    }
}