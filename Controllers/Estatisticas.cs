using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers {
    public class Estatisticas : Controller {
        public IActionResult Index() {
            ViewBag.Dados = new List<int> { 100, 200, 300, 400 };

            return View();
        }
    }
}
