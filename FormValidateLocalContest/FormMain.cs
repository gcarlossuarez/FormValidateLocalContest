
using System.Diagnostics;
using System.Text;

namespace FormValidateLocalContest;

public partial class FormMain : Form
{
    /*
     * ===============================
     *  DIAGRAMA DE FLUJO PRINCIPAL
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
    private string? problemaDirectory;
    private CancellationTokenSource? cancellationTokenSource;

    // Ruta donde se extrae el ZIP incrustado
    private string? extractedProblemsPath;

    public FormMain()
    {
        InitializeComponent();
        // Al iniciar, extrae el ZIP de problemas y llena el ListBox
        extractedProblemsPath = ExtractEmbeddedZipToTemp();
        PopulateProblemsListBox();

        // Habilita encodings extendidos (Windows-1252, etc.) en .NET Core/5+/6+/7+/8+
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }
    /// <summary>
    /// Llena el ListBox lstProblemas con los nombres de los problemas extraídos del ZIP.
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
    /// Extrae el recurso incrustado Problems.zip a un directorio temporal y retorna la ruta de extracción.
    /// </summary>
    /// <returns>Ruta del directorio donde se extrajo el ZIP</returns>
    private string ExtractEmbeddedZipToTemp()
    {
        // Nombre del recurso incrustado (namespace + nombre de archivo)
        var resourceName = "FormValidateLocalContest.Problems.zip";
        var tempDir = Path.Combine(Path.GetTempPath(), "ProblemasContest_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        using var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            MessageBox.Show($"No se encontró el recurso incrustado: {resourceName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            throw new FileNotFoundException($"No se encontró el recurso incrustado: {resourceName}");
        }

        // Guardar el ZIP temporalmente
        var tempZipPath = Path.Combine(tempDir, "Problems.zip");
        using (var fileStream = File.Create(tempZipPath))
        {
            stream.CopyTo(fileStream);
        }

        // Extraer el ZIP
        System.IO.Compression.ZipFile.ExtractToDirectory(tempZipPath, tempDir);

        // Eliminar el ZIP temporal
        File.Delete(tempZipPath);

        return Path.Combine(tempDir, "Problems"); // Asumiendo que el ZIP contiene una carpeta raíz llamada "Problems"
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
                txtCode.Text = File.ReadAllText(selectedFilePath, Encoding.UTF8);
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
    /// Evento: cuando el usuario selecciona un problema del ListBox, se actualiza el directorio de trabajo y se muestra la descripción.
    /// </summary>
    private void lstProblems_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lstProblems.SelectedItem == null || string.IsNullOrEmpty(extractedProblemsPath))
            return;

        // Construir la ruta del problema seleccionado
        string selectedProblem = lstProblems.SelectedItem.ToString()!;
        string problemPath = Path.Combine(extractedProblemsPath, selectedProblem);

        // Actualizar el textbox y la variable problemaDirectory para que el validador use este dataset
        txtProblemaDir.Text = problemPath;
        problemaDirectory = problemPath;
        toolStripStatusLabel.Text = $"Problema seleccionado: {selectedProblem}";

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

        // Buscar el archivo .csproj en el directorio del problema
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
                created = Directory.Exists(workDir);
            }
            catch (Exception ex)
            {
                lastCreateEx = ex;
                await Task.Delay(100);
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

        try
        {
            // Copiar el .csproj al directorio de trabajo
            string workCsprojPath = Path.Combine(workDir, Path.GetFileName(csprojPath));
            File.Copy(csprojPath, workCsprojPath, true);

            // Guardar el código en Program.cs en el directorio de trabajo
            string programPath = Path.Combine(workDir, "Program.cs");
            await File.WriteAllTextAsync(programPath, txtCode.Text, Encoding.UTF8);

            // Compilar en el directorio de trabajo
            toolStripStatusLabel.Text = "Compilando el proyecto...";
            Application.DoEvents();

            //var compileResult = await RunProcessAsync("dotnet", "build -c Release", workDir, timeoutSeconds * 1000);
            var compileResult = await RunProcessAsync("dotnet", "build", workDir, timeoutSeconds * 1000);
            
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
                throw new DirectoryNotFoundException("No se encontró el directorio del framework en bin/Release");
            }

            string exePath = Path.Combine(frameworks[0], $"{projectName}.exe");
            if (!File.Exists(exePath))
            {
                throw new FileNotFoundException($"No se encontró el ejecutable: {exePath}");
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

            int totalTests = inputFiles.Count;
            int passedTests = 0;
            int failedTests = 0;
            StringBuilder results = new StringBuilder();

            foreach (var inputFile in inputFiles)
            {
                // Verificar si se ha solicitado cancelación
                cancellationToken.ThrowIfCancellationRequested();

                string fileName = Path.GetFileName(inputFile);
                string testNumber = fileName.Replace("datos", "").Replace(".txt", "");
                string expectedOutputFile = Path.Combine(outDir, $"Output_datos{testNumber}.txt");
                string generatedOutputFile = Path.Combine(outputGeneradoDir, $"Output_datos{testNumber}.txt");

                if (!File.Exists(expectedOutputFile))
                {
                    string message = $"⚠️ Test {testNumber}: Archivo de salida esperado no encontrado";
                    results.AppendLine(message);
                    lstResults.Items.Add(message);
                    failedTests++;
                    continue;
                }

                toolStripStatusLabel.Text = $"Ejecutando test {testNumber}... ({passedTests + failedTests}/{totalTests})";
                Application.DoEvents();

                try
                {
                    // Leer el archivo de entrada como UTF-8 sin BOM
                    // Esto asegura que los acentos y caracteres especiales se lean correctamente
                    string inputText;
                    using (var reader = new StreamReader(inputFile, new UTF8Encoding(false)))
                    {
                        inputText = await reader.ReadToEndAsync();
                    }

                    // Guardar inputText en archivo temporal UTF-8 sin BOM
                    string tempInputFile = Path.Combine(Path.GetTempPath(), $"input_{Guid.NewGuid():N}.txt");
                    await File.WriteAllTextAsync(tempInputFile, inputText, new UTF8Encoding(false), cancellationToken);

                    // Ejecutar usando cmd.exe, chcp 65001 y redirección type archivo | exe
                    // Esto fuerza la consola a UTF-8 y pasa el input por stdin
                    var runResult = await RunProcessWithRedirectionAsync(exePath, tempInputFile, problemaDirectory!, timeoutSeconds * 1000);

                    // Borrar archivo temporal
                    try { File.Delete(tempInputFile); } catch { }

                    string message;
                    if (runResult.TimedOut)
                    {
                        message = $"⏱️ Test {testNumber}: TIMEOUT (excedió {timeoutSeconds} segundos)";
                        results.AppendLine(message);
                        lstResults.Items.Add(message);
                        failedTests++;
                        continue;
                    }

                    if (runResult.ExitCode != 0)
                    {
                        // Mostrar solo la primera línea del error para que sea más legible
                        string errorDetail = "El programa terminó con código de error";
                        
                        if (!string.IsNullOrWhiteSpace(runResult.Error))
                        {
                            var errorLines = runResult.Error.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            // Buscar la línea que contiene "exception" o mostrar la primera línea
                            var exceptionLine = errorLines.FirstOrDefault(l => l.Contains("Exception", StringComparison.OrdinalIgnoreCase));
                            errorDetail = exceptionLine ?? errorLines.FirstOrDefault() ?? errorDetail;
                        }
                        
                        message = $"❌ Test {testNumber}: {errorDetail}";
                        results.AppendLine(message);
                        // Para el resumen completo, agregar más detalles
                        if (!string.IsNullOrWhiteSpace(runResult.Error))
                        {
                            results.AppendLine($"   Detalles: {runResult.Error.Substring(0, Math.Min(200, runResult.Error.Length))}...");
                        }
                        lstResults.Items.Add(message);
                        
                        // Si hay salida, también guardarla para análisis
                        if (!string.IsNullOrWhiteSpace(runResult.Output))
                        {
                            await File.WriteAllTextAsync(generatedOutputFile, runResult.Output, Encoding.UTF8, cancellationToken);
                        }
                        
                        failedTests++;
                        continue;
                    }

                    // Guardar la salida generada como UTF-8 sin BOM
                    await File.WriteAllTextAsync(generatedOutputFile, runResult.Output, new UTF8Encoding(false), cancellationToken);

                    // Comparar salidas
                    string expectedOutput;
                    // Leer el archivo esperado como UTF-8 explícitamente
                    // Esto es importante para que la comparación sea byte a byte en UTF-8
                    using (var reader = new StreamReader(expectedOutputFile, Encoding.UTF8))
                    {
                        expectedOutput = await reader.ReadToEndAsync();
                    }

                    string actualOutput = runResult.Output;

                    // Normalizar líneas para comparación (quitar espacios al final y líneas vacías al final)
                    string normalizedExpected = NormalizeOutput(expectedOutput);
                    string normalizedActual = NormalizeOutput(actualOutput);

                    if (normalizedExpected == normalizedActual)
                    {
                        message = $"✅ Test {testNumber}: CORRECTO";
                        results.AppendLine(message);
                        lstResults.Items.Add(message);
                        passedTests++;
                    }
                    else
                    {
                        message = $"❌ Test {testNumber}: ERRÓNEO (salida no coincide)";
                        results.AppendLine(message);
                        lstResults.Items.Add(message);
                        failedTests++;
                    }
                    
                    // Auto-scroll al último elemento
                    lstResults.TopIndex = lstResults.Items.Count - 1;
                }
                catch (Exception ex)
                {
                    string message = $"💥 Test {testNumber}: EXCEPCIÓN - {ex.Message}";
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

    // Ejecuta el programa del usuario usando cmd.exe, chcp 65001 y redirección de archivo
    // La salida estándar y de error se lee como UTF-8 para soportar caracteres especiales
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

        process.OutputDataReceived += (s, e) => { if (e.Data != null) output.AppendLine(e.Data); };
        process.ErrorDataReceived += (s, e) => { if (e.Data != null) error.AppendLine(e.Data); };

        process.Start();

        try { process.PriorityClass = ProcessPriorityClass.BelowNormal; } catch { }

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

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
                        memoryExceeded = true;
                        try { process.Kill(true); } catch { }
                        break;
                    }
                }
                catch { break; }
                await Task.Delay(100);
            }
        });

        bool completed = await Task.Run(() => process.WaitForExit(timeoutMs));

        if (!completed)
        {
            try { process.Kill(true); } catch { }
            await monitorTask;
            return new ProcessResult
            {
                TimedOut = !memoryExceeded,
                ExitCode = -1,
                Error = memoryExceeded ? $"Memoria excedida (límite: {MAX_MEMORY_MB} MB)" : "Timeout"
            };
        }

        await monitorTask;

        if (memoryExceeded)
        {
            return new ProcessResult
            {
                ExitCode = -1,
                Error = $"El programa excedió el límite de memoria ({MAX_MEMORY_MB} MB)"
            };
        }

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
