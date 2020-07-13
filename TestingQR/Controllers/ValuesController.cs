using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using TestingQR.Models;

namespace TestingQR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        public ValuesController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost]
        public ActionResult GenerateQRCode(QRCodeGenerate qRCodeGenerate)
        {
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(qRCodeGenerate.QRTxt, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return Ok(BitmapToBytesCode(qrCodeImage));
        }

        private protected Byte[] BitmapToBytesCode(Bitmap image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        [HttpPost("Send")]
        public ActionResult SendEmail(EmailModel emailModel)
        {
            try
            {
                string text = System.IO.File.ReadAllText(@"C:/Users/JEVG/source/repos/TestingQR/TestingQR/Models/Mail.html");
                var x = text.Replace("{{base64}}", emailModel.Img);
                emailModel.Message = x;
                _emailSender.SendEmail(emailModel.Destination, emailModel.Subject, emailModel.Message);
                return Ok(x);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
