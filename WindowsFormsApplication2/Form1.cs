using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-PFVR5JB\SQLEXPRESS;Initial Catalog=mbstu;Integrated Security=True");
        SqlCommand cmd;

        ReportDocument rptdoc = new ReportDocument();
        SqlDataAdapter adpt;

        List<String> demo_type = new List<String>();
        List<String> type = new List<String>();
        List<String> paramtr = new List<String>();
        List<TextBox> txt_list = new List<TextBox>();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            close_btn.Visible = false;

            cmd = new SqlCommand("demo", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlCommandBuilder.DeriveParameters(cmd);

            System.Windows.Forms.Label lbl = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox txt = new System.Windows.Forms.TextBox();


            int i = 0;
            foreach (SqlParameter p in cmd.Parameters)
            {
                paramtr.Add(p.ParameterName);
                demo_type.Add(p.SqlDbType.ToString());

            }


            for (i = 1; i < paramtr.Count; i++)
            {

                type.Add(demo_type[i]);

                Addlabel(paramtr[i], i);
                txt_list.Add(Addtextbox(paramtr[i], i));

            }
           

        }
       
        public System.Windows.Forms.Label Addlabel(string List, int j)
        {
            Label lbl = new Label();
            this.Controls.Add(lbl);
            lbl.Text = paramtr[j];
            lbl.Top = j * 28;
            lbl.Left = 15;
            lbl.Width = 50;
           

            return lbl;
        }
       
        public System.Windows.Forms.TextBox Addtextbox(string List, int j)
        {
            TextBox txt = new TextBox();
            this.Controls.Add(txt);
            txt.Name = paramtr[j];
            
            txt.Top = j * 28;
            txt.Left = 95;
            

            return txt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void add_Click(object sender, EventArgs e)
        {
            rptdoc.Load(@"C:\Users\Hirok\Documents\Visual Studio 2012\Projects\WindowsFormsApplication2\WindowsFormsApplication2\CrystalReport1.rpt");


            adpt = new SqlDataAdapter("demo", conn);
            adpt.SelectCommand.CommandType = CommandType.StoredProcedure;


            int j;
            for (j = 0; j < txt_list.Count; j++)
            {
                if (txt_list[j].Text=="")
                adpt.SelectCommand.Parameters.Add(txt_list[j].Name.ToString(), type[j].ToString()).Value = null;
                else
                adpt.SelectCommand.Parameters.Add(txt_list[j].Name.ToString(), type[j].ToString()).Value = txt_list[j].Text;
               
             
            }
            DataSet dtset = new DataSet();
            adpt.Fill(dtset, "info");
            rptdoc.SetDataSource(dtset);
            crystalReportViewer1.ReportSource = rptdoc;
            crystalReportViewer1.Visible = true;
            crystalReportViewer1.AllowDrop = true;
            close_btn.Visible = true;
           
        }

        

        private void inset_btn_Click(object sender, EventArgs e)
        {
            try
            {

                String query_insert = "INSERT INTO client([ID],[name],[mobile]) VALUES('" + id_box.Text + "','" + name_box.Text + "','" + mobile_box.Text + "')";
                String query_insert2 = "INSERT INTO age([ID],[age],[status]) VALUES('" + id_box.Text + "','" + Convert.ToInt32(age_box.Text) + "','" + status_box.Text + "')";
                String query_insert3 = "INSERT INTO salary([ID],[scale],[salary]) VALUES('" + id_box.Text + "','" + Convert.ToInt32(scale_box.Text) + "','" + Convert.ToDouble(salary_box.Text) + "')";

                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd = new SqlCommand(query_insert, conn);
                int x = cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query_insert2, conn);
                int y = cmd.ExecuteNonQuery();
                cmd = new SqlCommand(query_insert3, conn);
                int z = cmd.ExecuteNonQuery();
                MessageBox.Show("insert successfull");
               
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("error!!!");

            }
            
            
            
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            close_btn.Visible = false;
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            crystalReportViewer1.Visible = false;
        }

        private void close_btn_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            crystalReportViewer1.Visible = false;
        }

        
    }
}
