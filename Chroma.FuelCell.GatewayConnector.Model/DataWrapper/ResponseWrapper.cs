namespace Chroma.FuelCell.GatewayConnector.Model
{
    /// <summary>
    /// Represent the response status of a request,
    /// being a wrapper around the <see cref="WrapperDataBase"/> instance
    /// </summary>
    internal class ResponseWrapper
    {
        /// <summary>
        /// Not enough data to be parsed
        /// </summary>
        internal const int Unknown = 0;

        /// <summary>
        /// The data have been parsed, but something indicates
        /// that the request/response must not taken in account
        /// (e.g. wrong logical address)
        /// </summary>
        internal const int Ignore = 1;

        /// <summary>
        /// A severe/critical error has been found
        /// </summary>
        internal const int Critical = 2;

        /// <summary>
        /// The data have been parsed successfully
        /// </summary>
        /// <remarks>
        /// Note that this indication means just the correct acknowledge
        /// by the protocol layer. However, the application layer could
        /// reveal a wrong query and refuse its servicing
        /// </remarks>
        internal const int Ack = 3;


        internal ResponseWrapper(
            WrapperDataBase data,
            int status)
        {
            Data = data;
            Status = status;
        }


        /// <summary>
        /// The wrapped instance containing the data of the query
        /// </summary>
        internal readonly WrapperDataBase Data;


        /// <summary>
        /// Indicate the status of the response
        /// </summary>
        internal readonly int Status;
    }
}
