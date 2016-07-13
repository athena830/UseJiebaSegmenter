using JiebaNet.Segmenter;
using JiebaNet.Segmenter.PosSeg;
using Newtonsoft.Json;
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
            getFilterKey();
            if (type == 1)
            {

            }
            else if (type == 2)
            {
                return RunFB();
            }
            return null;
        }

        public static Dictionary<string, int> FilterPoints;
        public static Dictionary<int, string> FilterKeys;
        /// <summary>
        /// 讀取過濾關鍵字
        /// </summary>
        public static void getFilterKey()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                select * from Keyword
            ";
            DataTable rssFilter = Persister.Execute(cmd);

            FilterPoints = new Dictionary<string, int>();
            FilterKeys = new Dictionary<int, string>();

            foreach (DataRow keyRow in rssFilter.Rows)
            {
                FilterPoints.Add(keyRow["KW_Keyword"].ToString(), Int32.Parse(keyRow["KW_Point"].ToString()));
                FilterKeys.Add(Int32.Parse(keyRow["KW_ID"].ToString()), keyRow["KW_Keyword"].ToString());
            }
        }

        /// <summary>
        /// 檢查  斷字是否在過濾清單中
        /// </summary>
        /// <returns></returns>
        public static int matchTitle(string message)
        {
            foreach (string aStr in FilterPoints.Keys)
            {

                if (message.IndexOf(aStr) >= 0)
                    return FilterPoints[aStr];
            }
            return -1;
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
                Dictionary<string, int> Words = new Dictionary<string, int>();
                var s = FBtable.Rows[i][0].ToString();
                //var tokens = segmenter.Cut(s);
                var tokens = posSeg.Cut(s);
                Data data = new Data();
                data.ID = FBtable.Rows[i][1].ToString();
                data.Number = i + 1;
                data.Sentence = s;
                //data.Sentence = string.Join("/ ", tokens);
                //data.Sentence = string.Join(" ", tokens
                    //.Where(token => token.Flag == "ns" || token.Flag == "nr"
                    //    || token.Flag == "nz" || token.Flag == "n" || token.Flag == "v")
                    //.Where(token => token.Flag != "p" && token.Flag != "x" && token.Flag != "eng"
                    //    && token.Flag != "uj" && token.Flag != "ul" && token.Flag != "b"
                    //    && token.Flag != "m" && token.Flag != "d"
                    //)
                    //ns:(南洋)；nr:(沙灘):nz:(海浪)；n:名詞；v:動詞
                    //p:介繫詞(在)；x:標點符號；eng:英文字母；uj:(的)；ul:語助詞(了)；b:形容詞(超級、所有)
                    //m:(一份)；d:(親自、就、也、都)
                    //.Select(token => string.Format("{0}/{1}", token.Word, token.Flag))
                    //);
                var tokenList = tokens.Where(token => token.Flag == "ns" || token.Flag == "nr"
                        || token.Flag == "nz" || token.Flag == "n" || token.Flag == "v").ToList();
                int score = 0;
                for (int w = 0; w < tokenList.Count; w++)
                {
                    string word = tokenList[w].Word.ToString();
                    int points=matchTitle(word);
                    if (points > -1)
                    {
                        score += points;
                        var key = word + "," + points;
                        if (Words.ContainsKey(key))
                        {
                            Words[key] += 1;
                        }
                        else
                        {
                            Words.Add(key, 1);
                        }
                    }

                }
                var dicSort = from objDic in Words orderby objDic.Value descending select objDic;

                data.Memo = JsonConvert.SerializeObject(Words, Formatting.Indented);
                data.Memo += "<br/>原：" +FilterKeys[Int32.Parse(FBtable.Rows[i][2].ToString())].ToString();
                data.Score = score;
                list.Add(data);
            }
            return list;
            //var dicSort = from objDic in Words orderby objDic.Value descending select objDic;
            //return JsonConvert.SerializeObject(Words, Formatting.Indented);
        }

        /// <summary>
        /// 取得FB_message
        /// </summary>
        /// <returns>Message</returns>
        private static DataTable getFBMessages()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                select top 10 Message, id, KWID from FB_message where KWID>-1 and FB_ChkDate is null
            ";
            DataTable dt = Persister.Execute(cmd);

            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }

        /// <summary>
        /// 更新FB_message
        /// </summary>
        /// <param name="permission">Yes: 1, No: 2</param>
        /// <param name="score"></param>
        /// <param name="id"></param>
        /// <returns>Message</returns>
        public static void updFBMessages(int score, int permission, string id)
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                update FB_message set FB_Score=@score, FB_Yesorno=@permission, FB_ChkDate=getDate()
                    where id=@id
            ";
            cmd.Parameters.Add("@score", SqlDbType.Int).Value = score;
            cmd.Parameters.Add("@permission", SqlDbType.Int).Value = permission;
            cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;
            Persister.ExecuteNonQuery(cmd);
        }

        public class Data
        {
            public string ID { get; set; }
            public int Number { get; set; }
            public string Sentence { get; set; }
            public string Memo { get; set; }
            public int Score { get; set; }
        }
        #endregion
    }
}