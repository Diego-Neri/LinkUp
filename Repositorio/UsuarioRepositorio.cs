using ControleDeContatos.Data;
using ControleDeContatos.Models;

namespace ControleDeContatos.Repositorio {
    public class UsuarioRepositorio : IUsuarioRepositorio {
        private readonly BancoContext _context;
        public UsuarioRepositorio(BancoContext bancoContext) {
            this._context = bancoContext;
        }
        public UsuarioSemSenhaModel ListarPorId(int id) {
            return _context.Usuarios.FirstOrDefault(x => x.Id == id);
        }

        public List<UsuarioSemSenhaModel> BuscarTodos() {

            return _context.Usuarios.ToList();
        }
        public UsuarioSemSenhaModel Adicionar(UsuarioSemSenhaModel usuario) {
            //Gravar no banco de dados
            usuario.DataCadastro = DateTime.Now;
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return usuario;
        }

        public UsuarioSemSenhaModel Atualizar(UsuarioSemSenhaModel usuario) {
            UsuarioSemSenhaModel usuarioDB = ListarPorId(usuario.Id);
            if (usuarioDB == null) throw new Exception("Houve um erro na atualização do usuário!");

            usuarioDB.Nome = usuario.Nome;
            usuarioDB.Email = usuario.Email;
            usuarioDB.Login = usuario.Login;
            usuarioDB.Perfil = usuario.Perfil;
            usuarioDB.DataCadastro = DateTime.Now;
            usuarioDB.DataAtualizacao = DateTime.Now;

            _context.Usuarios.Update(usuarioDB);
            _context.SaveChanges();

            return usuarioDB;
        }

        public bool Apagar(int id) {
            UsuarioSemSenhaModel usuarioDB = ListarPorId(id);
            if (usuarioDB == null) throw new Exception("Houve um erro ao deletar usuário");

            _context.Usuarios.Remove(usuarioDB);
            _context.SaveChanges();

            return true;
        }
    }
}
