﻿using ControleDeContatos.Filters;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers {

    [PaginaRestritaSomenteAdmin]

    public class UsuarioController : Controller {

        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio) {
            _usuarioRepositorio = usuarioRepositorio;
        }
        public IActionResult Index() {
            List<UsuarioModel> usuario = _usuarioRepositorio.BuscarTodos();

            return View(usuario);
        }
        public IActionResult Criar() {
            return View();
        }
        public IActionResult Editar(int id) {
                UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View(usuario);
        }
        public IActionResult ApagarConfirmacao(int id) {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);

            return View(usuario);
        }
         


        public IActionResult Apagar(int id) {
            try {
                bool apagado = _usuarioRepositorio.Apagar(id);

                if (apagado) {
                    TempData["MensagemSucesso"] = "usuário apagado com sucesso!";
                } else {
                    TempData["MensagemErro"] = "Não foi possível apagar seu usuário!";
                }
                return RedirectToAction("Index");

            } catch (System.Exception erro) {
                TempData["MensagemErro"] = "Não foi possível apagar seu usuário, tente novamente!" +
                    $"detalhe do erro:{erro.Message}";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario) {
            try {
                if (ModelState.IsValid) {
                    _usuarioRepositorio.Adicionar(usuario);
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(usuario);
            } catch (System.Exception erro) {
                TempData["MensagemErro"] = "Não foi possível cadastrar seu usuário, tente novamente!" +
                    $"detalhe do erro:{erro.Message}";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Editar(UsuarioSemSenhaModel usuarioSemSenhaModel) {
            try {

                UsuarioModel usuario = null;

                if (ModelState.IsValid) {

                    usuario = new UsuarioModel() {
                        Id = usuarioSemSenhaModel.Id,
                        Nome = usuarioSemSenhaModel.Nome,
                        Login = usuarioSemSenhaModel.Login,
                        Email = usuarioSemSenhaModel.Email,
                        Perfil = usuarioSemSenhaModel.Perfil
                    };

                    usuario = _usuarioRepositorio.Atualizar(usuario);
                    TempData["MensagemSucesso"] = "Usuário alterado com sucesso!";
                    return RedirectToAction("Index");
                }

                return View(usuario);

            } catch (System.Exception erro) {
                TempData["MensagemErro"] = "Não foi possível atualizar seu usuário, tente novamente!" +
                    $"detalhe do erro:{erro.Message}";
                return RedirectToAction("Index");
            }
        }

    }
}
