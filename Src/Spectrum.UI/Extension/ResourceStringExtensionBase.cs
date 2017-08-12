using System;
using System.Windows.Markup;

namespace Spectrum.UI.Extension
{
    /// <summary>
    /// This class is a markup extension class that retrieves a string from Properties.Resource.
    /// * implement so that each module inherits this class and returns a unique ResourceManager.
    /// </summary>
    /// <typeparam name="T">ResourceManager</typeparam>
    [MarkupExtensionReturnType(typeof(string))]
    public abstract class ResourceStringExtensionBase<T> : MarkupExtension
     where T : System.Resources.ResourceManager
    {
        /// <summary>
        /// Resource manager
        /// </summary>
        protected abstract T ResourceManager
        {
            get;
        }

        /// <summary>
        /// Id
        /// </summary>
        protected abstract string ResourceKey
        {
            get;
        }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension. 
        /// </summary>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Resource string</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(this.ResourceKey))
            {
                return string.Empty;
            }

            var result = this.ResourceManager.GetString(this.ResourceKey);
            if (result == null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Resource [{0}] is undefined.", this.ResourceKey));
                return string.Empty;
            }

            return result;
        }
    }
}