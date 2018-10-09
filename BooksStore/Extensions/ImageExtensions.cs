using System;
using System.Drawing;

namespace BooksStore
{
    static class ImageExtensions
    {
        public static Image Resize(this Image image, float height)
        {
            if(height == 0)
            {
                throw new ArgumentException("Image height should be greater than zero");
            }

            var width = height / image.Height * image.Width; // пропорционально
           return image.GetThumbnailImage((int)width, (int)height, null, IntPtr.Zero);
        }
    }
}
