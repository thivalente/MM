import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component';
import { SimuladorComponent } from './simulador/simulador.component';

import { ClienteGuard } from './_services/cliente.guard';

const clienteRouterConfig: Routes = [
    { path: '', redirectTo: '/cliente/dashboard', pathMatch: 'full' },

    {
        path: '',
        children:
        [
            { path: 'dashboard', component: DashboardComponent, canActivate: [ClienteGuard] },
            { path: 'simulador', component: SimuladorComponent, canActivate: [ClienteGuard] }
        ]
    }
];

@NgModule({
    imports:
    [
        RouterModule.forChild(clienteRouterConfig)
    ],
    exports:[RouterModule]
})
export class ClienteRoutingModule{}