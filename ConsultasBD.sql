--INSTRUÇÕES SQL DE CONSULTA DA API

-- Instrução que pega os contatos do usuario comparando a chave estrangeira UsuarioId com o Id da tabela usuario.
--SELECT * FROM Usuarios u LEFT JOIN 
--Contatos c ON c.UsuarioId = u.Id WHERE u.Id = 1;

-- Instrução que pega os contatos e endereços do usuario comparando a chave estrangeira UsuarioId com o Id da tabela usuario.
--SELECT * FROM Usuarios u LEFT JOIN 
--Contatos c ON c.UsuarioId = u.Id LEFT JOIN 
--EnderecosEntrega en ON en.UsuarioId = u.Id WHERE u.Id = 1;

-- Instrução que pega os contatos, endereços e departamentos do usuario comparando a chave estrangeira UsuarioId com o Id da tabela usuario.
--SELECT * FROM Usuarios u LEFT JOIN 
--Contatos c ON c.UsuarioId = u.Id LEFT JOIN 
--EnderecosEntrega en ON en.UsuarioId = u.Id LEFT JOIN
--UsuariosDepartamentos ud ON ud.UsuarioId = u.Id LEFT JOIN
--Departamentos d ON ud.Id = d.Id
--WHERE u.Id = 2002;

--INSTRUÇÕES SQL DE INSERÇÃO DA API

-- Instrução que insere os valores nas respectivas colunas da tabela Contatos
--INSERT INTO Contatos(UsuarioId, Telefone, Celular)
--VALUES (@UsuarioId, @Telefone, @Celular)
--SELECT CAST(scope_identity() AS int);

-- Instrução que insere os valores nas respectivas colunas da tabela EnderecosEntrega
--INSERT INTO EnderecosEntrega(UsuarioId, NomeEndereco, CEP, Estado, Cidade, Bairro, Endereco, Numero, Complemento)
--VALUES (@UsuarioId, @NomeEndereco, @CEP, @Estado, @Cidade, @Bairro, @Endereco, @Numero, @Complemento)
--SELECT CAST(scope_identity() AS int);

-- Instrução que insere os valores nas respectivas colunas da tabela EnderecosEntrega
--INSERT INTO UsuariosDepartamentos(UsuarioId, DepartamentoId)
--VALUES (@UsuarioId, @DepartamentoId)
--SELECT CAST(scope_identity() AS int);

SELECT * From UsuariosDepartamentos WHERE UsuarioId = 4002;