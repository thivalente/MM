import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { DashboardComponent } from './dashboard/dashboard.component';
import { SimuladorComponent } from './simulador/simulador.component';

import { PeriodoCabecalhoFilterPipe } from '../utils/pipes/periodoCabecalhoFilter.pipe';
import { PeriodoHistoricoFilterPipe } from '../utils/pipes/periodoHistoricoFilter.pipe';
import { PeriodoPorMesFilterPipe } from '../utils/pipes/periodoPorMesFilter.pipe';

import { ChartsModule } from 'ng2-charts';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxCurrencyModule } from 'ngx-currency';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';

@NgModule(
    {
        declarations:
        [
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

           ChartsModule,
           NgbModule,
           NgxCurrencyModule,
           NgxUiLoaderModule,
           PerfectScrollbarModule
        ],
        exports:
        [
            DashboardComponent,
            SimuladorComponent,
        ]
    })
export class ClienteModule { }