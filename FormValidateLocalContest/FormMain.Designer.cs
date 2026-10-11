namespace FormValidateLocalContest;

partial class FormMain
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        btnSelectFile = new Button();
        lstProblems = new ListBox();
        txtCode = new TextBox();
        btnCompileAndRun = new Button();
        txtTimeout = new TextBox();
        lblTimeout = new Label();
        statusStrip = new StatusStrip();
        toolStripStatusLabel = new ToolStripStatusLabel();
        lblProblemaDir = new Label();
        txtProblemaDir = new TextBox();
        txtDescripcion = new TextBox();
        btnSelectProblemaDir = new Button();
        lstResults = new ListBox();
        lblResults = new Label();
        btnCancel = new Button();
        cbDatasets = new ComboBox();
        lblDatasets = new Label();
        txtStdin = new TextBox();
        lblStdin = new Label();
        txtStdout = new TextBox();
        lblStdout = new Label();
        btnRunConsole = new Button();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // btnSelectFile
        // 
        btnSelectFile.Location = new Point(12, 12);
        btnSelectFile.Name = "btnSelectFile";
        btnSelectFile.Size = new Size(976, 30);
        btnSelectFile.TabIndex = 0;
        btnSelectFile.Text = "Seleccionar Archivo C#, copiar código o escribirlo";
        btnSelectFile.UseVisualStyleBackColor = true;
        btnSelectFile.Click += btnSelectFile_Click;
        // 
        // txtCode
        // 
        txtCode.Font = new Font("Consolas", 10F);
        txtCode.Location = new Point(12, 48);
        txtCode.Multiline = true;
        txtCode.Name = "txtCode";
        txtCode.ScrollBars = ScrollBars.Both;
        txtCode.Size = new Size(480, 320);
        txtCode.TabIndex = 1;
        txtCode.WordWrap = false;
        txtCode.TextChanged += TxtCode_TextChanged;
        // 
        // lblProblemaDir
        // 
        lblProblemaDir.AutoSize = true;
        lblProblemaDir.Location = new Point(12, 375);
        lblProblemaDir.Name = "lblProblemaDir";
        lblProblemaDir.Size = new Size(126, 15);
        lblProblemaDir.TabIndex = 6;
        lblProblemaDir.Text = "Directorio Problemas:";
        // 
        // txtProblemaDir
        // 
        txtProblemaDir.Location = new Point(144, 372);
        txtProblemaDir.Name = "txtProblemaDir";
        txtProblemaDir.Size = new Size(200, 23);
        txtProblemaDir.TabIndex = 7;
        txtProblemaDir.ReadOnly = true;
        // 
        // btnSelectProblemaDir
        // 
        btnSelectProblemaDir.Location = new Point(350, 370);
        btnSelectProblemaDir.Name = "btnSelectProblemaDir";
        btnSelectProblemaDir.Size = new Size(142, 27);
        btnSelectProblemaDir.TabIndex = 8;
        btnSelectProblemaDir.Text = "Seleccionar Directorio";
        btnSelectProblemaDir.UseVisualStyleBackColor = true;
        btnSelectProblemaDir.Click += btnSelectProblemaDir_Click;
        // 
        // lblTimeout
        // 
        lblTimeout.AutoSize = true;
        lblTimeout.Location = new Point(12, 407);
        lblTimeout.Name = "lblTimeout";
        lblTimeout.Size = new Size(82, 15);
        lblTimeout.TabIndex = 4;
        lblTimeout.Text = "Timeout (seg):";
        // 
        // txtTimeout
        // 
        txtTimeout.Location = new Point(100, 404);
        txtTimeout.Name = "txtTimeout";
        txtTimeout.Size = new Size(60, 23);
        txtTimeout.TabIndex = 3;
        txtTimeout.Text = "5";
        txtTimeout.TextAlign = HorizontalAlignment.Center;
        // 
        // lblDatasets
        // 
        lblDatasets.AutoSize = true;
        lblDatasets.Location = new Point(170, 407);
        lblDatasets.Name = "lblDatasets";
        lblDatasets.Size = new Size(59, 15);
        lblDatasets.TabIndex = 14;
        lblDatasets.Text = "Datasets:";
        // 
        // cbDatasets
        // 
        cbDatasets.FormattingEnabled = true;
        cbDatasets.Location = new Point(235, 404);
        cbDatasets.Name = "cbDatasets";
        cbDatasets.Size = new Size(257, 23);
        cbDatasets.TabIndex = 15;
        cbDatasets.SelectedIndexChanged += cbDatasets_SelectedIndexChanged;
        // 
        // lstProblems
        // 
        lstProblems.FormattingEnabled = true;
        lstProblems.ItemHeight = 15;
        lstProblems.Location = new Point(12, 440);
        lstProblems.Name = "lstProblemas";
        lstProblems.Size = new Size(480, 200);
        lstProblems.TabIndex = 12;
        lstProblems.SelectedIndexChanged += lstProblems_SelectedIndexChanged;
        // 
        // statusStrip
        // 
        statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
        statusStrip.Location = new Point(0, 655);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1000, 22);
        statusStrip.TabIndex = 5;
        statusStrip.Text = "statusStrip1";
        // 
        // toolStripStatusLabel
        // 
        toolStripStatusLabel.Name = "toolStripStatusLabel";
        toolStripStatusLabel.Size = new Size(39, 17);
        toolStripStatusLabel.Text = "Listo.";
        // 
        // lstResults
        // 
        lstResults.Font = new Font("Consolas", 9F);
        lstResults.FormattingEnabled = true;
        lstResults.HorizontalScrollbar = true;
        lstResults.ItemHeight = 13;
        lstResults.Location = new Point(508, 48);
        lstResults.Name = "lstResults";
        lstResults.Size = new Size(480, 320);
        lstResults.TabIndex = 9;
        // 
        // lblResults
        // 
        lblResults.AutoSize = true;
        lblResults.Location = new Point(508, 30);
        lblResults.Name = "lblResults";
        lblResults.Size = new Size(128, 15);
        lblResults.TabIndex = 10;
        lblResults.Text = "Resultados de Pruebas:";
        // 
        // txtDescripcion
        // 
        txtDescripcion.Font = new Font("Consolas", 9F);
        txtDescripcion.Location = new Point(508, 375);
        txtDescripcion.Multiline = true;
        txtDescripcion.ScrollBars = ScrollBars.Vertical;
        txtDescripcion.Name = "txtDescripcion";
        txtDescripcion.ReadOnly = true;
        txtDescripcion.Size = new Size(480, 265);
        txtDescripcion.TabIndex = 13;
        txtDescripcion.WordWrap = false;
        // 
        // lblStdin
        // 
        lblStdin.AutoSize = true;
        lblStdin.Location = new Point(12, 655);
        lblStdin.Name = "lblStdin";
        lblStdin.Size = new Size(46, 15);
        lblStdin.TabIndex = 16;
        lblStdin.Text = "Entrada:";
        // 
        // txtStdin
        // 
        txtStdin.Font = new Font("Consolas", 9F);
        txtStdin.Location = new Point(12, 675);
        txtStdin.Multiline = true;
        txtStdin.Name = "txtStdin";
        txtStdin.ScrollBars = ScrollBars.Both;
        txtStdin.Size = new Size(480, 120);
        txtStdin.TabIndex = 17;
        txtStdin.WordWrap = false;
        // 
        // lblStdout
        // 
        lblStdout.AutoSize = true;
        lblStdout.Location = new Point(508, 655);
        lblStdout.Name = "lblStdout";
        lblStdout.Size = new Size(52, 15);
        lblStdout.TabIndex = 18;
        lblStdout.Text = "Salida:";
        // 
        // txtStdout
        // 
        txtStdout.Font = new Font("Consolas", 9F);
        txtStdout.Location = new Point(508, 675);
        txtStdout.Multiline = true;
        txtStdout.Name = "txtStdout";
        txtStdout.ReadOnly = true;
        txtStdout.ScrollBars = ScrollBars.Both;
        txtStdout.Size = new Size(480, 120);
        txtStdout.TabIndex = 19;
        txtStdout.WordWrap = false;
        // 
        // btnCompileAndRun
        // 
        btnCompileAndRun.Location = new Point(12, 645);
        btnCompileAndRun.Name = "btnCompileAndRun";
        btnCompileAndRun.Size = new Size(236, 30);
        btnCompileAndRun.TabIndex = 2;
        btnCompileAndRun.Text = "Compilar y Ejecutar";
        btnCompileAndRun.UseVisualStyleBackColor = true;
        btnCompileAndRun.Click += btnCompileAndRun_Click;
        // 
        // btnRunConsole
        // 
        btnRunConsole.Location = new Point(254, 645);
        btnRunConsole.Name = "btnRunConsole";
        btnRunConsole.Size = new Size(238, 30);
        btnRunConsole.TabIndex = 20;
        btnRunConsole.Text = "Ejecutar Consola (Prueba)";
        btnRunConsole.UseVisualStyleBackColor = true;
        btnRunConsole.Click += btnRunConsole_Click;
        // 
        // btnCancel
        // 
        btnCancel.Enabled = false;
        btnCancel.Location = new Point(508, 645);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(480, 30);
        btnCancel.TabIndex = 11;
        btnCancel.Text = "Cancelar";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 820);
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
        Controls.Add(btnRunConsole);
        Controls.Add(txtStdout);
        Controls.Add(lblStdout);
        Controls.Add(txtStdin);
        Controls.Add(lblStdin);
        Controls.Add(btnCancel);
        Controls.Add(btnCompileAndRun);
        Controls.Add(cbDatasets);
        Controls.Add(lblDatasets);
        Controls.Add(txtDescripcion);
        Controls.Add(lstProblems);
        Controls.Add(btnSelectProblemaDir);
        Controls.Add(txtProblemaDir);
        Controls.Add(lblProblemaDir);
        Controls.Add(txtTimeout);
        Controls.Add(lblTimeout);
        Controls.Add(statusStrip);
        Controls.Add(lblResults);
        Controls.Add(lstResults);
        Controls.Add(txtCode);
        Controls.Add(btnSelectFile);
        Name = "FormMain";
        Text = "Validador de Programas C# de contest Local";
        statusStrip.ResumeLayout(false);
        statusStrip.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button btnSelectFile;
    private ListBox lstProblems;
    private TextBox txtCode;
    private Button btnCompileAndRun;
    private TextBox txtTimeout;
    private Label lblTimeout;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel toolStripStatusLabel;
    private Label lblProblemaDir;
    private TextBox txtProblemaDir;
    private TextBox txtDescripcion;
    private Button btnSelectProblemaDir;
    private ListBox lstResults;
    private Label lblResults;
    private Button btnCancel;
    private ComboBox cbDatasets;
    private Label lblDatasets;
    private TextBox txtStdin;
    private Label lblStdin;
    private TextBox txtStdout;
    private Label lblStdout;
    private Button btnRunConsole;
}
