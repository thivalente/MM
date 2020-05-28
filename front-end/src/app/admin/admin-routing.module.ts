import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AdminUsuarioListaComponent } from './usuario/lista/lista.component';
import { AdminUsuarioCadastroComponent } from './usuario/cadastro/cadastro.component';
import { AdminUsuarioMovimentacaoComponent } from './usuario/movimentacao/movimentacao.component';

import { AdminGuard } from './_services/admin.guard';
import { AdminUsuarioResolve } from './usuario/_services/usuario.resolve';

const adminRouterConfig: Routes = [
    { path: '', redirectTo: '/admin/usuario', pathMatch: 'full' },
    
    {
        path: '',
        children:
        [
            { path: 'usuario', component: AdminUsuarioListaComponent, canActivate: [AdminGuard] },
            {
                path: 'usuario/editar/:id', component: AdminUsuarioCadastroComponent, 
                canActivate: [AdminGuard], canDeactivate: [AdminGuard],
                resolve: { usuario: AdminUsuarioResolve }
            },
            { path: 'usuario/novo', component: AdminUsuarioCadastroComponent, canActivate: [AdminGuard], canDeactivate: [AdminGuard] },
            {
                path: 'usuario/movimentacao/:id', component: AdminUsuarioMovimentacaoComponent,
                canActivate: [AdminGuard], canDeactivate: [AdminGuard],
                resolve: { usuario: AdminUsuarioResolve }
            }
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