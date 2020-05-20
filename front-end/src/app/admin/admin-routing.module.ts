import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AdminUsuarioListaComponent } from './usuario/lista/lista.component';

const adminRouterConfig: Routes = [
    { path: '', redirectTo: '/admin/usuario', pathMatch: 'full' },

    { path: 'usuario', component: AdminUsuarioListaComponent }
];

@NgModule({
    imports:
    [
        RouterModule.forChild(adminRouterConfig)
    ],
    exports:[RouterModule]
})
export class AdminRoutingModule{}