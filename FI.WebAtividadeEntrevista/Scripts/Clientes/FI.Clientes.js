var beneficiarios = [];
$(document).ready(function () {

    $("#CPF").mask("999.999.999-99");

    $('#formCadastro').submit(function (e) {
        e.preventDefault();
        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "CPF": $(this).find("#CPF").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val()
            },
            error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
            success:
            function (r) {
                ModalDialog("Sucesso!", r)
                $("#formCadastro")[0].reset();
            }
        });
    })
    
})

function AbrirModalBeneficiarios(e) {
    e.preventDefault();

    let modalContent = `
        <div class="row" style="display: flex; align-items: center">
            <div class="col-sm-4">
                <div class="form-group">
                    <label for="CPFBeneficiario">CPF:</label>
                    <input required="required" type="text" class="form-control" id="CPFBeneficiario" name="CPFBeneficiario" placeholder="Ex.: 010.011.111-00" maxlength="11">
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <label for="NomeBeneficiario">Nome:</label>
                    <input required="required" type="text" class="form-control" id="NomeBeneficiario" name="NomeBeneficiario" placeholder="Ex.: Maria" maxlength="50">
                </div>
            </div>

            <div class="col-sm-2">
                <button class="btn btn-success text-center" onclick="AdicionarBeneficiario()">Incluir</button>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <table class="table table-striped">
                <thead>
                        <tr>
                            <th style="width: 20%">CPF</th>
                            <th style="width: 30%">Nome</th>
                            <th style="width: 30%">Ações</th>
                        </tr>
                    </thead>
                    <tbody id="BeneficiariosGrid"></tbody>
                </table>
            </div>
        </div>
    `;

    ModalDialog('Beneficiários', modalContent);
    $("#CPFBeneficiario").mask("999.999.999-99");
}


function AdicionarBeneficiario() {
    var beneficiarioId = Math.random().toString().replace('.', '');

    let cpf = $("#CPFBeneficiario").val();
    let nome = $("#NomeBeneficiario").val();

    if (!cpf || !nome) {
        ModalDialog("Campos obrigatórios", "Os campos CPF e Nome são obrigatórios.");
        return;
    }

    if (beneficiarios.filter(element => element.CPF === cpf).length > 0) {
        ModalDialog("Beneficiário já adicionado", "O beneficiário informado já foi adicionado.");
        return;
    }

    if (!validarCPF(cpf)) {
        ModalDialog("CPF inválido", "O CPF informado é inválido.");
        return;
    }

    beneficiarios.push({ Id: beneficiarioId, CPF: cpf, Nome: nome });

    $("#BeneficiariosGrid").append(`<tr id="${beneficiarioId}">
                                        <td>${cpf}</td>
                                        <td>${nome}</td>
                                        <td>
                                            <button class="btn btn-primary" onclick="AlterarBeneficiario('${beneficiarioId}')">Alterar</button>
                                            <button class="btn btn-primary" onclick="RemoverBeneficiario('${beneficiarioId}')">Excluir</button>
                                        </td>
                                    </tr>`);

    $("#CPFBeneficiario").val('');
    $("#NomeBeneficiario").val('');
}

function RemoverBeneficiario(id) {
    beneficiarios = beneficiarios.filter(element => element.Id !== id);
    $(`#${id}`).remove();
}

function AlterarBeneficiario(id) {
    let beneficiario = beneficiarios.find(element => element.Id === id);

    $("#CPFBeneficiario").val(beneficiario.CPF);
    $("#NomeBeneficiario").val(beneficiario.Nome);

    RemoverBeneficiario(id);
}

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);

    $('#' + random).on('hidden.bs.modal', function () {
        $('#' + random).remove();
    });
    $('#' + random).modal('show');
}
