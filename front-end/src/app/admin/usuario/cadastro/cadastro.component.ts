import { Component, OnInit, AfterViewInit, ElementRef, ViewChildren } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControlName } from '@angular/forms';

import { merge, Observable, fromEvent } from 'rxjs';
import { MASKS, NgBrazilValidators } from 'ng-brazil';

import { ValidationMessages, GenericValidator, DisplayMessage } from './../../../utils/generic-form-validation';

import { Usuario } from './../../../_models/usuario';
import { ContaService } from 'src/app/conta/conta.service';

@Component({ selector: 'admin-usuario-cadastro', templateUrl: './cadastro.component.html', styleUrls: ['./cadastro.component.css'] })
export class AdminUsuarioCadastroComponent implements OnInit, AfterViewInit
{
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

  cadastroForm: FormGroup;
  usuario: Usuario;

  displayMessage: DisplayMessage = {};
  errors: any[] = [];
  genericValidator: GenericValidator;
  validationMessages: ValidationMessages;

  public MASKS = MASKS;

  constructor(private contaService: ContaService, private fb: FormBuilder)
  {
    this.validationMessages = 
    {
      nome: { required: "Informe o nome" },
      cpf: { required: "Informe o CPF", cpf: "Formato do CPF inválido" },
      email: { required: "Informe o e-mail", email: "Formato do e-mail inválido" },
      taxa_acima_di: { required: "Informe a taxa acima do CDI" }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void
  {
    this.cadastroForm = this.fb.group(
      {
        nome: ['', Validators.required],
        cpf: ['', Validators.required, NgBrazilValidators.cpf],
        email: ['', Validators.required, Validators.email],
        taxa_acima_di: ['', Validators.required]
      });
  }

  ngAfterViewInit(): void
  {
    let controlBlurs: Observable<any>[] = this.formInputElements.map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));
    merge(...controlBlurs).subscribe(() => { this.displayMessage = this.genericValidator.processarMensagens(this.cadastroForm); });
  }

  salvar()
  {
    if (this.cadastroForm.dirty && this.cadastroForm.valid)
    {
      this.usuario = Object.assign({}, this.usuario, this.cadastroForm.value);
      this.contaService.salvarUsuario(this.usuario);
    }
  }
}
