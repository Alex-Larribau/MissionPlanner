using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Accord.Math;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MissionPlanner;
using MissionPlanner.GCSViews;
using Renci.SshNet;
using SixLabors.ImageSharp.ColorSpaces;
using static MissionPlanner.Utilities.LTM;

public class UsvPolarPlotControl : UserControl
{
    private PictureBox canvas;
    private ComboBox scaleCombo;
    private Button simulateButton;
    private int canvasSize = 500;
    private PointF[] lastPoints = new PointF[3];
    private string lastAuvState = "";
    private DateTime? lastTimestamp = null;
    private Thread simulationThread;
    private bool simulating = false;
    private SplitContainer splitContainer1;
    private PictureBox pictureBox1;
    private TableLayoutPanel tableLayoutPanel1;
    private Label label_Bearing;
    private Label label25;
    private Label label_Distance;
    private Label label21;
    private Label labelModeAuv;
    private Label labelOrdreUsv;
    private Label labelGPS;
    private Label label9;
    private Label labelDuree;
    private Label label5;
    private Label label17;
    private Label label13;
    private Label label2;
    private Label label1;
    public ComboBox ComboFormat;
    private Button button1;
    private SplitContainer splitContainer2;
    public TextBox textBoxMp;
    private Label label8;
    public TextBox textBoxId;
    private Label label7;
    private Label label4;
    public TextBox textBox_adresse;
    private TableLayoutPanel tableLayoutPanel2;
    private TextBox textBox4;
    private Button button3;
    private Button button_connect;
    private TextBox sshOutputBox;
    private PictureBox pictureBox2;
    private ComboBox comboBox1;
    private Label label6;
    private Label labelErreur;
    private SplitContainer splitContainer3;
    private Label label10;
    private NumericUpDown numericUpDown1;
    private Label label11;
    private Label label3;
    private Label label_SNR;
    private Label label_Elevation;
    private Button button4;
    private TextBox textBoxAllMessages;
    public ComboBox ordreCombo;

    public UsvPolarPlotControl()
    {
        InitializeComponent();
    }

    //designer
    private void InitializeComponent()
    {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxMp = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_adresse = new System.Windows.Forms.TextBox();
            this.textBoxId = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button_connect = new System.Windows.Forms.Button();
            this.ComboFormat = new System.Windows.Forms.ComboBox();
            this.textBoxAllMessages = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.labelErreur = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label_Bearing = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label_Distance = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.labelModeAuv = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.labelOrdreUsv = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.labelGPS = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelDuree = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.sshOutputBox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label_Elevation = new System.Windows.Forms.Label();
            this.label_SNR = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.ordreCombo = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(578, 689);
            this.splitContainer1.SplitterDistance = 102;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.textBoxAllMessages);
            this.splitContainer2.Panel2.Controls.Add(this.label6);
            this.splitContainer2.Panel2.Controls.Add(this.comboBox1);
            this.splitContainer2.Panel2.Controls.Add(this.pictureBox2);
            this.splitContainer2.Size = new System.Drawing.Size(578, 102);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.984639F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.73733F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.528418F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.56067F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.48848F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.2811F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.27957F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.139785F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxMp, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBox_adresse, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxId, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.label7, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.button_connect, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.ComboFormat, 7, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(578, 25);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // textBoxMp
            // 
            this.textBoxMp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMp.Location = new System.Drawing.Point(320, 3);
            this.textBoxMp.Name = "textBoxMp";
            this.textBoxMp.Size = new System.Drawing.Size(93, 22);
            this.textBoxMp.TabIndex = 6;
            this.textBoxMp.Text = "raspberry";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 25);
            this.label4.TabIndex = 2;
            this.label4.Text = "Host";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(277, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 25);
            this.label8.TabIndex = 5;
            this.label8.Text = "MP";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_adresse
            // 
            this.textBox_adresse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_adresse.Location = new System.Drawing.Point(60, 3);
            this.textBox_adresse.Name = "textBox_adresse";
            this.textBox_adresse.Size = new System.Drawing.Size(113, 22);
            this.textBox_adresse.TabIndex = 0;
            this.textBox_adresse.Text = "192.168.1.11";
            this.textBox_adresse.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_adresse_KeyDown);
            // 
            // textBoxId
            // 
            this.textBoxId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxId.Location = new System.Drawing.Point(216, 3);
            this.textBoxId.Name = "textBoxId";
            this.textBoxId.Size = new System.Drawing.Size(55, 22);
            this.textBoxId.TabIndex = 4;
            this.textBoxId.Text = "pi";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(179, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 25);
            this.label7.TabIndex = 3;
            this.label7.Text = "ID";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_connect
            // 
            this.button_connect.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button_connect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_connect.Location = new System.Drawing.Point(416, 0);
            this.button_connect.Margin = new System.Windows.Forms.Padding(0);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(105, 25);
            this.button_connect.TabIndex = 7;
            this.button_connect.Text = "Connect";
            this.button_connect.UseVisualStyleBackColor = false;
            this.button_connect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // ComboFormat
            // 
            this.ComboFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComboFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboFormat.FormattingEnabled = true;
            this.ComboFormat.Items.AddRange(new object[] {
            "Format 1",
            "Format 2"});
            this.ComboFormat.Location = new System.Drawing.Point(524, 3);
            this.ComboFormat.Name = "ComboFormat";
            this.ComboFormat.Size = new System.Drawing.Size(51, 28);
            this.ComboFormat.TabIndex = 31;
            this.ComboFormat.SelectedIndexChanged += new System.EventHandler(this.ComboFormat_Click);
            // 
            // textBoxAllMessages
            // 
            this.textBoxAllMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAllMessages.Location = new System.Drawing.Point(0, 0);
            this.textBoxAllMessages.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxAllMessages.Multiline = true;
            this.textBoxAllMessages.Name = "textBoxAllMessages";
            this.textBoxAllMessages.ReadOnly = true;
            this.textBoxAllMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxAllMessages.Size = new System.Drawing.Size(578, 73);
            this.textBoxAllMessages.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "Portée (m)";
            this.label6.Visible = false;
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "25",
            "50",
            "100",
            "200",
            "500"});
            this.comboBox1.Location = new System.Drawing.Point(2, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(87, 28);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.Visible = false;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = global::MissionPlanner.Properties.Resources.polaires_usv;
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(578, 73);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
            this.pictureBox2.Resize += new System.EventHandler(this.pictureBox2_Resize);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(578, 102);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.labelErreur);
            this.splitContainer3.Panel1MinSize = 0;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer3.Size = new System.Drawing.Size(578, 583);
            this.splitContainer3.SplitterDistance = 62;
            this.splitContainer3.TabIndex = 0;
            // 
            // labelErreur
            // 
            this.labelErreur.AutoSize = true;
            this.labelErreur.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErreur.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelErreur.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(88)))), ((int)(((byte)(71)))));
            this.labelErreur.Location = new System.Drawing.Point(0, 0);
            this.labelErreur.Name = "labelErreur";
            this.labelErreur.Size = new System.Drawing.Size(0, 26);
            this.labelErreur.TabIndex = 5;
            this.labelErreur.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.87886F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.49135F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.87543F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.01038F));
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label10, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_Bearing, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label25, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label_Distance, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label21, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.labelModeAuv, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label17, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelOrdreUsv, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelGPS, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelDuree, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.sshOutputBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.button3, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBox4, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label_Elevation, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label_SNR, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.button4, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDown1, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.button1, 3, 8);
            this.tableLayoutPanel1.Controls.Add(this.ordreCombo, 2, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.17677F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.17677F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.17677F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.7147F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.04117F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.17677F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.17677F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.18013F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.18013F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(578, 517);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(3, 453);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(172, 64);
            this.label11.TabIndex = 39;
            this.label11.Text = "SNR";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(310, 282);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(122, 20);
            this.label10.TabIndex = 25;
            this.label10.Text = "Distance max :";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(301, 57);
            this.label1.TabIndex = 28;
            this.label1.Text = "Retour d\'informations";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Bearing
            // 
            this.label_Bearing.AutoSize = true;
            this.label_Bearing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Bearing.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Bearing.ForeColor = System.Drawing.Color.Cyan;
            this.label_Bearing.Location = new System.Drawing.Point(181, 339);
            this.label_Bearing.Name = "label_Bearing";
            this.label_Bearing.Size = new System.Drawing.Size(123, 57);
            this.label_Bearing.TabIndex = 25;
            this.label_Bearing.Text = "N/A";
            this.label_Bearing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(3, 339);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(172, 57);
            this.label25.TabIndex = 24;
            this.label25.Text = "Bearing";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Distance
            // 
            this.label_Distance.AutoSize = true;
            this.label_Distance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Distance.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Distance.ForeColor = System.Drawing.Color.Yellow;
            this.label_Distance.Location = new System.Drawing.Point(181, 282);
            this.label_Distance.Name = "label_Distance";
            this.label_Distance.Size = new System.Drawing.Size(123, 57);
            this.label_Distance.TabIndex = 21;
            this.label_Distance.Text = "N/A";
            this.label_Distance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(3, 282);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(172, 57);
            this.label21.TabIndex = 20;
            this.label21.Text = "Distance";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelModeAuv
            // 
            this.labelModeAuv.AutoSize = true;
            this.labelModeAuv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModeAuv.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelModeAuv.Location = new System.Drawing.Point(181, 231);
            this.labelModeAuv.Name = "labelModeAuv";
            this.labelModeAuv.Size = new System.Drawing.Size(123, 51);
            this.labelModeAuv.TabIndex = 17;
            this.labelModeAuv.Text = "N/A";
            this.labelModeAuv.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(3, 231);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(172, 51);
            this.label17.TabIndex = 16;
            this.label17.Text = "Mode AUV";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelOrdreUsv
            // 
            this.labelOrdreUsv.AutoSize = true;
            this.labelOrdreUsv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOrdreUsv.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOrdreUsv.Location = new System.Drawing.Point(181, 171);
            this.labelOrdreUsv.Name = "labelOrdreUsv";
            this.labelOrdreUsv.Size = new System.Drawing.Size(123, 60);
            this.labelOrdreUsv.TabIndex = 13;
            this.labelOrdreUsv.Text = "N/A";
            this.labelOrdreUsv.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(3, 171);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(172, 60);
            this.label13.TabIndex = 12;
            this.label13.Text = "Ordre USV ";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGPS
            // 
            this.labelGPS.AutoSize = true;
            this.labelGPS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGPS.Location = new System.Drawing.Point(181, 114);
            this.labelGPS.Name = "labelGPS";
            this.labelGPS.Size = new System.Drawing.Size(123, 57);
            this.labelGPS.TabIndex = 9;
            this.labelGPS.Text = "N/A";
            this.labelGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 114);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(172, 57);
            this.label9.TabIndex = 8;
            this.label9.Text = "Statut GPS USV";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDuree
            // 
            this.labelDuree.AutoSize = true;
            this.labelDuree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDuree.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDuree.Location = new System.Drawing.Point(181, 57);
            this.labelDuree.Name = "labelDuree";
            this.labelDuree.Size = new System.Drawing.Size(123, 57);
            this.labelDuree.TabIndex = 5;
            this.labelDuree.Text = "N/A";
            this.labelDuree.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(172, 57);
            this.label5.TabIndex = 4;
            this.label5.Text = "Durée entre 2 messages";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sshOutputBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.sshOutputBox, 2);
            this.sshOutputBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sshOutputBox.Enabled = false;
            this.sshOutputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sshOutputBox.Location = new System.Drawing.Point(310, 3);
            this.sshOutputBox.Multiline = true;
            this.sshOutputBox.Name = "sshOutputBox";
            this.tableLayoutPanel1.SetRowSpan(this.sshOutputBox, 2);
            this.sshOutputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.sshOutputBox.Size = new System.Drawing.Size(265, 108);
            this.sshOutputBox.TabIndex = 37;
            this.sshOutputBox.Text = "Pas encore reçu de message intéressant";
            this.sshOutputBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ShowFenetreTerm);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Enabled = false;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(447, 174);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(128, 54);
            this.button3.TabIndex = 36;
            this.button3.Text = "Envoyer";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button123_Click);
            // 
            // textBox4
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBox4, 2);
            this.textBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(310, 117);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(265, 51);
            this.textBox4.TabIndex = 35;
            this.textBox4.Tag = "envoie message custom";
            this.textBox4.Text = "envoyer un message custom";
            this.textBox4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox4_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 396);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(172, 57);
            this.label3.TabIndex = 38;
            this.label3.Text = "Elevation";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Elevation
            // 
            this.label_Elevation.AutoSize = true;
            this.label_Elevation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Elevation.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label_Elevation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label_Elevation.Location = new System.Drawing.Point(181, 396);
            this.label_Elevation.Name = "label_Elevation";
            this.label_Elevation.Size = new System.Drawing.Size(123, 57);
            this.label_Elevation.TabIndex = 41;
            this.label_Elevation.Text = "N/A";
            this.label_Elevation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_SNR
            // 
            this.label_SNR.AutoSize = true;
            this.label_SNR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_SNR.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_SNR.Location = new System.Drawing.Point(181, 453);
            this.label_SNR.Name = "label_SNR";
            this.label_SNR.Size = new System.Drawing.Size(123, 64);
            this.label_SNR.TabIndex = 40;
            this.label_SNR.Text = "N/A";
            this.label_SNR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(310, 339);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(265, 57);
            this.label2.TabIndex = 29;
            this.label2.Tag = "Selectionner un ordre à envoyer ";
            this.label2.Text = "Envoyer Ordre :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel1.SetColumnSpan(this.button4, 2);
            this.button4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(310, 234);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(265, 45);
            this.button4.TabIndex = 43;
            this.button4.Text = "Envoyer \"startusv\"";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button123_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDown1.AutoSize = true;
            this.numericUpDown1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown1.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Location = new System.Drawing.Point(447, 310);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(56, 26);
            this.numericUpDown1.TabIndex = 26;
            this.numericUpDown1.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Enabled = false;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(447, 456);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 58);
            this.button1.TabIndex = 33;
            this.button1.Text = "Envoyer";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button123_Click);
            // 
            // ordreCombo
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ordreCombo, 2);
            this.ordreCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ordreCombo.Enabled = false;
            this.ordreCombo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ordreCombo.FormattingEnabled = true;
            this.ordreCombo.Items.AddRange(new object[] {
            "PAS D\'ORDRE",
            "DISARM",
            "MISSION_GO",
            "MISSION_ABORT",
            "STABILIZED_ASCEND",
            "HOMING",
            "RECOVER_STUCK"});
            this.ordreCombo.Location = new System.Drawing.Point(310, 399);
            this.ordreCombo.Name = "ordreCombo";
            this.ordreCombo.Size = new System.Drawing.Size(265, 28);
            this.ordreCombo.TabIndex = 44;
            // 
            // UsvPolarPlotControl
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "UsvPolarPlotControl";
            this.Size = new System.Drawing.Size(578, 689);
            this.Load += new System.EventHandler(this.UsvPolarPlotControl_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

    }

    private void UsvPolarPlotControl_Load(object sender, EventArgs e)
    {
        //splitContainer3.SplitterDistance = 1;
        comboBox1.SelectedIndex = 1;
    }

    #region variables globales

    //traduction des ordres pour les drones 
    private Dictionary<string, string> ordreMapping = new Dictionary<string, string>();
    private Dictionary<string, string> ordreMapping2 = new Dictionary<string, string>()
    {
        ["DISARM"] = "0",
        ["MISSION_GO"] = "2",
        ["MISSION_ABORT"] = "3",
        ["STABILIZED_ASCEND"] = "4",
        ["HOMING"] = "5",
        ["RECOVER_STUCK"] = "9"
    };
    private Dictionary<string, string> ordreMapping1 = new Dictionary<string, string>() 
    {
        ["DISARM"] = "0",
        ["DEPTH_HOLD"] = "2",
        ["ALT_HOLD"] = "3",
        ["DEPTH_HOLD_FOLLOW"] = "4",
        ["ALT_HOLD_FOLLOW"] = "5",
        ["SURFACE"] = "6"
    };

    //[old envoi ordres urgents]
    private readonly Dictionary<string, string> urgenceMapping = new Dictionary<string, string>()
    {
        ["DISARM"] = "p\n",
        ["DEPTH_HOLD"] = "z\n",
        ["ALT_HOLD"] = "e\n",
        ["DEPTH_HOLD_FOLLOW"] = "r\n",
        ["ALT_HOLD_FOLLOW"] = "t\n",
        ["SURFACE"] = "y\n"
    };

    //com/thread started ?
    private bool com_etablie = false;
    bool readThread_started = false;

    //ssh
    private SshClient sshClient;
    private ShellStream shellStream;

    private FenetreTerminal fenetreTerminal;

    //graph
    private int maxRadius = 50; //les radius sont en pixel
    private int maxDistance = 50; //les distances en m 
    private double scale = 1;    //echelle maxRadius/maxDistance pour avoir des pixels/m

    //stockage des 3 dernieres positions
    private List<(float Distance, int Bearing)> lastPositions = new List<(float, int)>(3);
    private float maxLastPositions = 0;
    
    //couleurs affichage points 
    private SolidBrush lastBrush = new SolidBrush(Color.FromArgb(255, 43, 150, 0));
    private SolidBrush previousBrush = new SolidBrush(Color.FromArgb(175, 43, 150, 0)); //(150, 113, 176, 84));
    private SolidBrush oldestBrush = new SolidBrush(Color.FromArgb(100, 43, 150, 0));  //(100, 174, 205, 159));

    #endregion

    private void buttonConnect_Click(object sender, EventArgs e)
    {
        if (com_etablie)
        {
            sshClient.Disconnect();

            ordreCombo.Enabled = false;
            //urgenceCombo.Enabled = false;
            sshOutputBox.Enabled = false;
            textBox4.Enabled = false;
            button1.Enabled = false; //button2.Enabled = false;
            button3.Enabled = false;
            button_connect.Text = "Connect";

            com_etablie = false;

            lastTimestamp = null;

            MessageBox.Show("Vous êtes déconnecté du serveur SSH, veuillez vous reconnecter");
        }
        else
        {
            //si on a rien selectionné ou qu'on a selectionné 2 on met ordremapping2
            ordreMapping = ordreMapping2; 
            if (ComboFormat.Text.Contains("1"))
            {
                ordreMapping = ordreMapping1;
                ordreCombo.Items.Clear();
                ordreCombo.Items.AddRange(new object[] {
                    "PAS D\'ORDRE",
                    "DISARM",
                    "DEPTH_HOLD",
                    "ALT_HOLD",
                    "DEPTH_HOLD_FOLLOW",
                    "ALT_HOLD_FOLLOW",
                    "SURFACE"
                });
            }
            try
            {
                string Host = textBox_adresse.Text;
                string Username = textBoxId.Text;
                string Password = textBoxMp.Text;

                sshClient = new SshClient(Host, Username, Password);
                sshClient.Connect();
                if (sshClient.IsConnected)
                {
                    ordreCombo.Enabled = true;
                    //urgenceCombo.Enabled = true;
                    sshOutputBox.Enabled = true;
                    textBox4.Enabled = true;
                    button1.Enabled = true;
                    //button2.Enabled = true; 
                    button3.Enabled = true;
                    button_connect.Text = "Disconnect";

                    startingShell();

                    com_etablie = true;

                    DialogResult result = MessageBox.Show("Connexion réussie ! Lancer byobu ?",
                            "Confirmation",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        shellStream.WriteLine("byobu");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur SSH : " + ex.Message);
            }
        }
    }
    private void textBox_adresse_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;
        else
        {
            buttonConnect_Click(sender, e);
        }
    }
    
    bool but4 = false;
    private void button123_Click(object sender, EventArgs e)
    {
        if (!readThread_started)
        {
            MessageBox.Show("Connectez vous d'abord !");
            return;
        }

        string message = "";
        string messageToSend = "";
        bool success = false;

        //bouton envoi libre 
        if (sender.Equals(button3))
        {
            messageToSend = textBox4.Text;
            success = true;
            textBox4.Text = "";
        }
        //bouton envoi ordre 
        else if (sender.Equals(button1))
        {
            #region ordres 
            //Ordres From USV
            //usbl_ros_usvCommands = {
            //    0: "DISARM",              # seaker 26
            //    1: "MISSION_GO",          # seaker 7
            //    2: "MISSION_ABORT",       # seaker 11
            //    3: "STABILIZED_ASCEND",   # seaker 13
            //    4: "HOMING",              # seaker 14
            //    8: "RECOVER_STUCK"        # seaker 16
            //}
            #endregion

            message = ordreCombo.SelectedItem.ToString();
            if (message == "PAS D'ORDRE")
            {
                return;
            }
            else
            {
                try
                {
                    //on traduit grace au dictionaire ordremapping 
                    success = ordreMapping.TryGetValue(message, out messageToSend);
                    ordreCombo.SelectedItem = 0;
                }
                catch { }
            }
        }
        //bouton ordre urgence
        else if (sender.Equals(button4))
        {
            #region [old urgence]
            //message = urgenceCombo.SelectedItem.ToString();
            //if (message == "PAS D'ORDRE") { return; }
            //else
            //{
            //    try
            //    {
            //        success = urgenceMapping.TryGetValue(message, out messageToSend);
            //        urgenceCombo.SelectedItem = 0;
            //    }
            //    catch { }
            //}
            #endregion
            but4= true;
            messageToSend = "cd && ./startusv.sh";
            success = true;
        }


        //si echec, on prévient et on arrete, sinon on envoie ! 
        if (!success)
        {
            MessageBox.Show("Erreur codage message, veuillez ré-essayer");
            return;
        }
        else
        {
            try
            {
                shellStream.WriteLine(messageToSend);
                //debug 
                //MessageBox.Show("Message envoyé : " + messageToSend);
                if (but4) { shellStream.WriteLine("o"); but4 = false; }
            }
            catch
            {
                MessageBox.Show("Erreur envoi message, vérifiez la connexion");
            }
        }
    }


    private DateTime lastPb;
    private bool yaunpb = false;
    string last_distance = "";
    string noPingAuvValue = "";
    private void decodeAndLabel(string line)
    {

        #region Etats Possibles AUV
        //usbl_ros_auvStates = {
        //    0: "DESCENDING",     # seaker 26
        //    1: "ASCENDING",      # seaker 7
        //    2: "MISSION",        # seaker 11
        //    3: "DISARM",         # seaker 13
        //    4: "RECOVER_STUCK",  # seaker 14
        //    5: "STUCK",          # seaker 19
        //    6: "FAILURE",        # seaker 23
        //    7: "HOMING",         # seaker 8
        //    8: "SURFACE"         # seaker 16
        //}

        //types de messages possibles
        //2025 - 11 - 07 08:59:13.102 USV: RTK_FIXED, DISARM, LOITER, MISSION_ABORT | AUV#0:MISSION, 0.6m, 58°/35°:19.5
        // USV:3D_FIX, DISARM, LOITER, DEPTH_HOLD | AUV#0:DISARMED, 10.7m, 66°/0°:24.5
        // USV:RTK_FIX, DISARM, LOITER, SURFACE | AUV#0:ALT_HOLD, 13.3m, 30°/-6°:25.3
        // USV:RTK_FLOAT, ARMED, MANUAL, DEPTH_HOLD | AUV#0:ALT_HOLD, 6.6m, 148°/23°:21.3 
        // USV:3D_FIX, ARM, LOITER, ALT_HOLD | AUV#0:EMERGENCY_STUCK, 3.4m, 306°/-23°:21.0
        //2025-11-07     15:38:07.097 USV:3D_FIX, DISARM, MANUAL, PAS D'ORDRE - Kogger - USV status = 0:Never, AUV ping:Never
        #endregion

        #region ya un pb ? 
        // erreur1 : no ping auv 
        var matchNoPingAuv = Regex.Match(line, @"ping AUV for ([^,]+)s");
        if (matchNoPingAuv.Success)
        {
            lastPb = DateTime.Now;
            noPingAuvValue = matchNoPingAuv.Groups[1].Value;
            labelErreur.Text = "ERREUR : No ping AUV for " + noPingAuvValue + " s !";
            labelDuree.Text = noPingAuvValue;
        }
        matchNoPingAuv = Regex.Match(line, @"AUV ping:([0-9.]+)s"); 
        if (matchNoPingAuv.Success)
        {
            noPingAuvValue = matchNoPingAuv.Groups[1].Value;
            float noPingAuvValueFloat = float.Parse(noPingAuvValue, CultureInfo.InvariantCulture);
            if (noPingAuvValueFloat > 30)
            {
                lastPb = DateTime.Now;
                yaunpb = true;
                labelErreur.Text = "ERREUR : No ping AUV for " + noPingAuvValue + " s !";
            }
            labelDuree.Text = noPingAuvValue +" s";
            // couleur en fonction de la durée, vert si <15, orange de 15 à 30, rouge de 30s à 2 min, puis déconnexion forcée si > 2min
            if (noPingAuvValueFloat < 15)
                labelDuree.ForeColor = Color.FromArgb(255, 143, 255, 71);
            else if (noPingAuvValueFloat < 30)
                labelDuree.ForeColor = Color.FromArgb(255, 255, 205, 71);
            else if (noPingAuvValueFloat < 120)
            {
                labelDuree.ForeColor = Color.FromArgb(255, 255, 88, 71);
            }
            else
            {
                //pas de messages depuis 2 minutes, on déconnecte
                //EventArgs _e = new EventArgs();
                //buttonConnect_Click(this, _e);
            }
        }

        //erreur2 : Emergency todo, voir si on a encore des emergency 
        var match = Regex.Match(line, @"(EMERGENCY_[^,]+)");
        if (match.Success)
        {
            lastPb = DateTime.Now;

            string matchEmergency = match.Groups[1].Value;

            //splitContainer3.Panel1.Height = 15;
            yaunpb = true;

            labelErreur.Text = "ERREUR : " + matchEmergency + " détecté !";
        }

        //si pas de pb pdt 10 s, on range la fenetre d'alerte
        if (!yaunpb)
        {
            if ((DateTime.Now - lastPb).TotalSeconds > 10 && !yaunpb)
            {
                // ça fait plus de 10 secondes qu'on a plus d'erreurs, on arrête l'alerte
                labelErreur.Text = "";
            }
        }
        else
        {
            Console.Beep(1000, 1000);
            yaunpb = false;
        }
        #endregion

        #region display messages
        //envoi de la ligne entière à fenetreTerminal si elle est ouverte, et à textBoxAllMessages dans tous les cas
        textBoxAllMessages.AppendText(line + "\n");
        textBoxAllMessages.ScrollToCaret();
        if (fenetreOuverte)
        {
            this.Invoke((Action)(() =>
            {
                fenetreTerminal.AppendLine(line);
            }));
        }


        Regex regex = new Regex(@"USV:(.*?)\| AUV#0:(.*)");
        Match matchRegex = regex.Match(line);

        if (matchRegex.Success)
        {
            // --- USV ---
            string usvPart = matchRegex.Groups[1].Value.Trim();
            string[] usvInfos = usvPart.Split(',');
            for (int i = 0; i < usvInfos.Length; i++)
                usvInfos[i] = usvInfos[i].Trim();

            labelGPS.Text = usvInfos.Length > 0 ? usvInfos[0] : "";
            labelOrdreUsv.Text = usvInfos.Length > 3 ? usvInfos[3] : "";

            // --- AUV ---
            string auvPart = matchRegex.Groups[2].Value.Trim();

            // Exemple d'extraction avec split
            string[] parts = auvPart.Split(',');
            labelModeAuv.Text = parts.Length > 0 ? parts[0].Trim() : "";
            label_Distance.Text = parts.Length > 1 ? parts[1].Trim() : "";

            label_Bearing.Text = "N/A";
            label_Elevation.Text = "N/A";
            label_SNR.Text = "N/A";

            if (parts.Length > 2)
            {
                string[] anglesAndValue = parts[2].Split(':');  // séparer angle et valeur
                string[] angles = anglesAndValue[0].Split('/'); // séparer les deux angles

                label_Bearing.Text = angles.Length > 0 ? angles[0].Trim() : "";
                label_Elevation.Text = angles.Length > 1 ? angles[1].Trim() : "";
                label_SNR.Text = anglesAndValue.Length > 1 ? anglesAndValue[1].Trim() : "";
            }

            label_Distance.ForeColor = Color.Yellow;
            label_Bearing.ForeColor = Color.Cyan;
            labelModeAuv.ForeColor = Color.LightGreen;

        }


        ////test ligne 
        //var lineMatch = Regex.Match(line, @"([^,]+)USV:([^,]+)AUV([^,]+)");
        //if (!lineMatch.Success)
        //{
        //    lineMatch = Regex.Match(line, @"([^,]+)USV:([^,]+) | AUV([^,]+)");
        //    if (!lineMatch.Success)
        //    {
        //        return;
        //    }
        //}

        //// si ligne ok on la met dans la sshoutputbox 
        //sshOutputBox.Text = line + Environment.NewLine;

        //// Infos USV


        //var usvMatch = Regex.Match(line, @"USV:(.*?)|");
        //string[] items = usvMatch.Groups[1].Value.Split(',');
        //if ((usvMatch.Success) && (items.Count()>2))
        //{

        //    //todo, voir si on garde l'info gps qui est déja marquée à gauche 
        //    labelGPS.Text =items[0].Trim();
        //    labelOrdreUsv.Text = items[3].Trim();
        //    if (labelGPS.Text == "") { labelGPS.Text = "N/A"; }
        //    if (labelOrdreUsv.Text == "") { labelOrdreUsv.Text = "N/A"; }
        //}
        //else
        //{
        //    //ici ça veut dire qu'on a pas réussi à matcher, on va donc essayer de récup ce qu'on peut
        //    usvMatch = Regex.Match(line, @"USV:([^,]+), ([^,]+), ([^,]+), ([^|]+)");
        //    if (usvMatch.Success)
        //    {
        //        labelGPS.Text = usvMatch.Groups[1].Value.Trim();
        //        labelOrdreUsv.Text = usvMatch.Groups[4].Value.Substring(1,9);
        //    }
        //}
        //// Infos AUV
        //var auvMatch = Regex.Match(line, @"AUV#([^,]+),([^,]+),([^,]+)/([^,]+):([^,]+)");

        //if (auvMatch.Success)
        //{
        //    string auvState = auvMatch.Groups[1].Value.Trim();
        //    string distance = auvMatch.Groups[2].Value.Trim();
        //    string bearing = auvMatch.Groups[3].Value.Trim();
        //    string elevation = auvMatch.Groups[4].Value.Trim();
        //    string SNR = auvMatch.Groups[5].Value.Trim();

        //    //on actualise le timestamp que si la nouvelle valeur est différente de la précédente
        //    if (last_distance != distance)
        //    {
        //        lastTimestamp = DateTime.Now;
        //        last_distance = distance;
        //    }

        //    // Mise à jour label
        //    labelModeAuv.Text = auvState;
        //    label_Distance.Text = distance; // $"{distance:F1} m";
        //    label_Bearing.Text = bearing; //  $"{bearing}°";
        //    label_Elevation.Text = elevation;
        //    label_SNR.Text = SNR;

        //}

        //label_Distance.ForeColor = Color.Yellow;
        //label_Bearing.ForeColor = Color.Cyan;
        //labelModeAuv.ForeColor = Color.LightGreen;
        #endregion
    }

private void startingShell()
    {
        shellStream = sshClient.CreateShellStream("xterm", 80, 24, 800, 600, 1024); //xterm car shell ne marchait pas, surement a cause de byobu

        StartSshReader();

        //shellStream.WriteLine("byobu");
    }

    private void StartSshReader()
    {
        Thread readThread = new Thread(() =>
        {
            while (sshClient?.IsConnected == true)
            {
                string line = shellStream.ReadLine();

                line = Regex.Replace(line, @"\x1B\[[0-9;]*[A-Za-z]", "").Trim();

                if (!string.IsNullOrEmpty(line))
                {
                    this.Invoke((Action)(() =>
                    {
                        decodeAndLabel(line);
                    }));
                }
            }
        });
        readThread.IsBackground = true;
        readThread.Start();
        readThread_started = true;
    }

    private void textBox4_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;
        else
        {
            button123_Click(this.button3, e);
        }
    }

    #region todo voir si on garde 
    private void pictureBox2_Paint(object sender, PaintEventArgs e)
    {
        var panel = splitContainer2.Panel2;
        int panelWidth = panel.Width;
        int panelHeight = panel.Height;

        int centerX = panelWidth / 2;
        int centerY = panelHeight / 2;

        //double radius = distance * maxRadius / maxDistance;
        //double angleRad = Math.PI * (90 - bearingDeg) / 180.0;

        //float x = centerX + (float)(radius * Math.Cos(angleRad));
        //float y = centerY - (float)(radius * Math.Sin(angleRad));

        //foreach (var (distance, bearing) in lastPositions)
        for (int i = 0; i < lastPositions.Count; i++)
        {
            float distance = lastPositions[i].Distance;
            int bearing = lastPositions[i].Bearing;

            SolidBrush couleur = lastBrush;

            if (lastPositions.Count == 3)
            {
                if (i == 0)
                    couleur = oldestBrush;
                else if (i == 1)
                    couleur = previousBrush;
                else
                    couleur = lastBrush;
            }
            else if (lastPositions.Count == 2)
            {
                if (i == 0)
                    couleur = previousBrush;
                else if (i == 1)
                    couleur = lastBrush;
            }
            //pour le else il ne reste qu'un point donc on laisse la couleur à lastBrush

            double radius = distance / maxDistance * maxRadius;
            double angleRad = Math.PI * (90 - bearing) / 180.0;

            float x = centerX + (float)(radius * Math.Cos(angleRad));
            float y = centerY - (float)(radius * Math.Sin(angleRad));

            //affichage 

            e.Graphics.FillEllipse(couleur, x - 5, y - 5, 10, 10);
        }
    }

    private void pictureBox2_Resize(object sender, EventArgs e)
    {
        maxRadius = (int)pictureBox2.Image.Width / 3;

        if (comboBox1.Text != "")
        {
            maxDistance = int.Parse(comboBox1.Text);
        }
    }
    #endregion

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        maxDistance = int.Parse(comboBox1.Text);
        //todo remove
    }

    static public bool fenetreOuverte = false;
    private void ShowFenetreTerm(object sender, MouseEventArgs e)
    {
        if (fenetreTerminal == null || fenetreTerminal.IsDisposed)
        {
            fenetreTerminal = new FenetreTerminal();
            fenetreTerminal.Show();
            fenetreOuverte = true;
        }
        else
        {
            fenetreTerminal.BringToFront();
        }
    }

    //todo faire quelque chose avec cette valeur
    private int valeurDistanceMax = 50;
    private void numericUpDown1_ValueChanged(object sender, EventArgs e)
    {
        try
        {
            ((Control)sender).Enabled = false;
            valeurDistanceMax = (int)numericUpDown1.Value;
        }
        catch
        {
        }
        ((Control)sender).Enabled = true;

    }

    private void ComboFormat_Click(object sender, EventArgs e)
    {
        if (ComboFormat.Text.Contains("1"))
        {
            ordreMapping = ordreMapping1;
            ordreCombo.Items.Clear();
            ordreCombo.Items.AddRange(new object[] {
                    "PAS D\'ORDRE",
                    "DISARM",
                    "DEPTH_HOLD",
                    "ALT_HOLD",
                    "DEPTH_HOLD_FOLLOW",
                    "ALT_HOLD_FOLLOW",
                    "SURFACE"
                });
        }
        else
        {
            ordreMapping = ordreMapping2;
            ordreCombo.Items.Clear();
            ordreCombo.Items.AddRange(new object[] {
                "PAS D\'ORDRE",
                "DISARM",
                "MISSION_GO",
                "MISSION_ABORT",
                "STABILIZED_ASCEND",
                "HOMING",
                "RECOVER_STUCK"});
        }
    }

}

