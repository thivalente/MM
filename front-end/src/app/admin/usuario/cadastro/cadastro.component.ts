import { Component, OnInit, AfterViewInit, ElementRef, ViewChildren } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControlName } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ToastrService } from 'ngx-toastr';

import { merge, Observable, fromEvent } from 'rxjs';
import { MASKS, NgBrazilValidators } from 'ng-brazil';

import { ValidationMessages, GenericValidator, DisplayMessage } from './../../../utils/generic-form-validation';

import { AdminUsuarioService } from './../_services/usuario.service';
import { Usuario } from './../../../_models/usuario';
import { CurrencyUtils } from 'src/app/utils/currency-utils';

declare const isEmpty: any;

@Component({ selector: 'admin-usuario-cadastro', templateUrl: './cadastro.component.html', styleUrls: ['./cadastro.component.css'] })
export class AdminUsuarioCadastroComponent implements OnInit, AfterViewInit
{
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

  cadastroForm: FormGroup;
  usuario: Usuario;
  mudancasNaoSalvas: boolean;
  is_admin: boolean;
  is_novo: boolean;

  displayMessage: DisplayMessage = {};
  errors: any[] = [];
  genericValidator: GenericValidator;
  validationMessages: ValidationMessages;

  public MASKS = MASKS;

  constructor(private adminService: AdminUsuarioService, private route: ActivatedRoute, private fb: FormBuilder,
    private ngxLoader: NgxUiLoaderService, private router: Router, private toastr: ToastrService)
  {
    this.mudancasNaoSalvas = false;

    this.validationMessages = 
    {
      nome: { required: "Informe o nome" },
      cpf: { required: "Informe o CPF", cpf: "Formato do CPF inválido" },
      email: { required: "Informe o e-mail", email: "Formato do e-mail inválido" },
      taxa_acima_cdi: { required: "Informe a taxa acima do CDI" }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);

    this.usuario = this.route.snapshot.data['usuario'];

    this.is_novo = isEmpty(this.usuario);
  }

  ngOnInit(): void
  {
    this.cadastroForm = this.fb.group(
      {
        nome: ['', Validators.required],
        cpf: ['', [Validators.required, NgBrazilValidators.cpf]],
        email: ['', [Validators.required, Validators.email]],
        taxa_acima_cdi: ['', Validators.required],
        is_admin: [''],
        ativo: ['']
      });

      this.cadastroForm.patchValue({ is_admin: false, ativo: true, taxa_acima_cdi: '0' });

      if (!isEmpty(this.usuario))
      {
        this.cadastroForm.patchValue({
          id: this.usuario.id,
          nome: this.usuario.nome,
          cpf: this.usuario.cpf,
          email: this.usuario.email,
          taxa_acima_cdi: CurrencyUtils.DecimalParaString(this.usuario.taxa_acima_cdi * 100),
          is_admin: this.usuario.is_admin.toString(),
          ativo: this.usuario.ativo
        });

        this.is_admin = this.checkIsAdmin();
      }
  }

  ngAfterViewInit(): void
  {
    let controlBlurs: Observable<any>[] = this.formInputElements.map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));
    merge(...controlBlurs).subscribe(() => { this.displayMessage = this.genericValidator.processarMensagens(this.cadastroForm); this.mudancasNaoSalvas = true; });

    this.cadastroForm.valueChanges.subscribe(() => 
    {
      this.is_admin = this.checkIsAdmin();

      if (!this.is_admin && isEmpty(this.cadastroForm.get('taxa_acima_cdi').value))
        this.cadastroForm.patchValue({ taxa_acima_cdi: 0 });
    });
  }

  private checkIsAdmin(): boolean
  {
    return this.cadastroForm.get("is_admin").value === 'true';
  }

  goToMovimentacoes()
  {
    this.router.navigate(['/admin/usuario/movimentacao/' + this.usuario.id]);
  }

  salvar(nAttempts: number)
  {
    if (this.cadastroForm.dirty && this.cadastroForm.valid)
    {
      this.usuario = Object.assign({}, this.usuario, this.cadastroForm.value);
      this.usuario.is_admin = this.is_admin;
      this.usuario.taxa_acima_cdi = this.is_admin ? 0 : this.usuario.taxa_acima_cdi / 100;

      this.adminService.salvarUsuario(this.usuario).subscribe(response =>
        {
            if (response != null)
            {
              this.mudancasNaoSalvas = false;
              this.router.navigate(['/admin/usuario']);
              this.toastr.success('Dados do usuário salvos com sucesso!', 'Operação Realizada');
            }
  
            this.ngxLoader.stopLoader('loader-principal');
        },
        error =>
        {
          nAttempts = nAttempts || 1;
          console.log(error, nAttempts);
  
          if (nAttempts >= 5)
          {
              this.toastr.error('Não foi possível salvar os dados deste usuário', 'Operação Não Realizada');
              this.ngxLoader.stopLoader('loader-principal');
              return;
          }
  
          this.salvar(++nAttempts);
        });
    }
  }
}
