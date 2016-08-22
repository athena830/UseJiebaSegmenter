using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UseJiebaSegmenter
{
    public partial class FB_List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    ListView1.DataBind();
        //}

        //protected void Button3_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("CorrectList.aspx");
        //}
        protected void Button5_Command(object sender, CommandEventArgs e)
        {
            int score = Int32.Parse(e.CommandArgument.ToString());
            JiebaService.updFBMessages(score, 2, "ALL");
            ListView1.DataBind();
        }
    }
}