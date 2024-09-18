using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;

namespace ControleDeContatos.Controllers {
    public class LoginController : Controller {

        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;
        private readonly IEmail _email;

        public LoginController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao, IEmail email) {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _email = email;
        }


        public IActionResult Index() {
            // Se o usuário estiver logado, redirecionar para home

            if (_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult RedefinirSenha() {
            return View();
        }

        public IActionResult Sair() {
            _sessao.RemoverSessaoDoUsuario();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel) {
            try {
                if (ModelState.IsValid) {

                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorLogin(loginModel.Login);

                    if (usuario != null) {

                        if (usuario.SenhaValida(loginModel.Senha)) {

                            _sessao.CriarSessaoDoUsuario(usuario);

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

        [HttpPost]
        public IActionResult EnviarLinkParaRedefinirSenha(RedefinirSenhaModel redefinirsSenhaModel) {
            try {
                if (ModelState.IsValid) {

                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorEmailELogin(redefinirsSenhaModel.Email, redefinirsSenhaModel.Login);

                    if (usuario != null) {

                        string novaSenha = usuario.GerarNovaSenha();

                        string mensagem = $@"
                                            <html>
                                              <body style='font-family: Arial, sans-serif; background-color: #f5f5f5; margin: 0; padding: 0;'>
                                                <table width='100%' cellpadding='0' cellspacing='0' style='margin: 0; padding: 20px 0;'>
                                                  <tr>
                                                    <td align='center'>
                                                      <table width='600' cellpadding='0' cellspacing='0' style='background-color: #ffffff; border-radius: 10px; box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1); overflow: hidden;'>
                                                        <!-- Header -->
                                                        <tr>
                                                          <td style='background-color: #007BFF; padding: 20px; text-align: center;'>
                                                            <img src='https://via.placeholder.com/200x50?text=LinkUp' alt='LinkUp Logo' style='max-width: 200px;'>
                                                          </td>
                                                        </tr>

                                                        <!-- Body -->
                                                        <tr>
                                                          <td style='padding: 30px;'>
                                                            <h1 style='font-size: 24px; color: #333333; margin-bottom: 20px; text-align: center;'>Solicitação de Redefinição de Senha</h1>
                
                                                            <p style='font-size: 16px; color: #333333; line-height: 1.6;'>
                                                              Olá,
                                                            </p>
                                                            <p style='font-size: 16px; color: #333333; line-height: 1.6;'>
                                                              Recebemos uma solicitação para redefinir sua senha. Se você não fez essa solicitação, entre em contato imediatamente com nosso suporte técnico.
                                                            </p>
                                                            <p style='font-size: 16px; color: #333333; line-height: 1.6;'>
                                                              Sua nova senha é:
                                                            </p>
                
                                                            <div style='text-align: center; margin: 20px 0;'>
                                                              <span style='font-size: 24px; color: #ffffff; background-color: #D9534F; padding: 15px 30px; border-radius: 5px; display: inline-block; font-weight: bold;'>
                                                                {novaSenha}
                                                              </span>
                                                            </div>

                                                            <p style='font-size: 16px; color: #333333; line-height: 1.6;'>
                                                              Por favor, altere sua senha após o primeiro login para garantir a segurança da sua conta.
                                                            </p>
                
                                                            <p style='font-size: 16px; color: #333333; line-height: 1.6;'>
                                                              Caso precise de mais assistência, nossa equipe de suporte está disponível para ajudá-lo. Entre em contato conosco pelo e-mail abaixo:
                                                            </p>

                                                            <p style='font-size: 18px; text-align: center;'>
                                                              <a href='mailto:suporte@dominio.com' style='color: #007BFF; font-weight: bold; text-decoration: none;'>suporte@dominio.com</a>
                                                            </p>
                
                                                            <hr style='border: none; border-top: 1px solid #dddddd; margin: 30px 0;' />

                                                            <p style='font-size: 14px; color: #888888; text-align: center;'>
                                                              Este é um e-mail automatizado enviado por <strong>LinkUp</strong>. Por favor, não responda a este e-mail.
                                                            </p>
                                                          </td>
                                                        </tr>

                                                        <!-- Footer -->
                                                        <tr>
                                                          <td style='background-color: #f7f7f7; padding: 20px; text-align: center;'>
                                                            <p style='font-size: 14px; color: #888888;'>
                                                              © 2024 LinkUp. Todos os direitos reservados.
                                                            </p>
                                                            <p style='font-size: 14px; color: #888888;'>
                                                              Rua Exemplo, 123 - São Paulo - SP
                                                            </p>
                                                          </td>
                                                        </tr>
                                                      </table>
                                                    </td>
                                                  </tr>
                                                </table>
                                              </body>
                                            </html>";


                        bool emailEnviado = _email.Enviar(usuario.Email, "LinkUp - Nova Senha", mensagem);

                        if (emailEnviado) {
                            _usuarioRepositorio.Atualizar(usuario);

                            TempData["MensagemSucesso"] = $"Foi enviado para o e-mail cadastrado uma nova senha.";
                        } else {
                            TempData["MensagemErro"] = $"Não foi possível redefinir sua senha. Por favor, tente novamente.";
                        }
                        return RedirectToAction("Index", "Login");


                    }

                    TempData["MensagemErro"] = $"Não foi possível redefinir sua senha. Por favor, verifique os dados informados.";

                }

                return View("Index");
            } catch (Exception erro) {

                TempData["MensagemErro"] = $"Ops, não foi possível redefinir sua senha, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
