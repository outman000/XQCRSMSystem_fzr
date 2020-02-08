using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using QRCoder;
using System.IO;
using System.Drawing.Imaging;
using System.Configuration;

namespace XQCRSMSystem.Common
{
    public class QRCoderHelper
    {
        /// <summary>
        /// 创建二维码返回文件路径名称
        /// </summary>
        /// <param name="plainText">二维码内容</param>
        public static string CreateQRCodeToFile(string plainText)
        {
            try
            {
                string fileName = "";
                if (String.IsNullOrEmpty(plainText))
                {
                    return "";
                }

                //二维码文件目录
                string filePath = System.Web.HttpContext.Current.Server.MapPath("~/") + "QRCoder\\";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string name = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 1000) + ".jpeg";
                //创建二维码文件路径名称
                fileName = filePath + name;

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                //QRCodeGenerator.ECCLevel:纠错能力,Q级：约可纠错25%的数据码字
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(plainText, QRCodeGenerator.ECCLevel.Q);
                QRCode qrcode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrcode.GetGraphic(15);
                qrCodeImage.Save(fileName, ImageFormat.Jpeg);

                //生成 http绝对路径
                string relativeUrl = ConfigurationManager.AppSettings["ImageQRCode"].ToString();
                string httpPath = relativeUrl + name;
                return httpPath;
            }
            catch (Exception ex)
            {
                throw new Exception("创建二维码返回文件路径名称方法异常", ex);
            }
        }

        /// <summary>
        /// 创建二维码返回byte数组
        /// </summary>
        /// <param name="plainText">二维码内容</param>
        public static byte[] CreateQRCodeToBytes(string plainText)
        {
            try
            {
                if (String.IsNullOrEmpty(plainText))
                {
                    return null;
                }

                QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
                //QRCodeGenerator.ECCLevel:纠错能力,Q级：约可纠错25%的数据码字
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(plainText, QRCodeGenerator.ECCLevel.Q);
                QRCode qrcode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrcode.GetGraphic(15);
                MemoryStream ms = new MemoryStream();
                qrCodeImage.Save(ms, ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();

                return arr;
            }
            catch (Exception ex)
            {
                throw new Exception("创建二维码返回byte数组方法异常", ex);
            }
        }

        /// <summary>
        /// 创建二维码返回Base64字符串
        /// </summary>
        /// <param name="plainText">二维码内容</param>
        public static string CreateQRCodeToBase64(string plainText, bool hasEdify = true)
        {
            try
            {
                string result = "";
                if (String.IsNullOrEmpty(plainText))
                {
                    return "";
                }

                QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
                //QRCodeGenerator.ECCLevel:纠错能力,Q级：约可纠错25%的数据码字
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(plainText, QRCodeGenerator.ECCLevel.Q);
                QRCode qrcode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrcode.GetGraphic(15);
                MemoryStream ms = new MemoryStream();
                qrCodeImage.Save(ms, ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                if (hasEdify)
                {
                    result = "data:image/jpeg;base64," + Convert.ToBase64String(arr);
                }
                else
                {
                    result = Convert.ToBase64String(arr);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("创建二维码返回Base64字符串方法异常", ex);
            }
        }
    }
}