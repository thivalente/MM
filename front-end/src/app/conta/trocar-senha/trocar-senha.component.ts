import { Component, OnInit, ViewChildren, ElementRef, AfterViewInit } from '@angular/core';
import { FormControlName, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';

import { CustomValidators } from 'ngx-custom-validators';
import { Observable, fromEvent, merge } from 'rxjs';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { DisplayMessage, GenericValidator, ValidationMessages } from 'src/app/utils/generic-form-validation';
import { ContaService } from '../_services/conta.service';

declare const isEmpty: any;

@Component({ selector: 'app-trocar-senha', templateUrl: './trocar-senha.component.html', styleUrls: ['./trocar-senha.component.css'] })
export class TrocarSenhaComponent implements OnInit, AfterViewInit
{
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];
  
  loading_login: boolean = false;

  errors: any[] = [];
  loginForm: FormGroup;

  displayMessage: DisplayMessage = {};
  genericValidator: GenericValidator;
  validationMessages: ValidationMessages;

  constructor(private contaService: ContaService, private fb: FormBuilder, 
    private ngxService: NgxUiLoaderService, private router: Router, private toastr: ToastrService)
  {
    this.validationMessages = {
      senha: { required: 'Informe a senha atual' },
      novasenha: { required: 'Informe a nova senha' },
      confirmacaosenha: {
        required: 'Informe a confirmação de senha',
        equalTo: 'As senhas não conferem'
      }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void
  {
    let novasenha = new FormControl('', [Validators.required]);
    let confirmacaosenha = new FormControl('', [Validators.required, CustomValidators.equalTo(novasenha)]);

    this.loginForm = this.fb.group({
      senha: ['', [Validators.required]],
      novasenha: novasenha,
      confirmacaosenha: confirmacaosenha
    });
  }

  ngAfterViewInit(): void
  {
    let controlBlurs: Observable<any>[] = this.formInputElements.map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));
    merge(...controlBlurs).subscribe(() => { this.displayMessage = this.genericValidator.processarMensagens(this.loginForm); });
  }

  trocar()
  {
    if (this.loginForm.dirty && this.loginForm.valid)
    {
      this.loading_login = true;

      this.contaService.trocarSenha(this.loginForm.get('senha').value, this.loginForm.get('novasenha').value)
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
    this.toastr.success('Sua senha foi atualizada com sucesso!', 'Operação Realizada com Sucesso');

    var usuario = this.contaService.LocalStorage.obterUsuario();
    usuario.trocar_senha = false;

    this.contaService.atualizarDadosUsuarioLogado(usuario);
    this.router.navigate([usuario.pagina_inicial]);
  }

  processarFalha(fail: any)
  {
    this.errors = fail.error.errors;
    var erro = "Erro Desconhecido";

    if (!isEmpty(this.errors) && this.errors.length > 0)
      erro = this.errors[0];

    this.toastr.error(erro, 'Não foi possível atualizar sua senha');
    this.loading_login = false;
  }
}
