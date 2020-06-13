import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FeaturesComponent } from './features/features.component';
import { ContactComponent } from './contact/contact.component';
import { ServicesComponent } from './services/services.component';
import { FooterComponent } from './footer/footer.component';

import { FeatherModule } from 'angular-feather';
import { HttpClientModule } from '@angular/common/http';

import {
  Mail, Link, PhoneCall, Clock, MapPin, Facebook, Twitter, Instagram, Linkedin, Send, Calendar, User, Server, Star, Layout, LifeBuoy,
  ArrowRightCircle, PieChart, Triangle
} from 'angular-feather/icons';
import { ScrollspyDirective } from './scrollspy.directive';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxLoadingModule } from 'ngx-loading';
import { ContactService } from './contact/contact.service';


const icons = {
  Mail, Link, PhoneCall, Clock, MapPin, Facebook, Twitter, Instagram, Linkedin, Send, Calendar, User, Server, Star, Layout, LifeBuoy,
  ArrowRightCircle, PieChart, Triangle
};

@NgModule({
  // tslint:disable-next-line: max-line-length
  declarations: [FeaturesComponent, ContactComponent, ServicesComponent, FooterComponent, ScrollspyDirective],
  imports: [
    CommonModule,
    HttpClientModule,
    FeatherModule.pick(icons),
    NgxLoadingModule.forRoot({}),
    ReactiveFormsModule
  ],
  providers:[ContactService],
  // tslint:disable-next-line: max-line-length
  exports: [FeaturesComponent, ContactComponent, ServicesComponent, FooterComponent, FeatherModule, ScrollspyDirective]
})
export class SharedModule { }
