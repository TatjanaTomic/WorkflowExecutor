namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;

/// <summary>
/// This enum represents different types of Steps in application.
/// </summary>
public enum Type
{
    /// <summary>
    /// Type <c>Executable</c> represents a Step that runs script or executable file.
    /// </summary>
    Executable,

    /// <summary>
    /// Type <c>Upload</c> represents a Step that uploads file to Server.
    /// </summary>
    Upload,

    /// <summary>
    /// Type <c>Download</c> represents a Step that downloads file from Server to local machine.
    /// </summary>
    Download
}
