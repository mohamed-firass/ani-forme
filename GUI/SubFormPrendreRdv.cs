﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class SubFormPrendreRdv : Form
    {
        public SubFormPrendreRdv()
        {
            InitializeComponent();
            I18N();
        }

        public void I18N()
        {
            this.Text = GUI.Lang.SUBFORM_PRENDRERDV_TITLE;
            this.buttonCancel.Text = GUI.Lang.SUBFORM_PRENDRERDV_BTN_CANCEL;
            this.buttonDelete.Text = GUI.Lang.SUBFORM_PRENDRERDV_BTN_DELETE;
            this.buttonSubmit.Text = GUI.Lang.SUBFORM_PRENDRERDV_BTN_SUBMIT;
            this.groupBoxClient.Text = GUI.Lang.SUBFORM_PRENDRERDV_GB_CLIENT;
            this.groupBoxDate.Text = GUI.Lang.SUBFORM_PRENDRERDV_GB_DATE;
            this.groupBoxVeto.Text = GUI.Lang.SUBFORM_PRENDRERDV_GB_VETO;
        }
        
        private void SubFormPrendreRdv_Load(object sender, EventArgs e)
        {
            UpdateContent();
        }
        
        private void numericUpDownMin_ValueChanged(object sender, EventArgs e)
        {
            if (this.numericUpDownMin.Value == 60)
                this.numericUpDownMin.Value = 0;
            if (this.numericUpDownMin.Value == -1)
                this.numericUpDownMin.Value = 59;
        }

        private void numericUpDownHeure_ValueChanged(object sender, EventArgs e)
        {
            if (this.numericUpDownMin.Value == 24)
                this.numericUpDownMin.Value = 0;
            if (this.numericUpDownMin.Value == -1)
                this.numericUpDownMin.Value = 23;
        }

        private void buttonAddClient_Click(object sender, EventArgs e)
        {
            SubFormDossierClientAnimal frm = new SubFormDossierClientAnimal();
            frm.CreateMode = true;
            frm.ShowDialog();
        }

        private void buttonAddAnimal_Click(object sender, EventArgs e)
        {
            //TODO Ouverture de la subform Animaux en mode ajout avec le client id en parametre
        }

        private void comboBoxVeterianire_SelectedIndexChanged(object sender, EventArgs e)
        {
            BO.Veterinaires veto = (BO.Veterinaires)this.comboBoxVeterianire.SelectedItem;
            this.dataGridViewAgenda.DataSource = BLL.AgendaMgr.GetAll(veto);
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            BO.Veterinaires veto = (BO.Veterinaires)this.comboBoxVeterianire.SelectedItem;
            BO.Animaux animal = (BO.Animaux)this.comboBoxAnimal.SelectedItem;
            DateTime date = new DateTime(
                this.dateTimePicker1.Value.Year,
                this.dateTimePicker1.Value.Month,
                this.dateTimePicker1.Value.Day,
                (int)this.numericUpDownHeure.Value,
                (int)this.numericUpDownMin.Value,
                0);

            BO.Agenda agenda = new BO.Agenda();
            agenda.Veterinaires = veto;
            agenda.DateRdv = date;
            agenda.Animal = animal;

            BLL.AgendaMgr.Add(agenda);
            UpdateContent();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            BO.Agenda agenda = (BO.Agenda)this.dataGridViewAgenda.SelectedCells[0].OwningRow.DataBoundItem;
            BLL.AgendaMgr.Delete(agenda);
            UpdateContent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            BO.Clients client = (BO.Clients)this.comboBoxClient.SelectedItem;
            this.comboBoxAnimal.DataSource = BLL.AnimauxMgr.GetAllByClient(client, false);
        }

        public void UpdateContent()
        {
            this.comboBoxClient.DataSource = BLL.ClientsMgr.GetAll();
            this.comboBoxVeterianire.DataSource = BLL.VeterinairesMgr.GetAll();
            this.dataGridViewAgenda.DataSource = BLL.AgendaMgr.GetAll();
        }

        private void buttonUrgence_Click(object sender, EventArgs e)
        {
            BO.Veterinaires veto = (BO.Veterinaires)this.comboBoxVeterianire.SelectedItem;
            BO.Animaux animal = (BO.Animaux)this.comboBoxAnimal.SelectedItem;
            DateTime date = DateTime.Now;

            BO.Agenda agenda = new BO.Agenda();
            agenda.Veterinaires = veto;
            agenda.DateRdv = date;
            agenda.Animal = animal;

            BLL.AgendaMgr.Add(agenda);
            UpdateContent();
        }
    }
}
