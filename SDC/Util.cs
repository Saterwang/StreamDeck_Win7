using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SDC
{
    [Serializable]
    public struct SerializableBitmapImageWrapper : ISerializable
    {
        readonly BitmapImage bitmapImage;

        public static implicit operator BitmapImage(SerializableBitmapImageWrapper wrapper)
        {
            return wrapper.BitmapImage;
        }

        public static implicit operator SerializableBitmapImageWrapper(BitmapImage bitmapImage)
        {
            return new SerializableBitmapImageWrapper(bitmapImage);
        }

        public BitmapImage BitmapImage { get { return bitmapImage; } }

        public SerializableBitmapImageWrapper(BitmapImage bitmapImage)
        {
            this.bitmapImage = bitmapImage;
        }

        public SerializableBitmapImageWrapper(SerializationInfo info, StreamingContext context)
        {
            byte[] imageBytes = (byte[])info.GetValue("image", typeof(byte[]));
            if (imageBytes == null)
                bitmapImage = null;
            else
            {
                using (var ms = new MemoryStream(imageBytes))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    bitmapImage = bitmap;
                }
            }
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            byte[] imageBytes;
            if (bitmapImage == null)
                imageBytes = null;
            else
                using (var ms = new MemoryStream())
                {
                    BitmapImage.SaveToPng(ms);
                    imageBytes = ms.ToArray();
                }
            info.AddValue("image", imageBytes);
        }

        #endregion
    }

    public static class BitmapHelper
    {
        public static void SaveToPng(this BitmapSource bitmap, Stream stream)
        {
            var encoder = new PngBitmapEncoder();
            SaveUsingEncoder(bitmap, stream, encoder);
        }

        public static void SaveUsingEncoder(this BitmapSource bitmap, Stream stream, BitmapEncoder encoder)
        {
            BitmapFrame frame = null;
            try
            {
                 frame = BitmapFrame.Create(bitmap);
            }
            catch
            {

            }
            if (frame != null)
            {
                encoder.Frames.Add(frame);
                encoder.Save(stream);
            }
        }

        public static BitmapImage FromUri(string path)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path);
            bitmap.EndInit();
            return bitmap;
        }
    }

    public class Util
    {
        public static T DeepCopy<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                //序列化成流
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                //反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }
    }

}
