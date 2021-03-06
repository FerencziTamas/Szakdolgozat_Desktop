﻿using Forest_Register.modell;
using Forest_Register.repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forest_Register
{
    public partial class FormForestRegister : MetroFramework.Forms.MetroForm
    {
        /// <summary>
        /// Vevőket tartalmazó adattábla
        /// </summary>
        private DataTable vevokDt = new DataTable();

        private void metroButtonVevokBetolt_Click(object sender, EventArgs e)
        {
            DataGridViewFrissiteseVevo();
            DataGridViewVevokBeallit();
            GombokBeallitasaVevo();
            dataGridViewVevok.SelectionChanged += dataGridViewVevok_SelectionChanged;
        }

        private void dataGridViewVevok_SelectionChanged(object sender, EventArgs e)
        {
            if (adatFelvetel)
            {
                GombokBeallitasaKattintaskorVevo();
            }
            if (dataGridViewVevok.SelectedRows.Count == 1)
            {
                metroTextBoxVevoAzon.ReadOnly = true;
                metroButtonVevoFelvetel.Visible = true;
                metroPanelVevoTorolModosit.Visible = true;
                metroPanelVevo.Visible = true;
                metroTextBoxVevoAzon.Text = dataGridViewVevok.SelectedRows[0].Cells[0].Value.ToString();
                metroTextBoxVevoNev.Text = dataGridViewVevok.SelectedRows[0].Cells[1].Value.ToString();
                metroTextBoxVevoCim.Text = dataGridViewVevok.SelectedRows[0].Cells[2].Value.ToString();
                metroTextBoxVevoTechAzon.Text = dataGridViewVevok.SelectedRows[0].Cells[3].Value.ToString();
                metroTextBoxVevoAdoszam.Text = dataGridViewVevok.SelectedRows[0].Cells[4].Value.ToString();
            }
        }

        private void GombokBeallitasaKattintaskorVevo()
        {
            adatFelvetel = false;
            metroButtonUjVevo.Visible = false;
            metroPanelVevoTorolModosit.Visible = true;
            ErrorProviderekTorleseVevo();
        }

        private void ErrorProviderekTorleseVevo()
        {
            errorProviderVevoNev.Clear();
            errorProviderVevoCim.Clear();
            errorProviderTechAzon.Clear();
            errorProviderAdoszam.Clear();
        }

        private void GombokBeallitasaVevo()
        {
            metroPanelVevo.Visible = false;
            metroPanelVevoTorolModosit.Visible = false;
            if(dataGridViewVevok.SelectedRows.Count > 0)
            {
                metroButtonVevoFelvetel.Visible = false;
            }
            else
            {
                metroButtonVevoFelvetel.Visible = true;
            }
        }

        private void DataGridViewVevokBeallit()
        {
            vevokDt.Columns[0].ColumnName = "Azonosító";
            vevokDt.Columns[0].Caption = "Vevő azonosító";
            vevokDt.Columns[1].ColumnName = "Név";
            vevokDt.Columns[1].Caption = "Vevő név";
            vevokDt.Columns[2].ColumnName = "Cím";
            vevokDt.Columns[2].Caption = "Vevő cím";
            vevokDt.Columns[3].ColumnName = "Technikai azonosító";
            vevokDt.Columns[3].Caption = "Vevő technikai azonosító";
            vevokDt.Columns[4].ColumnName = "Adószám";
            vevokDt.Columns[4].Caption = "Vevő adószám";
            
            dataGridViewVevok.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dataGridViewVevok.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridViewVevok.ReadOnly = true;
            dataGridViewVevok.AllowUserToDeleteRows = false;
            dataGridViewVevok.AllowUserToAddRows = false;
            dataGridViewVevok.MultiSelect = false;
        }

        private void metroTextBoxVevoAdoszam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

        }

        private void metroButtonVevoFelvetel_Click(object sender, EventArgs e)
        {
            adatFelvetel = true;
            metroPanelVevo.Visible = true;
            int ujVevoId = repo.getKovVevoId();
            metroTextBoxVevoAzon.ReadOnly = true;
            metroPanelVevoTorolModosit.Visible = true;
            metroTextBoxVevoAzon.Text = ujVevoId.ToString();
            metroTextBoxVevoNev.Text = string.Empty;
            metroTextBoxVevoCim.Text = string.Empty;
            metroTextBoxVevoTechAzon.Text = string.Empty;
            metroTextBoxVevoAdoszam.Text = string.Empty; 
        }

        private void metroButtonVevokTorol_Click(object sender, EventArgs e)
        {
            HibauzenetTorlese();
            if ((dataGridViewVevok.Rows == null) || (dataGridViewVevok.Rows.Count == 0))
                return;

            int vevoId = Convert.ToInt32(dataGridViewVevok.SelectedRows[0].Cells[0].Value.ToString());
            if (!int.TryParse(
                         dataGridViewVevok.SelectedRows[0].Cells[0].Value.ToString(),
                         out vevoId))
                return;
            if (MessageBox.Show(
                "Valóban törölni akarja a sort?",
                "Törlés",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                //Törlés listából
                try
                {
                    repo.VevoTorleseListabol(vevoId);
                }
                catch (RepositoryExceptionNemTudTorolni rennt)
                {
                    HibaUzenetKiirasa(rennt.Message);
                    Debug.WriteLine("A vevő törlése nem sikerült, nincs a listába!");
                }

                //Törlés adatbázisból
                VevokRepositoryAdatbazisTabla vrat = new VevokRepositoryAdatbazisTabla();
                try
                {
                    vrat.vevoTorleseAdatbazisbol(vevoId);
                }
                catch (Exception ex)
                {
                    HibaUzenetKiirasa(ex.Message);
                }

                //DataGridView frissítése
                DataGridViewFrissiteseVevo();
                DataGridViewVevokBeallit();
            }
        }

        private void metroButtonVevokModosit_Click(object sender, EventArgs e)
        {
            HibauzenetTorlese();
            ErrorProviderekTorleseVevo();
            try
            {
                Vevo modosult = new Vevo(
                    Convert.ToInt32(metroTextBoxVevoAzon.Text),
                    metroTextBoxVevoNev.Text,
                    metroTextBoxVevoCim.Text,
                    metroTextBoxVevoTechAzon.Text,
                    Convert.ToInt32(metroTextBoxVevoAdoszam)
                    );
                int vevoId = Convert.ToInt32(metroTextBoxVevoAzon.Text);

                //Módosítás listában
                try
                {
                    repo.VevoModositasaListaban(vevoId, modosult);
                }
                catch (Exception ex)
                {
                    HibaUzenetKiirasa(ex.Message);
                    return;
                }

                //Módosítás adatbázisban
                VevokRepositoryAdatbazisTabla vrat = new VevokRepositoryAdatbazisTabla();
                try
                {
                    vrat.VevoModositasaAdatbazisban(vevoId, modosult);
                }
                catch (Exception ex)
                {
                    HibaUzenetKiirasa(ex.Message);
                }

                //DataGridView frissítése
                DataGridViewFrissiteseVevo();
            }
            catch (RepositoryExceptionNemTudModositani rentm)
            {
                HibaUzenetKiirasa(rentm.Message);
                Debug.WriteLine("A módosítás nem sikerült, a vevő nincs a listában!");
            }
            catch (Exception)
            {

            }
        }

        private void metroButtonUjVevo_Click(object sender, EventArgs e)
        {
            if(metroTextBoxVevoAzon.Text != string.Empty)
            {
                if (metroTextBoxVevoNev.Text != string.Empty)
                {
                    if(metroTextBoxVevoCim.Text != string.Empty)
                    {
                        if(metroTextBoxVevoTechAzon.Text != string.Empty)
                        {
                            if (metroTextBoxVevoAdoszam.Text != string.Empty)
                            {
                                HibauzenetTorlese();
                                ErrorProviderekTorleseVevo();
                                try
                                {
                                    Vevo ujVevo = new Vevo(
                                        Convert.ToInt32(metroTextBoxVevoAzon.Text),
                                        metroTextBoxVevoNev.Text,
                                        metroTextBoxVevoCim.Text,
                                        metroTextBoxVevoTechAzon.Text,
                                        Convert.ToInt32(metroTextBoxVevoAdoszam.Text)
                                        );
                                    int azonosito = Convert.ToInt32(metroTextBoxVevoAzon.Text);

                                    //Hozzáadás listához
                                    try
                                    {
                                        repo.VevoHozzaadasaListahoz(ujVevo);
                                    }
                                    catch (Exception ex)
                                    {
                                        HibaUzenetKiirasa(ex.Message);
                                    }

                                    //Hozzáadás adatbázishoz
                                    VevokRepositoryAdatbazisTabla vrat = new VevokRepositoryAdatbazisTabla();
                                    try
                                    {
                                        vrat.VevoAdatbazisbaIllesztese(ujVevo);
                                    }
                                    catch (Exception ex)
                                    {
                                        HibaUzenetKiirasa(ex.Message);
                                    }

                                    //DataGridView frissítése
                                    DataGridViewFrissiteseVevo();
                                    if (dataGridViewVevok.SelectedRows.Count == 1)
                                    {
                                        DataGridViewVevokBeallit();
                                    }
                                }
                                catch (HibasVevoNevException hvne)
                                {
                                    errorProviderVevoNev.SetError(metroTextBoxVevoNev, hvne.Message);
                                }
                                catch (HibasVevoCimException hvce)
                                {
                                    errorProviderVevoCim.SetError(metroTextBoxVevoCim, hvce.Message);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            else
                            {
                                errorProviderAdoszam.SetError(metroTextBoxVevoAdoszam, "Töltse ki a mezőt!");
                            }
                        }
                        else
                        {
                            errorProviderTechAzon.SetError(metroTextBoxVevoTechAzon, "Töltse ki a mezőt!");
                        }
                    }
                    else
                    {
                        errorProviderVevoCim.SetError(metroTextBoxVevoCim, "Töltse ki a mezőt!");
                    }
                }
                else
                {
                    errorProviderVevoNev.SetError(metroTextBoxVevoNev, "Töltse ki a mezőt!");
                }
            }
            else
            {
                errorProviderVevoId.SetError(metroTextBoxVevoAzon, "Töltse ki a mezőt!");
            }
            
        }

        private void metroButtonVevoMegse_Click(object sender, EventArgs e)
        {
            metroTextBoxVevoAzon.Text = string.Empty;
            metroTextBoxVevoNev.Text = string.Empty;
            metroTextBoxVevoCim.Text = string.Empty;
            metroTextBoxVevoTechAzon.Text = string.Empty;
            metroTextBoxVevoAdoszam.Text = string.Empty;
        }
    }
}
