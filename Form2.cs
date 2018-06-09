using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Reminder_1._0
{
    public partial class Form2 : MetroFramework.Forms.MetroForm
    {
        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\HP\Documents\d_reminder.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True");
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel3_Click(object sender, EventArgs e)
        {

        }

        private void metroLink1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 )
            {
                MessageBox.Show(this, "Enter a task description", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = " insert into Table2 values('" + textBox1.Text + "','" + dateTimePicker1.Text + "','" + dateTimePicker2.Text + "')";
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show(this, "Successfully added your task", "Message", MessageBoxButtons.OK, MessageBoxIcon.None);
                this.Close();
            }
        }
    }
}
