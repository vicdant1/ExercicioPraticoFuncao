using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.Extension;
using FI.AtividadeEntrevista.Util;
using FI.WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            model.CPF = model.CPF.LimparCPF();
            if(!CPFHelper.VerificarCPFValido(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("CPF inválido");
            }
            if(bo.VerificarExistencia(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("Já existe um cliente cadastrado com o mesmo CPF.");
            }

            model.Id = bo.Incluir(new Cliente()
            {                    
                CEP = model.CEP,
                Cidade = model.Cidade,
                Email = model.Email,
                Estado = model.Estado,
                Logradouro = model.Logradouro,
                Nacionalidade = model.Nacionalidade,
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                Telefone = model.Telefone,
                CPF = model.CPF
            });

            if(model.Beneficiarios != null)
            {
                BoBeneficiario boBeneficiario = new BoBeneficiario();
                foreach(var beneficiario in model.Beneficiarios)
                {
                    if(!boBeneficiario.VerificarBeneficiarioPorCliente(beneficiario.CPF, model.Id))
                    {
                        boBeneficiario.Incluir(new Beneficiario()
                        {
                            CPF = beneficiario.CPF.LimparCPF(),
                            Nome = beneficiario.Nome,
                            IdCliente = model.Id
                        });
                    }
                }
            }

           
            return Json("Cadastro efetuado com sucesso");
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
       
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            Cliente cliente = bo.Consultar(model.Id);
            if(cliente == null)
            {
                Response.StatusCode = 400;
                return Json("Cliente não encontrado");
            }

            model.CPF = model.CPF.LimparCPF();
            if (!CPFHelper.VerificarCPFValido(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("CPF inválido");
            }

            if (!cliente.CPF.Equals(model.CPF) && bo.VerificarExistencia(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("Já existe um cliente cadastrado com o mesmo CPF.");
            }

            if(model.Beneficiarios != null)
            {
                BoBeneficiario boBeneficiario = new BoBeneficiario();
                var beneficiariosCliente = boBeneficiario.Listar(model.Id);

                var beneficiariosParaAdicionar = model.Beneficiarios.Where(benef => !beneficiariosCliente.Any(benefCliente => benefCliente.CPF.LimparCPF().Equals(benef.CPF.LimparCPF()))).ToList();
                foreach (var beneficiario in beneficiariosParaAdicionar)
                {
                    boBeneficiario.Incluir(new Beneficiario()
                    {
                        CPF = beneficiario.CPF.LimparCPF(),
                        Nome = beneficiario.Nome,
                        IdCliente = model.Id
                    });
                }


                foreach (var beneficiario in beneficiariosCliente)
                {
                    if (!model.Beneficiarios.Any(x => x.CPF.LimparCPF().Equals(beneficiario.CPF.LimparCPF())))
                    {
                        boBeneficiario.Excluir(beneficiario.Id);
                    }
                    else
                    {
                        var beneficiarioModel = model.Beneficiarios.FirstOrDefault(x => x.CPF.LimparCPF().Equals(beneficiario.CPF.LimparCPF()));
                        if(boBeneficiario.VerificarBeneficiarioPorCliente(beneficiario.CPF, model.Id))
                        {
                            boBeneficiario.Alterar(new Beneficiario()
                            {
                                Id = beneficiario.Id,
                                CPF = beneficiarioModel.CPF.LimparCPF(),
                                Nome = beneficiarioModel.Nome,
                            });
                        }
                    }
                }
            }

            bo.Alterar(new Cliente()
            {
                Id = model.Id,
                CEP = model.CEP,
                Cidade = model.Cidade,
                Email = model.Email,
                Estado = model.Estado,
                Logradouro = model.Logradouro,
                Nacionalidade = model.Nacionalidade,
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                Telefone = model.Telefone,
                CPF = model.CPF
            });
                               
            return Json("Cadastro alterado com sucesso");
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                BoBeneficiario boBeneficiario = new BoBeneficiario();
                List<Beneficiario> beneficiarios = boBeneficiario.Listar(cliente.Id);
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF.LimparCPF(),
                    Beneficiarios = beneficiarios.Select(x => new BeneficiarioModel()
                    {
                        Id = x.Id.ToString(),
                        CPF = x.CPF.FormatAsCPF(),
                        Nome = x.Nome
                    }).ToList()
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}