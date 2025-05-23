﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace MissionPlanner.Controls
{
    public partial class ParamCompare : Form
    {
        public delegate void dtlvcallbackHandler(string param, double value);

        public event dtlvcallbackHandler dtlvcallback;

        DataGridView dgv;
        Dictionary<string, double> param = new Dictionary<string, double>();
        Dictionary<string, double> param2 = new Dictionary<string, double>();

        public ParamCompare(DataGridView dgv, Dictionary<string, double> param, Dictionary<string, double> param2)
        {
            InitializeComponent();
            this.param = param;
            this.param2 = param2;
            this.dgv = dgv;
			this.DialogResult = DialogResult.Cancel;
            processToScreen();
        }

        void processToScreen()
        {
            Params.Rows.Clear();

            // process hashdefines and update display
            foreach (string value in param.Keys)
            {
                if (value == null || value == "")
                    continue;

                //System.Diagnostics.Debug.WriteLine("Doing: " + value);
                try
                {
                    if (param.ContainsKey(value) && param2.ContainsKey(value))
                    {
                        // check double != double
                        if (param[value].ToString() != param2[value].ToString())
                        // this will throw is there is no matching key
                        {
                            Console.WriteLine("{0} {1} vs {2}", value, param[value], param2[value]);
                            Params.Rows.Add();
                            Params.Rows[Params.RowCount - 1].Cells[Command.Index].Value = value;
                            Params.Rows[Params.RowCount - 1].Cells[Value.Index].Value = param[value].ToString();

                            Params.Rows[Params.RowCount - 1].Cells[newvalue.Index].Value = param2[value].ToString();
                            Params.Rows[Params.RowCount - 1].Cells[Use.Index].Value = true;
                        }
                    }
                }
                catch
                {
                }
                ; //if (Params.RowCount > 1) { Params.Rows.RemoveAt(Params.RowCount - 1); } }
            }
            Params.Sort(Params.Columns[0], ListSortDirection.Ascending);
        }

        private void BUT_save_Click(object sender, EventArgs e)
        {
            if (dgv == null)
            {
                try
                {
                    foreach (DataGridViewRow row in Params.Rows)
                    {
                        if ((bool)row.Cells[Use.Index].Value == true)
                        {
                            if (dtlvcallback != null)
                                dtlvcallback(row.Cells[Command.Index].Value.ToString().Trim(),
                                    double.Parse(row.Cells[newvalue.Index].Value.ToString()));
                            else
                                MainV2.comPort.setParam((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, row.Cells[Command.Index].Value.ToString().Trim(),
                                    double.Parse(row.Cells[newvalue.Index].Value.ToString()));
                        }
                    }
                }
                catch
                {
                    CustomMessageBox.Show(Strings.ErrorSettingParameter, Strings.ERROR);
                    return;
                }
            }
            else
            {
                foreach (DataGridViewRow row in Params.Rows)
                {
                    if ((bool)row.Cells[Use.Index].Value == true)
                    {
                        foreach (DataGridViewRow dgvr in dgv.Rows)
                        {
                            if (dgvr.Cells[0].Value.ToString().Trim() ==
                                row.Cells[Command.Index].Value.ToString().Trim())
                            {
                                dgvr.Cells[1].Value = row.Cells[newvalue.Index].Value.ToString();
                                break;
                            }
                        }
                    }
                }
            }
			this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CHK_toggleall_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in Params.Rows)
            {
                row.Cells[Use.Index].Value = CHK_toggleall.Checked;
            }
        }
    }
}