// USING LLAMADOS
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// FORM1 ---> lOGIN, FORM2 ---> PANEL DE CONTROL, FORM3 ---> CAMBIO DE CONTRASEÑA, FORM4 ---> VER BASE DE DATOS
namespace BuscamosTuRepuesto
{
    public partial class Form4 : Form
    {
        int cont=0;
        public Form4()
        {
            InitializeComponent();
        }
        //BASE DE DATOS DE FORMA VISUAL
        private void Form4_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'dataBuscamosTuRepuestoDataSet.Clientes' Puede moverla o quitarla según sea necesario.
            this.clientesTableAdapter.Fill(this.dataBuscamosTuRepuestoDataSet.Clientes);

            dataGridView1.RowCount = 5;

        }

        private void clientesBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }
        //LLAMADA DEL FORMULARIO 4
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 Sistem = new Form2();
            Sistem.Show();
        }
        //SALIR DE LA APP
        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        //MANEJO DE ADAPTACION
        private void Form4_Resize(object sender, EventArgs e)
        {
            cont = cont + 1;
            
            //// 100

            if (cont == 1)
            {
                dataGridView1.Height = this.Size.Height - 220;
                dataGridView1.Width = this.Size.Width - 40;
                dataGridView1.Location = new Point(12, 95);
                button1.Location = new Point(12,this.Size.Height - 100);
                historialDataGridViewTextBoxColumn.Width = this.Size.Height;
                correoDataGridViewTextBoxColumn.Width = 155;
            }
            else
            {
                if (cont == 2)
                {
                    dataGridView1.Height = 200;
                    dataGridView1.Width = 650;
                    button1.Location = new Point(12,251);
                    dataGridView1.Location = new Point(12, 45);
                    historialDataGridViewTextBoxColumn.Width = 100;
                    correoDataGridViewTextBoxColumn.Width = 100;
                    cont = 0;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Exportar exp = new Exportar();
            exp.ExportarDataGridViewExcel(dataGridView1);
        }
    }
}
