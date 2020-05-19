import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DashboardComponent } from './cliente/dashboard/dashboard.component';
import { NotFoundComponent } from './navegacao/not-found/not-found.component';
import { SimuladorComponent } from './cliente/simulador/simulador.component';

const routes: Routes = 
[
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'simulador', component: SimuladorComponent },

	{ path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
