namespace FormValidateLocalContest;

partial class FormMain
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

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
        lblDescripcion = new Label();
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
        grpIOData = new GroupBox();
        statusStrip.SuspendLayout();
        grpIOData.SuspendLayout();
        SuspendLayout();
        
        this.ClientSize = new System.Drawing.Size(1000, 800);
        this.WindowState = FormWindowState.Maximized;
        this.StartPosition = FormStartPosition.CenterScreen;
        
        btnSelectFile.Location = new Point(12, 12);
        btnSelectFile.Size = new Size(476, 30);
        btnSelectFile.Text = "Seleccionar Archivo C#, copiar código o escribirlo";
        btnSelectFile.Click += btnSelectFile_Click;
        
        lblResults.AutoSize = true;
        lblResults.Location = new Point(508, 16);
        lblResults.Text = "Resultados de Pruebas:";
        
        txtCode.Location = new Point(12, 48);
        txtCode.Size = new Size(476, 330);
        txtCode.Font = new Font("Consolas", 10F);
        txtCode.Multiline = true;
        txtCode.ScrollBars = ScrollBars.Both;
        txtCode.WordWrap = false;
        txtCode.TextChanged += TxtCode_TextChanged;
        
        lstResults.Location = new Point(1000, 58);
        lstResults.Size = new Size(300, 352);
        lstResults.Font = new Font("Consolas", 9F);
        lstResults.HorizontalScrollbar = true;
        
        lblProblemaDir.AutoSize = true;
        lblProblemaDir.Location = new Point(12, 390);
        lblProblemaDir.Text = "Directorio:";
        
        txtProblemaDir.Location = new Point(95, 387);
        txtProblemaDir.Size = new Size(180, 23);
        txtProblemaDir.ReadOnly = true;
        
        btnSelectProblemaDir.Location = new Point(281, 387);
        btnSelectProblemaDir.Size = new Size(70, 23);
        btnSelectProblemaDir.Text = "Examinar";
        btnSelectProblemaDir.Click += btnSelectProblemaDir_Click;
        
        lblTimeout.AutoSize = true;
        lblTimeout.Location = new Point(359, 390);
        lblTimeout.Text = "Timeout:";
        
        txtTimeout.Location = new Point(415, 387);
        txtTimeout.Size = new Size(50, 23);
        txtTimeout.Text = "5";
        txtTimeout.TextAlign = HorizontalAlignment.Center;
        
        lblDatasets.AutoSize = true;
        lblDatasets.Location = new Point(505, 412);
        lblDatasets.Text = "Datasets:";
        
        cbDatasets.Location = new Point(558, 407);
        cbDatasets.Size = new Size(428, 23);
        cbDatasets.SelectedIndexChanged += cbDatasets_SelectedIndexChanged;
        
        lstProblems.Location = new Point(12, 420);
        lstProblems.Size = new Size(476, 100);
        lstProblems.SelectedIndexChanged += lstProblems_SelectedIndexChanged;
        
        lblDescripcion.AutoSize = true;
        lblDescripcion.Location = new Point(508, 440);
        lblDescripcion.Text = "Descripción del problema:";

        txtDescripcion.Location = new Point(508, 460);
        txtDescripcion.Size = new Size(476, 200);
        txtDescripcion.Font = new Font("Consolas", 9F);
        txtDescripcion.Multiline = true;
        txtDescripcion.ReadOnly = true;
        txtDescripcion.ScrollBars = ScrollBars.Vertical;
        
        btnCompileAndRun.Location = new Point(12, 550);
        btnCompileAndRun.Size = new Size(230, 35);
        btnCompileAndRun.Text = "Compilar y Ejecutar";
        btnCompileAndRun.Click += btnCompileAndRun_Click;
        
        btnRunConsole.Location = new Point(250, 550);
        btnRunConsole.Size = new Size(230, 35);
        btnRunConsole.Text = "Ejecutar Consola";
        btnRunConsole.Click += btnRunConsole_Click;
        
        btnCancel.Location = new Point(12, 600);
        btnCancel.Size = new Size(476, 35);
        btnCancel.Text = "Cancelar";
        btnCancel.Click += btnCancel_Click;
        
        grpIOData.Location = new Point(508, 48);
        grpIOData.Size = new Size(476, 362);
        grpIOData.Text = "Entrada / Salida";
        
        lblStdin.AutoSize = true;
        lblStdin.Location = new Point(10, 20);
        lblStdin.Text = "Entrada (stdin):";
        
        txtStdin.Location = new Point(10, 40);
        txtStdin.Size = new Size(456, 150);
        txtStdin.Font = new Font("Consolas", 9F);
        txtStdin.Multiline = true;
        txtStdin.ScrollBars = ScrollBars.Both;
        txtStdin.WordWrap = false;
        
        lblStdout.AutoSize = true;
        lblStdout.Location = new Point(10, 200);
        lblStdout.Text = "Salida (stdout):";
        
        txtStdout.Location = new Point(10, 220);
        txtStdout.Size = new Size(456, 130);
        txtStdout.Font = new Font("Consolas", 9F);
        txtStdout.Multiline = true;
        txtStdout.ReadOnly = true;
        txtStdout.ScrollBars = ScrollBars.Both;
        txtStdout.WordWrap = false;
        
        statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
        statusStrip.Location = new Point(0, 778);
        statusStrip.Size = new Size(1000, 22);
        
        toolStripStatusLabel.Text = "Listo.";
        
        grpIOData.Controls.Add(lblStdout);
        grpIOData.Controls.Add(txtStdout);
        grpIOData.Controls.Add(lblStdin);
        grpIOData.Controls.Add(txtStdin);
        
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 800);
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
        Controls.Add(btnRunConsole);
        Controls.Add(btnCancel);
        Controls.Add(btnCompileAndRun);
        Controls.Add(grpIOData);
        Controls.Add(cbDatasets);
        Controls.Add(lblDatasets);
        Controls.Add(lblDescripcion);
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
        grpIOData.ResumeLayout(false);
        grpIOData.PerformLayout();
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
    private Label lblDescripcion;
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
    private GroupBox grpIOData;
}
