using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RessurectIT.Pdf.Print;

//TODO: logging
//TODO: handle stderr
//TODO: handle unresponsivity
//TODO: grpc client comments fix
//TODO: Jering.Javascript.NodeJS possible use of
//TODO: code signing, regenerate
//TODO: readme.md for each project for nuget

/// <summary>
/// gRPC node print server instance
/// </summary>
public class GrpcNodePrintServer : IDisposable
{
    #region constants

    /// <summary>
    /// Name of embedded resources prefix
    /// </summary>
    private const string AssemblyResourcePrefix = "RessurectIT.Pdf.Print.";

    /// <summary>
    /// Name of embedded resources proto prefix
    /// </summary>
    private const string AssemblyResourceProtoPrefix = $"{AssemblyResourcePrefix}proto.";

    /// <summary>
    /// Name of directory for gRPC node server
    /// </summary>
    private const string GRPCNodeServerDir = "_nodeFiles";
    #endregion


    #region private fields

    /// <summary>
    /// Length of <see cref="AssemblyResourcePrefix"/>
    /// </summary>
    private readonly int AssemblyResourcePrefixLength = AssemblyResourcePrefix.Length;

    /// <summary>
    /// Length of <see cref="AssemblyResourceProtoPrefix"/>
    /// </summary>
    private readonly int AssemblyResourceProtoPrefixLength = AssemblyResourceProtoPrefix.Length;

    /// <summary>
    /// Instance of running process
    /// </summary>
    private Process? _process;
    #endregion


    #region public properties

    /// <summary>
    /// Gets port number of currently running gRPC server
    /// </summary>
    public int? PortNumber
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets indication whether is gRPC server running
    /// </summary>
    public bool Running
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets path to currently used node js, used for running gRPC server
    /// </summary>
    public string NodeJsPath
    {   
        get;
    }

    /// <summary>
    /// Path to directory containing node files for gRPC server
    /// </summary>
    public string GRPCServerFilesDir
    {
        get;
        private set;
    } = $"./{GRPCNodeServerDir}";
    #endregion


    #region constructors

    /// <summary>
    /// Creates instance of <see cref="GrpcNodePrintServer"/>
    /// </summary>
    /// <param name="nodeJsPath">Path to 'node' that will be used for running gRPC server</param>
    public GrpcNodePrintServer(string nodeJsPath)
    {
        NodeJsPath = nodeJsPath;

        if (string.IsNullOrEmpty(nodeJsPath))
        {
            throw new ArgumentNullException("Missing argument 'nodeJsPath'");
        }
    }

    /// <summary>
    /// Creates instance of <see cref="GrpcNodePrintServer"/> with <see cref="NodeJsPath"/> set to 'node' (using installed node from path)
    /// </summary>
    public GrpcNodePrintServer() : this("node")
    {
        _initializeFiles();
    }
    #endregion


    #region public methods - implementation of IDisposable

    /// <inheritdoc />
    public void Dispose()
    {
        Stop();
    }
    #endregion


    #region public methods

    /// <summary>
    /// Starts gRPC print server
    /// </summary>
    /// <returns>Port on which is gRPC server running</returns>
    public Task<int> Start()
    {
        _process ??= new Process
        {
            StartInfo =
            {
                FileName = NodeJsPath,
                WorkingDirectory = GRPCServerFilesDir,
                Arguments = "index.js",
                RedirectStandardInput = false,
                RedirectStandardOutput = true,
                RedirectStandardError = false,
                CreateNoWindow = true,
                UseShellExecute = false
            }
        };

        TaskCompletionSource<int> task = new TaskCompletionSource<int>();;

        void StdOutParser(object sender, DataReceivedEventArgs e)
        {
            Match match = Regex.Match(e.Data, @"PDF GRPC SERVER: running on '(?<port>\d+)'");

            if (match.Success)
            {
                PortNumber = int.Parse(match.Groups["port"].Value);
                Running = true;
                _process.CancelOutputRead();
                _process.OutputDataReceived -= StdOutParser;

                task.SetResult(PortNumber.Value);
            }
        }

        _process.OutputDataReceived += StdOutParser;

        _process.Start();
        _process.BeginOutputReadLine();

        return task.Task;
    }

    /// <summary>
    /// Stops running gRPC print server
    /// </summary>
    public void Stop()
    {
        _process?.Kill();

        PortNumber = null;
        Running = false;
    }
    #endregion


    #region private methods

    /// <summary>
    /// Initialize javascript files for gRPC node server
    /// </summary>
    private void _initializeFiles()
    {
        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        string? currentDirectory = Path.GetDirectoryName(currentAssembly.Location);
        string? zipFileName = null;

        //backup to current directory
        if (string.IsNullOrEmpty(currentDirectory))
        {
            currentDirectory = ".";
        }

        GRPCServerFilesDir = Path.Combine(currentDirectory, GRPCNodeServerDir);

        string[] files = currentAssembly.GetManifestResourceNames();

        foreach (string file in files)
        {
            string fileName = file;
            string dir = GRPCServerFilesDir;

            //removes proto resource prefix
            if (file.StartsWith(AssemblyResourceProtoPrefix))
            {
                fileName = file.Substring(AssemblyResourceProtoPrefixLength);
                dir = Path.Combine(GRPCServerFilesDir, "proto");
            }
            //removes resource prefix
            else if (file.StartsWith(AssemblyResourcePrefix))
            {
                fileName = file.Substring(AssemblyResourcePrefixLength);
            }

            using Stream filestream = currentAssembly.GetManifestResourceStream(file);
            using MemoryStream memoryStream = new MemoryStream();
            filestream.CopyTo(memoryStream);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (fileName.EndsWith(".zip"))
            {
                zipFileName = fileName;
            }

            File.WriteAllBytes(Path.Combine(dir, fileName), memoryStream.ToArray());
        }

        string nodeModulesPath = Path.Combine(GRPCServerFilesDir, "node_modules");

        //removes node_modules if exists
        if (Directory.Exists(nodeModulesPath))
        {
            Directory.Delete(nodeModulesPath, true);
        }

        if (!string.IsNullOrEmpty(zipFileName))
        {
            ZipFile.ExtractToDirectory(Path.Combine(GRPCServerFilesDir, zipFileName), GRPCServerFilesDir);
        }
    }
    #endregion
}