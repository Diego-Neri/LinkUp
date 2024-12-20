﻿using ControleDeContatos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ControleDeContatos.Repositorio {
    public interface IUsuarioRepositorio {
        UsuarioModel BuscarPorLogin(string login);
        UsuarioModel BuscarPorEmailELogin(string email, string login);
        UsuarioModel ListarPorId(int id);
        List<UsuarioModel> BuscarTodos();
        UsuarioModel Adicionar(UsuarioModel usuario);
        UsuarioModel Atualizar(UsuarioModel usuario);

        UsuarioModel AlterarSenha(AlterarSenhaModel alterarSenha);
        bool Apagar(int id);

    }
}
