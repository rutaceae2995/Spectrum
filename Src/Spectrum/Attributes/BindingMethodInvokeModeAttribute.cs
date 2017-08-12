using System;

namespace Spectrum.Attributes
{
    /// <summary>
    /// This attribute define the execution mode of the method called by command or property changed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
    public sealed class BindingMethodInvokeModeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the BindingMethodInvokeModeAttribute class.
        /// </summary>
        /// <param name="firstDelayWaitTime">The wait time (millisecond) to execute command (0 = not delay).</param>
        /// <param name="coolingTime">The cooling time (millisecond) to suppress execution (0 = not suppress execution).</param>
        /// <param name="invokeAfterCoolingTime">(only coolingTime > 0) Whether to execute the command when the suppression period is completed.</param>
        public BindingMethodInvokeModeAttribute(long firstDelayWaitTime = 0, long coolingTime = 0, bool invokeAfterCoolingTime = false)
        {
            this.FirstDelayWaitTime = firstDelayWaitTime;
            this.CoolingTime = coolingTime;
            this.InvokeAfterCoolingTime = invokeAfterCoolingTime;
        }

        /// <summary>
        /// Gets or sets the wait time (millisecond) to execute command.
        /// </summary>
        public long FirstDelayWaitTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets of sets the cooling time (millisecond) to suppress execution. 
        /// </summary>
        public long CoolingTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets of sets the value indicating whether to execute the command when the suppression period is completed.
        /// </summary>
        public bool InvokeAfterCoolingTime
        {
            get;
            set;
        }
    }
}