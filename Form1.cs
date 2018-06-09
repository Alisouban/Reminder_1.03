using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.Media;

namespace Reminder_1._0
{
    public partial class Reminder : MetroFramework.Forms.MetroForm
    {
        string time, curdt;
        
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\HP\Documents\d_reminder.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True");
        
        public Reminder()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            disp_data();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {

        }

        private void metroLink1_Click(object sender, EventArgs e)
        {
            Form2 se = new Form2();
            se.Show();
            RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            reg.SetValue("reminder2", Application.ExecutablePath.ToString());
        }

        public void disp_data()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select task, time from Table2 where date='" + metroDateTime1.Value.ToString("MM-dd-yyyy") + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            metroGrid1.DataSource = dt;
            con.Close();

        }

        private void metroLink3_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select task, time from Table2 where date='" + curdt + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            metroGrid1.DataSource = dt;
            con.Close();
            metroDateTime1.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }

        private void metroLink4_Click(object sender, EventArgs e)
        {
            disp_data();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            var curtim = DateTime.Now.ToString("HH:mm:ss");
            metroLabel1.Text = DateTime.Now.ToString("HH:mm:ss"); ;
            curdt = DateTime.Now.ToString("MM-dd-yyyy");
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "select task, time from Table2 where date='" + curdt + "' AND time='" + curtim + "'";

            var taskn = cmd.ExecuteReader();
            if (taskn.HasRows)
            {
                taskn.Read();
                var taskvl = taskn.GetString(0);
                con.Close();
                SoundPlayer player = new SoundPlayer();
                player.SoundLocation = @"G:\SONGS\MALAYALAM\alarm2.wav";
                player.PlayLooping();
                if (this.WindowState == FormWindowState.Minimized)
                {
                    notifyIcon1.ShowBalloonTip(5000, "Reminder", "'" + taskvl + "' ", ToolTipIcon.None);
                    System.Threading.Thread.Sleep(5000);
                    player.Stop();
                }
                else
                {
                    var v = MetroFramework.MetroMessageBox.Show(this, taskvl, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.None);
                    if (v == DialogResult.OK)
                        player.Stop();
                }
            }
            con.Close();

        }

        private void metroLink2_Click(object sender, EventArgs e)
        {
            if (time != null)
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from Table2 where time='" + time + "'";
                cmd.ExecuteNonQuery();
                con.Close();
                disp_data();
                MessageBox.Show(this, "Selected task has been deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.None);
                time = null;
            }
            else
                MessageBox.Show(this, "Select a task to delete", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void metroGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            time = metroGrid1.SelectedRows[0].Cells["timeDataGridViewTextBoxColumn"].Value.ToString();

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }
    }
}
