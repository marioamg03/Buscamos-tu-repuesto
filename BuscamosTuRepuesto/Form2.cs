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
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Data.SqlClient;
// FORM1 ---> lOGIN, FORM2 ---> PANEL DE CONTROL, FORM3 ---> CAMBIO DE CONTRASEÑA, FORM4 ---> VER BASE DE DATOS
namespace BuscamosTuRepuesto
{
    public partial class Form2 : Form
    {
        //VARIABLES GLOBALES
        DataTable dt;
        SqlConnection Connection;
        SqlCommand Command;
        SqlDataReader Reader;
        //INICIO
        public Form2()
        {
            InitializeComponent();
        }

        //CONEXION
        private void Form2_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            Connection = new SqlConnection("Data Source=.;Initial Catalog=DataBuscamosTuRepuesto;Integrated Security=True");
            Command = Connection.CreateCommand();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            recibir();
        }

         //////////////////////////////////////////////////////////////
        ////                                                        ////
       //// --------> RECEPCION DE MENSAJES EN LA BANDEJA <------- ////
      ////                                                        ////
       //////////////////////////////////////////////////////////////                                                        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < dt.Rows.Count && e.RowIndex >= 0)
            {
                webBrowser1.DocumentText = dt.Rows[e.RowIndex]["Body"].ToString();
            }
        }
        private void recibir()
        {
            try
            {
                Outlook._Application _app = new Outlook.Application();
                Outlook._NameSpace _ns = _app.GetNamespace("MAPI");
                Outlook.MAPIFolder inbox = _ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
                _ns.SendAndReceive(true);
                dt = new DataTable("Inbox");
                dt.Columns.Add("Subject", typeof(string));
                dt.Columns.Add("Sender", typeof(string));
                dt.Columns.Add("Body", typeof(string));
                dt.Columns.Add("Date", typeof(string));
                dataGridView1.DataSource = dt;
                foreach (Outlook.MailItem item in inbox.Items)
                dt.Rows.Add(new object[] { item.Subject, item.Sender, item.HTMLBody, item.SentOn.ToLongDateString() + " " + item.SentOn.ToLongTimeString() });
            }

            catch (Exception ex)
            {
                //ERROR
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // LIMPIAR
        public void Clean()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox1.Focus();
        }
        //INSERTAR
        private void button2_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text == "") || (textBox2.Text == "") || (textBox3.Text == "") || (textBox4.Text == "") || (textBox5.Text == "") || (textBox6.Text == "") || (textBox7.Text == ""))
            {
                MessageBox.Show("Por favor llene los campos de forma correcta", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                try
                {
                    Command.CommandText = "INSERT INTO Clientes (Nombre, Apellido, Cedula, Telefono, Correo, Historial) VALUES('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "-" + textBox5.Text + "', '" + textBox6.Text + "', '" + textBox7.Text + "')";
                    Command.CommandType = CommandType.Text;
                    Connection.Open();
                    Command.ExecuteNonQuery();
                    MessageBox.Show("El cliente se agrego a la base de datos de forma exitosa", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clean();
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
        }
        //BUSCAR
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Por favor llene el campo cedula para poder iniciar la busqueda", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                button3.Enabled = true;
                button4.Enabled = true;
                try
                {
                    Command.CommandText = "SELECT * FROM Clientes WHERE Cedula='" + textBox3.Text + "'";
                    Command.CommandType = CommandType.Text;
                    Connection.Open();
                    Reader = Command.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            textBox1.Text = Reader["Nombre"].ToString();
                            textBox2.Text = Reader["Apellido"].ToString();
                            textBox4.Text = Reader["Telefono"].ToString().Substring(0,4); //MID
                            textBox5.Text = Reader["Telefono"].ToString().Substring(5,7); //MID
                            textBox6.Text = Reader["Correo"].ToString();
                            textBox7.Text = Reader["Historial"].ToString();
                        }
                        groupBox3.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("El cliente no existe dentro de la base de datos", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
        }
        //MODIFICAR
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Command.CommandText = "UPDATE Clientes SET Nombre='" + textBox1.Text + "', Apellido='" + textBox2.Text + "', Telefono='" +textBox4.Text + "-" + textBox5.Text + "', Correo='" + textBox6.Text + "', Historial='" + textBox7.Text + "' WHERE Cedula ='" + textBox3.Text + "'";
                Command.CommandType = CommandType.Text;
                Connection.Open();
                Command.ExecuteNonQuery();
                MessageBox.Show("Los datos del cliente se han modificado de forma exitosa", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        //ELIMINAR
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Command.CommandText = "DELETE FROM Clientes WHERE Cedula= '" + textBox3.Text + "'";
                Command.CommandType = CommandType.Text;
                Connection.Open();
                Command.ExecuteNonQuery();
                MessageBox.Show("Los datos del cliente se han eliminado correctamente", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        //CORREO
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                //CREAR EL OBJETO CORREO
                Outlook._Application _app = new Outlook.Application();
                Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);

                //DESTINATARIO
                mail.To = textBox8.Text;
                //ASUSNTO
                mail.Subject = textBox9.Text;
                //CUERPO
                mail.HTMLBody = "<h1>" + textBox10.Text + "</h1>";
                //IMPORTANCIA
                mail.Importance = Outlook.OlImportance.olImportanceHigh;
                //ENVIAR
                ((Outlook._MailItem)mail).Send();
                //VERIFICACION
                MessageBox.Show("Tu correo ha sido enviado satisfactoriamente", "Correo enviado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MODIFICAR CONTRASEÑA EN LA BASE DE DATOS
            }
            catch (Exception ex)
            {
                //ERROR
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //MANEJO DE INTERFACE
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
            groupBox3.Visible = false;
            Clean();
            
        
        }
        //MANEJO DE INTERFACE
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = true;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = true;
        }
        //MANEJO DE INTERFACE
        private void button7_Click(object sender, EventArgs e)
        {
            Clean();
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = true;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = true;
        }
        //LLAMADA DEL FORMULARIO 4
        private void button8_Click(object sender, EventArgs e)
        {
           this.Hide();
            Form4 Sistem = new Form4();
            Sistem.Show();
        }
        //SALIR DE LA APP
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
