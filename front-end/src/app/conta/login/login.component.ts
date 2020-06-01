import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Component, OnInit, ViewChildren, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControlName } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { Observable, fromEvent, merge } from 'rxjs';

import { DisplayMessage, GenericValidator, ValidationMessages } from 'src/app/utils/generic-form-validation';

import { Usuario } from 'src/app/_models/usuario';
import { ContaService } from '../_services/conta.service';

declare const isEmpty: any;

@Component({ selector: 'app-login', templateUrl: './login.component.html', styleUrls: ['./login.component.css']})
export class LoginComponent implements OnInit
{
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

  loading_login: boolean = false;

  errors: any[] = [];
  loginForm: FormGroup;
  usuario: Usuario;

  displayMessage: DisplayMessage = {};
  genericValidator: GenericValidator;
  validationMessages: ValidationMessages;

  returnUrl: string;

  constructor(private contaService: ContaService, private route: ActivatedRoute, private fb: FormBuilder,
    private ngxService: NgxUiLoaderService, private router: Router, private toastr: ToastrService)
  {
      this.validationMessages = {
        email: {
          required: 'Informe o e-mail',
          email: 'Email inválido'
        },
        senha: {
          required: 'Informe a senha',
          rangeLength: 'A senha deve possuir entre 6 e 15 caracteres'
        }
      };

      this.genericValidator = new GenericValidator(this.validationMessages);

      this.returnUrl = this.route.snapshot.queryParams['returnUrl'];  
  }

  ngOnInit(): void
  {
    this.loginForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        senha: ['', [Validators.required]]
      });
  }

  ngAfterViewInit(): void
  {
    let controlBlurs: Observable<any>[] = this.formInputElements.map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));
    merge(...controlBlurs).subscribe(() => { this.displayMessage = this.genericValidator.processarMensagens(this.loginForm); });
  }

  login()
  {
    if (this.loginForm.dirty && this.loginForm.valid)
    {
      this.usuario = Object.assign({}, this.usuario, this.loginForm.value);

      // this.processarSucesso({
      //   accessToken: 'F87A97CD-B584-4154-B462-5008445E7EA8',
      //   userToken: { id: '13226661-4927-46f3-969a-2a3919183747', nome: 'Elói Gonçalves', primeiro_nome: 'Elói', email: 'eloi@hotmail.com', cpf: '123.456.789-00', ativo: true,
      //   is_admin: false, data_criacao: '', aceitou_termos: false, data_aceitou_termos: null, taxa_acima_cdi: 2 }
      // });

      this.loading_login = true;

      this.contaService.efetuarLogin(this.usuario.email, this.usuario.senha)
      .subscribe(
          sucesso => {this.processarSucesso(sucesso)},
          falha => {this.processarFalha(falha)}
      );
    }
  }

  processarSucesso(response: any)
  {
    this.loginForm.reset();
    this.errors = [];

    if (isEmpty(response) || isEmpty(response.data.userToken))
    {
      this.processarFalha({ error: { errors: ['Erro desconhecido. Tente novamente'] }});
      return;
    }

    var dados = response.data;
    this.loading_login = false;

    var pagina_inicial = this.contaService.obterPaginaInicial(dados.userToken);
    dados.userToken.pagina_inicial = pagina_inicial;

    this.contaService.LocalStorage.salvarDadosLocaisUsuario(dados);

    if (!dados.userToken.trocar_senha)
    {
      this.toastr.success('Login realizado com Sucesso!', 'Bem vindo!!!');
      this.returnUrl ? this.router.navigate([this.returnUrl]) : this.router.navigate([pagina_inicial]); 
    }
    else
      this.router.navigate(['/trocar-senha']);
  }

  processarFalha(fail: any)
  {
    this.errors = fail.error.errors;
    var erro = "Erro Desconhecido";

    if (!isEmpty(this.errors) && this.errors.length > 0)
      erro = this.errors[0];

    this.toastr.error(erro, 'Não foi possível realizar o login');
    this.loading_login = false;
  }
}
