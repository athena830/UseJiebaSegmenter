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
        public static Dictionary<string, int> FilterPoints;
        public static Dictionary<int, string> FilterKeys;
        private static System.Timers.Timer myTimer;
        /// <summary>
        /// 取得FB_message
        /// <param name="type">1: RSS; 2: FB; 3: FB變異點;</param>
        /// <param name="score">分數</param>
        /// </summary>
        /// <returns>斷字結果</returns>
        public static List<Data> onStart(int type, int score)
        {
            getFilterKey();
            if (type == 1)
            {
                DataTable dt = getRSSMessages(score);
                if (dt != null)
                {
                    return RunFB(dt);
                }
                return null;
            }
            else if (type == 2)
            {
                DataTable dt = getFBMessages(score);
                if (dt != null)
                {
                    return RunFB(dt);
                }
                return null;
            }
            else if (type == 3)
            {
                DataTable dt = getFBCorrectList();
                if (dt != null)
                {
                    return RunFB(dt);
                }
                return null;
            }
            else if (type == 4)
            {
                DataTable dt = getRSSCorrectList();
                if (dt != null)
                {
                    return RunFB(dt);
                }
                return null;
            }
            return null;
        }
        /// <summary>
        /// 計算分數-timer
        /// </summary>
        public static void onRunCalculate()
        {
            try
            {
                int Interval = 60;//60*1000為1分鐘
                myTimer = new System.Timers.Timer(Interval * 1000);
                myTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                myTimer.Enabled = true;
                myTimer.Start();
                //RunRSS();
                //CreatePieChart();
            }
            catch { }

        }
        /// <summary>
        /// 計算分數處理程序
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //JiebaService.onStartCalculate();
            //CreatePieChart();
            if (FilterKeys == null) getFilterKey();
            DataTable dt = getFBMessagesNoScore();
            calculateScore(dt, "FB");

        }

        /// <summary>
        /// 計算分數
        /// </summary>
        public static void onStartCalculate(string type)
        {
            if (FilterKeys == null) getFilterKey();
            if (type.Equals("RSS"))
            {
                DataTable dt = getRSSMessagesNoScore();
                calculateScore(dt, type);
            }
            else if (type.Equals("FB"))
            {
                DataTable dt = getFBMessagesNoScore();
                calculateScore(dt, type);
            }

        }

        /// <summary>
        /// 製造圖表
        /// </summary>
        public static string onCreatePieChart(string type)
        {
            var chart = "";
            if (type == "FB")
            {
                double total = getFBScoreTotal();
                DataTable dt = getFBScorePercent();
                chart=CreatePieChart(total, dt);
            }
            else if (type == "RSS")
            {
                double total = getRSSScoreTotal();
                DataTable dt = getRSSScorePercent();
                chart = CreatePieChart(total, dt);
            }
            return chart;
        }
        /// <summary>
        /// 製造圖表
        /// </summary>
        public static string CreatePieChart(double total, DataTable dt)
        {
            string chart = "";
            //double total = getFBScoreTotal();
            //DataTable dt = getFBScorePercent();
            string data = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int count = Int32.Parse(dt.Rows[i]["number"].ToString());
                double percent = Math.Round(count / total, 4);
                data += "{'score': '" + dt.Rows[i]["score"].ToString() + "','percent': " + percent * 100 + "},";
            }

            if (!string.IsNullOrEmpty(data))
            {
                chart = " [" + data.Substring(0, data.Length - 1) + "]";
            }
            return chart;
        }

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

        /// <summary>
        /// 計算分數
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type">FB; RSS;</param>
        private static void calculateScore(DataTable dt, string type)
        {
            //DataTable FBtable = getFBMessages();
            var posSeg = new PosSegmenter();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, int> Words = new Dictionary<string, int>();
                var s = dt.Rows[i][0].ToString();
                var id = dt.Rows[i][1].ToString();
                try
                {
                    var tokens = posSeg.Cut(s);
                    var tokenList = tokens.Where(token => token.Flag == "ns" || token.Flag == "nr"
                            || token.Flag == "nz" || token.Flag == "n" || token.Flag == "v").ToList();
                    int score = 0;
                    for (int w = 0; w < tokenList.Count; w++)
                    {
                        string word = tokenList[w].Word.ToString();
                        int points = matchTitle(word);
                        if (points > -1)
                        {
                            //score += points;
                            var key = word + "," + points;
                            if (Words.ContainsKey(key))
                            {
                                Words[key] += 1;
                            }
                            else
                            {
                                score += points;
                                Words.Add(key, 1);
                            }
                        }

                    }
                    if (type.Equals("FB"))
                    {
                        updFBMessagesScore(score, id);
                    }
                    else
                    {
                        updRSSMessagesScore(score, id);
                    }
                }
                catch (Exception ex)
                {
                    if (type.Equals("FB"))
                    {
                        updFBMessagesScore(-1, id);
                    }
                    else
                    {
                        updRSSMessagesScore(-1, id);
                    }
                }
            }
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
        /// <summary>
        /// 取得FB_message no score
        /// </summary>
        /// <returns>Message</returns>
        private static DataTable getRSSMessagesNoScore()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                select top 300 RM_Title, RM_ID, RM_KWID from RSS_message where RM_KWID>-1 and RM_Score is null
            ";
            DataTable dt = Persister.Execute(cmd);

            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }
        /// <summary>
        /// 取得分數比例
        /// </summary>
        /// <returns>Message</returns>
        public static DataTable getRSSScorePercent()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                select RM_Score as score, count(*) as number from RSS_message 
                where RM_Score >0 and RM_ChkDate is null
                group by RM_Score
                order by RM_Score desc
                ";
            DataTable dt = Persister.Execute(cmd);

            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }
        /// <summary>
        /// 更新RSS_message score
        /// </summary>
        /// <param name="score">-1: fault; Number>0: success;</param>
        /// <param name="id"></param>
        /// <returns>Message</returns>
        public static void updRSSMessagesScore(int score, string id)
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                update RSS_message set RM_Score=@score
                    where RM_ID=@id
            ";
            cmd.Parameters.Add("@score", SqlDbType.Int).Value = score;
            cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;
            Persister.ExecuteNonQuery(cmd);
        }
        /// <summary>
        /// 取得FB_message
        /// </summary>
        /// <returns>Message</returns>
        private static DataTable getRSSMessages(int score)
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                select RM_Title, RM_ID, RM_KWID from RSS_message where RM_Score=@score and RM_ChkDate is null
        ";
            cmd.Parameters.Add("@score", SqlDbType.Int).Value = score;
            DataTable dt = Persister.Execute(cmd);

            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }
        /// <summary>
        /// 取得比例總數
        /// </summary>
        /// <returns>Message</returns>
        public static int getRSSScoreTotal()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                select count(*) from RSS_message 
                where RM_Score >0 and RM_ChkDate is null
                ";
            DataTable dt = Persister.Execute(cmd);
            int total = Int32.Parse(dt.Rows[0][0].ToString());
            return total;
        }
        /// <summary>
        /// 取得RSS_message 疑似為變異點
        /// </summary>
        /// <returns>Message</returns>
        private static DataTable getRSSCorrectList()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                select RM_Title, RM_ID, RM_KWID from RSS_message where RM_Yesorno=1
                order by RM_ID 
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
        private static List<Data> RunFB(DataTable FBtable)
        {
            //DataTable FBtable = getFBMessages();
            //var segmenter = new JiebaSegmenter();
            var posSeg = new PosSegmenter();
            List<Data> list = new List<Data>();
            for (int i = 0; i < FBtable.Rows.Count; i++)
            {
                Dictionary<string, int> Words = new Dictionary<string, int>();
                var s = FBtable.Rows[i][0].ToString();
                var id = FBtable.Rows[i][1].ToString();
                //var tokens = segmenter.Cut(s);
                try
                {
                    var tokens = posSeg.Cut(s);
                    Data data = new Data();
                    data.ID = id;
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
                        int points = matchTitle(word);
                        if (points > -1)
                        {
                            //score += points;
                            var key = word + "," + points;
                            if (Words.ContainsKey(key))
                            {
                                Words[key] += 1;
                            }
                            else
                            {
                                score += points;
                                Words.Add(key, 1);
                            }
                        }

                    }
                    var dicSort = from objDic in Words orderby objDic.Value descending select objDic;

                    data.Memo = JsonConvert.SerializeObject(Words, Formatting.Indented);
                    data.Memo += "<br/>原：" + FilterKeys[Int32.Parse(FBtable.Rows[i][2].ToString())].ToString();
                    data.Score = score;
                    list.Add(data);

                    //2016/07/25 原本4分改為5分
                    //if (score >= 5)
                    //{
                    //    list.Add(data);
                    //}
                    //else
                    //{
                    //    updFBMessages(score, 2, id);
                    //}


                }
                catch (Exception ex)
                {
                    //updFBMessages(-1, 2, id);
                }
            }
            return list;
            //var dicSort = from objDic in Words orderby objDic.Value descending select objDic;
            //return JsonConvert.SerializeObject(Words, Formatting.Indented);
        }

        /// <summary>
        /// 取得比例總數
        /// </summary>
        /// <returns>Message</returns>
        public static int getFBScoreTotal()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                select count(*) from FB_message 
                where FB_Score >0 and FB_ChkDate is null
                ";
            DataTable dt = Persister.Execute(cmd);
            int total = Int32.Parse(dt.Rows[0][0].ToString());
            return total;
        }
        /// <summary>
        /// 取得分數比例
        /// </summary>
        /// <returns>Message</returns>
        public static DataTable getFBScorePercent()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                select FB_Score as score, count(*) as number from FB_message 
                where FB_Score >0 and FB_ChkDate is null
                group by FB_Score
                order by FB_Score desc
                ";
            DataTable dt = Persister.Execute(cmd);

            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }
        /// <summary>
        /// 取得FB_message
        /// </summary>
        /// <returns>Message</returns>
        private static DataTable getFBMessages(int score)
        {
            var cmd = new SqlCommand();
            //2016/07/25 top 100
            //2016/07/26 top 20->50
//            cmd.CommandText = @"
//                select top 50 Message, id, KWID from FB_message where KWID>-1 and FB_ChkDate is null
//            ";
            //2016/08/08 改以分數取列表
            cmd.CommandText = @"
                select Message, id, KWID from FB_message where FB_Score=@score and FB_ChkDate is null
            ";
            cmd.Parameters.Add("@score", SqlDbType.Int).Value = score;
            DataTable dt = Persister.Execute(cmd);

            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }
        /// <summary>
        /// 取得FB_message no score
        /// </summary>
        /// <returns>Message</returns>
        private static DataTable getFBMessagesNoScore()
        {
            var cmd = new SqlCommand();
            //2016/08/04 先針對有分數的重算分數
            //            cmd.CommandText = @"
            //                select Message, id, KWID from FB_message where KWID>-1 and FB_Score is not null
            //            ";
            cmd.CommandText = @"
                select top 300 Message, id, KWID from FB_message where KWID>-1 and FB_Score is null
            ";
            DataTable dt = Persister.Execute(cmd);

            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }
         /// <summary>
        /// 取得FB_message 疑似為變異點
        /// </summary>
        /// <returns>Message</returns>
        private static DataTable getFBCorrectList()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                select Message, id, KWID from FB_message where FB_Yesorno=1
                order by id 
            ";
            DataTable dt = Persister.Execute(cmd);

            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }
       
        /// <summary>
        /// 取得FB_message還未判斷的數量
        /// </summary>
        /// <returns>Quatity</returns>
        public static int getFBMessagesNumbers()
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                select count(*)  from FB_message where KWID>-1 and FB_ChkDate is null
            ";
            DataTable dt = Persister.Execute(cmd);
            if (dt.Rows.Count > 0)
                return Int32.Parse(dt.Rows[0][0].ToString());
            else
                return 0;
        }

        /// <summary>
        /// 更新FB_message score
        /// </summary>
        /// <param name="score">-1: fault; Number>0: success;</param>
        /// <param name="id"></param>
        /// <returns>Message</returns>
        public static void updFBMessagesScore(int score, string id)
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"
                update FB_message set FB_Score=@score
                    where id=@id
            ";
            cmd.Parameters.Add("@score", SqlDbType.Int).Value = score;
            cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;
            Persister.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 更新FB_message
        /// </summary>
        /// <param name="permission">Yes: 1, No: 2</param>
        /// <param name="score">-1: fault; Number>0: success;</param>
        /// <param name="id">id or All</param>
        /// <returns>Message</returns>
        public static void updFBMessages(int score, int permission, string id)
        {
            var cmd = new SqlCommand();
            if (id == "ALL")
            {
                cmd.CommandText = @"
                    update FB_message set FB_Yesorno=@permission, FB_ChkDate=getDate()
                    where FB_Score=@score and FB_ChkDate is null
                ";
                cmd.Parameters.Add("@score", SqlDbType.Int).Value = score;
                cmd.Parameters.Add("@permission", SqlDbType.Int).Value = permission;
            }
            else
            {
                cmd.CommandText = @"
                    update FB_message set FB_Score=@score, FB_Yesorno=@permission, FB_ChkDate=getDate()
                        where id=@id
                ";
                cmd.Parameters.Add("@score", SqlDbType.Int).Value = score;
                cmd.Parameters.Add("@permission", SqlDbType.Int).Value = permission;
                cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;
            }
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