using ControleDeContatos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ControleDeContatos.Repositorio {
    public interface IUsuarioRepositorio {
        UsuarioSemSenhaModel ListarPorId(int id);
        List<UsuarioSemSenhaModel> BuscarTodos();
        UsuarioSemSenhaModel Adicionar(UsuarioSemSenhaModel usuario);
        UsuarioSemSenhaModel Atualizar(UsuarioSemSenhaModel usuario);

        bool Apagar(int id);

    }
}
