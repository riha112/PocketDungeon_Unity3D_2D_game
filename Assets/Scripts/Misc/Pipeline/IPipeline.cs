namespace Assets.Scripts.Misc.Pipeline
{
    /// <summary>
    /// Describes pipeline of how T type data should be modified
    /// </summary>
    /// <typeparam name="T">Type to modify through pipeline</typeparam>
    public interface IPipeline<T>
    {
        /// <summary>
        /// Moves passed data through pipeline
        /// </summary>
        /// <param name="data">Object that will be moved through pipeline</param>
        void RunThroughPipeline(T data);
    }
}
