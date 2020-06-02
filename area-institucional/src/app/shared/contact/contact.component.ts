import { Component, OnInit, ViewChildren, ElementRef, AfterViewInit } from '@angular/core';
import { FormControlName, FormGroup, FormBuilder, Validators } from '@angular/forms';

import { Observable, fromEvent, merge } from 'rxjs';

import { ContactService } from './contact.service';
import { DisplayMessage, GenericValidator, ValidationMessages } from 'src/app/utils/generic-form.validation';
import { Mensagem } from 'src/app/_models/mensagem';

declare const isEmpty: any;

@Component({ selector: 'app-contact', templateUrl: './contact.component.html', styleUrls: ['./contact.component.scss'] })
export class ContactComponent implements OnInit, AfterViewInit
{
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

  sucesso: number = 0;
  mensagem_retorno: string = 'Não foi possível enviar a mensagem';
  loading_contato: boolean = false;
  mensagem: Mensagem;

  errors: any[] = [];
  contatoForm: FormGroup;

  displayMessage: DisplayMessage = {};
  genericValidator: GenericValidator;
  validationMessages: ValidationMessages;

  constructor(private contatoService: ContactService, private fb: FormBuilder)
  {
    this.validationMessages = {
      email: {
        required: 'Informe o e-mail',
        email: 'Email inválido'
      },
      nome: {
        required: 'Informe o seu nome'
      },
      mensagem: {
        required: 'Informe a mensagem que deseja enviar'
      }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void
  {
    this.contatoForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        nome: ['', [Validators.required]],
        mensagem: ['', [Validators.required]]
      });
  }

  ngAfterViewInit(): void
  {
    let controlBlurs: Observable<any>[] = this.formInputElements.map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));
    merge(...controlBlurs).subscribe(() => { this.displayMessage = this.genericValidator.processarMensagens(this.contatoForm); });
  }

  enviar()
  {
    if (this.contatoForm.dirty && this.contatoForm.valid)
    {
      this.mensagem = Object.assign({}, this.mensagem, this.contatoForm.value);
      this.mensagem.assunto = "Contato Via Site";

      this.loading_contato = true;

      this.contatoService.enviar(this.mensagem)
      .subscribe(
          sucesso => {this.processarSucesso(sucesso)},
          falha => {this.processarFalha(falha)}
      );
    }
  }

  processarSucesso(response: any)
  {
    this.contatoForm.reset();
    this.errors = [];

    if (isEmpty(response))
    {
      this.processarFalha({ error: { errors: ['Erro desconhecido. Tente novamente'] }});
      return;
    }

    this.mensagem_retorno = 'Mensagem enviada com sucesso!';
    this.sucesso = 1;
    this.loading_contato = false;
  }

  processarFalha(fail: any)
  {
    this.errors = fail.error.errors;
    console.log(this.errors);
    var erro = "Erro Desconhecido";

    if (!isEmpty(this.errors) && this.errors.length > 0)
      erro = this.errors[0];

    this.mensagem_retorno = 'Não foi possível enviar a mensagem!';
    this.sucesso = 2;
    this.loading_contato = false;
  }
}
