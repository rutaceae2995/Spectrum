﻿using System;
using System.Runtime.InteropServices;

namespace Spectrum.UI.Core
{
    /// <summary>
    /// Native method import class.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// The DeleteObject function deletes a logical pen, brush, font, bitmap, region, or palette,
        ///  freeing all system resources associated with the object.
        /// After the object is deleted, the specified handle is no longer valid.
        /// </summary>
        /// <param name="hObject">A handle to a logical pen, brush, font, bitmap, region, or palette.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the specified handle is not valid or is currently selected into a DC, the return value is zero.
        /// </returns>
        [DllImport("gdi32")]
        internal static extern int DeleteObject(IntPtr hObject);
    }
}