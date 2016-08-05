﻿/*没有过多检查，可能会有些许bug*/
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
namespace ExtensionLibHelper.FileLibHelper
{
    public class IniHelper
    {
        private string iniFile;
        private Encoding myEncoding = Encoding.UTF8;
        /// <summary>
        /// 构造函数，只传递文件路径
        /// </summary>
        /// <param name="file">文件路径</param>
        public IniHelper(string file)
        {
            this.iniFile = file;
        }
        /// <summary>
        /// 构造函数，传递文件路径和编码格式
        /// </summary>
        /// <param name="file"></param>
        /// <param name="encoding"></param>
        public IniHelper(string file, Encoding encoding)
        {
            this.iniFile = file;
            this.myEncoding = encoding;
        }
        #region 开放接口

        #endregion

        /// <summary>
        /// 批量读取键值对
        /// </summary>
        /// <returns>返回INI配置结构体列表,单独结构可以通过索引获取或设置</returns>
        public List<IniStruct> ReadValues()
        {
            return ReadValues(this.iniFile);
        }
        /// <summary>
        /// 读取指定节点的指定Key的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="section">节点</param>
        /// <returns>值</returns>
        public string ReadValue(string key, string section)
        {
            string comments = "";
            return ReadValue(this.iniFile, key, section, ref comments);
        }
        private string ReadValue(string key, string section, ref string comments)
        {
            if (string.IsNullOrEmpty(this.iniFile)) throw new System.Exception("没有设置文件路径");
            return ReadValue(this.iniFile, key, section, ref comments);
        }
        public static string ReadValue(string file, string key, string section)
        {
            string comments = "";
            return ReadValue(file, key, section, ref comments);
        }
        private static string GetText(string file)
        {
            string content = File.ReadAllText(file);
            if (content.Contains("�"))
            {
                var encode = System.Text.Encoding.GetEncoding("GBK");
                content = File.ReadAllText(file, encode);
            }
            return content;
        }
        private static string ReadValue(string file, string key, string section, ref string comments)
        {
            string valueText = "";
            string content = GetText(file);
            if (!string.IsNullOrEmpty(section)) //首先遍历节点
            {
                MatchCollection matches = new Regex(@"\[\s*(?'section'[^\[\]\s]+)\s*\]").Matches(content);
                if (matches.Count <= 0) return "";
                Match currMatch = null;
                Match tailMatch = null;
                foreach (Match match in matches)
                {
                    string match_section = match.Groups["section"].Value;
                    if (match_section.ToLower() == section.ToLower())
                    {
                        currMatch = match;
                        continue;
                    }
                    else if (currMatch != null)
                    {
                        tailMatch = match;
                        break;
                    }

                }
                valueText = content.Substring(currMatch.Index + currMatch.Length, (tailMatch != null ? tailMatch.Index : content.Length) - currMatch.Index - currMatch.Length);//截取有效值域


            }
            else
                valueText = content;
            string[] lines = valueText.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line) || line == "\r\n" || line.Contains("["))
                    continue;
                string valueLine = line;
                if (line.Contains(";"))
                {
                    string[] seqPairs = line.Split(';');
                    if (seqPairs.Length > 1)
                        comments = seqPairs[1].Trim();
                    valueLine = seqPairs[0];
                }
                string[] keyValuePairs = valueLine.Split('=');
                string line_key = keyValuePairs[0];
                string line_value = "";
                if (keyValuePairs.Length > 1)
                {
                    line_value = keyValuePairs[1];
                }
                if (key.ToLower().Trim() == line_key.ToLower().Trim())
                {
                    return line_value;
                }
            }
            return "";
        }
        private static List<IniStruct> ReadValues(string file)
        {
            System.Collections.Generic.List<IniStruct> iniStructList = new System.Collections.Generic.List<IniStruct>();
            string content = GetText(file);
            System.Text.RegularExpressions.MatchCollection matches = new System.Text.RegularExpressions.Regex(@"\[\s*(?'section'[^\[\]\s]+)\s*\](?'valueContent'[^\[\]]*)").Matches(content);
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                IniStruct iniStruct = new IniStruct();
                string match_section = match.Groups["section"].Value;
                string match_value = match.Groups["valueContent"].Value;
                iniStruct.Section = match_section;

                string[] lines = match_value.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line) || line == "\r\n" || line.Contains("["))
                        continue;
                    string comments = "";//注释
                    string valueLine = line;
                    if (line.Contains(";"))
                    {
                        string[] seqPairs = line.Split(';');
                        if (seqPairs.Length > 1)
                            comments = seqPairs[1].Trim();
                        valueLine = seqPairs[0];
                    }
                    string[] keyValuePairs = valueLine.Split('=');
                    string line_key = keyValuePairs[0];
                    string line_value = "";
                    if (keyValuePairs.Length > 1)
                    {
                        line_value = keyValuePairs[1];
                    }
                    iniStruct.Add(line_key, line_value, comments);
                }
                iniStructList.Add(iniStruct);
            }

            return iniStructList;
        }
        public void Write(string section, string key, string value)
        {
            Write(section, key, value, null);
        }
        private void Write(string section, string key, string value, string comment)
        {
            Write(this.iniFile, section, key, value, comment, this.myEncoding);
        }
        public static void Write(string file, string section, string key, string value, string comment, Encoding encoding)
        {
            bool isModified = false;
            StringBuilder stringBuilder = new StringBuilder();
            string content = GetText(file);
            StringBuilder newValueContent = new StringBuilder();
            #region 写入了节点
            if (!string.IsNullOrEmpty(section))
            {
                string pattern = string.Format(@"\[\s*{0}\s*\](?'valueContent'[^\[\]]*)", section);
                MatchCollection matches = new Regex(pattern).Matches(content);
                if (matches.Count <= 0)
                {
                    stringBuilder.AppendLine(string.Format("[{0}]", section)); //检查节点是否存在
                    stringBuilder.AppendLine(string.Format("{0}={1}{2}", key, value, !string.IsNullOrEmpty(comment) ? (";" + comment) : ""));
                    stringBuilder.AppendLine(content);
                    isModified = true;
                }
                else
                {
                    Match match = matches[0];
                    string valueContent = match.Groups["valueContent"].Value;
                    string[] lines = valueContent.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);

                    newValueContent.AppendLine(string.Format("[{0}]", section));
                    foreach (string line in lines)
                    {
                        if (string.IsNullOrEmpty(line) || line == "\r\n" || line.Contains("["))
                        {
                            continue;
                        }

                        string valueLine = line;
                        string comments = "";
                        if (line.Contains(";"))
                        {
                            string[] seqPairs = line.Split(';');
                            if (seqPairs.Length > 1)
                                comments = seqPairs[1].Trim();
                            valueLine = seqPairs[0];
                        }
                        string[] keyValuePairs = valueLine.Split('=');
                        string line_key = keyValuePairs[0];
                        string line_value = "";
                        if (keyValuePairs.Length > 1)
                        {
                            line_value = keyValuePairs[1];
                        }
                        if (key.ToLower().Trim() == line_key.ToLower().Trim())
                        {
                            isModified = true;
                            newValueContent.AppendLine(string.Format("{0}={1}{2}", key, value, !string.IsNullOrEmpty(comment) ? (";" + comment) : ""));
                        }
                        else
                        {
                            newValueContent.AppendLine(line);
                        }


                    }
                    if (!isModified)
                        newValueContent.AppendLine(string.Format("{0}={1}{2}", key, value, !string.IsNullOrEmpty(comment) ? (";" + comment) : ""));
                    string newVal = newValueContent.ToString();
                    content = content.Replace(match.Value, newVal);
                    stringBuilder.Append(content);

                }
            }
            #endregion
            #region 没有指明节点
            else
            {
                string valueText = "";
                //如果节点为空
                MatchCollection matches = new Regex(@"\[\s*(?'section'[^\[\]\s]+)\s*\](?'valueContent'[^\[\]]*)").Matches(content);
                if (matches.Count > 0)
                {
                    valueText = matches[0].Index > 0 ? content.Substring(0, matches[0].Index) : "";
                    string[] lines = valueText.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
                    foreach (string line in lines)
                    {
                        if (string.IsNullOrEmpty(line) || line == "\r\n" || line.Contains("["))
                        {
                            continue;
                        }

                        string valueLine = line;
                        string comments = "";
                        if (line.Contains(";"))
                        {
                            string[] seqPairs = line.Split(';');
                            if (seqPairs.Length > 1)
                                comments = seqPairs[1].Trim();
                            valueLine = seqPairs[0];
                        }
                        string[] keyValuePairs = valueLine.Split('=');
                        string line_key = keyValuePairs[0];
                        string line_value = "";
                        if (keyValuePairs.Length > 1)
                        {
                            line_value = keyValuePairs[1];
                        }
                        if (key.ToLower().Trim() == line_key.ToLower().Trim())
                        {
                            isModified = true;
                            newValueContent.AppendLine(string.Format("{0}={1}{2}", key, value, !string.IsNullOrEmpty(comment) ? (";" + comment) : ""));
                        }
                        else
                        {
                            newValueContent.AppendLine(line);
                        }


                    }
                    if (!isModified)
                        newValueContent.AppendLine(string.Format("{0}={1}{2}", key, value, !string.IsNullOrEmpty(comment) ? (";" + comment) : ""));
                    string newVal = newValueContent.ToString();
                    content = content.Replace(valueText, newVal);
                    stringBuilder.Append(content);
                }
                else
                {
                    stringBuilder.AppendLine(string.Format("{0}={1}{2}", key, value, !string.IsNullOrEmpty(comment) ? (";" + comment) : ""));
                }
            }
            #endregion
            File.WriteAllText(file, stringBuilder.ToString(), encoding);
        }
    }
    /// <summary>
    /// 键值对的KeyPair类
    /// </summary>
    public class IniStruct : System.Collections.IEnumerable
    {

        private List<string> _commentList;
        public IniStruct()
        {
            this._keyValuePairs = new System.Collections.Generic.SortedList<string, string>();
            _commentList = new System.Collections.Generic.List<string>();
        }
        public string GetComment(string key)
        {
            if (this._keyValuePairs.ContainsKey(key))
            {
                int index = this._keyValuePairs.IndexOfKey(key);
                return this._commentList[index];
            }
            return "";
        }
        public string this[int index]
        {
            get
            {
                if (this._keyValuePairs.Count > index)
                    return this._keyValuePairs.Values[index];
                else return "";
            }
            set
            {
                if (this._keyValuePairs.Count > index)
                    this._keyValuePairs.Values[index] = value;
            }
        }
        public string this[string key]
        {
            get
            {
                if (this._keyValuePairs.ContainsKey(key))
                    return this._keyValuePairs[key];
                else return "";
            }
            set
            {
                if (this._keyValuePairs.ContainsKey(key))
                    this._keyValuePairs[key] = value;
            }
        }
        public string Section { get; set; }
        private System.Collections.Generic.SortedList<string, string> _keyValuePairs;
        public void Add(string key, string value, string commont)
        {
            this._keyValuePairs.Add(key, value);
            this._commentList.Add(commont);
        }
        public override string ToString()
        {
            return string.Format("{0}", this.Section);
        }

        public bool ContainKey(string key)
        {
            return this._keyValuePairs.ContainsKey(key);
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this._keyValuePairs.GetEnumerator();
        }
    }
}