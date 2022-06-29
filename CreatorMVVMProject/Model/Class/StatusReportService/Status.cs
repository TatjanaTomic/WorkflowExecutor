namespace CreatorMVVMProject.Model.Class.StatusReportService
{
    /// <summary>
    /// This enum represents values of Step Statuses
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Status Disabled - Step cannot be started because it's dependency Steps are not executed successfully
        /// </summary>
        Disabled,

        /// <summary>
        /// Status NotStarted - Step can be started, it has no dependency Steps or all of it's dependency Steps are executed successfully 
        /// </summary>
        NotStarted,

        /// <summary>
        /// Status InProgress - Step execution started and it is executing currently
        /// </summary>
        InProgress,

        /// <summary>
        /// Status Success - Step execution finished successfully
        /// </summary>
        Success,

        /// <summary>
        /// Status Failed - Step execution finished unsuccessfully
        /// </summary>
        Failed,

        /// <summary>
        /// Status Obsolete - When the execution of a Step that was previously executed successfully is started, all the Steps that depend on that Step change their Status to Obsolete
        /// </summary>
        Obsolete,
    }
}
