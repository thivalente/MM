import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NgBrazil } from 'ng-brazil'
import { NgxCurrencyModule } from 'ngx-currency';
import { TextMaskModule } from 'angular2-text-mask';

import { AdminGuard } from './_services/admin.guard';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminUsuarioCadastroComponent } from './usuario/cadastro/cadastro.component';
import { AdminUsuarioListaComponent } from './usuario/lista/lista.component';

@NgModule(
    {
        declarations:
        [
            AdminUsuarioCadastroComponent,
            AdminUsuarioListaComponent
        ],
        imports:
        [
           CommonModule,
           FormsModule,
           NgBrazil,
           NgxCurrencyModule,
           ReactiveFormsModule,
           TextMaskModule,

           AdminRoutingModule
        ],
        exports:
        [
            AdminUsuarioListaComponent,
        ],
        providers:[AdminGuard]
    })
export class AdminModule { }