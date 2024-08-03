function formatarCpf(value) {
    const cpf = value.replace(/\D/g, '');
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/g, "\$1.\$2.\$3-\$4");
}

function validarCPF(CPF) {
    const multiplicadorInterno = [10, 9, 8, 7, 6, 5, 4, 3, 2];
    const multiplicadorExterno = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

    CPF = limparCPF(CPF);
    if (CPF.length !== 11)
        return false;

    for (let j = 0; j < 10; j++) {
        if (j.toString().padStart(11, j.toString()) === CPF)
            return false;
    }

    let subCpf = CPF.substring(0, 9);
    let somaDigitos = 0;

    for (let i = 0; i < 9; i++)
        somaDigitos += parseInt(subCpf[i]) * multiplicadorInterno[i];

    let resto = somaDigitos % 11;
    if (resto < 2)
        resto = 0;
    else
        resto = 11 - resto;

    let digitoValidador = resto.toString();
    subCpf = subCpf + digitoValidador;
    somaDigitos = 0;

    for (let i = 0; i < 10; i++) {
        somaDigitos += parseInt(subCpf[i]) * multiplicadorExterno[i];
    }

    resto = somaDigitos % 11;
    if (resto < 2)
        resto = 0;
    else
        resto = 11 - resto;

    digitoValidador = digitoValidador + resto.toString();

    return CPF.endsWith(digitoValidador);
}

function limparCPF(CPF) {
    return CPF.replace(/\D/g, '');
}
