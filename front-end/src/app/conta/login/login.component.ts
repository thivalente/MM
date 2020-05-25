import { Component, OnInit, ViewChildren, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControlName } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { Observable, fromEvent, merge } from 'rxjs';

import { DisplayMessage, GenericValidator, ValidationMessages } from 'src/app/utils/generic-form-validation';

import { Usuario } from 'src/app/_models/usuario';
import { ContaService } from '../conta.service';

@Component({ selector: 'app-login', templateUrl: './login.component.html', styleUrls: ['./login.component.css']})
export class LoginComponent implements OnInit
{
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

  errors: any[] = [];
  loginForm: FormGroup;
  usuario: Usuario;

  displayMessage: DisplayMessage = {};
  genericValidator: GenericValidator;
  validationMessages: ValidationMessages;

  returnUrl: string;

  constructor(private contaService: ContaService, private route: ActivatedRoute, private fb: FormBuilder,
    private router: Router, private toastr: ToastrService)
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

    this.toastr.success('Login realizado com Sucesso!', 'Bem vindo!!!');

    var dados = response.data;

    this.contaService.LocalStorage.salvarDadosLocaisUsuario(dados);

    let is_admin = dados.userToken.is_admin;

    this.returnUrl ? this.router.navigate([this.returnUrl])
        : (is_admin ? this.router.navigate(['/admin/usuario']) : this.router.navigate(['/cliente/dashboard']));
  }

  processarFalha(fail: any)
  {
    this.errors = fail.error.errors;
    this.toastr.error(this.errors[0], 'Não foi possível realizar o login');
  }
}
