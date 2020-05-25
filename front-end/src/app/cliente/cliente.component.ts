import { Component } from '@angular/core';

@Component({ selector: 'app-root', template: 
`
    <app-menu></app-menu>
    <div class="site-container d-flex flex-column">
        <router-outlet></router-outlet>
    </div>
    <app-footer></app-footer>
` })
export class ClienteComponent
{ }
