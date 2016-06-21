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
        public static string onStart(int type)
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
        private static string RunFB()
        {
            DataTable FBtable = getFBMessages();
            //var segmenter = new JiebaSegmenter();
            var posSeg = new PosSegmenter();
            //List<Data> list = new List<Data>();
            Dictionary<string, int> Words = new Dictionary<string, int>();
            for (int i = 0; i < FBtable.Rows.Count; i++)
            {
                var s = FBtable.Rows[i][0].ToString();
                //var tokens = segmenter.Cut(s);
                var tokens = posSeg.Cut(s);
                //Data data = new Data();
                //data.Number = i + 1;
                //data.Sentence = s;
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
                for (int w = 0; w < tokenList.Count; w++)
                {
                    string word = tokenList[w].ToString();
                    if (Words.ContainsKey(word))
                    {
                        Words[word] += 1;
                    }
                    else
                    {
                        Words.Add(word, 1);
                    }
                }

                //data.Memo = JsonConvert.SerializeObject(Words, Formatting.Indented);
                //list.Add(data);
            }
            //return list;
            var dicSort = from objDic in Words orderby objDic.Value descending select objDic;
            return JsonConvert.SerializeObject(Words, Formatting.Indented);
        }

        /// <summary>
        /// 取得FB_message
        /// </summary>
        /// <returns>Message</returns>
        private static DataTable getFBMessages()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                select top 100 Message from FB_message where KWID>-1
            ";
            DataTable dt = Persister.Execute(cmd);

            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }

        //public class Data 
        //{
        //    public int Number { get; set; }
        //    public string Sentence { get; set; }
        //}
        #endregion
    }
}