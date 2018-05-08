// USING LLAMADOS (using System.Data.SqlClient;)
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
// FORM1 ---> lOGIN, FORM2 ---> PANEL DE CONTROL, FORM3 ---> CAMBIO DE CONTRASEÑA, FORM4 ---> VER BASE DE DATOS
namespace BuscamosTuRepuesto
{
    public partial class Form1 : Form
    {
        //VARIABLES GLOBALES
        SqlConnection Connection;
        SqlCommand Command;
        SqlDataReader Reader;
        byte cont = 0;
        public Form1()
        {
            InitializeComponent();
            linkLabel1.Visible = false;
        }
        //CONEXION
        private void Form1_Load(object sender, EventArgs e)
        {
            Connection = new SqlConnection("Data Source=.;Initial Catalog=DataBuscamosTuRepuesto;Integrated Security=True");
            Command = Connection.CreateCommand();
        }
        // COMPROBAR EL USUARIO Y CONTRASEÑA
        private void button1_Click(object sender, EventArgs e)
        {
            try
                {
                   Command.CommandText = "SELECT * FROM Login WHERE Usuario ='" + textBox1.Text + "' AND Contraseña ='"+ textBox2.Text +"';";
                   Command.CommandType = CommandType.Text;
                   Connection.Open();
                   Reader = Command.ExecuteReader();
                   if (Reader.HasRows)
                   {
                   MessageBox.Show("Bienvenido al Sistema " + textBox1.Text, "Usuario Autorizado",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    cont = Convert.ToByte(0);
                    //LLAMADA DEL FORMULARIO 2
                    this.Hide();
                    Form2 Sistem = new Form2();
                    Sistem.Show();
                }
                   else
                   {
                   MessageBox.Show("Contraseña invalida","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                   cont = Convert.ToByte(cont+1);
                }
                   if(cont==3)
                   {
                   linkLabel1.Visible = true;
                    button1.Enabled= false;
                   }
               }
               catch (Exception)
               {
                    throw;
               }
               finally
               {
                  if (Connection != null)
                  {
                       Connection.Close();
                   }
               }
           }
        //LLAMADA DEL FORMULARIO 3
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form3 Reset = new Form3();
            Reset.Show();
        }
        //SALIR DE LA APP
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
   }
