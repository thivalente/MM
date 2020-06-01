import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AcessoNegadoComponent } from './navegacao/acesso-negado/acesso-negado.component';
import { EsqueciSenhaComponent } from './conta/esqueci-senha/esqueci-senha.component';
import { TrocarSenhaComponent } from './conta/trocar-senha/trocar-senha.component';
import { LoginComponent } from './conta/login/login.component';
import { NotFoundComponent } from './navegacao/not-found/not-found.component';

import { ContaGuard } from './conta/_services/conta.guard';
import { ContaTrocaSenhaGuard } from './conta/_services/conta-troca-senha.guard';

const routes: Routes = 
[
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent, canActivate: [ContaGuard] },
  { path: 'esqueci-senha', component: EsqueciSenhaComponent },
  { path: 'trocar-senha', component: TrocarSenhaComponent, canActivate: [ContaTrocaSenhaGuard] },

  { path: 'acesso-negado', component: AcessoNegadoComponent },
  { path: 'nao-encontrado', component: NotFoundComponent },
	{ path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
