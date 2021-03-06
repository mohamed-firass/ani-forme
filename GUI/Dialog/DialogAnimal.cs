﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Dialog
{
    public enum DialogAnimalMode
    {
        ANIMAL,
        CLIENT,
        CREATE,
    }

    public partial class DialogAnimal : Form
    {
        private DialogAnimalMode mode = DialogAnimalMode.CREATE;
        private BO.Animaux animalEdited = null;
        private BO.Clients clientEdited = null;
        private List<BO.Clients> clientsList = new List<BO.Clients>();

        /// <summary>
        /// Mode ajout de la fenètre
        /// </summary>
        public DialogAnimal(BO.Clients client = null)
        {
            InitializeComponent();
            UpdateContent();
            CreateMode(client);
            I18N(); ///!\ DOIT TOUJOURS ETRE APPELER EN DERNIER /!\
        }

        /// <summary>
        /// Mode édition de la fenètre pour un animal
        /// </summary>
        /// <param name="animal"></param>
        public DialogAnimal(BO.Animaux animal)
        {
            InitializeComponent();
            UpdateContent();
            EditMode(animal);
            I18N(); ///!\ DOIT TOUJOURS ETRE APPELER EN DERNIER /!\
        }

        private void I18N()
        {
            this.Text = GUI.Lang.DIALOG_ANIMAL_TITLE;
            this.Text += "-";

            switch (mode)
            {
                case DialogAnimalMode.ANIMAL:
                    this.Text += String.Format(GUI.Lang.DIALOG_ANIMAL_TITLE_EDIT_ANIMAL, animalEdited.NomAnimal);
                    break;

                case DialogAnimalMode.CLIENT:
                    this.Text += String.Format(GUI.Lang.DIALOG_ANIMAL_TITLE_ADD_CLIENT, clientEdited.getFullName());
                    break;

                case DialogAnimalMode.CREATE:
                    this.Text += String.Format(GUI.Lang.DIALOG_ANIMAL_TITLE_CREATE);
                    break;
            }

            this.buttonCancel.Text = GUI.Lang.FORM_DEFAULT_CANCEL;
            this.buttonValidate.Text = GUI.Lang.FORM_DEFAULT_VALIDATE;
            this.buttonMedicalFolder.Text = GUI.Lang.DIALOG_ANIMAL_MEDICAL_FOLDER;

            this.labelCode.Text = GUI.Lang.DIALOG_ANIMAL_LIB_CODE;
            this.labelColor.Text = GUI.Lang.DIALOG_ANIMAL_LIB_COLOR;
            this.labelCustomer.Text = GUI.Lang.DIALOG_ANIMAL_LIB_CUSTOMER;
            this.labelEspece.Text = GUI.Lang.DIALOG_ANIMAL_LIB_ESPECE;
            this.labelName.Text = GUI.Lang.DIALOG_ANIMAL_LIB_NAME;
            this.labelRace.Text = GUI.Lang.DIALOG_ANIMAL_LIB_RACE;
            this.labelTatoo.Text = GUI.Lang.DIALOG_ANIMAL_LIB_TATOO;
        }
        
        #region Methodes

        //================
        //METHODES =======
        //================
   
        /// <summary>
        /// Vérifie l'ensemble des textbox de la fenêtre
        /// </summary>
        private void CheckBox()
        {
            bool canValidate = true;

            //=========================
            //Nom requis
            if (this.textBoxName.TextLength == 0)
            {
                this.textBoxName.BackColor = Color.Red;
                canValidate = false;
            }
            else
                this.textBoxName.BackColor = Color.LightGreen;

            //=========================
            //Client requis
            if (comboBoxCustomer.SelectedItem == null)
            {
                this.comboBoxSexe.BackColor = Color.Red;
                canValidate = false;
            }
            else
            {
                this.comboBoxSexe.BackColor = Color.LightGreen;
            }

            //=========================
            //Sexe requis
            if (comboBoxSexe.SelectedItem == null)
            {
                this.comboBoxSexe.BackColor = Color.Red;
                canValidate = false;
            }
            else
            {
                this.comboBoxSexe.BackColor = Color.LightGreen;
            }

            //=========================
            //Espece
            if (comboBoxEspece.SelectedItem == null)
            {
                this.comboBoxEspece.BackColor = Color.Red;
                canValidate = false;
            }
            else
            {
                this.comboBoxEspece.BackColor = Color.LightGreen;
            }

            //Race
            if (comboBoxRace.SelectedItem == null)
            {
                this.comboBoxRace.BackColor = Color.Red;
                canValidate = false;
            }
            else
            {
                this.comboBoxRace.BackColor = Color.LightGreen;
            }

            //=========================
            //Couleur requis
            if (this.textBoxColor.TextLength == 0)
            {
                this.textBoxColor.BackColor = Color.Red;
                canValidate = false;
            }
            else
                this.textBoxColor.BackColor = Color.LightGreen;

            //=========================
            //Tatoo non requis
            this.textBoxTatoo.BackColor = Color.LightGreen;

            //=========================
            //Activation du bouton
            this.buttonValidate.Enabled = canValidate;
        }

        /// <summary>
        /// Charge les clients, les races and co
        /// </summary>
        private void UpdateContent()
        {
            clientsList = BLL.ClientsMgr.GetAll(false);
            this.comboBoxCustomer.DataSource = clientsList;
            this.comboBoxSexe.DataSource = BLL.AnimauxMgr.GetSexe();
            this.comboBoxEspece.DataSource = BLL.RacesMgr.GetAllEspeces();
        }

        /// <summary>
        /// Permet de passer en mode création
        /// </summary>
        private void CreateMode(BO.Clients client = null)
        {
            animalEdited = null;

            if (client != null)
            {
                clientEdited = clientsList.Find(x => x.CodeClient == client.CodeClient);

                if (clientEdited == null)
                {                    
                    this.Close();
                    MessageBox.Show(GUI.Lang.DIALOG_ANIMAL_CLIENT_ERROR, 
                                    GUI.Lang.FORM_DEFAULT_ERROR_TITLE,   
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Error);
                }
                this.comboBoxCustomer.SelectedItem = clientEdited;
                mode = DialogAnimalMode.CLIENT;
                this.comboBoxCustomer.Enabled = false;
            }

            this.buttonMedicalFolder.Enabled = false; //Mode création pas de bouton de liaison

        }

        /// <summary>
        /// Permet de passer en mode édition
        /// </summary>
        /// <param name="animal"></param>
        private void EditMode(BO.Animaux animal)
        {
            mode = DialogAnimalMode.ANIMAL;
            animalEdited = animal;

            this.textBoxCode.Text = animalEdited.CodeAnimal.ToString();
            this.textBoxColor.Text = animalEdited.Couleur;
            this.textBoxName.Text = animalEdited.NomAnimal;
            this.textBoxTatoo.Text = animalEdited.Tatouage;
            this.comboBoxSexe.SelectedIndex = ((List<Char>)this.comboBoxSexe.DataSource).IndexOf(animalEdited.Sexe, 0);
            this.comboBoxEspece.SelectedIndex = ((List<String>)this.comboBoxEspece.DataSource).IndexOf(animalEdited.Espece, 0);
            this.comboBoxRace.SelectedIndex = ((List<String>)this.comboBoxRace.DataSource).IndexOf(animalEdited.Race, 0);

        }

        #endregion

        #region Evenements
        //========================
        //EVENEMENTS =============
        //========================

        private void buttonMedicalFolder_Click(object sender, EventArgs e)
        {
            if (animalEdited != null)
            {
                SubFormDossierMedical formDossierMedical = new SubFormDossierMedical(animalEdited);
                formDossierMedical.ShowDialog();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
           this.Close();
        }

        private void buttonValidate_Click(object sender, EventArgs e)
        {
            try
            {
                BO.Animaux finalAnimal = animalEdited;

                switch (mode)
                {
                    case DialogAnimalMode.CREATE:
                    case DialogAnimalMode.CLIENT:
                       finalAnimal = BLL.AnimauxMgr.Create(
                            new BO.Animaux
                            {
                                Sexe = (char)comboBoxSexe.SelectedItem,
                                Client = (BO.Clients)comboBoxCustomer.SelectedItem,
                                Couleur = textBoxColor.Text,
                                NomAnimal = textBoxName.Text,
                                Tatouage = textBoxTatoo.Text,
                                Espece = (String)comboBoxEspece.SelectedItem,
                                Race = (String)comboBoxRace.SelectedItem,
                            });
                        break;

                    case DialogAnimalMode.ANIMAL:
                        animalEdited.Sexe = (char)comboBoxSexe.SelectedItem;
                        animalEdited.Client = (BO.Clients)comboBoxCustomer.SelectedItem;
                        animalEdited.Couleur = textBoxColor.Text;
                        animalEdited.NomAnimal = textBoxName.Text;
                        animalEdited.Tatouage = textBoxTatoo.Text;
                        animalEdited.Espece = (String)comboBoxEspece.SelectedItem;
                        animalEdited.Race = (String)comboBoxRace.SelectedItem;
                        if (!BLL.AnimauxMgr.Update(animalEdited))
                            throw new Exception("L'animal n'a pas pu être mis à jour");
                        break;
                }

                MessageBox.Show(String.Format(Lang.DIALOG_ANIMAL_CREATE_UPDATE_SUCCEFULL, finalAnimal.NomAnimal), Lang.FORM_DEFAULT_CREATE_UPDATE_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateContent(); //reload data
                EditMode(finalAnimal); //Mode edition de cet animal
                I18N(); //rafraichis la trad
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                GUI.Lang.FORM_DEFAULT_ERROR_TITLE,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void EventCheckBox(object sender, EventArgs e)
        {
            CheckBox();
        }


        private void comboBoxEspece_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBoxRace.DataSource = BLL.RacesMgr.GetAllRacesByEspece(this.comboBoxEspece.Text);
        }
        #endregion




    }
}
