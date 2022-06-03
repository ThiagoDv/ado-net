-- Instrução que pega os contatos do usuario comparando a chave estrangeira UsuarioId com o Id da tabela usuario.
SELECT * FROM Usuarios u LEFT JOIN Contatos c ON c.UsuarioId = u.Id WHERE u.Id = 1;

-- Instrução que pega os contatos e endereços do usuario comparando a chave estrangeira UsuarioId com o Id da tabela usuario.
--SELECT * FROM Usuarios u LEFT JOIN Contatos c ON c.UsuarioId = u.Id LEFT JOIN EnderecosEntrega en ON en.UsuarioId = u.Id WHERE u.Id = 1;