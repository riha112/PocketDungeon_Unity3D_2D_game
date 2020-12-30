namespace Assets.Scripts.Misc.Pipeline
{
    /// <summary>
    /// Manipulates room, used in pipeline to perform room manipulations
    /// like texture id setting, decoration setting, room type definition
    /// </summary>
    public interface IPipelineProcess<T>
    {
        bool IsEnabled { get; }

        /// <summary>
        /// Order in which processor is executed (smallest goes first)
        /// Used to ensure that some processors are executed before others, as
        /// some of the processors may be depended of others, or may change
        /// room in way that other processor can't correctly process data
        /// </summary>
        int PriorityId { get; }


        /// <summary>
        /// Performs the data manipulations
        /// </summary>
        /// <param name="data">Initial data</param>
        /// <returns>Processed data</returns>
        T Translate(T data);
    }
}
