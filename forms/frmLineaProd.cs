﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistema_Inventarios
{
    public partial class frmLineaProd : Form
    {
        SQL sql = new SQL();
        public SqlDataAdapter bdLineas;
        public DataSet tbLineas;
        public DataRow regLineas;
        
        int m = 0;
        public frmLineaProd()
        {
            InitializeComponent();
        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            m = 0;
            showData();
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            m = BindingContext[tbLineas, "Lineas"].Count - 1;
            showData();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmLineaProd_Load(object sender, EventArgs e)
        {
            sql.connect();
            showData();
        }

        void showData()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Lineas", sql.getConn());
            bdLineas = new SqlDataAdapter(cmd);
            tbLineas = new DataSet();
            bdLineas.Fill(tbLineas, "Lineas");
            if (m > BindingContext[tbLineas, "Lineas"].Count - 1)
            {
                m -= 1;
            }
            else if (m <= 0)
            {
                m = 0;
            }
            BindingContext[tbLineas, "Lineas"].Position = m;
            regLineas = tbLineas.Tables["Lineas"].Rows[m];
            txtId.Text = Convert.ToString(regLineas["Id"]);
            txtNombre.Text = Convert.ToString(regLineas["Nombre"]);
            txtDescuento.Text = Convert.ToString(regLineas["Descuento"]);
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            m -= 1;
            showData();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            m += 1;
            showData();
        }
        SqlCommand cmd;
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (btnRegistrar.Text == "&Registrar")
            {
                cmd = new SqlCommand("INSERT INTO Lineas VALUES ('',0) ; SELECT SCOPE_IDENTITY()", sql.getConn());
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                btnPrimero.Enabled = false;
                btnUltimo.Enabled = false;
                btnSiguiente.Enabled = false;
                btnAnterior.Enabled = false;
                btnEliminar.Enabled = false;
                btnActualizar.Enabled = false;
                btnSalir.Enabled = false;
                txtNombre.Text = "";
                txtDescuento.Text = "";
                txtId.Text = Convert.ToString(id);
                btnRegistrar.Text = "Aceptar";
            }
            else
            {
                cmd.CommandText = "UPDATE Lineas SET " +
                                  "Nombre='" + txtNombre.Text + "'," +
                                  "Descuento='" + txtDescuento.Text + "' " +
                                  "WHERE Id = " + txtId.Text;
                cmd.ExecuteNonQuery();
                btnPrimero.Enabled = true;
                btnUltimo.Enabled = true;
                btnSiguiente.Enabled = true;
                btnAnterior.Enabled = true;
                btnEliminar.Enabled = true;
                btnActualizar.Enabled = true;
                btnSalir.Enabled = true;
                btnRegistrar.Text = "&Registrar";
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Quieres eliminar esta linea?", "Advertencia", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Lineas WHERE Id = " + txtId.Text, sql.getConn());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Eliminado");
                if (BindingContext[tbLineas, "Lineas"].Position == 0)
                {
                    m += 1;
                }
                else
                {
                    m -= 1;
                }
                showData();
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            cmd.CommandText = "UPDATE Lineas SET " +
                                  "Nombre='" + txtNombre.Text + "'," +
                                  "Descuento='" + txtDescuento.Text + "' " +
                                  "WHERE Id = " + txtId.Text;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Registro Actualizado");
        }
    }
}
