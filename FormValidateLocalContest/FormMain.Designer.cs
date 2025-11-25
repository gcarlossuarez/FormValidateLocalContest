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
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // lstProblemas
        // 
        lstProblems.FormattingEnabled = true;
        lstProblems.ItemHeight = 15;
        lstProblems.Location = new Point(12, 455);
        lstProblems.Name = "lstProblemas";
        lstProblems.Size = new Size(400, 94);
        lstProblems.TabIndex = 12;
        lstProblems.SelectedIndexChanged += lstProblems_SelectedIndexChanged;
        // 
        // btnSelectFile
        // 
        btnSelectFile.Location = new Point(12, 12);
        btnSelectFile.Name = "btnSelectFile";
        btnSelectFile.Size = new Size(400, 30);
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
        txtCode.Size = new Size(400, 300);
        txtCode.TabIndex = 1;
        txtCode.WordWrap = false;
        // 
        // btnCompileAndRun
        // 
        btnCompileAndRun.Location = new Point(680, 410);
        btnCompileAndRun.Name = "btnCompileAndRun";
        btnCompileAndRun.Size = new Size(150, 30);
        btnCompileAndRun.TabIndex = 2;
        btnCompileAndRun.Text = "Compilar y Ejecutar";
        btnCompileAndRun.UseVisualStyleBackColor = true;
        btnCompileAndRun.Click += btnCompileAndRun_Click;
        // 
        // txtTimeout
        // 
        txtTimeout.Location = new Point(100, 414);
        txtTimeout.Name = "txtTimeout";
        txtTimeout.Size = new Size(60, 23);
        txtTimeout.TabIndex = 3;
        txtTimeout.Text = "5";
        txtTimeout.TextAlign = HorizontalAlignment.Center;
        // 
        // lblTimeout
        // 
        lblTimeout.AutoSize = true;
        lblTimeout.Location = new Point(12, 418);
        lblTimeout.Name = "lblTimeout";
        lblTimeout.Size = new Size(82, 15);
        lblTimeout.TabIndex = 4;
        lblTimeout.Text = "Timeout (seg):";
        // 
        // statusStrip
        // 
        statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
        statusStrip.Location = new Point(0, 448);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(800, 22);
        statusStrip.TabIndex = 5;
        statusStrip.Text = "statusStrip1";
        // 
        // toolStripStatusLabel
        // 
        toolStripStatusLabel.Name = "toolStripStatusLabel";
        toolStripStatusLabel.Size = new Size(39, 17);
        toolStripStatusLabel.Text = "Listo.";
        // 
        // lblProblemaDir
        // 
        lblProblemaDir.AutoSize = true;
        lblProblemaDir.Location = new Point(12, 362);
        lblProblemaDir.Name = "lblProblemaDir";
        lblProblemaDir.Size = new Size(126, 15);
        lblProblemaDir.TabIndex = 6;
        lblProblemaDir.Text = "Directorio Problemas:";
        // 
        // txtProblemaDir
        // 
        txtProblemaDir.Location = new Point(144, 359);
        txtProblemaDir.Name = "txtProblemaDir";
        txtProblemaDir.Size = new Size(154, 23);
        txtProblemaDir.TabIndex = 7;

        // txtDescripcion
        // 
        txtDescripcion.Location = new Point(200, 390);
        txtDescripcion.Multiline = true;
        txtDescripcion.ScrollBars = ScrollBars.Both;
        txtDescripcion.Name = "txtDescripcion";
        txtDescripcion.ReadOnly = true;
        txtDescripcion.Size = new Size(400, 205);
        txtDescripcion.TabIndex = 13;
        txtDescripcion.WordWrap = false;
        // 
        // btnSelectProblemaDir
        // 
        btnSelectProblemaDir.Location = new Point(304, 357);
        btnSelectProblemaDir.Name = "btnSelectProblemaDir";
        btnSelectProblemaDir.Size = new Size(158, 27);
        btnSelectProblemaDir.TabIndex = 8;
        btnSelectProblemaDir.Text = "Seleccionar Directorio";
        btnSelectProblemaDir.UseVisualStyleBackColor = true;
        btnSelectProblemaDir.Click += btnSelectProblemaDir_Click;
        // 
        // lstResults
        // 
        lstResults.Font = new Font("Consolas", 8F);
        lstResults.FormattingEnabled = true;
        lstResults.HorizontalScrollbar = true;
        lstResults.ItemHeight = 13;
        lstResults.Location = new Point(420, 48);
        lstResults.Name = "lstResults";
        lstResults.Size = new Size(560, 300);
        lstResults.TabIndex = 9;
        // 
        // lblResults
        // 
        lblResults.AutoSize = true;
        lblResults.Location = new Point(420, 30);
        lblResults.Name = "lblResults";
        lblResults.Size = new Size(128, 15);
        lblResults.TabIndex = 10;
        lblResults.Text = "Resultados de Pruebas:";
        // 
        // btnCancel
        // 
        btnCancel.Enabled = false;
        btnCancel.Location = new Point(836, 410);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(144, 30);
        btnCancel.TabIndex = 11;
        btnCancel.Text = "Cancelar";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 670);
        StartPosition = FormStartPosition.CenterScreen;
        Controls.Add(btnCancel);
        Controls.Add(lblResults);
        Controls.Add(lstResults);
        Controls.Add(btnSelectProblemaDir);
        Controls.Add(txtProblemaDir);
        Controls.Add(txtDescripcion);
        Controls.Add(lblProblemaDir);
        Controls.Add(statusStrip);
        Controls.Add(lblTimeout);
        Controls.Add(txtTimeout);
        Controls.Add(btnCompileAndRun);
        Controls.Add(txtCode);
        Controls.Add(lstProblems);
        Controls.Add(btnSelectFile);
        Name = "FormMain";
        Text = "Validador de Programas C# de contest Local";
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
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
}
