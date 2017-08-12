using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Spectrum.UI.Core
{
    /// <summary>
    /// This class supplies that can be shared the ResourceDictionaries.
    /// </summary>
    public sealed class SharedResourceDictionary : ResourceDictionary
    {
        /// <summary>
        /// Weak reference dictionary to manage the ResourceDictionaries.
        /// </summary>
        private static readonly Dictionary<Uri, WeakReference> ResourceReferences = new Dictionary<Uri, WeakReference>();

        /// <summary>
        /// The source uri.
        /// </summary>
        private Uri source;

        /// <summary>
        /// Gets or sets the source uri.
        /// </summary>
        public new Uri Source
        {
            get
            {
                return IsInDesignMode ? base.Source : this.source;
            }

            set
            {
                if (IsInDesignMode)
                {
                    base.Source = value;
                    return;
                }

                this.source = value;
                if (!ResourceReferences.ContainsKey(this.source))
                {
                    base.Source = this.source;
                    AddToCache(this);
                }
                else
                {
                    var weakReference = ResourceReferences[this.source];
                    if (weakReference != null && weakReference.IsAlive)
                    {
                        this.MergedDictionaries.Add(weakReference.Target as ResourceDictionary);
                    }
                    else
                    {
                        base.Source = this.source;
                        AddToCache(this);
                    }
                }

            }
        }

        /// <summary>
        /// Add the SharedResouceDictionary.Source to cache.
        /// </summary>
        private static void AddToCache(SharedResourceDictionary sharedDictionary)
        {
            if (ResourceReferences.ContainsKey(sharedDictionary.source))
            {
                ResourceReferences.Remove(sharedDictionary.source);
            }

            ResourceReferences.Add(sharedDictionary.source, new WeakReference(sharedDictionary, false));
        }

        /// <summary>
        /// Gets the value of the IsInDesignMode attached property.
        /// </summary>
        private static bool IsInDesignMode => (bool)DependencyPropertyDescriptor.FromProperty(
            DesignerProperties.IsInDesignModeProperty,
            typeof(DependencyObject)).Metadata.DefaultValue;
    }
}