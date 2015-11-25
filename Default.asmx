using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ClassDataUp
{
    /// <summary>
    /// ClassDataUp 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ClassDataUp : System.Web.Services.WebService
    {
        public static string consqlserver = "server=127.0.0.1;database=C-Thing;User ID=C-DataReade;Password=a2120clover;Pooling=true;Max Pool Size=40000;Min Pool Size=0";
        public static string ErrInfo = null;

        //获取errinfo
        public void LookErrinfo()
        {
            string sql = "select * from Server where servername='errinfo'";

            //定义SQL Server连接对象 打开数据库
            SqlConnection con = new SqlConnection(consqlserver);

            con.Open();

            //定义查询命令:表示对数据库执行一个SQL语句或存储过程
            SqlCommand com = new SqlCommand(sql, con);

            //执行查询:提供一种读取数据库行的方式
            SqlDataReader sread = com.ExecuteReader();

            try
            {
                //如果存在用户名和密码正确数据执行进入系统操作
                if (sread.Read())
                {
                    ErrInfo = sread.GetString(1).Trim();
                    con.Close();                    //关闭连接


                }
            }
            catch (Exception)
            {
                con.Close();                    //关闭连接

            }
        }
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public string Up(string errinfo,string ly,string js,string wl,string pv,string hv,string ty,string ip)
        {
            string resulet = "检查结果上传失败！";
            if(errinfo==ErrInfo)
            {

               SqlConnection conn = new SqlConnection(consqlserver);
               conn.Open();
               SqlCommand cmd = new SqlCommand("select count(*) from ClassCheck where Ly='" + ly + "' and  Js='" + js + "' and DateDiff(dd,DateTime,getdate())=0", conn);

               if ("2"==cmd.ExecuteScalar().ToString())
                {          
                     resulet = "此教室今天检查已在存在！";
                     conn.Close();
                }
                else
                {
                    conn.Close();
                    SqlConnection conn2 = new SqlConnection(consqlserver);
                    conn2.Open();
                    string tianjia = "INSERT INTO ClassCheck (Ly,Js,Wl,Pv,Hv,Ty,Ip,DateTime) VALUES('" + ly + "','" + js + "','" + wl + "','" + pv + "','" + hv + "','" + ty + "','" + ip + "','" + DateTime.Now + "')";
                    SqlCommand cmd1 = new SqlCommand(tianjia,conn2);
                    try
                    {
                        int i = cmd1.ExecuteNonQuery(); ;
                        if (i >= 1)
                        {
                            resulet = "检查结果上传成功！";
                            conn2.Close();
                        }
                    }
                    catch
                    {
                        resulet = "检查结果上传失败！";
                        conn2.Close();
                    }
                }
            }
            return resulet;
        }
        [WebMethod]//读取教室数据
        public DataSet ReaderClass(string ly, string errinfo)
        {
            LookErrinfo();
            SqlConnection sqlconn = new SqlConnection(consqlserver);
            //all
            string sql= "SELECT Js FROM  ClassRoom  where Ly='"+ ly +"' ORDER BY Js";

            sqlconn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql, sqlconn);
            DataSet ds = new DataSet();
            if (errinfo == ErrInfo)
            {
                da.Fill(ds, "ClassRoom");
                sqlconn.Close();
            }
            else
            {
                da.Fill(ds, null);
                sqlconn.Close();
            }
            return ds;

        }

        [WebMethod]//读取教室当天状态数据
        public DataSet ReaderCheckClass(string ly, string errinfo)
        {
            LookErrinfo();
            SqlConnection sqlconn = new SqlConnection(consqlserver);
            //all
            string sql = "SELECT Js 教室,Pv,Hv,Ty FROM  ClassCheck  where Ly='" + ly + "' and DateDiff(dd,DateTime,getdate())=0  ORDER BY Js";

            sqlconn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql, sqlconn);
            DataSet ds = new DataSet();
            if (errinfo == ErrInfo)
            {
                da.Fill(ds, "ClassCheck");
                sqlconn.Close();
            }
            else
            {
                da.Fill(ds, null);
                sqlconn.Close();
            }
            return ds;

        }

        [WebMethod]//读取教室前一天状态数据
        public DataSet ReaderAgoCheckClass(string ly, string errinfo)
        {
            LookErrinfo();
            SqlConnection sqlconn = new SqlConnection(consqlserver);
            //all
            string sql = "select Js 教室,Pv,Hv,Ty from  ClassCheck  where Ly='" + ly + "' and DateDiff(dd,DateTime,getdate())=1  ORDER BY Js";
            //string sql1 = "select JS 教室,PV,HV,TY from   ClassRoom where JS not in(select JS from ClassCheck where LY='" + ly + "' and DateDiff(dd,DateTime,getdate())=1) and LY='" + ly + "' ORDER BY JS";

            sqlconn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql, sqlconn);
            DataSet ds = new DataSet();
            if (errinfo == ErrInfo)
            {
                da.Fill(ds, "ClassCheck");
                sqlconn.Close();
            }
            else
            {
                da.Fill(ds, null);
                sqlconn.Close();
            }
            return ds;

        }

        [WebMethod]//读取值班
        public string ReaderZB(string errinfo, string day, string name)
        {
            LookErrinfo();
             if (errinfo == ErrInfo)
            {
            string sql = "select * from Beonduty where " + day + "like '%" + name + "%' ";

            //定义SQL Server连接对象 打开数据库
            SqlConnection con = new SqlConnection(consqlserver);
            con.Open();
            //定义查询命令:表示对数据库执行一个SQL语句或存储过程
            SqlCommand com = new SqlCommand(sql, con);
            //执行查询:提供一种读取数据库行的方式
            SqlDataReader sread = com.ExecuteReader();
                try
                {
                    if (sread.Read())
                    {
                        return sread.GetString(0);

                    }
                    else
                    {
                        con.Close();
                        return "无值班";
                    }
                }
                catch (Exception)
                {
                    con.Close();
                    return "服务器异常！";
                }
            }
            else
            {
                return "无权查询！";
             }
        }
        [WebMethod]//登录
        public string login(string errinfo, string gh, string mpass)
        {
            LookErrinfo();
            if (errinfo == ErrInfo)
            {
                //定义SQL查询语句:用户名 密码
                string sql = "select * from Employee where 工号='" + gh + "' and 密码='" + mpass + "'";

                //定义SQL Server连接对象 打开数据库
                SqlConnection con = new SqlConnection(consqlserver);
                con.Open();
                //定义查询命令:表示对数据库执行一个SQL语句或存储过程
                SqlCommand com = new SqlCommand(sql, con);

                //执行查询:提供一种读取数据库行的方式
                SqlDataReader sread = com.ExecuteReader();

                try
                {
                    //如果存在用户名和密码正确数据执行进入系统操作
                    if (sread.Read())
                    {
                        return "IsLogin," + sread.GetString(1).Trim() + "," + sread.GetString(6).Trim() + "," + sread.GetString(18).Trim();
                    }
                    else
                    {
                        return "NoLogin,Null,Null,Null";
                    }
                }
                catch (Exception)
                {
                    return "NoLogin,Null,Null,Null";
                }
            }
            else
            {
                return "无权,Null,Null,Null";
            }
           
        }

         [WebMethod]//公告
         public string information(string errinfo)
         {
             LookErrinfo();
             if (errinfo == ErrInfo)
             {
                 string sql2 = "select * from Message where DateDiff(dd,date,getdate())=0 ";

                 //定义SQL Server连接对象 打开数据库
                 SqlConnection con2 = new SqlConnection(consqlserver);
                 con2.Open();
                 //定义查询命令:表示对数据库执行一个SQL语句或存储过程
                 SqlCommand com2 = new SqlCommand(sql2, con2);

                 //执行查询:提供一种读取数据库行的方式
                 SqlDataReader sread2 = com2.ExecuteReader();


                 try
                 {
                     //如果存在用户名和密码正确数据执行进入系统操作
                     if (sread2.Read())
                     {
                         return sread2.GetString(2);

                     }

                     else
                     {
                         return "No Information！";
                     }
                 }
                 catch (Exception)
                 {
                     return "服务器异常！";
                 }
                
             }
             else
             {
                 return "No Information！";
             }
         }

         [WebMethod]//签到
         public string Sign(byte[] fileBytes, string fileName,string gh,string name,string bumen,string xinqi,string jiesu)
         {
             string result = null;
               SqlConnection conn = new SqlConnection(consqlserver);
               conn.Open();
               SqlCommand cmd = new SqlCommand("select count(*) from Sign where 工号='" + gh + "' and  星期='" + xinqi + "' and  节数='" + jiesu + "' and DateDiff(dd,时间,getdate())=0", conn);

               if ("2" == cmd.ExecuteScalar().ToString())
               {
                   result = "你此次签到考勤已达两次！";
                   conn.Close();
               }
               else
               {
                   try
                   {
                       MemoryStream memoryStream = new MemoryStream(fileBytes); //1.定义并实例化一个内存流，以存放提交上来的字节数组。   
                       FileStream fileUpload = new FileStream("C:\\签到\\" + fileName, FileMode.Create); ///2.定义实际文件对象，保存上载的文件。   
                       memoryStream.WriteTo(fileUpload); ///3.把内存流里的数据写入物理文件   
                       memoryStream.Close();
                       fileUpload.Close();
                       fileUpload = null;
                       memoryStream = null;
                       SqlConnection conn2 = new SqlConnection(consqlserver);
                       conn2.Open();
                       string tianjia = "INSERT INTO Sign (工号,姓名,部门,时间,星期,节数,ImageId) VALUES('" + gh + "','" + name + "','" + bumen + "','" + DateTime.Now + "','" + xinqi + "','" + jiesu + "','" + fileName + "')";
                       SqlCommand cmd1 = new SqlCommand(tianjia, conn2);
                       try
                       {
                           int i = cmd1.ExecuteNonQuery(); ;
                           if (i >= 1)
                           {
                               result = "签到考勤成功！";
                               conn2.Close();
                           }
                       }
                       catch
                       {
                           result = "签到考勤失败！";
                           conn2.Close();
                       }

                   }
                   catch (Exception)
                   {
                       result = "签到考勤失败！";
                   }
               }
             return result;
         }    

        
    }
}
