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
    public partial class CorrectList : System.Web.UI.Page
    {
        public static string Chart_data { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            ListView_result.DataBind();
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

    }
}