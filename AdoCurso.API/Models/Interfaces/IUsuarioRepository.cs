using AdoCurso.API.Models.Entities;
using System.Collections.Generic;

namespace AdoCurso.API.Models.Interfaces
{
    public interface IUsuarioRepository
    {
        public List<Usuario> GetAll();
        public Usuario GetById(int id);
        public void Insert(Usuario usuario);
        public void Update(Usuario usuario);
        public void Delete(int id);

    }
}
