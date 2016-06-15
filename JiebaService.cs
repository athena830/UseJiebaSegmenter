using JiebaNet.Segmenter;
using JiebaNet.Segmenter.PosSeg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Timers;
using System.Web;

namespace UseJiebaSegmenter
{
    public class JiebaService
    {
        /// <summary>
        /// 取得FB_message
        /// <param name="type">1: RSS; 2: FB</param>
        /// </summary>
        /// <returns>斷字結果</returns>
        public static List<Data> onStart(int type)
        {
            if (type == 1)
            {

            }
            else if (type == 2)
            {
                return RunFB();
            }
            return null;
        }

        #region RSS處理
        /// <summary>
        /// 取得RSS_message
        /// </summary>
        /// <returns>RM_Title</returns>
        private static DataTable getRSSMessages()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                select RM_Title from RSS_message where RM_KWID>-1
            ";
            DataTable dt = Persister.Execute(cmd);

            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }
        #endregion

        #region FB處理
        /// <summary>
        /// 執行斷詞
        /// </summary>
        /// <returns>斷字結果</returns>
        private static List<Data> RunFB()
        {
            DataTable FBtable = getFBMessages();
            //var segmenter = new JiebaSegmenter();
            var posSeg = new PosSegmenter();
            List<Data> list = new List<Data>();
            for (int i = 0; i < FBtable.Rows.Count; i++)
            {
                var s = FBtable.Rows[i][0].ToString();
                //var segments = segmenter.Cut(row[0].ToString());
                var tokens = posSeg.Cut(s);
                Data data = new Data();
                data.Number = i + 1;
                //data.Sentence = string.Join("/ ", segments);
                data.Sentence = string.Join(" ", tokens.Select(token => string.Format("{0}/{1}", token.Word, token.Flag)));
                list.Add(data);
            }
            return list;
        }

        /// <summary>
        /// 取得FB_message
        /// </summary>
        /// <returns>Message</returns>
        private static DataTable getFBMessages()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                select top 10 Message from FB_message where KWID>-1
            ";
            DataTable dt = Persister.Execute(cmd);

            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }

        public class Data 
        {
            public int Number { get; set; }
            public string Sentence { get; set; }
        }
        #endregion
    }
}