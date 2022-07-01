using AdoCurso.API.Models.Entities;
using System.Collections.Generic;

namespace AdoCurso.API.Models.Interfaces.v1
{
    public interface IUsuarioRepositoryV1
    {
        public List<Usuario> GetAll();
        public Usuario GetById(int id);
        public void Insert(Usuario usuario);
    }
}
