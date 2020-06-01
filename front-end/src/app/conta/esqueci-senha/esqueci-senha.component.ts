import { Component, OnInit, AfterViewInit, ElementRef, ViewChildren } from '@angular/core';
import { FormGroup, Validators, FormBuilder, FormControlName } from '@angular/forms';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ToastrService } from 'ngx-toastr';

import { Observable, fromEvent, merge } from 'rxjs';

import { DisplayMessage, GenericValidator, ValidationMessages } from 'src/app/utils/generic-form-validation';
import { ContaService } from '../_services/conta.service';

declare const isEmpty: any;

@Component({ selector: 'app-esqueci-senha', templateUrl: './esqueci-senha.component.html', styleUrls: ['./esqueci-senha.component.css'] })
export class EsqueciSenhaComponent implements OnInit, AfterViewInit
{
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];
  
  loading_login: boolean = false;

  errors: any[] = [];
  loginForm: FormGroup;

  displayMessage: DisplayMessage = {};
  genericValidator: GenericValidator;
  validationMessages: ValidationMessages;

  constructor(private contaService: ContaService, private fb: FormBuilder, 
    private ngxService: NgxUiLoaderService, private toastr: ToastrService)
  {
    this.validationMessages = {
      email: {
        required: 'Informe o e-mail',
        email: 'Email inválido'
      }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void
  {
    this.loginForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]]
      });
  }

  ngAfterViewInit(): void
  {
    let controlBlurs: Observable<any>[] = this.formInputElements.map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));
    merge(...controlBlurs).subscribe(() => { this.displayMessage = this.genericValidator.processarMensagens(this.loginForm); });
  }

  recuperar()
  {
    if (this.loginForm.dirty && this.loginForm.valid)
    {
      this.loading_login = true;

      this.contaService.recuperarSenha(this.loginForm.get('email').value)
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
    this.loading_login = false;
    this.toastr.success('Sua senha foi enviada a seu e-mail!', 'Operação Realizada com Sucesso');
  }

  processarFalha(fail: any)
  {
    this.errors = fail.error.errors;
    var erro = "Erro Desconhecido";

    if (!isEmpty(this.errors) && this.errors.length > 0)
      erro = this.errors[0];

    this.toastr.error(erro, 'Não foi possível recuperar a senha');
    this.loading_login = false;
  }
}
