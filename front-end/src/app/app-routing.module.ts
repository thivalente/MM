import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AcessoNegadoComponent } from './navegacao/acesso-negado/acesso-negado.component';
import { LoginComponent } from './conta/login/login.component';
import { NotFoundComponent } from './navegacao/not-found/not-found.component';

const routes: Routes = 
[
  { path: '', redirectTo: '/conta/login', pathMatch: 'full' },
  { path: 'conta/login', component: LoginComponent },

  { path: 'acesso-negado', component: AcessoNegadoComponent },
  { path: 'nao-encontrado', component: NotFoundComponent },
	{ path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
