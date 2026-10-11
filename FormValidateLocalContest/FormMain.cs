

/*
 * =============================================
 *  BOOKMARK: VALIDATION FLOW WITH PER-PROBLEM VALIDATOR
 * =============================================
 *
 * For each test (input file):
 *   1. Compile and run the student's program with the input file.
 *   2. Save the student's output to a file.
 *   3. Check if a Validator<id> subdirectory exists for this test:
 *      - If YES:
 *          a. Compile the validator's .csproj.
 *          b. Run the validator, passing as arguments:
 *             [inputPath] [expectedPath] [actualPath]
 *          c. If the validator prints "OK" and exits with code 0, the test is correct.
 *             Otherwise, the test is incorrect and the validator's output is shown.
 *      - If NO:
 *          a. Compare the student's output with the expected output (normalized).
 *          b. If they match, the test is correct; otherwise, incorrect.
 *
 *   4. Show the result in the UI for each test.
 *
 *
 * Text Diagram:
 *
 * [START TEST]
 *     |
 *     v
 * [Compile & Run Student]
 *     |
 *     v
 * Does Validator<id> exist?
 *   /        \
 *  Yes        No
 *  |           |
 * [Compile   [Compare
 *  Validator]  outputs]
 *  |           |
 * [Run       Do outputs
 *  Validator]  match?
 *  |        /      \
 *OK&0?   Yes      No
 * /   \   |        |
 *Yes  No [OK]   [WRONG]
 * |    |
 *[OK] [Custom message]
 *
 * This allows advanced, per-problem validation logic, or default output comparison.
 */

using System.Diagnostics;
using System.Text;

namespace FormValidateLocalContest;

public partial class FormMain : Form
{
    // Controles de progreso para la descompresión
    private ProgressBar progressBarExtract;
    private Label labelExtractStatus;

    /*
     * ===============================
     *  BOOKMARK: DIAGRAMA DE FLUJO PRINCIPAL
     * ===============================
     *
     * [Inicio App]
     *     |
     *     v
     * [Extrae ZIP de problemas]
     *     |
     *     v
     * [Usuario selecciona problema]
     *     |
     *     v
     * [Lee descripción .docx y muestra en TextBox]
     *     |
     *     v
     * [Usuario carga código y ejecuta pruebas]
     *     |
     *     v
     * [Por cada test:]
     *     |
     *     +--> [Lee datos*.txt como UTF-8 sin BOM]
     *     |
     *     +--> [Guarda input temporal como UTF-8 sin BOM]
     *     |
     *     +--> [Ejecuta programa usuario con cmd.exe/chcp 65001/type archivo | exe]
     *     |         |
     *     |         +--> [Redirige salida estándar y error]
     *     |         +--> [Lee salida como UTF-8]
     *     |
     *     +--> [Guarda salida generada como UTF-8 sin BOM]
     *     |
     *     +--> [Lee Output esperado como UTF-8]
     *     |
     *     +--> [Normaliza y compara]
     *     |
     *     +--> [Muestra resultado en UI]
     *     |
     *     v
     * [Resumen y limpieza]
     *
     *
     * ===============================
     *  NOTAS SOBRE ENCODING
     * ===============================
     *
     * - Todos los archivos de entrada y salida se leen/escriben como UTF-8 sin BOM.
     * - El proceso hijo (programa del usuario) se ejecuta bajo cmd.exe con chcp 65001 (UTF-8 en consola Windows).
     * - La salida estándar y de error del proceso hijo se lee como UTF-8 (StandardOutputEncoding = Encoding.UTF8).
     * - Esto asegura que los caracteres especiales y acentos se transmitan correctamente de extremo a extremo.
     *
     * - Si el programa del usuario imprime en otra codificación (ej: Windows-1252), se debe ajustar StandardOutputEncoding.
     *   Pero para .NET modernos y la mayoría de compiladores recientes, UTF-8 es lo más robusto.
     */


    private string? selectedFilePath;
    private string? selectedProblem;
    private string? problemaDirectory;
    private CancellationTokenSource? cancellationTokenSource;

    // Ruta donde se extrae el ZIP incrustado
    private string? extractedProblemsPath;
    
    // Archivo de datos seleccionado (datos*.txt)
    private string? selectedDataFile;

    public FormMain()
    {
        InitializeComponent();

        // Agregar ProgressBar y Label para mostrar progreso de descompresión
        progressBarExtract = new ProgressBar
        {
            Minimum = 0,
            Maximum = 100,
            Value = 0,
            Visible = false,
            Width = 300,
            Height = 20,
            Top = 10,
            Left = this.ClientSize.Width - 330
        };
        labelExtractStatus = new Label
        {
            Text = "",
            Visible = false,
            AutoSize = true,
            Top = progressBarExtract.Bottom + 5,
            Left = this.ClientSize.Width - 330
        };
        Controls.Add(progressBarExtract);
        Controls.Add(labelExtractStatus);

        // Al iniciar, extrae el ZIP de problemas y llena el ListBox.
        // Se dispara en un Task para no bloquear la UI y que se pueda visualizar el
        // formulario mientras se extrae.
        Task.Run(async () =>
        {
            btnCompileAndRun.Enabled = false;
            btnRunConsole.Enabled = false;
            btnSelectProblemaDir.Enabled = false;
            await ExtractEmbeddedZipToTempWithProgress();
            // Una vez extraído, llena el ListBox en el hilo de la UI
            Invoke(new Action(() => PopulateProblemsListBox()));
            Invoke(new Action(() =>
            {
                btnCompileAndRun.Enabled = true;
                btnSelectProblemaDir.Enabled = true;
                btnRunConsole.Enabled = true;
            }));
        });

        // Habilita encodings extendidos (Windows-1252, etc.) en .NET Core/5+/6+/7+/8+
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    /// <summary>
    /// Normaliza el texto de código para asegurar que los saltos de línea se preserven correctamente.
    /// Maneja diferentes formatos de saltos de línea (CRLF, LF) y garantiza UTF-8 consistente.
    /// </summary>
    private string NormalizeCodeText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        // Reemplaza todos los tipos de saltos de línea con \n primero
        text = text.Replace("\r\n", "\n").Replace("\r", "\n");

        // Luego convierte \n a Environment.NewLine (CRLF en Windows)
        text = text.Replace("\n", Environment.NewLine);

        return text;
    }

    /// <summary>
    /// BOOKMARK: Llena el ListBox lstProblemas con los nombres de los problemas extraídos del ZIP.
    /// </summary>
    private void PopulateProblemsListBox()
    {
        if (string.IsNullOrEmpty(extractedProblemsPath) || !Directory.Exists(extractedProblemsPath))
            return;

        // Se asume que cada subdirectorio es un problema
        var dirs = Directory.GetDirectories(extractedProblemsPath);
        lstProblems.Items.Clear();
        foreach (var dir in dirs)
        {
            lstProblems.Items.Add(Path.GetFileName(dir));
        }
    }

    /// <summary>
    /// BOOKMARK: Extrae el recurso incrustado Problems.zip a un directorio temporal y retorna la ruta de extracción.
    /// </summary>
    /// <returns>Ruta del directorio donde se extrajo el ZIP</returns>
    // Nueva versión asíncrona con progreso
    private async Task ExtractEmbeddedZipToTempWithProgress()
    {
        Invoke(new Action(() =>
        {
            progressBarExtract.Value = 0;
            progressBarExtract.Visible = true;
            labelExtractStatus.Text = "Descomprimiendo problemas...";
            labelExtractStatus.Visible = true;
            Refresh();
        }));

        var resourceName = "FormValidateLocalContest.Problems.zip";
        var tempDir = Path.Combine(Path.GetTempPath(), "ProblemasContest_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        using var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            Invoke(new Action(() =>
            {
                progressBarExtract.Visible = false;
                labelExtractStatus.Text = "No se encontró el recurso incrustado: " + resourceName;
            }));
            MessageBox.Show($"No se encontró el recurso incrustado: {resourceName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            throw new FileNotFoundException($"No se encontró el recurso incrustado: {resourceName}");
        }

        // Guardar el ZIP temporalmente
        var tempZipPath = Path.Combine(tempDir, "Problems.zip");
        using (var fileStream = File.Create(tempZipPath))
        {
            await stream.CopyToAsync(fileStream);
        }

        // Leer entradas del ZIP para mostrar progreso
        using (var zip = System.IO.Compression.ZipFile.OpenRead(tempZipPath))
        {
            int total = zip.Entries.Count;
            int current = 0;
            foreach (var entry in zip.Entries)
            {
                string destPath = Path.Combine(tempDir, entry.FullName);
                string? destDir = Path.GetDirectoryName(destPath);
                if (!string.IsNullOrEmpty(destDir)) Directory.CreateDirectory(destDir);
                if (!entry.FullName.EndsWith("/"))
                {
                    using var entryStream = entry.Open();
                    using var outStream = File.Create(destPath);
                    await entryStream.CopyToAsync(outStream);
                }
                current++;
                int percent = (int)(current * 100.0 / total);
                // BOOKMARK Invoke(new Action(() =>):
                // La instrucción `Invoke(new Action(() { ... }))` ejecuta el bloque
                // de código dentro del hilo principal de la interfaz gráfica (UI 
                // thread). 
                // Esto es necesario porque los controles de WinForms solo pueden ser 
                // modificados desde el hilo de la UI.
                // Dentro de ese bloque, se actualizan el valor de la barra de 
                // progreso y el texto del label, y se llama a `Refresh()` para forzar
                // el repintado inmediato de los controles.
                // IMPORTANTE:
                // No es exactamente equivalente a `Application.DoEvents()`.  
                // - `Invoke(...)` asegura que el código se ejecute en el hilo 
                // correcto (UI), útil cuando el trabajo se hace en un hilo secundario.
                // - `Application.DoEvents()` procesa todos los mensajes pendientes de
                // la cola de eventos de la aplicación, permitiendo que la UI responda 
                // mientras se ejecuta código largo en el hilo principal.
                // En este caso, `Invoke(...)` es lo correcto para actualizar la UI 
                // desde un hilo de fondo.
                Invoke(new Action(() =>
                {
                    progressBarExtract.Value = percent;
                    labelExtractStatus.Text = $"Descomprimiendo: {percent}% ({current}/{total})";
                    Refresh();
                }));
            }
        }

        // Eliminar el ZIP temporal
        File.Delete(tempZipPath);

        extractedProblemsPath = Path.Combine(tempDir, "Problems");

        Invoke(new Action(() =>
        {
            progressBarExtract.Visible = false;
            labelExtractStatus.Visible = false;
        }));
    }

    private void btnSelectFile_Click(object sender, EventArgs e)
    {
        using OpenFileDialog openFileDialog = new()
        {
            Filter = "Archivos C#|*.cs|Todos los archivos|*.*",
            Title = "Seleccionar archivo C#"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            selectedFilePath = openFileDialog.FileName;
            try
            {
                string fileContent = File.ReadAllText(selectedFilePath, Encoding.UTF8);
                txtCode.Text = NormalizeCodeText(fileContent);
                toolStripStatusLabel.Text = $"Archivo cargado: {Path.GetFileName(selectedFilePath)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer el archivo: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void btnSelectProblemaDir_Click(object sender, EventArgs e)
    {
        using FolderBrowserDialog folderDialog = new()
        {
            Description = "Seleccionar directorio del problema",
            ShowNewFolderButton = false
        };

        if (folderDialog.ShowDialog() == DialogResult.OK)
        {
            problemaDirectory = folderDialog.SelectedPath;
            txtProblemaDir.Text = problemaDirectory;
            toolStripStatusLabel.Text = $"Directorio de problema: {problemaDirectory}";
        }
    }

    private async void btnCompileAndRun_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtCode.Text))
        {
            MessageBox.Show("No hay código para compilar.", "Advertencia", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(problemaDirectory) || !Directory.Exists(problemaDirectory))
        {
            MessageBox.Show("Debe seleccionar un directorio de problema válido.", "Advertencia", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!int.TryParse(txtTimeout.Text, out int timeout) || timeout <= 0)
        {
            MessageBox.Show("El timeout debe ser un número entero positivo.", "Advertencia", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnCompileAndRun.Enabled = false;
        btnSelectProblemaDir.Enabled = false;
        btnCancel.Enabled = true;
        lstResults.Items.Clear();
        toolStripStatusLabel.Text = "Compilando...";
        Application.DoEvents();

        cancellationTokenSource = new CancellationTokenSource();

        try
        {
            await CompileAndRunTests(timeout, cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            toolStripStatusLabel.Text = "Ejecución cancelada por el usuario.";
            lstResults.Items.Add("\u274c Ejecución cancelada.");
        }
        catch (Exception ex)
        {
            toolStripStatusLabel.Text = $"Error: {ex.Message}";
            MessageBox.Show($"Error durante la ejecución: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnCompileAndRun.Enabled = true;
            btnSelectProblemaDir.Enabled = true;
            btnCancel.Enabled = false;
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        cancellationTokenSource?.Cancel();
        btnCancel.Enabled = false;
    }

    /// <summary>
    /// Evento: normaliza el texto del código cuando se pega o se escribe.
    /// Esto asegura que los saltos de línea se preserven correctamente.
    /// </summary>
    private static bool isNormalizing = false;

    private void TxtCode_TextChanged(object sender, EventArgs e)
    {
        // Evita recursión infinita al normalizar
        if (isNormalizing) return;

        isNormalizing = true;
        try
        {
            string originalText = txtCode.Text;
            string normalizedText = NormalizeCodeText(originalText);

            // Solo actualiza si el texto cambió después de normalizar
            if (originalText != normalizedText)
            {
                int cursorPos = txtCode.SelectionStart;
                txtCode.Text = normalizedText;
                // Restaura la posición del cursor aproximadamente
                txtCode.SelectionStart = Math.Min(cursorPos, txtCode.Text.Length);
            }
        }
        finally
        {
            isNormalizing = false;
        }
    }

    /// <summary>
    /// Evento: cuando el usuario selecciona un problema del ListBox, se actualiza el directorio de trabajo y se muestra la descripción.
    /// También carga los datasets disponibles.
    /// </summary>
    private void lstProblems_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lstProblems.SelectedItem == null || string.IsNullOrEmpty(extractedProblemsPath))
            return;

        // Construir la ruta del problema seleccionado
        selectedProblem = lstProblems.SelectedItem.ToString()!;
        string problemPath = Path.Combine(extractedProblemsPath, selectedProblem);

        // Actualizar el textbox y la variable problemaDirectory para que el validador use este dataset
        txtProblemaDir.Text = problemPath;
        problemaDirectory = problemPath;
        toolStripStatusLabel.Text = $"Problema seleccionado: {selectedProblem}";

        // Cargar datasets disponibles
        LoadAvailableDatasets(problemPath);

        // Buscar y mostrar la descripción del problema (.docx)
        try
        {
            string[] docxFiles = Directory.GetFiles(problemPath, "*.docx");
            if (docxFiles.Length > 0)
            {
                string docxPath = docxFiles[0];
                txtDescripcion.Text = ReadDocxLikeText(docxPath);
            }
            else
            {
                txtDescripcion.Text = "No se encontró archivo .docx de descripción en el problema.";
            }
        }
        catch (Exception ex)
        {
            txtDescripcion.Text = $"Error al leer la descripción: {ex.Message}";
        }
    }

    /// <summary>
    /// Carga los archivos datos*.txt disponibles del problema en el ComboBox y limpia el stdin/stdout.
    /// </summary>
    /// <param name="problemPath">Ruta del directorio del problema</param>
    private void LoadAvailableDatasets(string problemPath)
    {
        cbDatasets.Items.Clear();
        selectedDataFile = null;
        txtStdin.Clear();
        txtStdout.Clear();

        try
        {
            string inDir = Path.Combine(problemPath, "IN");
            
            if (!Directory.Exists(inDir))
            {
                cbDatasets.Items.Add("No hay directorio IN");
                cbDatasets.SelectedIndex = 0;
                cbDatasets.Enabled = false;
                return;
            }

            var dataFiles = Directory.GetFiles(inDir, "datos*.txt").OrderBy(f => f).ToList();

            if (dataFiles.Count == 0)
            {
                cbDatasets.Items.Add("No hay archivos datos*.txt");
                cbDatasets.SelectedIndex = 0;
                cbDatasets.Enabled = false;
                return;
            }

            // Agregar archivos encontrados
            foreach (var dataFile in dataFiles)
            {
                string fileName = Path.GetFileName(dataFile);
                cbDatasets.Items.Add(fileName);
            }

            cbDatasets.SelectedIndex = 0;
            cbDatasets.Enabled = true;
        }
        catch (Exception ex)
        {
            cbDatasets.Items.Add($"Error: {ex.Message}");
            cbDatasets.SelectedIndex = 0;
            cbDatasets.Enabled = false;
        }
    }

    /// <summary>
    /// Evento: cuando el usuario selecciona un archivo datos*.txt, carga su contenido en txtStdin.
    /// </summary>
    private void cbDatasets_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbDatasets.SelectedIndex < 0 || string.IsNullOrEmpty(problemaDirectory))
        {
            txtStdin.Clear();
            return;
        }

        string selectedDataset = cbDatasets.SelectedItem?.ToString() ?? "";

        if (selectedDataset.Contains("No hay") || selectedDataset.Contains("Error"))
        {
            txtStdin.Clear();
            return;
        }

        // Construir ruta completa del archivo
        string dataFile = Path.Combine(problemaDirectory, "IN", selectedDataset);
        selectedDataFile = dataFile;

        try
        {
            if (File.Exists(dataFile))
            {
                // Leer contenido como UTF-8 sin BOM
                using (var reader = new StreamReader(dataFile, new UTF8Encoding(false)))
                {
                    txtStdin.Text = NormalizeCodeText(reader.ReadToEnd());
                }
                toolStripStatusLabel.Text = $"Dataset cargado: {selectedDataset}";
            }
            else
            {
                txtStdin.Text = $"Error: No se pudo encontrar el archivo {dataFile}";
            }
        }
        catch (Exception ex)
        {
            txtStdin.Text = $"Error al leer el archivo: {ex.Message}";
        }
    }

    /// <summary>
    /// Lee el contenido de un archivo .docx y lo retorna como texto plano usando DocumentFormat.OpenXml.
    /// </summary>
    /// <param name="docxPath">Ruta al archivo .docx</param>
    /// <returns>Texto plano extraído del documento</returns>
    private string ReadDocxLikeText(string docxPath)
    {
        try
        {
            var sb = new System.Text.StringBuilder();
            using (var wordDoc = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open(docxPath, false))
            {
                var mainPart = wordDoc.MainDocumentPart;
                var document = mainPart?.Document;
                var body = document?.Body;
                if (body != null)
                {
                    foreach (var element in body.Elements())
                    {
                        if (element is DocumentFormat.OpenXml.Wordprocessing.Paragraph para)
                        {
                            sb.AppendLine(para.InnerText);
                        }
                        else if (element is DocumentFormat.OpenXml.Wordprocessing.Table table)
                        {
                            foreach (var row in table.Elements<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
                            {
                                var cellTexts = row.Elements<DocumentFormat.OpenXml.Wordprocessing.TableCell>()
                                    .Select(cell => GetCellTextWithBreaks(cell));
                                sb.AppendLine(string.Join("\t", cellTexts));
                            }
                            sb.AppendLine(); // Salto de línea después de cada tabla
                        }
                    }
                }
                else
                {
                    sb.AppendLine("No se pudo leer el contenido del documento.");
                }
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Error al leer el archivo .docx: {ex.Message}";
        }

        // Método auxiliar para obtener el texto de una celda respetando los saltos de línea internos
        string GetCellTextWithBreaks(DocumentFormat.OpenXml.Wordprocessing.TableCell cell)
        {
            var cellSb = new System.Text.StringBuilder();
            foreach (var para in cell.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>())
            {
                foreach (var run in para.Elements<DocumentFormat.OpenXml.Wordprocessing.Run>())
                {
                    foreach (var child in run.ChildElements)
                    {
                        if (child is DocumentFormat.OpenXml.Wordprocessing.Text text)
                        {
                            cellSb.Append(text.Text);
                        }
                        else if (child is DocumentFormat.OpenXml.Wordprocessing.Break)
                        {
                            cellSb.AppendLine();
                        }
                    }
                }
                cellSb.AppendLine(); // Salto de línea al final de cada párrafo
            }
            return cellSb.ToString().TrimEnd();
        }
    }

    /// <summary>
    /// BOOKMARK: Ejecuta el programa compilado con el contenido del txtStdin como entrada estándar.
    /// Muestra el resultado en txtStdout. Esta es una prueba rápida, no evaluación oficial.
    /// </summary>
    private async void btnRunConsole_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(problemaDirectory))
        {
            MessageBox.Show("Selecciona un problema primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrEmpty(txtCode.Text))
        {
            MessageBox.Show("Escribe o carga código C# primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            btnRunConsole.Enabled = false;
            btnCompileAndRun.Enabled = false;
            txtStdout.Text = "Compilando y ejecutando...";
            Application.DoEvents();

            // Buscar el .csproj
            string[] csprojFiles = Directory.GetFiles(problemaDirectory!, "*.csproj");
            if (csprojFiles.Length == 0)
            {
                MessageBox.Show("No se encontró un archivo .csproj en el problema", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string csprojPath = csprojFiles[0];
            const int buildTimeoutMs = 300_000;

            // Crear directorio de trabajo
            string workDir = Path.Combine(problemaDirectory!, $"WorkDir_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}");
            Directory.CreateDirectory(workDir);

            try
            {
                // Copiar .csproj
                File.Copy(csprojPath, Path.Combine(workDir, Path.GetFileName(csprojPath)), true);

                // Guardar código en Program.cs
                string programPath = Path.Combine(workDir, "Program.cs");
                await File.WriteAllTextAsync(programPath, txtCode.Text, Encoding.UTF8);

                // Compilar
                var compileResult = await RunProcessAsync("dotnet", "build", workDir, buildTimeoutMs);
                
                if (compileResult.ExitCode != 0)
                {
                    txtStdout.Text = $"❌ Error de compilación:\n{compileResult.Output}\n{compileResult.Error}";
                    return;
                }

                // Buscar el ejecutable
                var binDebug = Path.Combine(workDir, "bin", "Debug");
                if (!Directory.Exists(binDebug))
                {
                    txtStdout.Text = "❌ No se encontró el directorio bin/Debug";
                    return;
                }

                var frameworks = Directory.GetDirectories(binDebug);
                string? exePath = null;
                foreach (var fwDir in frameworks)
                {
                    var exes = Directory.GetFiles(fwDir, "*.exe");
                    if (exes.Length > 0)
                    {
                        exePath = exes[0];
                        break;
                    }
                }

                if (string.IsNullOrEmpty(exePath))
                {
                    txtStdout.Text = "❌ No se encontró el ejecutable compilado";
                    return;
                }

                // Guardar stdin en archivo temporal
                string tempInputFile = Path.Combine(Path.GetTempPath(), $"input_{Guid.NewGuid():N}.txt");
                await File.WriteAllTextAsync(tempInputFile, txtStdin.Text, new UTF8Encoding(false));

                try
                {
                    // Ejecutar con stdin
                    int timeout = int.TryParse(txtTimeout.Text, out int parsedTimeout) ? parsedTimeout : 30;
                    var runResult = await RunProcessWithRedirectionAsync(exePath, tempInputFile, problemaDirectory!, timeout * 1000);

                    if (runResult.TimedOut)
                    {
                        txtStdout.Text = $"⏱️ Timeout - El programa tardó más de {timeout} segundos";
                    }
                    else if (runResult.ExitCode != 0)
                    {
                        txtStdout.Text = $"❌ Error (Código {runResult.ExitCode}):\r\n{runResult.Error}\r\n\r\n📤 Salida:\r\n{runResult.Output}";
                    }
                    else
                    {
                        txtStdout.Text = $"✅ Ejecutado exitosamente\r\n\r\n📤 Salida:\r\n{runResult.Output}";
                    }

                    toolStripStatusLabel.Text = "Ejecución de consola completada";
                }
                finally
                {
                    try { File.Delete(tempInputFile); } catch { }
                }
            }
            finally
            {
                // Limpiar directorio de trabajo
                try { Directory.Delete(workDir, true); } catch { }
            }
        }
        catch (Exception ex)
        {
            txtStdout.Text = $"❌ Error: {ex.Message}";
        }
        finally
        {
            btnRunConsole.Enabled = true;
            btnCompileAndRun.Enabled = true;
        }
    }


    /// <summary>
    /// BOOKMARK: Compila el código fuente proporcionado y ejecuta las pruebas en el
    /// directorio del problema.
    /// </summary>
    /// <param name="timeoutSeconds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="IOException"></exception>
    private async Task CompileAndRunTests(int timeoutSeconds, CancellationToken cancellationToken)
    {
        // Verificar estructura del problema
        string inDir = Path.Combine(problemaDirectory!, "IN");
        string outDir = Path.Combine(problemaDirectory!, "OUT");
        
        if (!Directory.Exists(inDir))
        {
            throw new DirectoryNotFoundException($"No se encontró el directorio IN en {problemaDirectory}");
        }

        if (!Directory.Exists(outDir))
        {
            throw new DirectoryNotFoundException($"No se encontró el directorio OUT en {problemaDirectory}");
        }

        // Buscar el archivo .csproj en el directorio del problema (NO en el dataset)
        string[] csprojFiles = Directory.GetFiles(problemaDirectory!, "*.csproj");
        if (csprojFiles.Length == 0)
        {
            throw new FileNotFoundException("No se encontró un archivo .csproj en el directorio del problema");
        }

        string csprojPath = csprojFiles[0];
        string projectName = Path.GetFileNameWithoutExtension(csprojPath);

        // Always use a unique WorkDir per run to avoid conflicts and locking issues
        string workDir = Path.Combine(problemaDirectory!, $"WorkDir_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}");
        int createRetries = 0;
        const int maxCreateRetries = 50;
        bool created = false;
        Exception? lastCreateEx = null;
        while (!created && createRetries < maxCreateRetries)
        {
            try
            {
                Directory.CreateDirectory(workDir);
                // Intentar obtener bloqueo exclusivo creando y abriendo un archivo temporal
                string lockFile = Path.Combine(workDir, "__lock.tmp");
                using (var fs = new FileStream(lockFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    // Si llegamos aquí, tenemos acceso exclusivo
                    created = Directory.Exists(workDir);
                }
                File.Delete(lockFile);
            }
            catch (Exception ex)
            {
                lastCreateEx = ex;
                MessageBox.Show($"Error al crear el directorio de trabajo o al obtener acceso exclusivo:\n{workDir}\n\n{ex}", "Error WorkDir", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await Task.Delay(200);
            }
            createRetries++;
        }
        if (!created)
        {
            throw new IOException($"Failed to create working directory: {workDir}\n{lastCreateEx}");
        }

        // Clean up old WorkDirs in the background (best effort, non-blocking)
        _ = Task.Run(() =>
        {
            try
            {
                var parent = Directory.GetParent(workDir)?.FullName ?? problemaDirectory!;
                var oldDirs = Directory.GetDirectories(parent, "WorkDir_*");
                foreach (var dir in oldDirs)
                {
                    try
                    {
                        if (dir != workDir)
                        {
                            Directory.Delete(dir, true);
                        }
                    }
                    catch { /* Ignore errors, best effort */ }
                }
            }
            catch { }
        });

        toolStripStatusLabel.Text = "Preparando directorio de trabajo...";
        Application.DoEvents();

        // Timeout extendido para compilación: 5 minutos (300000 ms)
        const int buildTimeoutMs = 300_000;
        try
        {
            // Copiar el .csproj al directorio de trabajo con reintentos
            string workCsprojPath = Path.Combine(workDir, Path.GetFileName(csprojPath));
            int copyTries = 0;
            while (copyTries < 10)
            {
                try { File.Copy(csprojPath, workCsprojPath, true); break; }
                catch { await Task.Delay(100); copyTries++; }
            }
            // BOOKMARK: Guardar el código en Program.cs en el directorio de trabajo con reintentos
            string programPath = Path.Combine(workDir, "Program.cs");
            int writeTries = 0;
            while (writeTries < 10)
            {
                try { await File.WriteAllTextAsync(programPath, txtCode.Text, Encoding.UTF8); break; }
                catch { await Task.Delay(100); writeTries++; }
            }

            // Compilar en el directorio de trabajo
            toolStripStatusLabel.Text = "Compilando el proyecto...";
            Application.DoEvents();

            // BOOKMARK: Compilar el código del estudiante usando 'dotnet build' en el directorio de trabajo
            var compileResult = await RunProcessAsync("dotnet", "build", workDir, buildTimeoutMs);
            
            if (compileResult.ExitCode != 0)
            {
                toolStripStatusLabel.Text = "Error de compilación";
                MessageBox.Show($"Error de compilación:\n{compileResult.Output}\n{compileResult.Error}", 
                    "Error de Compilación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Esperar un momento para asegurar que todos los archivos se escribieron
            await Task.Delay(100);

            // Buscar el ejecutable compilado en el directorio de trabajo
            string binPath = Path.Combine(workDir, "bin", "Debug");
            string[] frameworks = Directory.GetDirectories(binPath);
            if (frameworks.Length == 0)
            {
                throw new DirectoryNotFoundException("No se encontró el directorio del framework en bin/Debug");
            }

            // Buscar el .exe generado en cualquier framework
            string? exePath = null;
            int exeTries = 0;
            while (exeTries < 10 && exePath == null)
            {
                foreach (var fwDir in frameworks)
                {
                    var exeCandidate = Path.Combine(fwDir, $"{projectName}.exe");
                    if (File.Exists(exeCandidate))
                    {
                        // Intentar abrir con acceso exclusivo
                        try
                        {
                            using (var fs = new FileStream(exeCandidate, FileMode.Open, FileAccess.Read, FileShare.None)) { }
                            exePath = exeCandidate;
                            break;
                        }
                        catch { }
                    }
                }
                if (exePath == null)
                {
                    foreach (var fwDir in frameworks)
                    {
                        var exes = Directory.GetFiles(fwDir, "*.exe");
                        foreach (var ex in exes)
                        {
                            try
                            {
                                using (var fs = new FileStream(ex, FileMode.Open, FileAccess.Read, FileShare.None)) { }
                                exePath = ex;
                                break;
                            }
                            catch { }
                        }
                        if (exePath != null) break;
                    }
                }
                if (exePath == null) await Task.Delay(200);
                exeTries++;
            }
            if (exePath == null)
            {
                throw new FileNotFoundException($"No se encontró ningún ejecutable .exe en {binPath} o subdirectorios.\nVerifique que la compilación fue exitosa y que el proyecto es de tipo WinExe/Exe.");
            }

            toolStripStatusLabel.Text = "Compilación exitosa. Ejecutando tests...";
            Application.DoEvents();

            // Crear/limpiar directorio OutputGenerado en el directorio del problema original
            string outputGeneradoDir = Path.Combine(problemaDirectory!, "OutputGenerado");
            if (Directory.Exists(outputGeneradoDir))
            {
                try
                {
                    Directory.Delete(outputGeneradoDir, true);
                    // Esperar a que Windows termine de eliminar el directorio
                    await Task.Delay(100);
                    
                    // Verificar que realmente se eliminó
                    int retries = 0;
                    while (Directory.Exists(outputGeneradoDir) && retries < 10)
                    {
                        await Task.Delay(50);
                        retries++;
                    }
                }
                catch
                {
                    // Si falla, intentar con nombre alternativo
                    outputGeneradoDir = Path.Combine(problemaDirectory!, $"OutputGenerado_{DateTime.Now:yyyyMMddHHmmss}");
                }
            }
            Directory.CreateDirectory(outputGeneradoDir);
            
            // Verificar que el directorio fue creado correctamente
            if (!Directory.Exists(outputGeneradoDir))
            {
                throw new IOException($"No se pudo crear el directorio de salida: {outputGeneradoDir}");
            }

            // Obtener archivos de entrada
            var inputFiles = Directory.GetFiles(inDir, "datos*.txt")
                .OrderBy(f => f)
                .ToList();

            if (inputFiles.Count == 0)
            {
                throw new FileNotFoundException("No se encontraron archivos datos*.txt en el directorio IN");
            }

            // BOOKMARK: --- Custom Validator Support (verificar si existe validador) ---
            // If Validator<id> exists, compile and run it
            string validatorDir = Path.Combine(problemaDirectory!, $"Validator{selectedProblem}");
            string? validatorExe = null;
            if (Directory.Exists(validatorDir))
            {
                // Find .csproj
                var validatorProj = Directory.GetFiles(validatorDir, "*.csproj").FirstOrDefault();
                if (validatorProj != null)
                {
                    // BOOKMARK: Compilar validator
                    var validatorBuild = await RunProcessAsync("dotnet", $"build \"{validatorProj}\"", validatorDir, buildTimeoutMs);
                    if (validatorBuild.ExitCode == 0)
                    {
                        // BOOKMARK: Buscar el validator exe con reintentos y bloqueo exclusivo
                        var binDebug = Path.Combine(validatorDir, "bin", "Debug");
                        if (Directory.Exists(binDebug))
                        {
                            frameworks = Directory.GetDirectories(binDebug);
                            int valExeTries = 0;
                            while (valExeTries < 10 && validatorExe == null)
                            {
                                foreach (var fwDir in frameworks)
                                {
                                    var exeName = Path.GetFileNameWithoutExtension(validatorProj) + ".exe";
                                    var exeCandidate = Path.Combine(fwDir, exeName);
                                    if (File.Exists(exeCandidate))
                                    {
                                        try
                                        {
                                            using (var fs = new FileStream(exeCandidate, FileMode.Open, FileAccess.Read, FileShare.None)) { }
                                            validatorExe = exeCandidate;
                                            break;
                                        }
                                        catch { }
                                    }
                                }
                                if (validatorExe == null)
                                {
                                    // BOOKMARK: Si no encontró el exe del validador, buscar cualquier .exe que corresponda al
                                    // validador
                                    foreach (var fwDir in frameworks)
                                    {
                                        var exes = Directory.GetFiles(fwDir, "*.exe");
                                        foreach (var ex in exes)
                                        {
                                            try
                                            {
                                                using (var fs = new FileStream(ex, FileMode.Open, FileAccess.Read, FileShare.None)) { }
                                                validatorExe = ex;
                                                break;
                                            }
                                            catch { }
                                        }
                                        if (validatorExe != null) break;
                                    }
                                }
                                if (validatorExe == null) await Task.Delay(200);
                                valExeTries++;
                            }
                        }
                    }
                }
            }
            int totalTests = inputFiles.Count;
            int passedTests = 0;
            int failedTests = 0;
            StringBuilder results = new StringBuilder();

            // BOOKMARK: Ejecutar test para cada archivo del directorio IN
            foreach (var inputFile in inputFiles)
            {
                // Check for cancellation
                cancellationToken.ThrowIfCancellationRequested();

                string fileName = Path.GetFileName(inputFile);
                string testNumber = fileName.Replace("datos", "").Replace(".txt", "");
                string expectedOutputFile = Path.Combine(outDir, $"Output_datos{testNumber}.txt");
                string generatedOutputFile = Path.Combine(outputGeneradoDir, $"Output_datos{testNumber}.txt");

                if (!File.Exists(expectedOutputFile))
                {
                    string message = $"⚠️ Test {testNumber}: Expected output file not found";
                    results.AppendLine(message);
                    lstResults.Items.Add(message);
                    failedTests++;
                    continue;
                }

                toolStripStatusLabel.Text = $"Running test {testNumber}... ({passedTests + failedTests}/{totalTests})";
                Application.DoEvents();

                try
                {
                    // Read input as UTF-8 without BOM
                    string inputText;
                    using (var reader = new StreamReader(inputFile, new UTF8Encoding(false)))
                    {
                        inputText = await reader.ReadToEndAsync();
                    }

                    // Save input to temp file
                    string tempInputFile = Path.Combine(Path.GetTempPath(), $"input_{Guid.NewGuid():N}.txt");
                    await File.WriteAllTextAsync(tempInputFile, inputText, new UTF8Encoding(false), cancellationToken);

                    // BOOKMARK: Run student's program
                    var runResult = await RunProcessWithRedirectionAsync(exePath, tempInputFile, problemaDirectory!, timeoutSeconds * 1000);

                    // Delete temp input
                    try { File.Delete(tempInputFile); } catch { }

                    string message;
                    if (runResult.TimedOut)
                    {
                        message = $"⏱️ Test {testNumber}: TIMEOUT (>{timeoutSeconds} seconds)";
                        results.AppendLine(message);
                        lstResults.Items.Add(message);
                        failedTests++;
                        continue;
                    }

                    if (runResult.ExitCode != 0)
                    {
                        string errorDetail = "Program exited with error code";
                        if (!string.IsNullOrWhiteSpace(runResult.Error))
                        {
                            var errorLines = runResult.Error.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            var exceptionLine = errorLines.FirstOrDefault(l => l.Contains("Exception", StringComparison.OrdinalIgnoreCase));
                            errorDetail = exceptionLine ?? errorLines.FirstOrDefault() ?? errorDetail;
                        }
                        message = $"❌ Test {testNumber}: {errorDetail}";
                        results.AppendLine(message);
                        if (!string.IsNullOrWhiteSpace(runResult.Error))
                        {
                            results.AppendLine($"   Details: {runResult.Error.Substring(0, Math.Min(200, runResult.Error.Length))}...");
                        }
                        lstResults.Items.Add(message);
                        if (!string.IsNullOrWhiteSpace(runResult.Output))
                        {
                            await File.WriteAllTextAsync(generatedOutputFile, runResult.Output, Encoding.UTF8, cancellationToken);
                        }
                        failedTests++;
                        continue;
                    }

                    // Save student's output
                    await File.WriteAllTextAsync(generatedOutputFile, runResult.Output, new UTF8Encoding(false), cancellationToken);

                    // BOOKMARK: --- Custom Validator Support (ejecuta el validador, si existe validador) ---
                    // If Validator<id> exists, compile and run it
                    string? validatorResult = null;
                    bool validatorOk = false;
                    if (Directory.Exists(validatorDir) && validatorExe != null && File.Exists(validatorExe))
                    {
                        // BOOKMARK: Run validator: args = input, expected, actual, program
                        var validatorRun = await RunProcessAsync(validatorExe, $"\"{inputFile}\" \"{expectedOutputFile}\" \"{generatedOutputFile}\" \"{programPath}\"", validatorDir, timeoutSeconds * 1000);
                        validatorResult = validatorRun.Output.Trim();
                        if (validatorRun.ExitCode == 0 && validatorResult.Equals("OK", StringComparison.OrdinalIgnoreCase))
                        {
                            validatorOk = true;
                        }
                        else
                        {
                            validatorOk = false;
                        }
                    }

                    if (validatorResult != null)
                    {
                        if (validatorOk)
                        {
                            message = $"✅ Test {testNumber}: OK (custom validator)";
                            results.AppendLine(message);
                            lstResults.Items.Add(message);
                            passedTests++;
                        }
                        else
                        {
                            message = $"❌ Test {testNumber}: {validatorResult}";
                            results.AppendLine(message);
                            lstResults.Items.Add(message);
                            failedTests++;
                        }
                    }
                    else
                    {
                        // Default: compare outputs
                        string expectedOutput;
                        using (var reader = new StreamReader(expectedOutputFile, Encoding.UTF8))
                        {
                            expectedOutput = await reader.ReadToEndAsync();
                        }
                        string actualOutput = runResult.Output;
                        string normalizedExpected = NormalizeOutput(expectedOutput);
                        string normalizedActual = NormalizeOutput(actualOutput);
                        if (normalizedExpected == normalizedActual)
                        {
                            message = $"✅ Test {testNumber}: CORRECT";
                            results.AppendLine(message);
                            lstResults.Items.Add(message);
                            passedTests++;
                        }
                        else
                        {
                            message = $"❌ Test {testNumber}: WRONG OUTPUT";
                            results.AppendLine(message);
                            lstResults.Items.Add(message);
                            failedTests++;
                        }
                    }
                    // Auto-scroll
                    lstResults.TopIndex = lstResults.Items.Count - 1;
                }
                catch (Exception ex)
                {
                    string message = $"💥 Test {testNumber}: EXCEPTION - {ex.Message}";
                    results.AppendLine(message);
                    lstResults.Items.Add(message);
                    failedTests++;
                }
            }

            // Mostrar resumen
            string summary = $"Completado: {passedTests}/{totalTests} correctos, {failedTests} fallidos";
            toolStripStatusLabel.Text = summary;

            MessageBox.Show($"{summary}\n\n{results}", "Resultados de la Validación", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        finally
        {
            // Limpiar el directorio de trabajo
            try
            {
                if (Directory.Exists(workDir))
                {
                    // Esperar un poco antes de intentar eliminar
                    await Task.Delay(100);
                    Directory.Delete(workDir, true);
                }
            }
            catch
            {
                // Si no se puede eliminar, no es crítico
                // El directorio se limpiará en la próxima ejecución
            }
        }
    }

    private string NormalizeOutput(string output)
    {
        // Normalizar saltos de línea y eliminar espacios al final de cada línea
        var lines = output.Replace("\r\n", "\n").Split('\n')
            .Select(line => line.TrimEnd())
            .ToList();

        // Eliminar líneas vacías al final
        while (lines.Count > 0 && string.IsNullOrWhiteSpace(lines[^1]))
        {
            lines.RemoveAt(lines.Count - 1);
        }

        return string.Join("\n", lines);
    }

    private async Task<ProcessResult> RunProcessAsync(string fileName, string arguments, 
        string workingDirectory, int timeoutMs)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
                // Para dotnet build, UTF8 está bien
                // StandardOutputEncoding = Encoding.UTF8,
                // StandardErrorEncoding = Encoding.UTF8
            }
        };

        var output = new StringBuilder();
        var error = new StringBuilder();

        process.OutputDataReceived += (s, e) => { if (e.Data != null) output.AppendLine(e.Data); };
        process.ErrorDataReceived += (s, e) => { if (e.Data != null) error.AppendLine(e.Data); };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        bool completed = await Task.Run(() => process.WaitForExit(timeoutMs));

        if (!completed)
        {
            try { process.Kill(true); } catch { }
            return new ProcessResult { TimedOut = true };
        }

        // Esperar a que se lean todos los datos de salida y error
        // Esto es importante para evitar condiciones de carrera
        await Task.Run(() => process.WaitForExit());

        return new ProcessResult
        {
            ExitCode = process.ExitCode,
            Output = output.ToString(),
            Error = error.ToString()
        };
    }

    /// <summary>
    /// BOOKMARK: Ejecuta un proceso externo (el programa del usuario) usando redirección de entrada 
    /// y salida, monitoreando en paralelo el uso de memoria y el tiempo de ejecución.
    /// 
    /// - El proceso se inicia y se lanza un monitor en segundo plano para verificar si excede el 
    /// límite de memoria (512 MB).
    /// - Se espera a que el proceso termine o se alcance el timeout especificado.
    /// - Si el proceso termina normalmente, se recopila la salida y el código de salida.
    /// - Si excede el tiempo o la memoria, se fuerza su terminación y se indica el motivo.
    /// - Se asegura que la salida estándar y de error se lean como UTF-8.
    /// </summary>
    /// <remarks>
    /// Ejecuta el programa del usuario usando cmd.exe, chcp 65001 y redirección de archivo
    /// La salida estándar y de error se lee como UTF-8 para soportar caracteres especiales
    /// </remarks>
    private async Task<ProcessResult> RunProcessWithRedirectionAsync(string exePath, string inputFile, string workingDirectory, int timeoutMs)
    {
        // Usar cmd.exe con chcp 65001 y redirección type archivo | exe
        // Esto fuerza la consola a UTF-8 y permite que la salida se capture correctamente
        string command = $"/c chcp 65001 >nul & type \"{inputFile}\" | \"{exePath}\"";

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = command,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                // CLAVE: leer la salida como UTF-8 para evitar corrupción de caracteres
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            }
        };

        // PROTECCIÓN: Configurar límites de recursos
        try
        {
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        }
        catch { }

        var output = new StringBuilder();
        var error = new StringBuilder();
        const long MAX_MEMORY_MB = 512;
        bool memoryExceeded = false;

        // Suscribirse a la salida estándar y de error para capturar la información del proceso
        process.OutputDataReceived += (s, e) => { if (e.Data != null) output.AppendLine(e.Data); };
        process.ErrorDataReceived += (s, e) => { if (e.Data != null) error.AppendLine(e.Data); };

        process.Start();

        // Intentar bajar la prioridad del proceso para evitar que bloquee el sistema
        try { process.PriorityClass = ProcessPriorityClass.BelowNormal; } catch { }

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        // Lanzar una tarea en segundo plano para monitorear el uso de memoria del proceso
        var monitorTask = Task.Run(async () =>
        {
            while (!process.HasExited)
            {
                try
                {
                    process.Refresh();
                    long memoryMB = process.WorkingSet64 / (1024 * 1024);
                    if (memoryMB > MAX_MEMORY_MB)
                    {
                        // Si se excede el límite de memoria, terminar el proceso
                        memoryExceeded = true;
                        try { process.Kill(true); } catch { }
                        break;
                    }
                }
                catch { break; }
                await Task.Delay(100);
            }
        });

        // Esperar a que el proceso termine o se alcance el timeout
        bool completed = await Task.Run(() => process.WaitForExit(timeoutMs));

        // Si no terminó a tiempo, forzar su terminación
        if (!completed)
        {
            try { process.Kill(true); } catch { }
            await monitorTask;
            return new ProcessResult
            {
                TimedOut = !memoryExceeded, // true si fue timeout, false si fue por memoria
                ExitCode = -1,
                Error = memoryExceeded ? $"Memoria excedida (límite: {MAX_MEMORY_MB} MB)" : "Timeout"
            };
        }

        // Esperar a que el monitor de memoria termine
        await monitorTask;

        // Si terminó por exceso de memoria, reportar el error
        if (memoryExceeded)
        {
            return new ProcessResult
            {
                ExitCode = -1,
                Error = $"El programa excedió el límite de memoria ({MAX_MEMORY_MB} MB)"
            };
        }

        // Asegurarse de que todo el output se haya leído
        await Task.Run(() => process.WaitForExit());

        // Convertir la salida de Windows-1252 a UTF-8 para la comparación y escritura
        string outputStr = output.ToString();
        string errorStr = error.ToString();
        // Si quieres, puedes hacer una validación extra aquí para detectar si ya es UTF-8 puro

        return new ProcessResult
        {
            ExitCode = process.ExitCode,
            Output = outputStr,
            Error = errorStr
        };
    }


    private class ProcessResult
    {
        public int ExitCode { get; set; }
        public string Output { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public bool TimedOut { get; set; }
    }
}
