using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers {
    public class LoginController : Controller {

        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;

        public LoginController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao) {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }


        public IActionResult Index() {
            return View();
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel) {
            try {
                if (ModelState.IsValid) {

                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorLogin(loginModel.Login);

                    if (usuario != null) {

                        if (usuario.SenhaValida(loginModel.Senha)) {

                            return RedirectToAction("Index", "Home");

                        }

                        TempData["MensagemErro"] = $"Senha do usuário é inválido(s). Por favor, tente novamente.";

                    }

                    TempData["MensagemErro"] = $"Usuário e/ou senha inválido(s). Por favor, tente novamente.";

                }

                return View("Index");
            } catch (Exception erro) {

                TempData["MensagemErro"] = $"Ops, não foi possível realizar seu login, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
