using Ajax_hw.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjAjax.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ajax_hw.Controllers
{
    public class ApiController : Controller
    {
        private DemoContext _context;
        private IWebHostEnvironment _host;

        public ApiController(DemoContext context, IWebHostEnvironment host)
        {
            _host = host;
            _context = context;
        }

        public IActionResult Index(CUser user)
        {

            if (string.IsNullOrEmpty(user.name))
                return Content("請輸入姓名", "text/plain", System.Text.Encoding.UTF8);
            else
            {
                Member mem = _context.Members.FirstOrDefault(m => m.Name == user.name);
                if (mem != null)
                    return Content("此帳號已註冊,請更換帳號", "text/plain", System.Text.Encoding.UTF8);
                else
                    return Content("此帳號可以註冊", "text/plain", System.Text.Encoding.UTF8);

            }

        }

        public IActionResult GetbytImg(int id=1)

        {

            Member mem = _context.Members.Find(id);
            byte[] img = mem.FileData;

            return File(img, "image/jpeg");
        
        
        }


        public IActionResult Register(Member mem, IFormFile file)
        {   //設定路徑
            string path = Path.Combine(_host.WebRootPath, "upload", file.FileName);
            //使用using完成動作即關閉,避免資料鎖住
            using (var filestream = new FileStream(path, FileMode.Create))
            {
                //儲存檔案
                file.CopyTo(filestream);

            }

            byte[] imgbyt = null;

            using (var memorystream = new MemoryStream())
            {
                //存入記憶體
                file.CopyTo(memorystream);
                //轉型成byte
                imgbyt = memorystream.ToArray();

            }
            mem.FileName = file.FileName;
            mem.FileData = imgbyt;

            _context.Add(mem);//加入資料庫
            _context.SaveChanges();//儲存資料
            string info = $"{ file.FileName}-{file.ContentType}-{file.Length}";
            return Content(info, "text/plain", System.Text.Encoding.UTF8);
        }

        public IActionResult AccountCheck()
        {

            return View();
        }


        public IActionResult City()
        {
            var city = _context.Addresses.Select(c => c.City).Distinct();
            return Json(city);

        }

        public IActionResult districts(string city)
        {
            var districts = _context.Addresses.Where(c => c.City == city).Select(c => c.SiteId).Distinct();
            return Json(districts);
        }

        public IActionResult Road(string districts)
        {

            var road = _context.Addresses.Where(c => c.SiteId == districts).Select(c => c.SiteId);
            return Json(road);

        }
        public IActionResult Address()
        {
            return View();
        }
        
    }
}




 
