using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhomB4.Controllers.Proxy
{
    public class Proxy
    {
        //Tải ảnh sản phẩm
        public interface IImage
        {
            void Display();
        }

        public class RealImage : IImage
        {
            private string _imageUrl;

            public RealImage(string imageUrl)
            {
                _imageUrl = imageUrl;
                LoadImageFromUrl(_imageUrl);
            }

            public void Display()
            {
                Console.WriteLine("Displaying image from URL: " + _imageUrl);
                // Code để hiển thị ảnh
            }

            private void LoadImageFromUrl(string url)
            {
                // Code để tải ảnh từ URL
                Console.WriteLine("Loading image from URL: " + url);
            }
        }

        public class ProxyImage : IImage
        {
            private string _imageUrl;
            private RealImage _realImage;

            public ProxyImage(string imageUrl)
            {
                _imageUrl = imageUrl;
            }

            public void Display()
            {
                if (_realImage == null)
                {
                    _realImage = new RealImage(_imageUrl);
                }
                _realImage.Display();
            }
        }

        public class TreeController : Controller
        {
            public ActionResult DisplayImage(string imageUrl)
            {
                IImage image = new ProxyImage(imageUrl);
                image.Display();
                return View();
            }
        }
    }
}