using System.Windows.Input;

namespace Spectrum.Mvvm
{
    /// <summary>
    /// ICommand basic implement interface.
    /// </summary>
    public interface ICommandBase : ICommand
    {
        /// <summary>
        /// Re-evaluate the return value of the CanExecute method.
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}