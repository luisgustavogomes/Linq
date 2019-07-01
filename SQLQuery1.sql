CREATE PROCEDURE [dbo].[sp_Vendas_Por_cliente]
    @clienteId int = 0 
AS
BEGIN
    SELECT
         i.FaixaId
        ,i.ItemNotaFiscalId
        ,i.NotaFiscalId
        ,i.PrecoUnitario
        ,i.Quantidade
        ,i.Precounitario * i.Quantidade As Total
        ,n.DataNotaFiscal
        ,f.nome
    FROM ItemNotaFiscal i
    JOIN NotaFiscal n on (i.NotaFiscalId=n.NotaFiscalId)
    JOIN Faixa f on (f.FaixaId = i.FaixaId)
    WHERE n.ClienteId = @clienteId 

END 
