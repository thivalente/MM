import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CurrencyMaskInputMode, NgxCurrencyModule } from "ngx-currency";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { ModalModule } from 'ngx-bootstrap/modal';
import { CustomFormsModule } from 'ngx-custom-validators'
import { NgxLoadingModule } from 'ngx-loading';
import { NgModule } from '@angular/core';
import { NgxMaskModule, IConfig } from 'ngx-mask'
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import { RouterModule } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';

import { AppRoutingModule } from './app-routing.module';
import { ChartsModule } from 'ng2-charts';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';

import { AdminModule } from './admin/admin.module';
import { ClienteModule } from './cliente/cliente.module';

import { AppComponent } from './app.component';
import { LoginComponent } from './conta/login/login.component';
import { EsqueciSenhaComponent } from './conta/esqueci-senha/esqueci-senha.component';
import { TrocarSenhaComponent } from './conta/trocar-senha/trocar-senha.component';

import { LOCALE_ID } from '@angular/core';
import localePt from '@angular/common/locales/pt';
import { registerLocaleData, CommonModule } from '@angular/common';

import { ErrorInterceptor } from './_services/error.handler.service';
export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
];

import { ContaGuard } from './conta/_services/conta.guard';
import { ContaTrocaSenhaGuard } from './conta/_services/conta-troca-senha.guard';
import { SettingsService } from './_services/settings.service';

import { AppLogadoRoutingModule } from './conta/app-logado-routing.module';


registerLocaleData(localePt, 'pt');
export const customCurrencyMaskConfig = { align: "right", allowNegative: true, allowZero: true, decimal: ",", precision: 2, prefix: "R$ ", suffix: "", thousands: ".", nullable: true, min: null, max: null, inputMode: CurrencyMaskInputMode.FINANCIAL };
export const maskConfigFunction: () => Partial<IConfig> = () => { return { validation: true }; };

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    EsqueciSenhaComponent,
    TrocarSenhaComponent
  ],
  imports: [
    AppLogadoRoutingModule,
    AppRoutingModule,
    
    BrowserModule,
    BrowserAnimationsModule,
    CommonModule,
    CustomFormsModule,
    FormsModule,
    HttpClientModule,
    ModalModule.forRoot(),
    ReactiveFormsModule,
    RouterModule,
    ToastrModule.forRoot({ closeButton: true, positionClass: 'toast-bottom-right' }),

    ChartsModule,
    NgbModule,
    NgxCurrencyModule.forRoot(customCurrencyMaskConfig),
    NgxLoadingModule.forRoot({}),
    NgxMaskModule.forRoot(maskConfigFunction),
    NgxUiLoaderModule,
    PerfectScrollbarModule,
    
    AdminModule,
    ClienteModule
  ],
  providers: 
  [
    ContaGuard,
    ContaTrocaSenhaGuard,
    SettingsService,
    { provide: LOCALE_ID, useValue: "pt-BR" },
    httpInterceptorProviders
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
