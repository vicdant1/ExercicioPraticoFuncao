CREATE PROC FI_SP_ConsBenefPorCliente
	@IDCLIENTE		BIGINT
AS
BEGIN
	SELECT ID, NOME, CPF, IDCLIENTE FROM BENEFICIARIOS WITH(NOLOCK) WHERE IDCLIENTE = @IDCLIENTE
END