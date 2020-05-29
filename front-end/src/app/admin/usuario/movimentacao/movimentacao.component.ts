import { Component, OnInit, ElementRef, ViewChildren, AfterViewInit } from '@angular/core';
import { FormControlName, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Usuario } from 'src/app/_models/usuario';

import { DisplayMessage, GenericValidator, ValidationMessages } from 'src/app/utils/generic-form-validation';
import { Observable, fromEvent, merge } from 'rxjs';

@Component({ selector: 'app-movimentacao', templateUrl: './movimentacao.component.html', styleUrls: ['./movimentacao.component.css']})
export class AdminUsuarioMovimentacaoComponent implements OnInit, AfterViewInit
{
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

  cadastroForm: FormGroup;
  
  paginaAtual: number = 1;
  usuario: Usuario;
  edit_mode: Boolean = false;

  displayMessage: DisplayMessage = {};
  errors: any[] = [];
  genericValidator: GenericValidator;
  validationMessages: ValidationMessages;

  constructor(private route: ActivatedRoute, private fb: FormBuilder, private router: Router)
  {
    this.usuario = this.route.snapshot.data['usuario'];

    this.validationMessages = 
    {
      tipo: { required: "Informe o tipo da movimentação" },
      data_criacao: { required: "Informe a data da movimentação" },
      valor: { required: "Informe o valor da movimentação" }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void
  {
    this.cadastroForm = this.fb.group(
      {
        entrada: [''],
        tipo: ['', Validators.required],
        data_criacao: ['', [Validators.required]],
        valor: ['', Validators.required]
      });

      this.cadastroForm.patchValue({ entrada: true, tipo: 'true', valor: '0' });
  }

  ngAfterViewInit(): void
  {
    let controlBlurs: Observable<any>[] = this.formInputElements.map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));
    merge(...controlBlurs).subscribe(() => { this.displayMessage = this.genericValidator.processarMensagens(this.cadastroForm); });
  }

  cancelarEdicao()
  {
    this.cadastroForm.reset();
    this.edit_mode = false;
  }

  goToCadastro()
  {
    this.router.navigate(['/admin/usuario/editar/' + this.usuario.id]);
  }

  obtervalue()
  {
    return JSON.stringify(this.cadastroForm.value);
  }

  salvar(nAttempt)
  {
    console.log('oi');
    if (this.cadastroForm.dirty && this.cadastroForm.valid)
    {
      this.usuario = Object.assign({}, this.usuario, this.cadastroForm.value);

      // this.adminService.salvarUsuario(this.usuario).subscribe(response =>
      //   {
      //       if (response != null)
      //       {
      //         this.mudancasNaoSalvas = false;
      //         this.router.navigate(['/admin/usuario']);
      //         this.toastr.success('Dados do usuário salvos com sucesso!', 'Operação Realizada');
      //       }
  
      //       this.ngxLoader.stopLoader('loader-principal');
      //   },
      //   error =>
      //   {
      //     nAttempts = nAttempts || 1;
      //     console.log(error, nAttempts);
  
      //     if (nAttempts >= 5)
      //     {
      //         this.toastr.error('Não foi possível salvar os dados deste usuário', 'Operação Não Realizada');
      //         this.ngxLoader.stopLoader('loader-principal');
      //         return;
      //     }
  
      //     this.salvar(++nAttempts);
      //   });
    }
  }
}
