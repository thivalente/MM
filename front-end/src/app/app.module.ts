import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CurrencyMaskInputMode, NgxCurrencyModule } from "ngx-currency";
import { NgxUiLoaderModule } from 'ngx-ui-loader';

import { AppRoutingModule } from './app-routing.module';
import { ChartsModule } from 'ng2-charts';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';

import { NavegacaoModule } from './navegacao/navegacao.module';
import { ClienteModule } from './cliente/cliente.module';

import { AppComponent } from './app.component';

import { LOCALE_ID } from '@angular/core';
import localePt from '@angular/common/locales/pt';
import { registerLocaleData } from '@angular/common';

import { SettingsService } from './_services/settings.service';

registerLocaleData(localePt, 'pt');
export const customCurrencyMaskConfig = { align: "right", allowNegative: true, allowZero: true, decimal: ",", precision: 2, prefix: "R$ ", suffix: "", thousands: ".", nullable: true, min: null, max: null, inputMode: CurrencyMaskInputMode.FINANCIAL };

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    FormsModule,
    HttpClientModule,

    ChartsModule,
    NgbModule,
    NgxCurrencyModule.forRoot(customCurrencyMaskConfig),
    NgxUiLoaderModule,
    PerfectScrollbarModule,
    
    ClienteModule,
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
