using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Accord.Math;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MissionPlanner;
using MissionPlanner.GCSViews;
using Renci.SshNet;

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
    private Label label3;
    private Label label2;
    private Label label1;
    public ComboBox ordreCombo;
    public ComboBox urgenceCombo;
    private Button button1;
    private Button button2;
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
    private Random rand = new Random();

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
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ordreCombo = new System.Windows.Forms.ComboBox();
            this.urgenceCombo = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.sshOutputBox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
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
            this.tableLayoutPanel1.SuspendLayout();
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
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(476, 459);
            this.splitContainer1.SplitterDistance = 268;
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
            this.splitContainer2.Panel2.Controls.Add(this.label6);
            this.splitContainer2.Panel2.Controls.Add(this.comboBox1);
            this.splitContainer2.Panel2.Controls.Add(this.pictureBox2);
            this.splitContainer2.Size = new System.Drawing.Size(476, 268);
            this.splitContainer2.SplitterDistance = 30;
            this.splitContainer2.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 7;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxMp, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBox_adresse, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxId, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.label7, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.button_connect, 6, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(476, 30);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // textBoxMp
            // 
            this.textBoxMp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMp.Location = new System.Drawing.Point(286, 3);
            this.textBoxMp.Name = "textBoxMp";
            this.textBoxMp.Size = new System.Drawing.Size(89, 26);
            this.textBoxMp.TabIndex = 6;
            this.textBoxMp.Text = "raspberry";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 30);
            this.label4.TabIndex = 2;
            this.label4.Text = "Host";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(239, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 30);
            this.label8.TabIndex = 5;
            this.label8.Text = "MP";
            // 
            // textBox_adresse
            // 
            this.textBox_adresse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_adresse.Location = new System.Drawing.Point(50, 3);
            this.textBox_adresse.Name = "textBox_adresse";
            this.textBox_adresse.Size = new System.Drawing.Size(89, 26);
            this.textBox_adresse.TabIndex = 0;
            this.textBox_adresse.Text = "192.168.1.11";
            // 
            // textBoxId
            // 
            this.textBoxId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxId.Location = new System.Drawing.Point(192, 3);
            this.textBoxId.Name = "textBoxId";
            this.textBoxId.Size = new System.Drawing.Size(41, 26);
            this.textBoxId.TabIndex = 4;
            this.textBoxId.Text = "pi";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(145, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 30);
            this.label7.TabIndex = 3;
            this.label7.Text = "ID";
            // 
            // button_connect
            // 
            this.button_connect.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button_connect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_connect.Location = new System.Drawing.Point(381, 3);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(92, 24);
            this.button_connect.TabIndex = 7;
            this.button_connect.Text = "Connect";
            this.button_connect.UseVisualStyleBackColor = false;
            this.button_connect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 25);
            this.label6.TabIndex = 5;
            this.label6.Text = "Portée (m)";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "50",
            "100",
            "200",
            "500"});
            this.comboBox1.Location = new System.Drawing.Point(2, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(87, 33);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = global::MissionPlanner.Properties.Resources.polaires_usv;
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(476, 234);
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
            this.pictureBox1.Size = new System.Drawing.Size(476, 268);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
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
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.ordreCombo, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.urgenceCombo, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.button1, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.button2, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.sshOutputBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.button3, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBox4, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.97326F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.83422F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(476, 187);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 26);
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
            this.label_Bearing.Location = new System.Drawing.Point(122, 156);
            this.label_Bearing.Name = "label_Bearing";
            this.label_Bearing.Size = new System.Drawing.Size(113, 31);
            this.label_Bearing.TabIndex = 25;
            this.label_Bearing.Text = "N/A";
            this.label_Bearing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(3, 156);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(113, 31);
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
            this.label_Distance.Location = new System.Drawing.Point(122, 130);
            this.label_Distance.Name = "label_Distance";
            this.label_Distance.Size = new System.Drawing.Size(113, 26);
            this.label_Distance.TabIndex = 21;
            this.label_Distance.Text = "N/A";
            this.label_Distance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(3, 130);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(113, 26);
            this.label21.TabIndex = 20;
            this.label21.Text = "Distance";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelModeAuv
            // 
            this.labelModeAuv.AutoSize = true;
            this.labelModeAuv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModeAuv.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelModeAuv.Location = new System.Drawing.Point(122, 106);
            this.labelModeAuv.Name = "labelModeAuv";
            this.labelModeAuv.Size = new System.Drawing.Size(113, 24);
            this.labelModeAuv.TabIndex = 17;
            this.labelModeAuv.Text = "N/A";
            this.labelModeAuv.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(3, 106);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(113, 24);
            this.label17.TabIndex = 16;
            this.label17.Text = "Mode AUV";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelOrdreUsv
            // 
            this.labelOrdreUsv.AutoSize = true;
            this.labelOrdreUsv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOrdreUsv.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOrdreUsv.Location = new System.Drawing.Point(122, 78);
            this.labelOrdreUsv.Name = "labelOrdreUsv";
            this.labelOrdreUsv.Size = new System.Drawing.Size(113, 28);
            this.labelOrdreUsv.TabIndex = 13;
            this.labelOrdreUsv.Text = "N/A";
            this.labelOrdreUsv.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(3, 78);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(113, 28);
            this.label13.TabIndex = 12;
            this.label13.Text = "Ordre USV ";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGPS
            // 
            this.labelGPS.AutoSize = true;
            this.labelGPS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGPS.Location = new System.Drawing.Point(122, 52);
            this.labelGPS.Name = "labelGPS";
            this.labelGPS.Size = new System.Drawing.Size(113, 26);
            this.labelGPS.TabIndex = 9;
            this.labelGPS.Text = "N/A";
            this.labelGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 26);
            this.label9.TabIndex = 8;
            this.label9.Text = "Statut GPS";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDuree
            // 
            this.labelDuree.AutoSize = true;
            this.labelDuree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDuree.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDuree.Location = new System.Drawing.Point(122, 26);
            this.labelDuree.Name = "labelDuree";
            this.labelDuree.Size = new System.Drawing.Size(113, 26);
            this.labelDuree.TabIndex = 5;
            this.labelDuree.Text = "N/A";
            this.labelDuree.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 26);
            this.label5.TabIndex = 4;
            this.label5.Text = "Durée entre 2 messages";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(241, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 24);
            this.label2.TabIndex = 29;
            this.label2.Text = "Ordre ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(360, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 24);
            this.label3.TabIndex = 30;
            this.label3.Text = "Emergency";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // ordreCombo
            // 
            this.ordreCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ordreCombo.Enabled = false;
            this.ordreCombo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ordreCombo.FormattingEnabled = true;
            this.ordreCombo.Items.AddRange(new object[] {
            "PAS D\'ORDRE",
            "DEPTH_HOLD",
            "ALT_HOLD",
            "DEPTH_HOLD_FOLLOW",
            "ALT_HOLD_FOLLOW",
            "SURFACE"});
            this.ordreCombo.Location = new System.Drawing.Point(241, 133);
            this.ordreCombo.Name = "ordreCombo";
            this.ordreCombo.Size = new System.Drawing.Size(113, 33);
            this.ordreCombo.TabIndex = 31;
            // 
            // urgenceCombo
            // 
            this.urgenceCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.urgenceCombo.Enabled = false;
            this.urgenceCombo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.urgenceCombo.FormattingEnabled = true;
            this.urgenceCombo.Items.AddRange(new object[] {
            "NO EMERGENCY",
            "DISARM",
            "DEPTH_HOLD",
            "ALT_HOLD",
            "DEPTH_HOLD_FOLLOW",
            "ALT_HOLD_FOLLOW",
            "SURFACE"});
            this.urgenceCombo.Location = new System.Drawing.Point(360, 133);
            this.urgenceCombo.Name = "urgenceCombo";
            this.urgenceCombo.Size = new System.Drawing.Size(113, 33);
            this.urgenceCombo.TabIndex = 32;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Enabled = false;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(241, 159);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 25);
            this.button1.TabIndex = 33;
            this.button1.Text = "Envoyer";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button123_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Enabled = false;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(360, 159);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 25);
            this.button2.TabIndex = 34;
            this.button2.Text = "Envoyer";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button123_Click);
            // 
            // sshOutputBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.sshOutputBox, 2);
            this.sshOutputBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sshOutputBox.Enabled = false;
            this.sshOutputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sshOutputBox.Location = new System.Drawing.Point(241, 3);
            this.sshOutputBox.Multiline = true;
            this.sshOutputBox.Name = "sshOutputBox";
            this.tableLayoutPanel1.SetRowSpan(this.sshOutputBox, 2);
            this.sshOutputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.sshOutputBox.Size = new System.Drawing.Size(232, 46);
            this.sshOutputBox.TabIndex = 37;
            this.sshOutputBox.Text = "Veuillez vous connecter";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Enabled = false;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(360, 81);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(113, 22);
            this.button3.TabIndex = 36;
            this.button3.Text = "Envoyer";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button123_Click);
            // 
            // textBox4
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBox4, 2);
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(241, 55);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(232, 26);
            this.textBox4.TabIndex = 35;
            this.textBox4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox4_KeyDown);
            // 
            // UsvPolarPlotControl
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "UsvPolarPlotControl";
            this.Size = new System.Drawing.Size(476, 459);
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
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

    }

    #region variables globales

    //traduction des ordres pour les drones 
    private readonly Dictionary<string, string> ordreMapping = new Dictionary<string, string>()
    {
        ["DEPTH_HOLD"] = "2",
        ["ALT_HOLD"] = "3",
        ["DEPTH_HOLD_FOLLOW"] = "4",
        ["ALT_HOLD_FOLLOW"] = "5",
        ["SURFACE"] = "6",
        ["DISARM"] = "0"
    };
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

    //graph
    private int maxRadius = 50; //les radius sont en pixel
    private int maxDistance = 50; //les distances en m 
    private double scale = 1;    //echelle maxRadius/maxDistance pour avoir des pixels/m

    //stockage des 3 dernieres positions
    private List<(float Distance, int Bearing)> lastPositions = new List<(float, int)>(3);
    private float maxLastPositions = 0; 
    //couleurs affichage points 
    private SolidBrush lastBrush = new SolidBrush(Color.FromArgb(255, 43, 150, 0));
    private SolidBrush previousBrush = new SolidBrush(Color.FromArgb(175,43,150,0)); //(150, 113, 176, 84));
    private SolidBrush oldestBrush = new SolidBrush(Color.FromArgb(100,43,150,0));  //(100, 174, 205, 159));
    
    #endregion

    private void buttonConnect_Click(object sender, EventArgs e)
    {
        if (com_etablie) 
        { 
            sshClient.Disconnect();

            ordreCombo.Enabled = false;
            urgenceCombo.Enabled = false;
            sshOutputBox.Enabled = false;
            textBox4.Enabled = false;
            button1.Enabled = false; button2.Enabled = false; button3.Enabled = false;
            button_connect.Text = "Connect";

            com_etablie = false;
        }
        else
        {
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
                    urgenceCombo.Enabled = true;
                    sshOutputBox.Enabled = true;
                    textBox4.Enabled = true;
                    button1.Enabled = true; button2.Enabled = true; button3.Enabled = true;
                    button_connect.Text = "Disconnect";

                    startingShell();

                    com_etablie = true;

                    DialogResult result = MessageBox.Show("Connexion réussi ! Lancer byobu ?",
                            "Confirmation",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question );
                    
                    if ( result == DialogResult.Yes )
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
            message = ordreCombo.SelectedItem.ToString();
            if (message == "PAS D'ORDRE") { return; }
            else
            {
                try
                {
                    //on traduit grace au dictionaire ordremapping 
                    success = ordreMapping.TryGetValue(message, out messageToSend);
                }
                catch { }
            }
        }
        //bouton ordre urgence
        else
        {
            message = urgenceCombo.SelectedItem.ToString();
            if (message == "PAS D'ORDRE") { return; }
            else
            {
                try
                {
                    success = urgenceMapping.TryGetValue(message, out messageToSend);
                }
                catch { }
            }
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
            }
            catch
            {
                MessageBox.Show("Erreur envoi message, vérifiez la connexion");
            }
        }
    }

    private void decodeAndLabel(string line)
    {
        var lineMatch = Regex.Match(line, @"^(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3}) USV:.*\| AUV#\d+:");
        if (!lineMatch.Success)
            return;

        sshOutputBox.Text = line + Environment.NewLine;

        // Calcul du temps entre messages
        DateTime currentTimestamp;
        if (!DateTime.TryParseExact(lineMatch.Groups[1].Value, "yyyy-MM-dd HH:mm:ss.fff", null, System.Globalization.DateTimeStyles.None, out currentTimestamp))
            return;

        if (lastTimestamp.HasValue)
        {
            double diffSeconds = (currentTimestamp - lastTimestamp.Value).TotalSeconds;

            if (diffSeconds < 6)
                labelDuree.ForeColor = Color.Green;
            else if (diffSeconds <= 21)
                labelDuree.ForeColor = Color.Orange;
            else
                labelDuree.ForeColor = Color.Red;

            labelDuree.Text = $"{diffSeconds:F2} s";
        }
        lastTimestamp = currentTimestamp;

        // Infos USV
        var usvMatch = Regex.Match(line, @"USV:([^,]+), ([^,]+), ([^,]+), ([^|]+) \|");
        if (usvMatch.Success)
        {
            labelGPS.Text = usvMatch.Groups[1].Value.Trim();
            labelOrdreUsv.Text = usvMatch.Groups[4].Value.Trim();
        }

        // Infos AUV
        var auvMatch = Regex.Match(line, @"AUV#\d+:([^,]+), ([\d\.]+)m, (\d+)°");
        if (auvMatch.Success)
        {
            string auvState = auvMatch.Groups[1].Value.Trim();
            string distance = auvMatch.Groups[2].Value.Trim();
            string bearing = auvMatch.Groups[3].Value.Trim();

            // Mise à jour labels
            labelModeAuv.Text = auvState;
            label_Distance.Text = $"{distance:F1} m";
            label_Bearing.Text = $"{bearing}°";

            //couleur emergency/normal
            if (auvState.ToUpper().Contains("EMERGENCY") && labelModeAuv.ForeColor != Color.Red)
            {
                labelModeAuv.ForeColor = Color.Red;
                try
                {
                    //todo comprendre pourquoi ce débile bip tout le temps 
                    //Console.Beep(1000, 100); 
                }
                catch
                {
                    //todo tester le son
                    //System.Media.SystemSounds.Beep.Play();
                }
            }
            else
            {
                labelModeAuv.ForeColor = Color.White;
            }

            //stockage et affichage 
            newPosition(bearing, distance);
        }

    }

    private void newPosition(string bearing, string distance)
    {
        //invariant culture permet de se mettre en americain pour utiliser les points comme virgule 
        lastPositions.Add((float.Parse(distance, CultureInfo.InvariantCulture), int.Parse(bearing, CultureInfo.InvariantCulture)));

        // max 3 points
        if (lastPositions.Count > 3)
        {
            lastPositions.RemoveAt(0);
        }

        pictureBox2.Invalidate(); //va déclencher le re-dessin de la picture box, et ajouter les points grace à l'evenement pictureBox2_Paint

        //check point sur le graph 
        float newDist = float.Parse(distance, CultureInfo.InvariantCulture);

        maxLastPositions = lastPositions.Any() ? lastPositions.Max(p => p.Distance) : 0;

        if (maxLastPositions > maxDistance && maxDistance < 500)
            comboBox1.SelectedIndex += 1;
        else if (maxLastPositions < maxDistance / 2 && maxDistance > 50)
            comboBox1.SelectedIndex -= 1;

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

    #region sim //todo voir si on garde ou pas
    //private void StartSimulation()
    //{
    //    if (simulating) return;
    //    simulating = true;
    //    simulationThread = new Thread(SimLoop);
    //    simulationThread.IsBackground = true;
    //    simulationThread.Start();
    //}

    //private void SimLoop()
    //{
    //    while (simulating)
    //    {
    //        string line = GenerateSimulatedData();
    //        this.Invoke((Action)(() => ParseLine(line)));
    //        Thread.Sleep(1000);
    //    }
    //}

    //private string GenerateSimulatedData()
    //{
    //    string gps = Rand("RTK_FIX", "2D_FIX", "3D_FIX");
    //    string usvStatus = Rand("ARM", "DISARM");
    //    string usvState = Rand("MANUAL", "AUTO");
    //    string order = Rand("ALT_HOLD_FOLLOW", "DEPTH_HOLD");
    //    string auvState = Rand("ALT_HOLD_FOLLOW", "DEPTH_HOLD", "EMERGENCY_LEAK");
    //    float distance = (float)Math.Round(rand.NextDouble() * 40 + 10, 1);
    //    int bearing = rand.Next(0, 360);
    //    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
    //    return $"{timestamp} USV:{gps}, {usvStatus}, {usvState}, {order} | AUV#1:{auvState}, {distance}m, {bearing}°";
    //}
    #endregion

    private void textBox4_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;
        else 
        {
            button123_Click(this.button3, e);
        }
    }

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
        for (int i = 0; i< lastPositions.Count; i++)
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

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        maxDistance = int.Parse(comboBox1.Text); 
    }
}
