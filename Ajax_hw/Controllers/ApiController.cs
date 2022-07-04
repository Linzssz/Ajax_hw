using Ajax_hw.Models;
using Microsoft.AspNetCore.Mvc;
using prjAjax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ajax_hw.Controllers
{
    public class ApiController : Controller
    {
        private DemoContext _context;

        public ApiController(DemoContext context)
        {
            _context = context;
        }

        public IActionResult Index(CUser user)
        {

            if (string.IsNullOrEmpty(user.name))
                return Content("請輸入姓名", "text/plain", System.Text.Encoding.UTF8);
            else
            {
                Member mem = _context.Members.FirstOrDefault(m => m.Name == user.name);
                if (mem!=null)
                    return Content("此帳號已註冊,請更換帳號", "text/plain", System.Text.Encoding.UTF8);
                else
                    return Content("此帳號可以註冊", "text/plain", System.Text.Encoding.UTF8);

            }

        }



        public IActionResult Register()
        {

            return View();
        }
    }


}

