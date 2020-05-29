import { Component, OnInit, ElementRef, ViewChildren, ViewChild } from '@angular/core';
import { FormControlName, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ToastrService } from 'ngx-toastr';
import { Observable, fromEvent, merge } from 'rxjs';

import { DisplayMessage, GenericValidator, ValidationMessages } from 'src/app/utils/generic-form-validation';

import { Usuario } from 'src/app/_models/usuario';

import { AdminUsuarioService } from './../_services/usuario.service';

declare const isEmpty: any;

@Component({ selector: 'app-movimentacao', templateUrl: './movimentacao.component.html', styleUrls: ['./movimentacao.component.css']})
export class AdminUsuarioMovimentacaoComponent implements OnInit
{
  @ViewChild('entrada') entrada: ElementRef;
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

  cadastroForm: FormGroup;
  datePattern: string = "^(?:(?:(?:(?:0[1-9]|1[0-9]|2[0-8])[\/](?:0[1-9]|1[012]))|(?:(?:29|30|31)[\/](?:0[13578]|1[02]))|(?:(?:29|30)[\/](?:0[4,6,9]|11)))[\/](?:19|[2-9][0-9])\d\d)|(?:29[\/]02[\/](?:19|[2-9][0-9])(?:00|04|08|12|16|20|24|28|32|36|40|44|48|52|56|60|64|68|72|76|80|84|88|92|96))$";
  
  paginaAtual: number = 1;
  usuario: Usuario;
  edit_mode: Boolean = false;

  displayMessage: DisplayMessage = {};
  errors: any[] = [];
  genericValidator: GenericValidator;
  validationMessages: ValidationMessages;

  constructor(private adminService: AdminUsuarioService, private route: ActivatedRoute, private fb: FormBuilder,
    private ngxLoader: NgxUiLoaderService, private router: Router, private toastr: ToastrService)
  {
    this.usuario = this.route.snapshot.data['usuario'];

    if (isEmpty(this.usuario))
    {
      this.toastr.error('Dados do usuário não encontrados', 'Usuário Não Encontrado');
      this.router.navigate(['/']);
    }
    else if (this.usuario.is_admin)
    {
      this.toastr.error('Somente clientes podem acessar movimentações', 'Usuário Inválido');
      this.goToCadastro();
    }

    this.validationMessages = 
    {
      data_criacao: { required: "Informe a data da movimentação", pattern: "A data da movimentação está inválida" },
      valor: { required: "Informe o valor da movimentação", min: "O valor da movimentação deve ser maior do que zero" }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void
  {
    this.cadastroForm = this.fb.group(
      {
        entrada: ['', Validators.required],
        data_criacao: [null, [Validators.required]],
        valor: ['', [Validators.required, Validators.min(0.01)]]
      });
  }

  edicao_Cancelar()
  {
    this.cadastroForm.reset();
    this.cadastroForm.markAsPristine();
    this.cadastroForm.markAsUntouched();
    this.cadastroForm.updateValueAndValidity();
    this.errors = [];
    this.displayMessage = {};

    this.edit_mode = false;
  }

  edicao_Iniciar()
  {
    this.edit_mode = true;
    this.cadastroForm.patchValue({ entrada: true, valor: '0' });

    setTimeout(() =>
    {
      let controlBlurs: Observable<any>[] = this.formInputElements.map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));
      merge(...controlBlurs).subscribe(() => { this.displayMessage = this.genericValidator.processarMensagens(this.cadastroForm); });
    });
  }

  goToCadastro()
  {
    this.router.navigate(['/admin/usuario/editar/' + this.usuario.id]);
  }

  isEntrada()
  {
    return this.entrada.nativeElement.selectedOptions[0].text === "Aporte";
  }

  salvar(nAttempts)
  {
    //this.ngxLoader.startLoader('loader-principal');

    if (this.cadastroForm.dirty && this.cadastroForm.valid)
    {
      let obj = this.cadastroForm.value;
      let tipo = this.entrada.nativeElement.selectedOptions[0].text;
      let entrada = tipo === "Aporte";
      let data_criacao = obj.data_criacao.year.toString() + '-' + obj.data_criacao.month.toString().padStart(2, '0') + '-' + obj.data_criacao.day.toString().padStart(2, '0') + 'T03:00:00';
      let valor = entrada ? obj.valor : -1 * obj.valor;

      let movimentacao = 
      {
        id: null,
        usuario_id: this.usuario.id,
        valor: valor,
        data_criacao: data_criacao,
        entrada: entrada,
        ativo: true,
        tipo: tipo
      };

      this.usuario.movimentacoes.unshift(movimentacao);

      this.edicao_Cancelar();
      return;

      this.adminService.salvarMovimentacao(movimentacao).subscribe(response =>
        {
            if (response != null)
            {
              this.edicao_Cancelar();
              this.toastr.success('Movimentação adicionada com sucesso!', 'Operação Realizada');
            }
  
            this.ngxLoader.stopLoader('loader-principal');
        },
        error =>
        {
          nAttempts = nAttempts || 1;
          console.log(error, nAttempts);
  
          if (nAttempts >= 5)
          {
              this.toastr.error('Não foi possível adicionar esta movimentação', 'Operação Não Realizada');
              this.ngxLoader.stopLoader('loader-principal');
              return;
          }
  
          this.salvar(++nAttempts);
        });
    }
  }

  textoMovimentacao() : string
  {
    if (this.cadastroForm.dirty && this.cadastroForm.valid)
    {
      let tipoTexto = this.isEntrada() ? 'feito um aporte de' : 'feita uma retirada de';
      let valorTexto = (this.cadastroForm.get("valor").value).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
      let data_criacao = this.cadastroForm.get("data_criacao").value;
      let dataTexto = data_criacao.day.toString().padStart(2, '0') + '/' + data_criacao.month.toString().padStart(2, '0') + '/' + data_criacao.year.toString();

      return 'Foi ' + tipoTexto + ' ' + valorTexto + ' no dia ' + dataTexto;
    }
    else
      return 'Preencha os dados acima para cadastrar a movimentação';
  }
}
