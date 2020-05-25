import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AdminGuard } from '../admin/_services/admin.guard';
import { ClienteGuard } from '../cliente/_services/cliente.guard';

import { AdminComponent } from '../admin/admin.component';
import { ClienteComponent } from './../cliente/cliente.component';

const routes: Routes = 
[
  { path: '', redirectTo: '/cliente/dashboard', pathMatch: 'full' },

  {
    path: 'cliente', component: ClienteComponent,
      loadChildren: () => import ('../cliente/cliente.module')
      .then(m => m.ClienteModule),
      canLoad: [ClienteGuard]
  },

  {
    path: 'admin', component: AdminComponent,
      loadChildren: () => import ('../admin/admin.module')
      .then(m => m.AdminModule),
       canLoad: [AdminGuard]
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppLogadoRoutingModule { }
