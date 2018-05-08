// USING LLAMADOS (using System.Data.SqlClient;) (using Outlook = Microsoft.Office.Interop.Outlook; ---> EN ESTE CASO TENEMOS QUE AGREGAR UNA REFERENCIA AL PROYECTO LLAMADA (Outlook 15.0 Object Library))
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
using Outlook = Microsoft.Office.Interop.Outlook;
// FORM1 ---> lOGIN, FORM2 ---> PANEL DE CONTROL, FORM3 ---> CAMBIO DE CONTRASEÑA, FORM4 ---> VER BASE DE DATOS
namespace BuscamosTuRepuesto
{
    public partial class Form3 : Form
    {
        //VARIABLES GLOBALES
        SqlConnection Connection;
        SqlCommand Command;
        public Form3()
        {
            InitializeComponent();
        }
        //CONEXION
        private void Form3_Load(object sender, EventArgs e)
        {
            Connection = new SqlConnection("Data Source=.;Initial Catalog=DataBuscamosTuRepuesto;Integrated Security=True");
            Command = Connection.CreateCommand();
        }
        //ENVIAN CONTRASEÑA NUEVA Y MODIFICAR LA ANTIGUA EN LA BASE DE DATOS
        private void button1_Click(object sender, EventArgs e)
        {
            //GENERAMOS ALEATORIAMENTE LA NUEVA CONTRASEÑA
            Random rnd = new Random();
            string nro1 = Convert.ToString(rnd.Next(0,9)); //GENERAR NUMERO 1
            string nro2 = Convert.ToString(rnd.Next(0,9)); //GENERAR NUMERO 2
            string nro3 = Convert.ToString(rnd.Next(0, 9)); //GENERAR NUMERO 3 
            string letra1 = Convert.ToString(Convert.ToChar(rnd.Next(65,90))); //GENERAR CARACTER 1
            string letra2 = Convert.ToString(Convert.ToChar(rnd.Next(65, 90))); //GENERAR CARACTER 2
            string contraseña = nro1 + letra1 + nro2 + letra2 + nro3; //UNIFICAR NUEVA CONTRASEÑA

            // ENVIO DE CLAVE AL CORREO
            try
            {
                //CREAR EL OBJETO CORREO
                Outlook._Application _app = new Outlook.Application();
                Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);

                //DESTINATARIO
                mail.To = "ibolyelh@outlook.com";
                //ASUSNTO
                mail.Subject = "Contraseña Nueva de " + textBox1.Text;
                //CUERPO
                mail.HTMLBody = "<h1>Tu nueva contraseña es: "+ contraseña +"</h1>";
                //IMPORTANCIA
                mail.Importance = Outlook.OlImportance.olImportanceHigh;
                //ENVIAR
                ((Outlook._MailItem)mail).Send();
                //VERIFICACION
                MessageBox.Show("Tu nueva contraseña ha sido enviada al siguiete correo xxxx@buscamosturepuesto.com por favor comuniquece con el administrador de dicho correo. ", "Contraseña enviada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MODIFICAR CONTRASEÑA EN LA BASE DE DATOS
                try
                {
                    Command.CommandText = "UPDATE Login SET Contraseña='" + contraseña + "' WHERE Usuario='" + textBox1.Text + "'";
                    Command.CommandType = CommandType.Text;
                    Connection.Open();
                    Command.ExecuteNonQuery();
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
            catch (Exception ex)
            {
                //ERROR
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        //LLAMADA DEL FORMULARIO 1
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 Reset = new Form1();
            Reset.Show();
        }
        //SALIR DE LA APP
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
