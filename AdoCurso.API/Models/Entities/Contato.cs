﻿namespace AdoCurso.API.Models.Entities
{
    public class Contato
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
    }
}
