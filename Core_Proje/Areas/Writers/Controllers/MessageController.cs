﻿using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.Areas.Writers.Controllers
{ 
    [Area("Writers")]
    [Route("Writers/Message")]
    public class MessageController : Controller
    {
        WriterMessageManager wm = new WriterMessageManager(new EfWriterMessageDal());
        private readonly UserManager<WriterUser> _userManager;

        public MessageController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        [Route("")]
        [Route("ReciverMessage")]
        public async Task< IActionResult> ReciverMessage(string p)
        {
          var values = await _userManager.FindByNameAsync(User.Identity.Name);
            p = values.Email;
            var messageList = wm.GetListReciverMessage(p);
            return View(messageList);
        }


        [Route("")]
        [Route("SenderMessage")]
        public async Task<IActionResult> SenderMessage(string p)
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            p = values.Email;
            var messageList = wm.GetListSendMessage(p);
            return View(messageList);
        }


        [Route("MessageDetails/{id}")]
        public IActionResult MessageDetails(int id)
        {
            WriterMessage writerMessage = wm.TGetByID(id);
            return View(writerMessage);
        }

        [Route("ReceiverMessageDetails/{id}")]
        public IActionResult ReceiverMessageDetails(int id)
        {
            WriterMessage writerMessage = wm.TGetByID(id);
            return View(writerMessage);
        }

        [HttpGet]
        [Route("")]
        [Route("SendMessage")]
        public IActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        [Route("SendMessage")]
        public async Task<IActionResult> SendMessage(WriterMessage p)
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            string mail = values.Email;
            string name = values.Name + " " + values.Surname;
            p.Date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            p.Sender = mail;
            p.SenderName = name;
            Context c = new Context();
            var usernamesurname = c.Users.Where(x => x.Mail == p.Reciver).Select(y => y.Name + " " + y.Surname).FirstOrDefault();
            p.ReciverName = usernamesurname;
            wm.TAdd(p);

            return RedirectToAction("SenderMessage", "Message");

        }
    }
}
