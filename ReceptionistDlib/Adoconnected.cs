using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;//for ADO.net classes
using System.Data;
using System.Configuration;

namespace ReceptionistDlib
{
    public class Adoconnected
    {

        SqlConnection con;
        SqlCommand cmd;
        public Adoconnected()
        {

            //creat connectionn
            con = new SqlConnection();
            con.ConnectionString = "Data Source=VDC01LTC4531;Initial Catalog=ValtechDB;User ID=sa;Password=welcome1@";
            //or 
            //read connection fron config file
           // string constr = ConfigurationManager.ConnectionStrings["sqlconstr"].ConnectionString;
           // con.ConnectionString = constr;
            cmd = new SqlCommand();
            cmd.Connection = con;
        }
        public List<TableDetails> Alltablelist(string status)
        {
            List<TableDetails> tblist = new List<TableDetails>();
            //configure command for select all statement
            cmd.CommandText = "select * from tabledetails where status=@status";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@status", status);
            //open the connection
            con.Open();
            //execute the command
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                TableDetails tb = new TableDetails();
                tb.tablenumber = (int)sdr[0];
                tb.capacity = (int)sdr[1];
                tb.customerId = (int)sdr[2];
                tb.status = sdr[3].ToString();
                tblist.Add(tb);
            }
            sdr.Close();
            con.Close();
            return tblist;
        }
        public List<CustomerDetails> orderlist()
        {
            List<CustomerDetails> tblist = new List<CustomerDetails>();
            //configure command for select all statement
            cmd.CommandText = "select * from customerdetails";
            cmd.CommandType = CommandType.Text;
            //open the connection
            con.Open();
            //execute the command
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                CustomerDetails cd = new CustomerDetails();
                cd.customerId = (int)sdr[0];
                cd.name= sdr[1].ToString();
                cd.phonenumber = (int)sdr[2];
                cd.date = (DateTime)sdr[3];
                cd.starttime= (TimeSpan)sdr[4];
                cd.endtime = (TimeSpan)sdr[5];
                cd.tablenumber = (int)sdr[6];
                cd.status = sdr[7].ToString();
                tblist.Add(cd);
            }
            sdr.Close();
            con.Close();
            return tblist;
        }
        public void placeneworder(CustomerDetails cd)
        {
            //configure command for insert statement\
            cmd.CommandText = "insert into customerdetails values(@cn,@cp,@sd,@st,@et,@tn,@s)";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@cn", cd.name);
            cmd.Parameters.AddWithValue("@cp", cd.phonenumber);
            cmd.Parameters.AddWithValue("@sd", cd.date);
            cmd.Parameters.AddWithValue("@st", cd.starttime);
            cmd.Parameters.AddWithValue("@et", cd.endtime);
            cmd.Parameters.AddWithValue("@tn", cd.tablenumber);
            cmd.Parameters.AddWithValue("@s", cd.status);
            //open connection
            con.Open();
            cmd.ExecuteNonQuery();
            //close connection
            con.Close();
        }
        public void addnewtable(TableDetails td)
        {
            //configure command for insert statement\
            cmd.CommandText = "insert into tabledetails values(@tn,@c,@s)";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@tn", td.tablenumber);
            cmd.Parameters.AddWithValue("@c", td.capacity);
            cmd.Parameters.AddWithValue("@s", td.status);
            //open connection
            con.Open();
            cmd.ExecuteNonQuery();
            //close connection
            con.Close();
        }
        public List<TableDetails> searchtablebasedondate(DateTime dd)
        {
            List<TableDetails> tblist = new List<TableDetails>();
            //configure command for select all statement
            cmd.CommandText = "select * from tabledetails where date=@dd ";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@dd", dd);
            //open the connection
            con.Open();
            //execute the command
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                    TableDetails tb = new TableDetails();
                    tb.tablenumber = (int)sdr[0];
                    tb.capacity = (int)sdr[1];
                    tb.customerId = (int)sdr[2];
                    tb.status = sdr[3].ToString();
                    tblist.Add(tb);
                
            }
            sdr.Close();
            con.Close();
            return tblist;
        }
        public void updatecustomer(int cid, int count)
        {
            try
            {
                cmd.CommandText = "update custometdetails set numberofpeople=@count where customerId=@cid";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@cid", cid);
                cmd.Parameters.AddWithValue("@count", count);
                con.Open();
                int recordeffected = cmd.ExecuteNonQuery();
                if (recordeffected == 0)
                {
                    throw new Exception("cuatomerID not exist");
                }
            }
            catch (SqlException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
        }
        public void Deletecustomer(int cid)
        {
            try
            {
                cmd.CommandText = "delete from customerdetails where customerId=" + cid;
                cmd.CommandType = CommandType.Text;
                // cmd.Parameters.AddWithValue("@ec", emp.Ecode);
                con.Open();
                int recordAffected = cmd.ExecuteNonQuery();
                if (recordAffected == 0)
                {
                    throw new Exception("customerID does not exit");
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            con.Close();

        }
    }
  public class CustomerDetails
    {
        public int customerId { get; set; }
        public string name { get; set; }
        public int phonenumber { get; set; }
        public DateTime date { get; set; }
        public TimeSpan starttime { get; set; }
        public TimeSpan endtime { get; set; }
        public int tablenumber { get; set; }
        public string status { get; set; }
    }
    public class TableDetails
    {
        public int tablenumber { get; set; }
        public int capacity { get; set; }
        public int customerId { get; set; }
        public string status { get; set; }

    }
}
