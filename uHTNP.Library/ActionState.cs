namespace uHTNP
{
    /// <summary>
    /// An action state is returned by action functions, and determines the
    /// whether the actions is marked as a success, fail or call again.
    /// </summary>
    public enum ActionState
    {
        /// <summary>
        /// The action succeeded.
        /// </summary>
        Success,
        /// <summary>
        /// The action is in progress, and needs to be called again next frame.
        /// </summary>
        InProgress,
        /// <summary>
        /// The action failed.
        /// </summary>
        Error,
    }
}