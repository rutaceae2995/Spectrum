using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Spectrum.UI.Core;

namespace Spectrum.UI.Extension
{
    /// <summary>
    /// Extension method definition class on Image.
    /// </summary>
    public static class ImageExtension
    {
        /// <summary>
        /// Convert to System.Windows.Media.Imaging.BitmapSource from System.Drawing.Bitmap.
        /// </summary>
        /// <param name="source">a source bitmap.</param>
        /// <returns>BitmapSource.</returns>
        public static BitmapSource ConvertToBitmapSource(this System.Drawing.Bitmap source)
        {
            if (source == null)
            {
                return null;
            }

            var ip = IntPtr.Zero;
            try
            {
                ip = source.GetHbitmap();
                var result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ip,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                if (result.CanFreeze)
                {
                    result.Freeze();
                }

                return result;
            }
            finally
            {
                if (ip != IntPtr.Zero)
                {
                    NativeMethods.DeleteObject(ip);
                }
            }
        }

        /// <summary>
        /// Convert to System.Drawing.Bitmap from System.Windows.Media.Imaging.BitmapSource.
        /// </summary>
        /// <param name="source">a source BitmapSource</param>
        /// <returns>Bitmap</returns>
        public static System.Drawing.Bitmap ConvertToBitmap(this BitmapSource source)
        {
            if (source == null)
            {
                return null;
            }

            var bmp = new System.Drawing.Bitmap(
                source.PixelWidth,
                source.PixelHeight,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            var data = bmp.LockBits(
                new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            source.CopyPixels(
                Int32Rect.Empty,
                data.Scan0,
                data.Height * data.Stride,
                data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }
    }
}