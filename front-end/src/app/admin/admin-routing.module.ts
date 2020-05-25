import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AdminUsuarioListaComponent } from './usuario/lista/lista.component';
import { AdminUsuarioCadastroComponent } from './usuario/cadastro/cadastro.component';

import { AdminGuard } from './_services/admin.guard';

const adminRouterConfig: Routes = [
    { path: '', redirectTo: '/admin/usuario', pathMatch: 'full' },
    
    {
        path: '',
        children:
        [
            { path: 'usuario', component: AdminUsuarioListaComponent, canActivate: [AdminGuard] },
            { path: 'usuario/editar/:id', component: AdminUsuarioCadastroComponent, canActivate: [AdminGuard], canDeactivate: [AdminGuard] },
            { path: 'usuario/novo', component: AdminUsuarioCadastroComponent, canActivate: [AdminGuard], canDeactivate: [AdminGuard] }
        ]
    }
];

@NgModule({
    imports:
    [
        RouterModule.forChild(adminRouterConfig)
    ],
    exports:[RouterModule]
})
export class AdminRoutingModule{}