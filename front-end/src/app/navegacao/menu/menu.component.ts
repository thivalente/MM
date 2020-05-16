import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({ selector: 'app-menu', templateUrl: './menu.component.html', styleUrls: ['./menu.component.css'] })

export class MenuComponent
{
  public isCollapsed: boolean;
  public router: Router;

  constructor(private routerService: Router)
  {
    this.isCollapsed = true;
    this.router = routerService;
  }
}
