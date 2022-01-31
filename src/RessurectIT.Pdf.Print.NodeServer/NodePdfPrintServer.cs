using System.IO.Compression;
using System.Reflection;
using Jering.Javascript.NodeJS;

namespace RessurectIT.Pdf.Print;

//TODO: logging
//TODO: code signing, regenerate
//TODO: readme.md for each project for nuget

/// <summary>
/// Node print server instance
/// </summary>
public class NodePdfPrintServer : IDisposable
{
    #region constants

    /// <summary>
    /// Name of embedded resources prefix
    /// </summary>
    private const string AssemblyResourcePrefix = "RessurectIT.Pdf.Print.dist.";

    /// <summary>
    /// Name of directory for node server
    /// </summary>
    private const string NodeServerDir = "_nodeFiles";
    #endregion


    #region private fields

    /// <summary>
    /// Length of <see cref="AssemblyResourcePrefix"/>
    /// </summary>
    private readonly int _assemblyResourcePrefixLength = AssemblyResourcePrefix.Length;
    #endregion


    #region public properties

    /// <summary>
    /// Gets path to currently used node js, used for running node scripts
    /// </summary>
    public string? NodeJsPath
    {   
        get;
    }

    /// <summary>
    /// Path to directory containing node files
    /// </summary>
    public string NodeServerFilesDir
    {
        get;
        private set;
    } = $"./{NodeServerDir}";
    #endregion


    #region constructors

    /// <summary>
    /// Creates instance of <see cref="NodePdfPrintServer"/>
    /// </summary>
    /// <param name="nodeJsPath">Path to 'node' that will be used for running Node scripts</param>
    public NodePdfPrintServer(string nodeJsPath)
    {
        NodeJsPath = nodeJsPath;

        if (string.IsNullOrEmpty(nodeJsPath))
        {
            throw new ArgumentNullException("Missing argument 'nodeJsPath'");
        }

        _initialize();
    }

    /// <summary>
    /// Creates instance of <see cref="NodePdfPrintServer"/> with <see cref="NodeJsPath"/> set to 'node' (using installed node from path)
    /// </summary>
    public NodePdfPrintServer()
    {
        NodeJsPath = null;

        _initialize();
    }
    #endregion


    #region public methods - implementation of IDisposable

    /// <inheritdoc />
    public void Dispose()
    {
        StaticNodeJSService.DisposeServiceProvider();
    }
    #endregion


    #region private methods

    /// <summary>
    /// Initialize javascript files for node server
    /// </summary>
    private void _initialize()
    {
        //sets nodejs path
        if (!string.IsNullOrEmpty(NodeJsPath))
        {
            StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ExecutablePath = NodeJsPath);
        }

        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        string? currentDirectory = Path.GetDirectoryName(currentAssembly.Location);
        string? zipFileName = null;

        //backup to current directory
        if (string.IsNullOrEmpty(currentDirectory))
        {
            currentDirectory = ".";
        }

        NodeServerFilesDir = Path.Combine(currentDirectory, NodeServerDir);

        string[] files = currentAssembly.GetManifestResourceNames();

        foreach (string file in files)
        {
            string fileName = file;
            string dir = NodeServerFilesDir;

            //removes resource prefix
            if (file.StartsWith(AssemblyResourcePrefix))
            {
                fileName = file.Substring(_assemblyResourcePrefixLength);
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

        string nodeModulesPath = Path.Combine(NodeServerFilesDir, "node_modules");

        //removes node_modules if exists
        if (Directory.Exists(nodeModulesPath))
        {
            Directory.Delete(nodeModulesPath, true);
        }

        if (!string.IsNullOrEmpty(zipFileName))
        {
            ZipFile.ExtractToDirectory(Path.Combine(NodeServerFilesDir, zipFileName), NodeServerFilesDir);
        }
    }
    #endregion
}