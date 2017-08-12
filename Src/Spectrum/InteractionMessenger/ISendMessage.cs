namespace Spectrum.InteractionMessenger
{
    /// <summary>
    /// This interface implements SendMessage to nofity view from viewmodel using messenger pattern.
    /// </summary>
    public interface ISendMessage
    {
        /// <summary>
        /// Sends an action message using the messenger pattern.
        /// </summary>
        /// <typeparam name="TParameter">The type of action message parameter.</typeparam>
        /// <param name="parameter">A message parameter</param>
        void SendMessage<TParameter>(TParameter parameter) where TParameter : MessageParameter;
    }
}