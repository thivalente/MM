import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NotFoundComponent } from './navegacao/not-found/not-found.component';

import { AdminGuard } from './admin/_services/admin.guard';
import { ClienteGuard } from './cliente/_services/cliente.guard';

const routes: Routes = 
[
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  
  {
    path: '',
      loadChildren: () => import ('./cliente/cliente.module')
      .then(m => m.ClienteModule),
      canActivate: [ClienteGuard]
  },

  {
    path: 'admin',
      loadChildren: () => import ('./admin/admin.module')
      .then(m => m.AdminModule),
      canLoad: [AdminGuard]
  },

	{ path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
