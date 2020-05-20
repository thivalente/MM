import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component';
import { SimuladorComponent } from './simulador/simulador.component';

const clienteRouterConfig: Routes = [
    { path: '', redirectTo: '/dashboard', pathMatch: 'full' },

    { path: 'dashboard', component: DashboardComponent },
    { path: 'simulador', component: SimuladorComponent },
];

@NgModule({
    imports:
    [
        RouterModule.forChild(clienteRouterConfig)
    ],
    exports:[RouterModule]
})
export class ClienteRoutingModule{}