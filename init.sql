CREATE DATABASE dbProtocolos;

\c dbProtocolos;

-- CREATE TABLE Protocolos (
--     Id SERIAL PRIMARY KEY,
--     NumeroProtocolo VARCHAR(50) UNIQUE NOT NULL,
--     NumeroVia INT NOT NULL,
--     Cpf VARCHAR(11) NOT NULL,
--     Rg VARCHAR(9) NOT NULL,
--     Nome VARCHAR(100) NOT NULL,
--     NomeMae VARCHAR(100),
--     NomePai VARCHAR(100),
--     Foto BYTEA NOT NULL,
--     UNIQUE(Cpf, NumeroVia),
--     UNIQUE(Rg, NumeroVia)
-- );