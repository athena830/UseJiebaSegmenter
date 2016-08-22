using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UseJiebaSegmenter
{
    public partial class RSS_List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button5_Command(object sender, CommandEventArgs e)
        {
            int score = Int32.Parse(e.CommandArgument.ToString());
            JiebaService.updFBMessages(score, 2, "ALL");
            ListView1.DataBind();
        }
    }
}