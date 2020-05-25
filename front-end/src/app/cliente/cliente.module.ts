import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ChartsModule } from 'ng2-charts';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxCurrencyModule } from 'ngx-currency';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';

import { ClienteRoutingModule } from './cliente-routing.module';
import { NavegacaoModule } from './../navegacao/navegacao.module';

import { ClienteComponent } from './cliente.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SimuladorComponent } from './simulador/simulador.component';

import { PeriodoCabecalhoFilterPipe } from '../utils/pipes/periodoCabecalhoFilter.pipe';
import { PeriodoHistoricoFilterPipe } from '../utils/pipes/periodoHistoricoFilter.pipe';
import { PeriodoPorMesFilterPipe } from '../utils/pipes/periodoPorMesFilter.pipe';

import { ClienteGuard } from './_services/cliente.guard';

@NgModule(
    {
        declarations:
        [
            ClienteComponent,
            DashboardComponent,
            SimuladorComponent,

            PeriodoCabecalhoFilterPipe,
            PeriodoHistoricoFilterPipe,
            PeriodoPorMesFilterPipe
        ],
        imports:
        [
           CommonModule,
           FormsModule,

           ClienteRoutingModule,
           NavegacaoModule,

           ChartsModule,
           NgbModule,
           NgxCurrencyModule,
           NgxUiLoaderModule,
           PerfectScrollbarModule
        ],
        exports:
        [
            ClienteComponent,
            DashboardComponent,
            SimuladorComponent
        ],
        providers: [ClienteGuard]
    })
export class ClienteModule { }