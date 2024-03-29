﻿using AdoCurso.API.Models.Entities;
using AdoCurso.API.Models.Interfaces;
using AdoCurso.API.Models.Interfaces.v1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AdoCurso.API.Repositories.v1
{
    public class UsuarioRepositoryV1 : IUsuarioRepositoryV1
    {
        private readonly IDbConnection _connectionDB;
        public UsuarioRepositoryV1()
        {
            _connectionDB = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AdoCurso;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
        }

        public List<Usuario> GetAll()
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Usuarios;",
                                                    (SqlConnection)_connectionDB);

                _connectionDB.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Usuario usuario = new Usuario();
                    usuario.Id = reader.GetInt32("Id");
                    usuario.Nome = reader.GetString("Nome");
                    usuario.Email = reader.GetString("Email");
                    usuario.Sexo = reader.GetString("Sexo");
                    usuario.RG = reader.GetString("RG");
                    usuario.CPF = reader.GetString("CPF");
                    usuario.NomeMae = reader.GetString("NomeMae");
                    usuario.SituacaoCadastro = reader.GetString("SituacaoCadastro");
                    usuario.DataCadastro = reader.GetDateTimeOffset(8);

                    usuarios.Add(usuario);
                }
            }
            finally
            {
                _connectionDB.Close();
            }

            return usuarios;
        }

        public Usuario GetById(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Usuarios u LEFT JOIN " +
                                                    $"Contatos c ON c.UsuarioId = u.Id LEFT JOIN " +
                                                    $"EnderecosEntrega en ON en.UsuarioId = u.Id LEFT JOIN " +
                                                    $"UsuariosDepartamentos ud ON ud.UsuarioId = u.Id LEFT JOIN " +
                                                    $"Departamentos d ON ud.Id = d.Id " +
                                                    $"WHERE u.Id = @Id;",
                                                    (SqlConnection)_connectionDB);
                // Segurança contra SQL Injection
                command.Parameters.AddWithValue("@Id", id);

                _connectionDB.Open();

                SqlDataReader reader = command.ExecuteReader();

                Dictionary<int, Usuario> usuarios = new Dictionary<int, Usuario>();

                while (reader.Read())
                {
                    Usuario usuario = new Usuario();
                    if (!(usuarios.ContainsKey(reader.GetInt32(0)))) // Irá validar se no dicionário possui algum usuário com aquele Id,
                                                                     // caso não tenha, ele irá criar um. 
                    {
                        usuario.Id = reader.GetInt32(0); // Irá pegar a primeira coluna que é o ID da tabela Usuario
                        usuario.Nome = reader.GetString("Nome");
                        usuario.Email = reader.GetString("Email");
                        usuario.Sexo = reader.GetString("Sexo");
                        usuario.RG = reader.GetString("RG");
                        usuario.CPF = reader.GetString("CPF");
                        usuario.NomeMae = reader.GetString("NomeMae");
                        usuario.SituacaoCadastro = reader.GetString("SituacaoCadastro");
                        usuario.DataCadastro = reader.GetDateTimeOffset(8);

                        Contato contato = new Contato();
                        contato.Id = reader.GetInt32(9); // Irá pegar a nona coluna da consulta que é o ID do contato.
                        contato.UsuarioId = usuario.Id;
                        contato.Celular = reader.GetString("Celular");
                        contato.Telefone = reader.GetString("Telefone");

                        // Atribui o contato encontrado no banco e atribui a propriedade Contato do objeto Usuario.
                        usuario.Contato = contato;

                        // Atribui o usuário criado ao dicionário
                        usuarios.Add(usuario.Id, usuario);
                    }

                    else
                    {
                        usuario = usuarios[reader.GetInt32(0)]; // Irá atribuir ao objeto Usuario o usuário que ele encontrou no dicionario.
                    }

                    EnderecoEntrega enderecoEntrega = new EnderecoEntrega();
                    enderecoEntrega.Id = reader.GetInt32(13);
                    enderecoEntrega.UsuarioId = usuario.Id;
                    enderecoEntrega.NomeEndereco = reader.GetString("NomeEndereco");
                    enderecoEntrega.CEP = reader.GetString("CEP");
                    enderecoEntrega.Estado = reader.GetString("Estado");
                    enderecoEntrega.Cidade = reader.GetString("Cidade");
                    enderecoEntrega.Bairro = reader.GetString("Bairro");
                    enderecoEntrega.Endereco = reader.GetString("Endereco");
                    enderecoEntrega.Numero = reader.GetString("Numero");
                    enderecoEntrega.Complemento = reader.GetString("Complemento");

                    // irá validar se o endereço de entrega for nulo no usuário, ele irá criar uma lista de endereço de entregas
                    // para aquele usuario, se não for, ele só irá atribuir o novo endereço.
                    usuario.EnderecosEntrega = (usuario.EnderecosEntrega == null) ? new List<EnderecoEntrega>() : usuario.EnderecosEntrega;

                    if (usuario.EnderecosEntrega.FirstOrDefault(point => point.Id == enderecoEntrega.Id) == null)
                    {
                        usuario.EnderecosEntrega.Add(enderecoEntrega);
                    }

                    Departamento departamento = new Departamento();
                    departamento.Id = reader.GetInt32(26);
                    departamento.Nome = reader.GetString(27);

                    usuario.Departamentos = (usuario.Departamentos == null) ? new List<Departamento>() : usuario.Departamentos;

                    if (usuario.Departamentos.FirstOrDefault(point => point.Id == departamento.Id) == null)
                    {
                        usuario.Departamentos.Add(departamento);
                    }
                }

                return usuarios[usuarios.Keys.First()];
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                _connectionDB.Close();
            }
        }

        public void Insert(Usuario usuario)
        {
            _connectionDB.Open();
            SqlTransaction transaction = (SqlTransaction)_connectionDB.BeginTransaction(); // Instanciará uma transaction
            try
            {
                #region Usuario
                SqlCommand commandUsuario = new SqlCommand($"INSERT INTO Usuarios(Nome, Email, Sexo, RG, CPF, NomeMae, SituacaoCadastro, DataCadastro) " +
                                                    $"VALUES(@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, @DataCadastro);" +
                                                    $"SELECT CAST(scope_identity() AS int);", // Irá pegar o último id do escopo da tabela Usuarios.
                                                    (SqlConnection)_connectionDB);

                commandUsuario.Transaction = transaction;

                // Segurança contra SQL Injection
                commandUsuario.Parameters.AddWithValue("@Nome", usuario.Nome);
                commandUsuario.Parameters.AddWithValue("@Email", usuario.Email);
                commandUsuario.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                commandUsuario.Parameters.AddWithValue("@RG", usuario.RG);
                commandUsuario.Parameters.AddWithValue("@CPF", usuario.CPF);
                commandUsuario.Parameters.AddWithValue("@NomeMae", usuario.NomeMae);
                commandUsuario.Parameters.AddWithValue("@SituacaoCadastro", usuario.SituacaoCadastro);
                commandUsuario.Parameters.AddWithValue("@DataCadastro", usuario.DataCadastro);


                usuario.Id = (int)commandUsuario.ExecuteScalar(); // irá atribuir o último id da tabela ao id do usuario recem criado.
                #endregion

                #region Contato
                SqlCommand commandContato = new SqlCommand($"INSERT INTO Contatos(UsuarioId, Telefone, Celular) " +
                                                           $"VALUES (@UsuarioId, @Telefone, @Celular) " +
                                                           $"SELECT CAST(scope_identity() AS int);",
                                                           (SqlConnection)_connectionDB);

                commandContato.Transaction = transaction;

                commandContato.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                commandContato.Parameters.AddWithValue("@Telefone", usuario.Contato.Telefone);
                commandContato.Parameters.AddWithValue("@Celular", usuario.Contato.Celular);

                usuario.Contato.UsuarioId = usuario.Id;
                usuario.Contato.Id = (int)commandContato.ExecuteScalar();
                #endregion

                #region Endereço
                foreach (var endereco in usuario.EnderecosEntrega)
                {
                    SqlCommand commandEndereco = new SqlCommand($"INSERT INTO EnderecosEntrega(UsuarioId, NomeEndereco, CEP, Estado, Cidade, Bairro, Endereco, Numero, Complemento) " +
                                          $"VALUES (@UsuarioId, @NomeEndereco, @CEP, @Estado, @Cidade, @Bairro, @Endereco, @Numero, @Complemento) " +
                                          $"SELECT CAST(scope_identity() AS int);",
                                          (SqlConnection)_connectionDB);

                    commandEndereco.Transaction = transaction;

                    commandEndereco.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                    commandEndereco.Parameters.AddWithValue("@NomeEndereco", endereco.NomeEndereco);
                    commandEndereco.Parameters.AddWithValue("@CEP", endereco.CEP);
                    commandEndereco.Parameters.AddWithValue("@Estado", endereco.Estado);
                    commandEndereco.Parameters.AddWithValue("@Cidade", endereco.Cidade);
                    commandEndereco.Parameters.AddWithValue("@Bairro", endereco.Bairro);
                    commandEndereco.Parameters.AddWithValue("@Endereco", endereco.Endereco);
                    commandEndereco.Parameters.AddWithValue("@Numero", endereco.Numero);
                    commandEndereco.Parameters.AddWithValue("@Complemento", endereco.Complemento);

                    endereco.Id = (int)commandEndereco.ExecuteScalar();
                }
                #endregion

                #region Departamento
                foreach (var departamento in usuario.Departamentos)
                {
                    SqlCommand commandDepartamento = new SqlCommand($"INSERT INTO UsuariosDepartamentos(UsuarioId, DepartamentoId) " +
                                                                    $"VALUES (@UsuarioId, @DepartamentoId) " +
                                                                    $"SELECT CAST(scope_identity() AS int);",
                                                                    (SqlConnection)_connectionDB);

                    commandDepartamento.Transaction = transaction;

                    commandDepartamento.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                    commandDepartamento.Parameters.AddWithValue("@DepartamentoId", departamento.Id);

                    commandDepartamento.ExecuteNonQuery();
                }
                #endregion

                transaction.Commit();
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                }
                throw new Exception($"Erro ao inserir dados no banco!");
            }
            finally
            {
                _connectionDB.Close();
            }
        }

    }
}
