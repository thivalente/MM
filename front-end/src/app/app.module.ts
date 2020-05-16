import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxUiLoaderModule } from 'ngx-ui-loader';

import { AppRoutingModule } from './app-routing.module';
import { ChartsModule } from 'ng2-charts';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';

import { NavegacaoModule } from './navegacao/navegacao.module';
import { SimuladorComponent } from './cliente/simulador/simulador.component';

import { AppComponent } from './app.component';
import { DashboardComponent } from './cliente/dashboard/dashboard.component';

import { PeriodoCabecalhoFilterPipe } from './utils/pipes/periodoCabecalhoFilter.pipe';
import { PeriodoHistoricoFilterPipe } from './utils/pipes/periodoHistoricoFilter.pipe';
import { PeriodoPorMesFilterPipe } from './utils/pipes/periodoPorMesFilter.pipe';

import { LOCALE_ID } from '@angular/core';
import localePt from '@angular/common/locales/pt';
import { registerLocaleData } from '@angular/common';

import { SettingsService } from './_services/settings.service';

registerLocaleData(localePt, 'pt');

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    SimuladorComponent,

    PeriodoCabecalhoFilterPipe,
    PeriodoHistoricoFilterPipe,
    PeriodoPorMesFilterPipe
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    FormsModule,
    HttpClientModule,

    ChartsModule,
    NgbModule,
    NgxUiLoaderModule,
    PerfectScrollbarModule,

    NavegacaoModule
  ],
  providers: 
  [
    SettingsService,
    { provide: LOCALE_ID, deps: [SettingsService], useFactory: (settingsService) => settingsService.getLocale() }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
