﻿using ControleDeContatos.Filters;
using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers {
    [PaginaParaUsuarioLogado]
    public class ContatoController : Controller {
        private readonly IContatoRepositorio _contatoRepositorio;
        private ISessao _sessao;
        public ContatoController(IContatoRepositorio contatoRepositorio, ISessao sessao) {
            _contatoRepositorio = contatoRepositorio;
            _sessao = sessao;
        }
        public IActionResult Index() {

            UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
            List<ContatoModel> contatos = _contatoRepositorio.BuscarTodos(usuarioLogado.Id);

            return View(contatos);
        }

        public IActionResult Criar() {
            return View();
        }

        public IActionResult Editar(int id) {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);
            return View(contato);
        }

        public IActionResult ApagarConfirmacao(int id) {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);

            return View(contato);
        }
        

        public IActionResult Apagar(int id) {
            try {
                bool apagado = _contatoRepositorio.Apagar(id);

                if (apagado) {
                    TempData["MensagemSucesso"] = "Contato apagado com sucesso!";
                } else {
                    TempData["MensagemErro"] = "Não foi possível apagar seu contato!";
                }
                return RedirectToAction("Index");

            } catch (System.Exception erro) {
                TempData["MensagemErro"] = "Não foi possível apagar seu contato, tente novamente!" +
                    $"detalhe do erro:{erro.Message}";
                return RedirectToAction("Index");
            }
        }

            [HttpPost]
            public IActionResult Criar(ContatoModel contato) {
                try {
                    if (ModelState.IsValid) {

                    UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                    contato.UsuarioId = usuarioLogado.Id;

                        _contatoRepositorio.Adicionar(contato);
                        TempData["MensagemSucesso"] = "Contato cadastrado com sucesso!";
                        return RedirectToAction("Index");
                    }
                    return View(contato);
                } catch (System.Exception erro) {
                    TempData["MensagemErro"] = "Não foi possível cadastrar seu contato, tente novamente!" +
                        $"detalhe do erro:{erro.Message}";
                    return RedirectToAction("Index");
                }
            }

            [HttpPost]
            public IActionResult Alterar(ContatoModel contato) {
                try {
                    if (ModelState.IsValid) {

                    UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                    contato.UsuarioId = usuarioLogado.Id;

                    _contatoRepositorio.Atualizar(contato);
                        TempData["MensagemSucesso"] = "Contato alterado com sucesso!";
                        return RedirectToAction("Index");
                    }

                    return View("Editar", contato);

                } catch (System.Exception erro) {
                    TempData["MensagemErro"] = "Não foi possível atualizar seu contato, tente novamente!" +
                        $"detalhe do erro:{erro.Message}";
                    return RedirectToAction("Index");
                }
            }
        }
    }

