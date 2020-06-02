import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgxLoadingModule } from 'ngx-loading';
import { ToastrModule } from 'ngx-toastr';

import { PagesModule } from './pages/pages.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ContactService } from './shared/contact/contact.service';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgxLoadingModule.forRoot({}),
    ToastrModule.forRoot({ closeButton: true, positionClass: 'toast-bottom-right' }),

    PagesModule
  ],
  providers: [ContactService],
  bootstrap: [AppComponent]
})
export class AppModule { }
