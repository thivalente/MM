import { NgModule, LOCALE_ID } from "@angular/core";
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NgbDatepickerModule, NgbDatepickerI18n, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgBrazil } from 'ng-brazil'
import { NgxCurrencyModule } from 'ngx-currency';
import { NgxMaskModule } from 'ngx-mask';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { TextMaskModule } from 'angular2-text-mask';

import { AdminRoutingModule } from './admin-routing.module';
import { NavegacaoModule } from '../navegacao/navegacao.module';

import { AdminGuard } from './_services/admin.guard';
import { AdminUsuarioResolve } from './usuario/_services/usuario.resolve';
import { AdminUsuarioService } from './usuario/_services/usuario.service';

import { CustomDatepickerI18n, I18n } from '../utils/custom-datepicker-I18n';
import { NgbDatePTParserFormatter } from '../utils/ngbDatePTParserFormatter';

import { AdminComponent } from './admin.component';
import { AdminUsuarioCadastroComponent } from './usuario/cadastro/cadastro.component';
import { AdminUsuarioListaComponent } from './usuario/lista/lista.component';
import { AdminUsuarioMovimentacaoComponent } from './usuario/movimentacao/movimentacao.component';

@NgModule(
    {
        declarations:
        [
            AdminComponent,
            AdminUsuarioCadastroComponent,
            AdminUsuarioListaComponent,
            AdminUsuarioMovimentacaoComponent
        ],
        imports:
        [
           CommonModule,
           FormsModule,
           NgbDatepickerModule,
           NgBrazil,
           NgxCurrencyModule,
           NgxMaskModule,
           NgxPaginationModule,
           NgxUiLoaderModule,
           PerfectScrollbarModule,
           ReactiveFormsModule,
           TextMaskModule,

           AdminRoutingModule,
           NavegacaoModule
        ],
        exports:
        [
            AdminComponent,
            AdminUsuarioCadastroComponent,
            AdminUsuarioListaComponent
        ],
        providers:
        [
            AdminGuard,
            AdminUsuarioResolve,
            AdminUsuarioService,

            { provide: LOCALE_ID, useValue: "pt-BR" },
            
            [I18n, { provide: NgbDatepickerI18n, useClass: CustomDatepickerI18n }],
            [{provide: NgbDateParserFormatter, useClass: NgbDatePTParserFormatter}]
        ]
    })
export class AdminModule { }